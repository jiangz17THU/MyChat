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


namespace myChat
{
   
    public partial class Login : Form
    {
        internal static EndPoint iepoint;

        public Login()
        {
            InitializeComponent();  //初始化界面
        }

        
        private void button_login_Click(object sender, EventArgs e)
        {
            
            Tcpclient.tcpclient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            //登录时采用同步套接字连接
            try
            {
                Tcpclient.tcpclient.Connect(Tcpclient.iepoint);
            }
            catch (SocketException)
            {
                MessageBox.Show(this, "无法连接到服务器，请检查你的网络连接", "网络错误",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string username = textBox1.Text;
            string password = textBox2.Text;
            string login_inv = username + "_" + password;//登录字符串

            byte[] login = Encoding.ASCII.GetBytes(login_inv);
            Tcpclient.tcpclient.Send(login);    //发送登录信息

            int char_number;
            byte[] bytes = new byte[1024];
            
            //发送登录信息后，使用同步接收信息，在这里没有必要使用异步，异步反而使程序冗长。
            char_number = Tcpclient.tcpclient.Receive(bytes);
            string data_string = System.Text.Encoding.ASCII.GetString(bytes, 0, char_number);
            if (data_string == "lol")
            {
                MessageBox.Show(this, "成功登录", "信息提示",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(this, "登录指令错误", "信息提示",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;    //返回，等待用户重新登录
            }

            //如果登录成功，新建一个线程，打开程序主窗口
            Thread Thread_Main;

            Thread_Main= new Thread(() => Application.Run(new MainWindow(Tcpclient.tcpclient,username)));
            Thread_Main.Start();
            this.Close();
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            //this.Text即当前窗口的标题的内容是用户的ID
            string logout_inv = "logout" + this.Text;

            byte[] buffer = Encoding.ASCII.GetBytes(logout_inv);
            Tcpclient.tcpclient.Send(buffer);    //发送登录信息

            int char_number;
            byte[] bytes_rec = new byte[1024];
            
            
            char_number = Tcpclient.tcpclient.Receive(bytes_rec);
            string data_string = System.Text.Encoding.ASCII.GetString(bytes_rec, 0, char_number);
            if (data_string == "loo")
            {
                MessageBox.Show(this, "成功下线", "信息提示",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(this, "下线失败，只能强制关闭客户端", "信息提示",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;    //返回，等待用户重新登录
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
    }
    
}
