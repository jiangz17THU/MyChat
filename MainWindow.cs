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
    
    public partial class MainWindow : Form
    {
        
        public MainWindow()
        {
            InitializeComponent();
        }
        //自定义的构造函数,为了传递登陆主人的IP以及套接字
        
        public MainWindow(Socket obj,string titletxt)
        {
            InitializeComponent();
            p2pserver server = new p2pserver();
            Mainclient.main_client = obj;
            this.Text = titletxt;
            username = titletxt;
            server.beginListening();  //开始监听
        }

        public static string username ;
        //查询好友是否在线
        private void button_query_Click(object sender, EventArgs e)
        {
                string ID_query = "q" + textBox1.Text;
                byte[] query_byte = Encoding.ASCII.GetBytes(ID_query);
                Mainclient.main_client.BeginSend(query_byte, 0, query_byte.Length, 0,
                            new AsyncCallback(QuerySend_Callback), Mainclient.main_client);    //发送查询信息

                byte[] bytes = new byte[1024];
                int char_byte = Mainclient.main_client.Receive(bytes);
                string data_string_from_server = System.Text.Encoding.ASCII.GetString(bytes, 0, char_byte);
            
                string receive_string= data_string_from_server;
            
            //输入错误信息
            if ("Please send the correct message." == receive_string )   
            {
                MessageBox.Show(this, "输入信息错误", "error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //输入的账号有误
            else if ("Incorrect No." == receive_string)
            {
                MessageBox.Show(this, "不存在该用户", "error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //如果不在线，自动加为好友
            else if ("n" == receive_string)
            {
                string[] friendinfo = new string[4];
                foreach (ListViewItem item in this.listView1.Items)
                {
                    if (textBox1.Text == item.SubItems[0].Text)
                    {
                        MessageBox.Show(this, "好友不在线，已经在好友列表中", "信息提示",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        item.SubItems[1].Text = "";
                        item.SubItems[2].Text = "不在线";
                        return;
                    }
                }
                MessageBox.Show(this, "好友不在线，已自动保存该好友信息", "信息提示",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

                //向 ListView 中添加好友信息 
                
                friendinfo[0] = textBox1.Text;
                friendinfo[1] = "";
                friendinfo[2] = "不在线";
                ListViewItem frienditem = new ListViewItem(friendinfo);
                listView1.Items.Add(frienditem);
            }

            //好友在线
            else
            {
                string[] friendinfo = new string[4];
                foreach (ListViewItem item in this.listView1.Items)
                {
                    if (textBox1.Text == item.SubItems[0].Text)
                    {
                        MessageBox.Show(this, "好友在线，已经在好友列表中", "信息提示",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                        item.SubItems[1].Text = receive_string; //ip地址
                        item.SubItems[2].Text = "在线";
                        return;
                    }
                }
                MessageBox.Show(this, "好友在线，IP地址为" + receive_string + " 已保存该好友信息"
                    , "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);                
                //向 ListView 中添加好友信息 
                
                friendinfo[0] = textBox1.Text;
                friendinfo[1] = receive_string;
                friendinfo[2] = "在线";
                ListViewItem frienditm = new ListViewItem(friendinfo);
                listView1.Items.Add(frienditm);
            }
        }

       
        void QuerySend_Callback(IAsyncResult iaresult)
        {
            try
            {
                Mainclient.main_client = (Socket)iaresult.AsyncState;
                Mainclient.main_client.EndSend(iaresult);
            }
            catch (SocketException ex)
            {
                MessageBox.Show(this, ex.ToString(), "error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button_chat_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count > 0)//判断listview有被选中项
            {
                int friends_Connected = 0;
                string Users_Broadcast_Msg;
                if (listView1.FindItemWithText("不在线")!=null)
                {
                    if(listView1.FindItemWithText("不在线").Selected)
                    {
                        MessageBox.Show("要发起会话的好友有人不在线", "无法发起会话"
                                  , MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    
                };
                
                Socket[] Chatters = new Socket[listView1.SelectedItems.Count];
                //向所有选中的人广播除了它自己外其他人的ID
                foreach (ListViewItem item_outloop in this.listView1.SelectedItems)
                {
                    //广播信息的第一条ID是自己的ID，ID与ID之间是连续的，通过逗号来分割
                    Users_Broadcast_Msg = this.Text;
                    foreach (ListViewItem item in this.listView1.SelectedItems)
                    {
                        if (item_outloop.SubItems[0].Text != item.SubItems[0].Text)
                            Users_Broadcast_Msg += "," + item.SubItems[0].Text;
                    }
                    Chatters[friends_Connected] = Connect_GroupChat(
                        item_outloop.SubItems[0].Text, Users_Broadcast_Msg);
                    friends_Connected++;
                }

                string friends = "";
                foreach (ListViewItem item in this.listView1.SelectedItems)
                {
                    friends += item.SubItems[0].Text + ",";
                }
                friends = friends.Substring(0, friends.Length - 1);
                
                Thread Thread_Chat = new Thread(() =>
                            Application.Run(new ChatDialog(Chatters, friends_Connected, this.Text , friends)));
                
                Thread_Chat.SetApartmentState(System.Threading.ApartmentState.STA);
                Thread_Chat.Start();
            }
        }

        public Socket Connect_GroupChat(string ID, string Users_Broadcast_Msg)  //群聊实现
        {
            //查询的IP地址
            
            string ID_query = "q" + ID;
            byte[] query_byte = Encoding.ASCII.GetBytes(ID_query);
            Mainclient.main_client.BeginSend(query_byte, 0, query_byte.Length, 0,
                        new AsyncCallback(QuerySend_Callback), Mainclient.main_client);    //发送查询信息

            byte[] bytes = new byte[1024];
            int char_byte = Mainclient.main_client.Receive(bytes);
            string data_string_from_server = System.Text.Encoding.ASCII.GetString(bytes, 0, char_byte);
            string IPstring = data_string_from_server;

            Socket tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint serverIpep = new IPEndPoint(IPAddress.Parse(IPstring), 50389);
            
            tcpClient.Connect(serverIpep);
            //连接成功后向对方发送除对方外，所有群聊者的学号
            byte[] data_send_chat = Encoding.UTF8.GetBytes(Users_Broadcast_Msg);
            tcpClient.Send(data_send_chat);
            return tcpClient;
        }
        
        private void button_delete_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count > 0)//判断listview有被选中项
            {
                foreach (ListViewItem item in this.listView1.SelectedItems)
                {
                    item.Remove();
                }
            }
        }

        //关闭主窗口即下线
        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            byte[] databytes = new byte[1024];
            string data_string;

            string logout_info = "logout" + this.Text;

            byte[] databuff = Encoding.ASCII.GetBytes(logout_info);
            Mainclient.main_client.Send(databuff);    //发送登录信息

           
            int char_logout = Mainclient.main_client.Receive(databytes);
            data_string = System.Text.Encoding.ASCII.GetString(databytes, 0, char_logout);
            
            if(data_string != "loo")                    
            {
                MessageBox.Show(this, "下线失败，只能强制关闭客户端", "信息提示",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;    //返回，等待用户重新登录
            }
            else //成功下线 服务器返回loo
            {
                MessageBox.Show(this, "成功下线", "信息提示",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void groupchat_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count > 0)//判断listview有被选中项
            {
                int friends_Connected = 0;
                string Users_Broadcast_Msg;
                if (listView1.FindItemWithText("不在线")!=null)
                {
                    if (listView1.FindItemWithText("不在线").Selected)
                    {
                        MessageBox.Show("要发起会话的好友有人不在线", "无法发起会话"
                                  , MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                        
                };

                Socket[] Chatters = new Socket[listView1.SelectedItems.Count];
                //向所有选中的人广播除了它自己外其他人的ID
                foreach (ListViewItem item_outloop in this.listView1.SelectedItems)
                {
                    //广播信息的第一条ID是自己的ID，ID与ID之间是连续的，通过逗号来分割
                    Users_Broadcast_Msg = this.Text;
                    foreach (ListViewItem item in this.listView1.SelectedItems)
                    {
                        if (item_outloop.SubItems[0].Text != item.SubItems[0].Text)
                            Users_Broadcast_Msg += "," + item.SubItems[0].Text;
                    }
                    Chatters[friends_Connected] = Connect_GroupChat(
                        item_outloop.SubItems[0].Text, Users_Broadcast_Msg);
                    friends_Connected++;
                }

                string friends = "";
                foreach (ListViewItem item in this.listView1.SelectedItems)
                {
                    friends += item.SubItems[0].Text + ",";
                }
                friends = friends.Substring(0, friends.Length - 1);

                Thread Thread_Chat = new Thread(() =>
                            Application.Run(new ChatDialog(Chatters, friends_Connected, this.Text, friends)));

                Thread_Chat.SetApartmentState(System.Threading.ApartmentState.STA);
                Thread_Chat.Start();
            }
        }
    }  
}
