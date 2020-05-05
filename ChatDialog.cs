using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace myChat 
{
    public partial class ChatDialog : Form
    {
        public ChatDialog()
        {
            InitializeComponent();
        }

        Socket[] alllinks;  //此次对话中所有的连接
        int client_num;
        String my_IDnumber;
        String friend_IDnumber;
        bool richTextBox_show_writing = false;

        public ChatDialog(Socket[] obj, int _num, String _myID, String _friendID)
        {
            InitializeComponent();
            this.Text = "与" + _friendID + "的会话";
            my_IDnumber = _myID;
            friend_IDnumber = _friendID;
            alllinks = obj;
            client_num = _num;
            AsynRecive(alllinks);          //异步接收消息
            Control.CheckForIllegalCrossThreadCalls = false;
        }
        

        #region 异步接受客户端消息
        /// <summary>
        /// 异步接受客户端消息
        /// </summary>
        /// <param name="tcpClient"></param>
        public void AsynRecive(Socket[] links)
        {
            byte[] datarec = new byte[1024];
            try
            {
                foreach (Socket tcpClient in links)   //遍历所有连接的套接字
                {
                    if (tcpClient == null) break;
                    tcpClient.BeginReceive(datarec, 0, datarec.Length, SocketFlags.None,
                    asyncResult =>
                    {
                        int length = 0;
                        try
                        {
                            length = tcpClient.EndReceive(asyncResult);
                            string rcv_msg = Encoding.UTF8.GetString(datarec, 0, length);

                            if (length == 0)
                            {
                                MessageBox.Show("好友退出了聊天", "信息提示",
                                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.Close();
                                return;
                            }
                            //如果是服务器，则向其他客户端转发该消息
                            foreach (Socket otherClient in links)
                            {
                                if (otherClient == null) break;
                                if (otherClient != tcpClient)
                                    AsynSend(otherClient, rcv_msg);
                            }

                            if (rcv_msg == "<__cmd__transfer__myfile__>")
                            {
                                allDone.Reset();
                                receive_save r_s = new receive_save(ReceiveFileConnect);
                                this.Invoke(r_s, new object[] { tcpClient });
                                allDone.WaitOne();
                            }
                           
                            else
                            {
                                //如果当前写字框没有被占用
                                while (richTextBox_show_writing) { };
                                //等到其他线程解除了写字框的占用
                                richTextBox_show_writing = true;   //占用之
                                RichBox_Show rb_s = new RichBox_Show(ShowMsg_inRichTextBox);
                                string show_string = rcv_msg;
                                this.Invoke(rb_s, new object[] { show_string, Color.Black, HorizontalAlignment.Left });
                                richTextBox_show_writing = false;  //恢复不被占用
                            }
                            AsynRecive(links);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(this, ex.ToString(), "出现异常",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    }, null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "出现异常",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
        }
        #endregion


        public void ShowMsg_inRichTextBox(string txtstr, Color color, HorizontalAlignment direction)
        {
            richTextBox_show.SelectionColor = color;
            richTextBox_show.SelectionAlignment = direction;
            //向文本框的文本追加文本
            richTextBox_show.AppendText(txtstr);
        }
        private void button_send_Click(object sender, EventArgs e)
        {
            try
            {
                string send_msg = richTextBox_send.Text;
                //如果当前写字框没有被占用
                while (richTextBox_show_writing) { };
                //等到其他线程解除了写字框的占用
                richTextBox_show_writing = true;   //占用
                RichBox_Show rb_s = new RichBox_Show(ShowMsg_inRichTextBox);
                string show_string = DateTime.Now.ToString()
                                            + "\n我说：\n" + send_msg + "\n\n";
                this.Invoke(rb_s, new object[] { show_string, Color.Blue, HorizontalAlignment.Right });
                richTextBox_show_writing = false;  //恢复不被占用
                richTextBox_send.Text = "";
                send_msg  = DateTime.Now.ToString() + "\n" + my_IDnumber +"说：\n" + send_msg + "\n\n";

                foreach (Socket Client in alllinks)
                {
                    if (Client == null) break;
                    AsynSend(Client, send_msg);
                }
                
            }
            catch (Exception)
            {
                MessageBox.Show("好友已关闭会话，不能发送信息", "出错啦。。。",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private delegate void RichBox_Show(string str, Color color, HorizontalAlignment direction);
        #region 异步发送消息
        /// <summary>
        /// 异步发送消息
        /// </summary>
        /// <param name="tcpClient">客户端套接字</param>
        /// <param name="message">发送消息</param>
        /// //if_relay表示此信息是否是服务器转发客户端的信息
        public void AsynSend(Socket tcpClient, string message)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            tcpClient.BeginSend(data, 0, data.Length, SocketFlags.None, asyncResult =>
            {
                //完成发送消息
                try
                {
                    int length = tcpClient.EndSend(asyncResult);        
                }
                catch (SocketException ex)
                {
                    MessageBox.Show(ex.ToString(), "发送失败",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }, null);
        }
        #endregion

        //解决关闭会话时的错误
        private void ChatDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (Socket Client in alllinks)
            {
                if (Client == null) break;
                if (!Client.Connected) continue;
                Client.Shutdown(SocketShutdown.Both);
                Client.Close();
            }
        }

        #region 同步接收文件
        /// <summary>
        /// 同步接收文件
        /// </summary>
        /// <param name="tcpServer"></param>

        //因为接收消息的操作是异步的，它是线程池中的一个子线程，不是主线程，无法调用SaveFileDialog，
        //故需要在一部接收消息的线程中，用委托来调用同步文件接收程序

        private ManualResetEvent allDone = new ManualResetEvent(false);

        private delegate void receive_save(Socket File_client);
        public void ReceiveFileConnect(Socket File_client)
        {
            DialogResult dr = MessageBox.Show("好友要向你传一个文件，是否接收？", "提示"
                                                , MessageBoxButtons.OKCancel);
            if (dr == DialogResult.OK)
            {
                //用户选择确认的操作
                saveFileDialog1.Title = "请保存文件";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string fileSavePath = saveFileDialog1.FileName;//获得用户保存文件的路径
                    FileStream fs = new FileStream(fileSavePath, FileMode.Create, FileAccess.Write);
                    int total = 0;
                    int received;
                    int buffer_size = 1000000;
                    byte[] buffer = new byte[buffer_size];
                    while (true)
                    { 
                        received = File_client.Receive(buffer, buffer_size, SocketFlags.None);
                        string string_send = Encoding.UTF8.GetString(buffer, 0, received);
                        foreach (Socket otherClient in alllinks)
                        {
                            if (otherClient == null) break;
                            if (otherClient != File_client)
                                AsynSend(otherClient, string_send);
                        }
                        fs.Write(buffer, total, received);
                        fs.Flush();
                        total += received;
                        if (received < buffer_size)
                        {
                            break;
                        }
                    }
                    fs.Close();
                    MessageBox.Show(fileSavePath + "文件接收完毕", "信息提示");
                }
                else
                {
                    //用户在保存文件对话框中没有选择文件，所以只转发
                    int total = 0;
                    int received;
                    int buffer_size = 1000000;
                    byte[] buffer = new byte[buffer_size];
                    while (true)
                    {
                        received = File_client.Receive(buffer, buffer_size, SocketFlags.None);
                        string string_send = Encoding.UTF8.GetString(buffer, 0, received);
                        foreach (Socket otherClient in alllinks)
                        {
                            if (otherClient == null) break;
                            if (otherClient != File_client)
                                AsynSend(otherClient, string_send);
                        }
                        total += received;
                        if (received < buffer_size)
                        {
                            break;
                        }
                    }
                }
            }
            else if (dr == DialogResult.Cancel)
            {
                //用户选择取消的操作，只转发
                int total = 0;
                int received;
                int buffer_size = 1000000;
                byte[] buffer = new byte[buffer_size];
                while (true)
                {
                    received = File_client.Receive(buffer, buffer_size, SocketFlags.None);
                    string string_send = Encoding.UTF8.GetString(buffer, 0, received);
                    foreach (Socket otherClient in alllinks)
                    {
                        if (otherClient == null) break;
                        if (otherClient != File_client)
                            AsynSend(otherClient, string_send);
                    }
                    total += received;
                    if (received < buffer_size)
                    {
                        break;
                    }
                }
            }
            
            allDone.Set();
        }
        #endregion
    
        private void button_fileTrans_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "请选择要传输的文件";
            openFileDialog1.Multiselect = false;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                
                String strOpenFileName = openFileDialog1.FileName;//打开的文件的全限定名
                FileInfo file = new FileInfo(strOpenFileName);  //创建文件
                byte[] len = BitConverter.GetBytes(file.Length);
                foreach (Socket Client in alllinks)
                {
                    if (Client == null) break;
                    byte[] data = Encoding.UTF8.GetBytes("<__cmd__transfer__myfile__>");
                    Client.Send(data);
                    Client.SendFile(strOpenFileName, null, null, TransmitFileOptions.UseDefaultWorkerThread);
                }
                MessageBox.Show(strOpenFileName + "文件传输成功", "信息提示");
            }
            else
            {
                MessageBox.Show("你没有选择文件", "信息提示");
            }
        }

      

    }   
}