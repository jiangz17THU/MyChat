namespace myChat
{
    partial class ChatDialog
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
            this.richTextBox_show = new System.Windows.Forms.RichTextBox();
            this.richTextBox_send = new System.Windows.Forms.RichTextBox();
            this.button_send = new System.Windows.Forms.Button();
            this.button_fileTrans = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // richTextBox_show
            // 
            this.richTextBox_show.Location = new System.Drawing.Point(206, 15);
            this.richTextBox_show.Margin = new System.Windows.Forms.Padding(4);
            this.richTextBox_show.Name = "richTextBox_show";
            this.richTextBox_show.ReadOnly = true;
            this.richTextBox_show.Size = new System.Drawing.Size(494, 223);
            this.richTextBox_show.TabIndex = 2;
            this.richTextBox_show.Text = "";
            // 
            // richTextBox_send
            // 
            this.richTextBox_send.Location = new System.Drawing.Point(206, 276);
            this.richTextBox_send.Margin = new System.Windows.Forms.Padding(4);
            this.richTextBox_send.Name = "richTextBox_send";
            this.richTextBox_send.Size = new System.Drawing.Size(494, 60);
            this.richTextBox_send.TabIndex = 0;
            this.richTextBox_send.Text = "";
            // 
            // button_send
            // 
            this.button_send.Location = new System.Drawing.Point(69, 307);
            this.button_send.Margin = new System.Windows.Forms.Padding(4);
            this.button_send.Name = "button_send";
            this.button_send.Size = new System.Drawing.Size(100, 29);
            this.button_send.TabIndex = 1;
            this.button_send.Text = "发送";
            this.button_send.UseVisualStyleBackColor = true;
            this.button_send.Click += new System.EventHandler(this.button_send_Click);
            // 
            // button_fileTrans
            // 
            this.button_fileTrans.Location = new System.Drawing.Point(69, 229);
            this.button_fileTrans.Margin = new System.Windows.Forms.Padding(4);
            this.button_fileTrans.Name = "button_fileTrans";
            this.button_fileTrans.Size = new System.Drawing.Size(100, 29);
            this.button_fileTrans.TabIndex = 3;
            this.button_fileTrans.Text = "传输文件";
            this.button_fileTrans.UseVisualStyleBackColor = true;
            this.button_fileTrans.Click += new System.EventHandler(this.button_fileTrans_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // ChatDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::ahaChat.Properties.Resources.chat;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(734, 461);
            this.Controls.Add(this.button_fileTrans);
            this.Controls.Add(this.button_send);
            this.Controls.Add(this.richTextBox_send);
            this.Controls.Add(this.richTextBox_show);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ChatDialog";
            this.Text = "开始聊天";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ChatDialog_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox_show;
        private System.Windows.Forms.RichTextBox richTextBox_send;
        private System.Windows.Forms.Button button_send;
        private System.Windows.Forms.Button button_fileTrans;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Timer timer1;
    }
}