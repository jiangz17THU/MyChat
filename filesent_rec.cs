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
    class filesent_rec
    {
        private string sendFilePath;
        private object targetUser;
        private char[] localNickName;
        private object serverSocket;
        private string receiveFilePath;
        private object userDialogDict;

        public object FormClient { get; private set; }

        /// <summary>
        /// 发送文件
        /// </summary>
        /// <param name="userName"></param>
        private void SendFileToClient(string userName)
       {
            //User targetUser = userListDict[userName];
            String targetUserIP = userName;

            System.IO.FileInfo EzoneFile = new FileInfo(sendFilePath);
            System.IO.FileStream EzoneStream = EzoneFile.OpenRead();
            //包的大小
            int packetSize = 1000;
            //包的数量
            int packetCount = (int)(EzoneFile.Length / ((long)packetSize));

            //最后一个包的大小
             int lastPacketData = (int)(EzoneFile.Length - ((long)packetSize * packetCount));

            byte[] data = new byte[packetSize];


            try
             {
                IPHostEntry ipHost = Dns.GetHostEntry(targetUserIP);
                IPAddress ipAdd = ipHost.AddressList[0];
                

                Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                client.Connect(Login.iepoint);
                 //发送本机昵称
               
                 
                 client.Shutdown(SocketShutdown.Both);
                 client.Close();

            }
             catch (System.Exception ex)
             {

             }
         }
         public static int SendVarData(Socket s, byte[] data)
         {
             int total = 0;
             int size = data.Length;
             int dataleft = size;
             int sent;
             byte[] datasize = new byte[4];
             datasize = BitConverter.GetBytes(size);
             sent = s.Send(datasize);

              while (total<size)
            {
                 sent = s.Send(data, total, dataleft, SocketFlags.None);
                 total += sent;
                 dataleft -= sent;
             }

             return total;
         }




       /// <summary>
       /// 接受其他客户端的文件
       /// </summary>
       private void ReceiveClientFile()
       {
           while (true)
           {

                Socket clientsocket;
               Thread receiveThread = new Thread(ReceiveClientFileData);
               receiveThread.Start();
               receiveThread.IsBackground = true;
           }

       }

       private void ReceiveClientFileData(object clientSocket)
       {
           Socket myClientSocket = (Socket)clientSocket;
           string totalSize;//文件大小
           int totalCount = 0;//总的包数量
           int receiveCount = 0;//统计已收的包的数量
           string sendClientName;

           if (File.Exists(receiveFilePath))
           {
               File.Delete(receiveFilePath);
           }

           FileStream fs = new FileStream(receiveFilePath, FileMode.Create, FileAccess.Write);
           //发送端的用户名字，用于确定对话框
            
           //文件大小
           
           //总的包数量
            

           
           while (true)
           {
               byte[] data =Encoding.UTF8.GetBytes("<__cmd__transfer__myfile__>");
               //接收来自socket的数据

               if (data.Length == 0)
               {
                   
                   fs.Write(data, 0, data.Length);
                   break;
               }
               else
               {
                   receiveCount++;
                   fs.Write(data, 0, data.Length);
               }


           }
           fs.Close();
           myClientSocket.Close();

       }

        private void AddReceiveFileInfo(object p, string v)
        {
            throw new NotImplementedException();
        }

        private static byte[] ReceiveVarData(Socket s)
       {
           int total = 0;
           int recv;
           byte[] datasize = new byte[4];
           recv = s.Receive(datasize, 0, 4, SocketFlags.None);
           int size = BitConverter.ToInt32(datasize, 0);
           int dataleft = size;
           byte[] data = new byte[size];
           while (total < size)
           {
               recv = s.Receive(data, total, dataleft, SocketFlags.None);
               if (recv == 0)
               {
                   data = null;
                   break;
               }
               total += recv;
               dataleft -= recv;
           }
           return data;
       }
    }
}
