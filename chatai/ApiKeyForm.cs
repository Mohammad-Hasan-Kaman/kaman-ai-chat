using System;
using System.Drawing;
using System.Windows.Forms;

namespace chatai
{
    /// <summary>
    /// Modal dialog used to collect the OpenModel API key on first run.
    /// The key is stored in %APPDATA%\kaman-ai-chat\settings.txt and is never
    /// written into the executable or the repository.
    /// </summary>
    internal sealed class ApiKeyForm : Form
    {
        private readonly TextBox keyBox;
        private readonly CheckBox saveBox;
        private readonly Label statusLabel;

        public ApiKeyForm()
        {
            Text = "OpenModel API key";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(430, 186);
            Font = new Font("Segoe UI", 9F);
            BackColor = Color.White;

            var heading = new Label
            {
                Text = "Enter your OpenModel API key to start chatting.",
                Location = new Point(16, 14),
                Size = new Size(398, 20),
                AutoSize = false
            };

            keyBox = new TextBox
            {
                Location = new Point(16, 40),
                Size = new Size(398, 23),
                UseSystemPasswordChar = true
            };

            statusLabel = new Label
            {
                Text = "Get a key at https://openmodel.ai",
                Location = new Point(16, 68),
                Size = new Size(398, 18),
                ForeColor = Color.DimGray
            };

            saveBox = new CheckBox
            {
                Text = "Remember this key on this computer",
                Location = new Point(16, 92),
                Size = new Size(260, 22),
                Checked = true
            };

            var okButton = new Button
            {
                Text = "Save and continue",
                Location = new Point(232, 140),
                Size = new Size(98, 30)
            };
            okButton.Click += OkButton_Click;

            var cancelButton = new Button
            {
                Text = "Quit",
                Location = new Point(338, 140),
                Size = new Size(76, 30)
            };
            cancelButton.Click += CancelButton_Click;

            Controls.Add(heading);
            Controls.Add(keyBox);
            Controls.Add(statusLabel);
            Controls.Add(saveBox);
            Controls.Add(okButton);
            Controls.Add(cancelButton);
            AcceptButton = okButton;
            CancelButton = cancelButton;
        }

        public string ApiKey
        {
            get { return keyBox.Text.Trim(); }
        }

        public bool Remember
        {
            get { return saveBox.Checked; }
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            if (ApiKey.Length == 0)
            {
                statusLabel.ForeColor = Color.Firebrick;
                statusLabel.Text = "The key cannot be empty.";
                keyBox.Focus();
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
