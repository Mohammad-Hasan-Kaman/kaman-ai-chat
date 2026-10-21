namespace chatai
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.clearChatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.speakRepliesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopSpeakingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setApiKeyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.bot_L = new System.Windows.Forms.Label();
            this.sendButton = new System.Windows.Forms.PictureBox();
            this.menuButton = new System.Windows.Forms.PictureBox();
            this.contextMenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            //
            // contextMenuStrip1
            //
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearChatToolStripMenuItem,
            this.speakRepliesToolStripMenuItem,
            this.stopSpeakingToolStripMenuItem,
            this.setApiKeyToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(196, 104);
            //
            // clearChatToolStripMenuItem
            //
            this.clearChatToolStripMenuItem.Name = "clearChatToolStripMenuItem";
            this.clearChatToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.clearChatToolStripMenuItem.Text = "Clear chat";
            this.clearChatToolStripMenuItem.Click += new System.EventHandler(this.clearChatToolStripMenuItem_Click);
            //
            // speakRepliesToolStripMenuItem
            //
            this.speakRepliesToolStripMenuItem.Checked = true;
            this.speakRepliesToolStripMenuItem.CheckOnClick = true;
            this.speakRepliesToolStripMenuItem.Name = "speakRepliesToolStripMenuItem";
            this.speakRepliesToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.speakRepliesToolStripMenuItem.Text = "Speak replies aloud";
            this.speakRepliesToolStripMenuItem.CheckedChanged += new System.EventHandler(this.speakRepliesToolStripMenuItem_CheckedChanged);
            //
            // stopSpeakingToolStripMenuItem
            //
            this.stopSpeakingToolStripMenuItem.Name = "stopSpeakingToolStripMenuItem";
            this.stopSpeakingToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.stopSpeakingToolStripMenuItem.Text = "Stop speaking";
            this.stopSpeakingToolStripMenuItem.Click += new System.EventHandler(this.stopSpeakingToolStripMenuItem_Click);
            //
            // setApiKeyToolStripMenuItem
            //
            this.setApiKeyToolStripMenuItem.Name = "setApiKeyToolStripMenuItem";
            this.setApiKeyToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.setApiKeyToolStripMenuItem.Text = "Set API key...";
            this.setApiKeyToolStripMenuItem.Click += new System.EventHandler(this.setApiKeyToolStripMenuItem_Click);
            //
            // panel2
            //
            this.panel2.AutoScroll = true;
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(253)))), ((int)(((byte)(230)))));
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(10, 10, 10, 10);
            this.panel2.Size = new System.Drawing.Size(800, 372);
            this.panel2.TabIndex = 1;
            //
            // panel1
            //
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(233)))), ((int)(((byte)(233)))));
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.bot_L);
            this.panel1.Controls.Add(this.sendButton);
            this.panel1.Controls.Add(this.menuButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 372);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 78);
            this.panel1.TabIndex = 2;
            //
            // textBox1
            //
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.BackColor = System.Drawing.Color.White;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(62, 20);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(663, 38);
            this.textBox1.TabIndex = 1;
            this.textBox1.Enter += new System.EventHandler(this.textBox1_Enter);
            this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            this.textBox1.Leave += new System.EventHandler(this.textBox1_Leave);
            //
            // bot_L
            //
            this.bot_L.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.bot_L.AutoSize = true;
            this.bot_L.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.bot_L.Location = new System.Drawing.Point(369, 62);
            this.bot_L.Name = "bot_L";
            this.bot_L.Size = new System.Drawing.Size(98, 15);
            this.bot_L.TabIndex = 4;
            this.bot_L.Text = "Bot is replying...";
            this.bot_L.Visible = false;
            //
            // sendButton
            //
            this.sendButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.sendButton.Image = ((System.Drawing.Image)(resources.GetObject("sendButton.Image")));
            this.sendButton.Location = new System.Drawing.Point(748, 20);
            this.sendButton.Name = "sendButton";
            this.sendButton.Size = new System.Drawing.Size(40, 38);
            this.sendButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.sendButton.TabIndex = 3;
            this.sendButton.TabStop = false;
            this.sendButton.Click += new System.EventHandler(this.sendButton_Click);
            //
            // menuButton
            //
            this.menuButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.menuButton.Image = ((System.Drawing.Image)(resources.GetObject("menuButton.Image")));
            this.menuButton.Location = new System.Drawing.Point(12, 20);
            this.menuButton.Name = "menuButton";
            this.menuButton.Size = new System.Drawing.Size(40, 38);
            this.menuButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.menuButton.TabIndex = 2;
            this.menuButton.TabStop = false;
            this.menuButton.Click += new System.EventHandler(this.menuButton_Click);
            //
            // Form1
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.MinimumSize = new System.Drawing.Size(520, 380);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "kaman chat AI";
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox menuButton;
        private System.Windows.Forms.PictureBox sendButton;
        private System.Windows.Forms.ToolStripMenuItem clearChatToolStripMenuItem;
        private System.Windows.Forms.Label bot_L;
        private System.Windows.Forms.ToolStripMenuItem speakRepliesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopSpeakingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setApiKeyToolStripMenuItem;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Panel panel2;
    }
}
