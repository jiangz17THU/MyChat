namespace myChat
{
    partial class MainWindow
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button_query = new System.Windows.Forms.Button();
            this.button_chat = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.List_IDnum = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.List_IPaddr = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.List_avail = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button_delete = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupchat = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(8, 25);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4);
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.textBox1.Size = new System.Drawing.Size(132, 25);
            this.textBox1.TabIndex = 0;
            // 
            // button_query
            // 
            this.button_query.Location = new System.Drawing.Point(159, 25);
            this.button_query.Margin = new System.Windows.Forms.Padding(4);
            this.button_query.Name = "button_query";
            this.button_query.Size = new System.Drawing.Size(100, 29);
            this.button_query.TabIndex = 2;
            this.button_query.Text = "添加好友";
            this.button_query.UseVisualStyleBackColor = true;
            this.button_query.Click += new System.EventHandler(this.button_query_Click);
            // 
            // button_chat
            // 
            this.button_chat.Location = new System.Drawing.Point(407, 231);
            this.button_chat.Margin = new System.Windows.Forms.Padding(4);
            this.button_chat.Name = "button_chat";
            this.button_chat.Size = new System.Drawing.Size(100, 29);
            this.button_chat.TabIndex = 5;
            this.button_chat.Text = "开始聊天";
            this.button_chat.UseVisualStyleBackColor = true;
            this.button_chat.Click += new System.EventHandler(this.button_chat_Click);
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.List_IDnum,
            this.List_IPaddr,
            this.List_avail});
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(13, 141);
            this.listView1.Margin = new System.Windows.Forms.Padding(4);
            this.listView1.Name = "listView1";
            this.listView1.ShowItemToolTips = true;
            this.listView1.Size = new System.Drawing.Size(364, 388);
            this.listView1.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listView1.TabIndex = 6;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // List_IDnum
            // 
            this.List_IDnum.Text = "好友ID";
            this.List_IDnum.Width = 70;
            // 
            // List_IPaddr
            // 
            this.List_IPaddr.Text = "IP地址";
            this.List_IPaddr.Width = 100;
            // 
            // List_avail
            // 
            this.List_avail.Text = "状态";
            this.List_avail.Width = 48;
            // 
            // button_delete
            // 
            this.button_delete.Location = new System.Drawing.Point(407, 355);
            this.button_delete.Margin = new System.Windows.Forms.Padding(4);
            this.button_delete.Name = "button_delete";
            this.button_delete.Size = new System.Drawing.Size(100, 29);
            this.button_delete.TabIndex = 7;
            this.button_delete.Text = "删好友";
            this.button_delete.UseVisualStyleBackColor = true;
            this.button_delete.Click += new System.EventHandler(this.button_delete_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.button_query);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Location = new System.Drawing.Point(29, 26);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(267, 82);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "查询好友ID";
            // 
            // groupchat
            // 
            this.groupchat.Location = new System.Drawing.Point(407, 291);
            this.groupchat.Name = "groupchat";
            this.groupchat.Size = new System.Drawing.Size(100, 32);
            this.groupchat.TabIndex = 9;
            this.groupchat.Text = "开始群聊";
            this.groupchat.UseVisualStyleBackColor = true;
            this.groupchat.Click += new System.EventHandler(this.groupchat_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::ahaChat.Properties.Resources.topwin;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(808, 552);
            this.Controls.Add(this.groupchat);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button_delete);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.button_chat);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "P2P聊天";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button_query;
        private System.Windows.Forms.Button button_chat;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader List_IDnum;
        private System.Windows.Forms.ColumnHeader List_IPaddr;
        private System.Windows.Forms.ColumnHeader List_avail;
        private System.Windows.Forms.Button button_delete;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button groupchat;
    }
}