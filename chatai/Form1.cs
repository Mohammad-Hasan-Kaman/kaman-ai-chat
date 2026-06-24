using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Net.Http; // اضافه شده برای ارتباط با API جدید
using Newtonsoft.Json; // از قبل در پروژه شما نصب است
using System.Messaging.Design;
using System.Speech.Synthesis;

namespace chatai
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            bunifuImageButton1.Enabled = true;
            bot_L.Hide();
            label1.Hide();
            textBox1.Text = "ask me...";
            textBox1.ForeColor = Color.DarkGray;
        }

        int top = 9;
        int left = 12;
        int count = 0;
        SpeechSynthesizer synthesizer = new SpeechSynthesizer();

        // تعریف کلاینت به صورت استاتیک برای بهینه‌سازی منابع سیستم
        private static readonly HttpClient httpClient = new HttpClient();

        // متد اختصاصی جدید برای دریافت پاسخ از OpenModel (با فرمت دیپ‌سیک)
        private async Task<string> GetDeepSeekResponse(string userInput)
        {
            string url = "https://api.openmodel.ai/v1/messages";
            string apiKey = "YOUR_API_KEY_HERE"; // حتماً کلید دریافت شده از openmodel.ai را بگذارید

            var requestBody = new
            {
                model = "deepseek-v4-flash",
                max_tokens = 1024,
                system = "You are a helpful AI assistant. Always reply in Persian (Farsi) or the language the user speaks to you. Avoid outputting Chinese unless requested.",
                messages = new[]
                {
            new { role = "user", content = userInput }
        }
            };

            string jsonPayload = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Add("x-api-key", apiKey);
            httpClient.DefaultRequestHeaders.Add("anthropic-version", "2023-06-01");

            try
            {
                HttpResponseMessage response = await httpClient.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    string shortError = errorContent.Length > 60 ? errorContent.Substring(0, 60) + "..." : errorContent;
                    return $"API Error ({response.StatusCode}): {shortError}";
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(jsonResponse))
                {
                    return "Error: Received empty response.";
                }

                // ۱. تلاش برای تجزیه بر اساس فرمت استاندارد Anthropic
                try
                {
                    dynamic resultObject = JsonConvert.DeserializeObject(jsonResponse);
                    if (resultObject?.content != null && resultObject.content.Count > 0 && resultObject.content[0].text != null)
                    {
                        return resultObject.content[0].text.ToString();
                    }

                    // ۲. تلاش برای تجزیه بر اساس فرمت استاندارد OpenAI (در صورت تغییر ساختار در سمت سرور)
                    if (resultObject?.choices != null && resultObject.choices.Count > 0 && resultObject.choices[0].message?.content != null)
                    {
                        return resultObject.choices[0].message.content.ToString();
                    }
                }
                catch { /* در صورت خطای ساختاری داینامیک، به سراغ متد متنی می‌رود */ }

                // ۳. استخراج هوشمند و متنی (Regex/Substring) برای مواقعی که کلیدهای JSON تغییر کرده‌اند
                // جستجوی فیلد text
                int textIndex = jsonResponse.IndexOf("\"text\":\"");
                if (textIndex != -1)
                {
                    int start = textIndex + 8;
                    int end = jsonResponse.IndexOf("\"", start);
                    if (end != -1) return jsonResponse.Substring(start, end - start).Replace("\\n", "\n");
                }

                // جستجوی فیلد content
                int contentIndex = jsonResponse.IndexOf("\"content\":\"");
                if (contentIndex != -1)
                {
                    int start = contentIndex + 11;
                    int end = jsonResponse.IndexOf("\"", start);
                    if (end != -1) return jsonResponse.Substring(start, end - start).Replace("\\n", "\n");
                }

                // اگر هیچ روشی پاسخ نداد، بخش بسیار کوتاهی از خروجی را نشان بده تا از صفحه بیرون نزند
                string safeSummary = jsonResponse.Length > 80 ? jsonResponse.Substring(0, 80) + "..." : jsonResponse;
                return $"Parse Failed. Data: {safeSummary}";
            }
            catch (Exception ex)
            {
                return $"Exception: {ex.Message}";
            }
        }

        private async void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == string.Empty || string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("The sending box is empty", "Send Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                SoundPlayer Send = new SoundPlayer("SOUND1.wav");
                SoundPlayer Rcv = new SoundPlayer("SOUND2.wav");
                count++;
                for (int i = 0; i < count; i++)
                {
                    bunifuImageButton1.Enabled = false;
                    bot_L.Show();
                    Label lnl_mess_user = new Label();
                    panel2.Controls.Add(lnl_mess_user);
                    string userInput = textBox1.Text;
                    textBox1.Text = "";
                    lnl_mess_user.AutoSize = true;
                    lnl_mess_user.BackColor = Color.Red;
                    lnl_mess_user.Font = new Font("Microsoft Sans Serif", 15.75F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                    lnl_mess_user.Location = new Point(left, top);
                    lnl_mess_user.Name = "label1";
                    lnl_mess_user.Size = new Size(76, 25);
                    lnl_mess_user.TabIndex = 0;
                    lnl_mess_user.Text = userInput;
                    Send.Play();
                    top = lnl_mess_user.Bottom + 30;
                    panel2.VerticalScroll.Value = panel2.VerticalScroll.Maximum;
                    panel2.PerformLayout();
                    count--;

                    // فراخوانی متد جدید به جای OpenAI
                    var result = await GetDeepSeekResponse(userInput);

                    Label lnl_mess_ai = new Label();
                    panel2.Controls.Add(lnl_mess_ai);
                    lnl_mess_ai.AutoSize = true;
                    lnl_mess_ai.BackColor = Color.SkyBlue;
                    lnl_mess_ai.Font = new Font("Microsoft Sans Serif", 15.75F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                    lnl_mess_ai.Location = new Point(left, top);
                    lnl_mess_ai.Name = "label1";
                    lnl_mess_ai.Size = new Size(76, 25);
                    lnl_mess_ai.TabIndex = 0;

                    // افکت تایپ انیمیشنی کاراکترها
                    for (int t = 0; t < result.Length; t++)
                    {
                        System.Threading.Thread.Sleep(100);
                        lnl_mess_ai.Text = lnl_mess_ai.Text.Substring(0, t) + result[t].ToString() + "|";
                        Application.DoEvents();
                    }
                    if (lnl_mess_ai.Text.Length > 0)
                        lnl_mess_ai.Text = lnl_mess_ai.Text.Substring(0, lnl_mess_ai.Text.Length - 1);

                    Rcv.Play();
                    top = lnl_mess_ai.Bottom + 30;
                    count--;
                    bot_L.Hide();
                    bunifuImageButton1.Enabled = true;
                    panel2.VerticalScroll.Value = panel2.VerticalScroll.Maximum;
                    panel2.PerformLayout();
                }
            }
        }

        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Show(bunifuImageButton2, new Point(0, -contextMenuStrip1.Size.Height));
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == "ask me...")
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black;
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = "ask me...";
                textBox1.ForeColor = Color.DarkGray;
            }
        }

        private async void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (textBox1.Text == string.Empty || string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    MessageBox.Show("The sending box is empty", "Send Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    SoundPlayer Send = new SoundPlayer("SOUND1.wav");
                    SoundPlayer Rcv = new SoundPlayer("SOUND2.wav");
                    count++;
                    for (int i = 0; i < count; i++)
                    {
                        bunifuImageButton1.Enabled = false;
                        bot_L.Show();
                        Label lnl_mess_user = new Label();
                        panel2.Controls.Add(lnl_mess_user);
                        string userInput = textBox1.Text;
                        textBox1.Text = "";
                        lnl_mess_user.AutoSize = true;
                        lnl_mess_user.BackColor = Color.Red;
                        lnl_mess_user.Font = new Font("Microsoft Sans Serif", 15.75F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                        lnl_mess_user.Location = new Point(left, top);
                        lnl_mess_user.Name = "label1";
                        lnl_mess_user.Size = new Size(76, 25);
                        lnl_mess_user.TabIndex = 0;
                        lnl_mess_user.Text = userInput;
                        Send.Play();
                        top = lnl_mess_user.Bottom + 30;
                        count--;
                        panel2.VerticalScroll.Value = panel2.VerticalScroll.Maximum;
                        panel2.PerformLayout();

                        // فراخوانی متد جدید به جای OpenAI
                        var result = await GetDeepSeekResponse(userInput);

                        Label lnl_mess_ai = new Label();
                        panel2.Controls.Add(lnl_mess_ai);
                        lnl_mess_ai.AutoSize = true;
                        lnl_mess_ai.BackColor = Color.SkyBlue;
                        lnl_mess_ai.Font = new Font("Microsoft Sans Serif", 15.75F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                        lnl_mess_ai.Location = new Point(left, top);
                        lnl_mess_ai.Name = "label1";
                        lnl_mess_ai.Size = new Size(76, 25);
                        lnl_mess_ai.TabIndex = 0;

                        // افکت تایپ انیمیشنی کاراکترها
                        for (int t = 0; t < result.Length; t++)
                        {
                            System.Threading.Thread.Sleep(100);
                            lnl_mess_ai.Text = lnl_mess_ai.Text.Substring(0, t) + result[t].ToString() + "|";
                            Application.DoEvents();
                        }
                        if (lnl_mess_ai.Text.Length > 0)
                            lnl_mess_ai.Text = lnl_mess_ai.Text.Substring(0, lnl_mess_ai.Text.Length - 1);

                        Rcv.Play();
                        top = lnl_mess_ai.Bottom + 30;
                        count--;
                        bot_L.Hide();
                        bunifuImageButton1.Enabled = true;
                        panel2.AutoScrollPosition = new Point(0, panel1.Height);
                        panel2.VerticalScroll.Value = panel2.VerticalScroll.Maximum;
                        panel2.PerformLayout();
                    }
                }
            }
        }

        private void clearChatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            top = 9;
            left = 12;
            count = 0;
            panel2.Controls.Clear();
        }
    }
}