﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp
{
    class UDPThread
    {
        Socket socket;
        MainWindow context;

        public UDPThread(MainWindow context)
        {
            this.socket = new Socket(SocketType.Dgram, ProtocolType.Udp);
            this.context = context;
        }

        public void run()
        {
            this.socket.Bind(new IPEndPoint(IPAddress.Any, 42069));
            Byte[] receiveBytes = new byte[256];
            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            EndPoint senderRemote = (EndPoint)sender;
            while (true)
            {
                this.socket.ReceiveFrom(receiveBytes, ref senderRemote);
                string returnData = Encoding.UTF8.GetString(receiveBytes).Trim();
                Debug.WriteLine("This is the message you received " + returnData.ToString());
                this.context.Update();
                receiveBytes = new byte[256];
            }
        }
    }
}
