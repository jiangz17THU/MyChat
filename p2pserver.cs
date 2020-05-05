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
    public class p2pserver
    {
        IPAddress myIP;
        #region Tcp协议异步监听
        /// <summary>
        /// Tcp协议异步监听
        /// </summary>
        public void beginListening()
        {
            //主机IP
            IPHostEntry myEntry = Dns.GetHostEntry(Dns.GetHostName());
            int i;
            for (i = 0; i != myEntry.AddressList.Length; i++)
            {
                //不是ipv6
                if (AddressFamily.InterNetwork == myEntry.AddressList[i].AddressFamily)
                {
                    break;
                }
            }
            myIP = myEntry.AddressList[i];
            IPEndPoint serverIP = new IPEndPoint(myIP, 50389);
            Socket tcpServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            tcpServer.Bind(serverIP);  //绑定主机IP和端口号
            tcpServer.Listen(20);  //监听
            AsynAccept(tcpServer);  //异步接收连接
        }
        #endregion

        #region 异步接受客户端消息
        /// <summary>
        /// 异步接受客户端消息
        /// </summary>
        /// <param name="tcpClient"></param>
        public void AsynRecive_ID(Socket tcpClient)
        {
            byte[] data = new byte[1024];
            try
            {
                tcpClient.BeginReceive(data, 0, data.Length, SocketFlags.None,
                asyncResult =>
                {
                    int length = tcpClient.EndReceive(asyncResult);
                    string Users_Broadcast_Received = Encoding.UTF8.GetString(data, 0, length);

                    Socket[] Connect_received = new Socket[1];
                    Connect_received[0] = tcpClient;
                    Thread Thread_Chat = new Thread(() =>
                            Application.Run(new ChatDialog(Connect_received, 1
                                                           , MainWindow.username, Users_Broadcast_Received)));
                    Thread_Chat.SetApartmentState(System.Threading.ApartmentState.STA);
                    Thread_Chat.Start();
                }, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "接收失败",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region 异步接受连接

        #endregion
        public void AsynAccept(Socket tcpServer)
        {
            tcpServer.BeginAccept(asyncResult =>
            {
                Socket tcpClient = tcpServer.EndAccept(asyncResult);
                AsynAccept(tcpServer);     //继续监听其他连接
                AsynRecive_ID(tcpClient);  //接收监听到的这条连接的广播信息
            }, null);
        }
        #endregion
    }
}
