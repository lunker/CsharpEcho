using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

using EchoLibrary;
using System.Collections;

namespace CsharpEchoServer
{
    class ClientInfo
    {
        Socket peer;
        int id;

        public ClientInfo(Socket peer, int id)
        {
            this.peer = peer;
            this.id = id;
        }
    }

    class Handler
    {
        public Socket clientSocket = null;
        public int clientID = 0;

        public Socket ClientSocket
        {
            set { this.clientSocket = value; }
            get { return this.clientSocket; }
        }
        public int ClientID
        {
            set { this.clientID = value; }
            get { return this.clientID; }
        }

        public void echo()
        {
            while (true)
            {
                //Message msg = null;
                Header header = null;
                Body body = null;

                try
                {


                    if (clientSocket.Poll(MyConst.HEART_BEAT_TIME * 1000, SelectMode.SelectRead))
                    {
                        Console.WriteLine("poll~~");
                    }

                    header = Utils.readHeader(clientSocket, MyConst.msgHeaderSerailizedSize);
                    //Console.WriteLine("body size : " + header.BodySize);

                    body = Utils.readBody(clientSocket, header.BodySize);
                    //Console.WriteLine("MSG from client : " + Encoding.UTF8.GetString(body.Msg));
                    Console.WriteLine("Message : <" + Encoding.UTF8.GetString(body.Msg) + "> received from client # " + clientID);

                    Thread.Sleep(5000);

                    Utils.sendHeader(clientSocket, header);
                    Utils.sendBody(clientSocket, body);
                }
                catch(SocketException se)
                {
                    Console.WriteLine("client# " + clientID + " disconnected");
                    clientSocket.Close();
                    return;
                }
                catch(ObjectDisposedException ode)
                {
                    Console.WriteLine(ode.ToString());
                }
                
            }
        }// end method

    }// end class

    class EchoServer
    {
        public const int MAX_CONNECTIONS = 1000;
        public const int MSG_HEADER_SIZE = 4;

        private int clientID = 0;

        private Socket waitSocket;

        public Socket WaitSocket
        {
            get { return this.waitSocket; }
            //set { this.waitSocket};
        }

        // echo server constructor
        public EchoServer()
        {
        }

        public void start()
        {
            Socket waitSocket = connectSocket();
            waitSocket.Listen(MAX_CONNECTIONS);
                        
            while (true)
            {
                Socket handler = waitSocket.Accept();
                Handler clientHandler = new Handler();
                clientHandler.ClientSocket = handler;
                clientHandler.ClientID = this.clientID;

                Thread workerThread = new Thread(new ThreadStart(clientHandler.echo));
                workerThread.Start();

                Console.WriteLine("connection established with client #" + this.clientID);
                this.clientID++;
            }

            //listen
        }

        public Socket connectSocket()
        {
            IPEndPoint localEP = new IPEndPoint(IPAddress.Parse(MyConst.IP), MyConst.PORT);

            waitSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            waitSocket.Bind(localEP);
            
            //waitSocket.Close();
            if (waitSocket.IsBound)
            {
                Console.WriteLine("Server is bounded on " + MyConst.PORT);

                return waitSocket;
            }
            else
            {
                Console.WriteLine("Server bound fail");
                return null;
            }

        }

       
    }
}
