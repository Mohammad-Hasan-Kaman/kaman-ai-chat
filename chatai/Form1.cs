using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Media;
using System.Net.Http;
using System.Reflection;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace chatai
{
    public partial class Form1 : Form
    {
        private const string ApiUrl = "https://api.openmodel.ai/v1/messages";
        private const string ModelName = "deepseek-v4-flash";
        private const string Placeholder = "ask me...";

        private static readonly HttpClient httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(120)
        };

        private readonly JavaScriptSerializer json = new JavaScriptSerializer();

        private SpeechSynthesizer synthesizer;
        private SoundPlayer sendSound;
        private SoundPlayer receiveSound;
        private bool busy;

        public Form1()
        {
            InitializeComponent();

            sendSound = CreateSoundPlayer("SOUND1.wav");
            receiveSound = CreateSoundPlayer("SOUND2.wav");

            try
            {
                synthesizer = new SpeechSynthesizer();
                synthesizer.Rate = -1;
            }
            catch (Exception)
            {
                // Speech synthesis is optional; the chat still works without it.
                synthesizer = null;
            }

            bot_L.Visible = false;
            textBox1.Text = Placeholder;
            textBox1.ForeColor = Color.DarkGray;
        }

        /// <summary>
        /// Loads a wav file that is embedded in the assembly.
        /// </summary>
        private static Stream LoadStream(string resourceName)
        {
            Assembly assembly = typeof(Form1).Assembly;

            Stream stream = assembly.GetManifestResourceStream(resourceName);
            if (stream != null) return stream;

            string stem = resourceName;
            int dot = stem.LastIndexOf('.');
            if (dot > 0) stem = stem.Substring(0, dot);

            foreach (string candidate in assembly.GetManifestResourceNames())
            {
                string candidateStem = candidate;
                int candidateDot = candidateStem.LastIndexOf('.');
                if (candidateDot > 0) candidateStem = candidateStem.Substring(0, candidateDot);

                if (string.Equals(candidateStem, stem, StringComparison.OrdinalIgnoreCase)
                    || candidateStem.EndsWith("." + stem, StringComparison.OrdinalIgnoreCase))
                {
                    return assembly.GetManifestResourceStream(candidate);
                }
            }

            return null;
        }

        /// <summary>
        /// Creates a sound player for an embedded wav file and preloads it.
        /// </summary>
        private static SoundPlayer CreateSoundPlayer(string resourceName)
        {
            try
            {
                Stream stream = LoadStream(resourceName);
                if (stream == null) return null;

                var player = new SoundPlayer(stream);
                player.Load();
                return player;
            }
            catch (Exception)
            {
                // Sound is optional; the chat still works without it.
                return null;
            }
        }

        #region API access

        /// <summary>
        /// Sends the user prompt to the OpenModel messages endpoint and returns
        /// the assistant reply.
        /// </summary>
        private async Task<string> GetDeepSeekResponse(string userInput)
        {
            string apiKey = Settings.GetApiKey();
            if (string.IsNullOrEmpty(apiKey))
            {
                return "No API key configured. Open the menu and choose \"Set API key...\".";
            }

            var requestBody = new Dictionary<string, object>
            {
                { "model", ModelName },
                { "max_tokens", 1024 },
                {
                    "system",
                    "You are a helpful AI assistant. Always reply in Persian (Farsi) or the language the user speaks to you. Avoid outputting Chinese unless requested."
                },
                {
                    "messages", new object[]
                    {
                        new Dictionary<string, object>
                        {
                            { "role", "user" },
                            { "content", userInput }
                        }
                    }
                }
            };

            string jsonPayload = json.Serialize(requestBody);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            using (var request = new HttpRequestMessage(HttpMethod.Post, ApiUrl))
            {
                request.Headers.TryAddWithoutValidation("x-api-key", apiKey);
                request.Headers.TryAddWithoutValidation("anthropic-version", "2023-06-01");
                request.Content = content;

                try
                {
                    HttpResponseMessage response = await httpClient.SendAsync(request).ConfigureAwait(true);

                    string responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

                    if (!response.IsSuccessStatusCode)
                    {
                        string shortError = responseBody.Length > 120 ? responseBody.Substring(0, 120) + "..." : responseBody;
                        return "API Error (" + (int)response.StatusCode + "): " + shortError;
                    }

                    return ExtractReply(responseBody);
                }
                catch (TaskCanceledException)
                {
                    return "The request timed out. Please try again.";
                }
                catch (HttpRequestException ex)
                {
                    return "Network error: " + ex.Message;
                }
                catch (Exception ex)
                {
                    return "Exception: " + ex.Message;
                }
            }
        }

        /// <summary>
        /// Extracts the assistant text from an Anthropic-style, an OpenAI-style,
        /// or a raw-text response body.
        /// </summary>
        private string ExtractReply(string responseBody)
        {
            if (string.IsNullOrEmpty(responseBody))
            {
                return "Error: received an empty response.";
            }

            // 1. Standard Anthropic shape: { "content": [ { "text": "..." } ] }
            try
            {
                var parsed = json.DeserializeObject(responseBody) as Dictionary<string, object>;
                if (parsed != null)
                {
                    object contentValue;
                    if (parsed.TryGetValue("content", out contentValue))
                    {
                        object first = FirstElement(contentValue);
                        if (first != null)
                        {
                            var firstItem = first as Dictionary<string, object>;
                            if (firstItem != null)
                            {
                                object textValue;
                                if (firstItem.TryGetValue("text", out textValue) && textValue != null)
                                {
                                    return textValue.ToString();
                                }
                            }
                        }
                    }

                    // 2. Standard OpenAI shape: { "choices": [ { "message": { "content": "..." } } ] }
                    object choicesValue;
                    if (parsed.TryGetValue("choices", out choicesValue))
                    {
                        object firstChoiceObject = FirstElement(choicesValue);
                        if (firstChoiceObject != null)
                        {
                            var firstChoice = firstChoiceObject as Dictionary<string, object>;
                            if (firstChoice != null)
                            {
                                object messageValue;
                                if (firstChoice.TryGetValue("message", out messageValue))
                                {
                                    var message = messageValue as Dictionary<string, object>;
                                    if (message != null)
                                    {
                                        object contentText;
                                        if (message.TryGetValue("content", out contentText) && contentText != null)
                                        {
                                            return contentText.ToString();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (ArgumentException)
            {
                // Fall through to the text-based extraction below.
            }
            catch (InvalidOperationException)
            {
                // Fall through to the text-based extraction below.
            }

            // 3. Text-based fallback for unexpected response shapes.
            string text = ExtractRawField(responseBody, "\"text\":\"");
            if (text != null) return text;

            text = ExtractRawField(responseBody, "\"content\":\"");
            if (text != null) return text;

            string safeSummary = responseBody.Length > 160 ? responseBody.Substring(0, 160) + "..." : responseBody;
            return "Parse failed. Raw data: " + safeSummary;
        }

        /// <summary>
        /// Returns the first element of a deserialized JSON array, regardless of
        /// whether it was materialized as an ArrayList or an object[].
        /// </summary>
        private static object FirstElement(object value)
        {
            var list = value as ArrayList;
            if (list != null) return list.Count > 0 ? list[0] : null;

            var array = value as object[];
            if (array != null) return array.Length > 0 ? array[0] : null;

            return null;
        }

        private static string ExtractRawField(string body, string marker)
        {
            int index = body.IndexOf(marker, StringComparison.Ordinal);
            if (index < 0) return null;

            int start = index + marker.Length;
            int end = body.IndexOf('"', start);
            if (end < 0) return null;

            return body.Substring(start, end - start).Replace("\\n", "\n").Replace("\\\"", "\"");
        }

        #endregion

        #region Chat flow

        private async void sendButton_Click(object sender, EventArgs e)
        {
            await SendCurrentMessage();
        }

        private async void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            if (e.Shift) return;

            e.SuppressKeyPress = true;
            await SendCurrentMessage();
        }

        private async Task SendCurrentMessage()
        {
            if (busy) return;

            string userInput = textBox1.Text;
            if (string.IsNullOrWhiteSpace(userInput) || userInput == Placeholder)
            {
                MessageBox.Show("The sending box is empty", "Send Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            busy = true;
            sendButton.Enabled = false;
            bot_L.Visible = true;
            textBox1.Clear();
            textBox1.ForeColor = Color.Black;

            try
            {
                AddUserMessage(userInput);
                PlaySound(sendSound);

                string result = await GetDeepSeekResponse(userInput);

                Label aiLabel = AddAiLabel();
                aiLabel.Text = result;
                LayoutBubble(aiLabel, true);
                await TypeOut(aiLabel, result);
                PlaySound(receiveSound);

                if (speakRepliesToolStripMenuItem.Checked)
                {
                    Speak(result);
                }
            }
            catch (Exception ex)
            {
                Label aiLabel = AddAiLabel();
                aiLabel.Text = "Unexpected error: " + ex.Message;
                LayoutBubble(aiLabel, true);
            }
            finally
            {
                bot_L.Visible = false;
                sendButton.Enabled = true;
                busy = false;
                ScrollToBottom();
            }
        }

        private Label AddUserMessage(string text)
        {
            Label label = new Label
            {
                AutoSize = true,
                MaximumSize = new Size(Math.Max(120, panel2.DisplayRectangle.Width - 20), 0),
                BackColor = Color.FromArgb(255, 220, 220),
                Font = new Font("Segoe UI", 11F, FontStyle.Regular),
                ForeColor = Color.FromArgb(120, 20, 20),
                Padding = new Padding(8, 5, 8, 5),
                Text = text
            };

            panel2.Controls.Add(label);
            LayoutBubble(label, false);
            return label;
        }

        /// <summary>
        /// Creates the label that holds an AI reply. The text is applied before the
        /// bubble is positioned so the right alignment uses the final width.
        /// </summary>
        private Label AddAiLabel()
        {
            Label label = new Label
            {
                AutoSize = true,
                MaximumSize = new Size(Math.Max(120, panel2.DisplayRectangle.Width - 20), 0),
                BackColor = Color.FromArgb(205, 235, 255),
                Font = new Font("Segoe UI", 11F, FontStyle.Regular),
                ForeColor = Color.FromArgb(20, 40, 80),
                Padding = new Padding(8, 5, 8, 5)
            };

            panel2.Controls.Add(label);
            return label;
        }


        private void LayoutBubble(Label label, bool alignRight)
        {
            // PerformLayout first so an AutoSize label reports its final width.
            panel2.PerformLayout();

            // DisplayRectangle already accounts for the panel padding and for a
            // visible vertical scrollbar, unlike ClientSize.
            Rectangle area = panel2.DisplayRectangle;

            int x = alignRight
                ? Math.Max(area.Left, area.Right - label.Width - 4)
                : area.Left;

            label.Location = new Point(x, NextBubbleTop());
        }

        private int NextBubbleTop()
        {
            int bottom = panel2.DisplayRectangle.Top;
            foreach (Control control in panel2.Controls)
            {
                if (control.Bottom > bottom) bottom = control.Bottom;
            }

            return bottom + 10;
        }

        /// <summary>
        /// Reveals the reply character by character.
        /// </summary>
        private async Task TypeOut(Label label, string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                label.Text = string.Empty;
                return;
            }

            label.Text = string.Empty;
            LayoutBubble(label, true);

            const int charactersPerTick = 3;
            const int tickMilliseconds = 12;

            var shown = new StringBuilder();
            for (int i = 0; i < text.Length; i += charactersPerTick)
            {
                int end = Math.Min(i + charactersPerTick, text.Length);
                shown.Append(text, i, end - i);
                label.Text = shown.ToString();
                LayoutBubble(label, true);
                ScrollToBottom();
                await Task.Delay(tickMilliseconds).ConfigureAwait(true);
            }

            label.Text = shown.ToString();
            LayoutBubble(label, true);
        }

        private void ScrollToBottom()
        {
            panel2.PerformLayout();

            int contentHeight = 0;
            foreach (Control control in panel2.Controls)
            {
                if (control.Bottom > contentHeight) contentHeight = control.Bottom;
            }

            int target = Math.Max(0, contentHeight - panel2.ClientSize.Height + 12);
            panel2.AutoScrollPosition = new Point(0, target);
        }

        private static void PlaySound(SoundPlayer player)
        {
            try
            {
                if (player != null) player.Play();
            }
            catch (Exception)
            {
                // A missing audio device must not interrupt the conversation.
            }
        }

        #endregion

        #region Speech

        /// <summary>
        /// Speaks the given text with the Windows speech engine.
        /// </summary>
        private void Speak(string text)
        {
            if (synthesizer == null || string.IsNullOrWhiteSpace(text)) return;

            try
            {
                synthesizer.SpeakAsync(text);
            }
            catch (Exception)
            {
                // Speech is a convenience feature; ignore failures.
            }
        }

        private void stopSpeakingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (synthesizer == null) return;

            try
            {
                synthesizer.SpeakAsyncCancelAll();
            }
            catch (Exception)
            {
                // Ignore.
            }
        }

        private void speakRepliesToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (speakRepliesToolStripMenuItem.Checked) return;

            try
            {
                if (synthesizer != null) synthesizer.SpeakAsyncCancelAll();
            }
            catch (Exception)
            {
                // Ignore.
            }
        }

        #endregion

        #region Menu

        private void menuButton_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Show(menuButton, new Point(0, -contextMenuStrip1.Size.Height));
        }

        private void clearChatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel2.Controls.Clear();
            ScrollToBottom();
        }

        private void setApiKeyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dialog = new ApiKeyForm())
            {
                if (dialog.ShowDialog(this) != DialogResult.OK) return;

                if (dialog.Remember)
                {
                    Settings.SaveApiKey(dialog.ApiKey);
                }

                MessageBox.Show(
                    "API key saved. It is stored on this computer only and is not part of the application.",
                    "API key",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == Placeholder)
            {
                textBox1.Text = string.Empty;
                textBox1.ForeColor = Color.Black;
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
            {
                textBox1.Text = Placeholder;
                textBox1.ForeColor = Color.DarkGray;
            }
        }

        #endregion
    }
}
