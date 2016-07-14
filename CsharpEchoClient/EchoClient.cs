using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using EchoLibrary;
using System.Runtime.InteropServices;

namespace CsharpEchoClient
{
    class EchoClient
    {
        static void Main(string[] args)
        {
            Client client = new Client();
            client.start();
        }
    }

    class Client
    {
        private Socket peer;
        public const String IP = "127.0.0.1";
        public const int PORT = 9999;
        public const int HEADER_SIZE = 4;

        public Client()
        {
            ;
        }

        public Socket Peer
        {
            get { return this.peer; }
        }


        public void start()
        {

            String inputMsg = "";

            if (connect())
            {
                Console.WriteLine("서버 연결 완료");
            }
            
            while (true)
            {

                Console.Write("메세지를 입력하세요 : ");
                
                inputMsg = Console.ReadLine();


                // generate body
                Body body = new Body(inputMsg);
                byte[] bodyBytes = Utils.ObjectToByte(body);

                // generate header
                Header header = new Header(bodyBytes.Length);
                byte[] headerBytes = Utils.ObjectToByte(header);

                
                peer.Send(headerBytes);
                peer.Send(bodyBytes);

                // read header from buff
                Header echoHeader = Utils.readHeader(this.Peer, MyConst.msgHeaderSerailizedSize);
                // read body from buff
                Body echoBody = Utils.readBody(peer, echoHeader.BodySize);


                // print message
                Console.WriteLine("[Client] : " + Encoding.UTF8.GetString(body.Msg));
                Console.WriteLine("[Echo Server] : " + Encoding.UTF8.GetString(echoBody.Msg));
                Console.WriteLine("----------------------------------------------\n\n");
              
            }// end while
        }// end method

        public Boolean connect()
        {
            IPEndPoint peerEP = new IPEndPoint(IPAddress.Parse(IP),MyConst.port);


            peer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            peer.Connect(peerEP);
            if (peer.Connected)
                return true;
            else
                return false;
        }// end method

    }// end client class
}// end namespace
