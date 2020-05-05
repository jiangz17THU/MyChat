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
    public class Tcpclient //C-S通信时的客户端
    {

        public static Socket tcpclient;   //创建tcp套接字
        //绑定服务器IP和端口
        public static IPAddress serverIP = IPAddress.Parse("166.111.140.57");
        public static IPEndPoint iepoint = new IPEndPoint(serverIP, 8000);

    }
    public class Mainclient
    {
        public static Socket main_client;
    }
}
