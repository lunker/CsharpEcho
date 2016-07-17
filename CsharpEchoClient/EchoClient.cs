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
        private Socket peer=null;
        private int HEART_BEATS = 0;

        public EchoClient()
        {
         
        }

        public Socket Peer
        {
            get { return this.peer; }
        }

        /*
         * connect to server
         */
        public Boolean connect()
        {
            IPEndPoint peerEP = null;
            peerEP = new IPEndPoint(IPAddress.Parse(MyConst.IP), MyConst.PORT);

            if (peer == null || !peer.Connected)
            {
                try
                {
                    peer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                }
                catch (SocketException se)
                {
                    Console.WriteLine("socket create error");
                }
            }

            try
            {
                peer.Connect(peerEP);
                if (peer.Connected)
                    return true;
                else
                    return false;
            }
            catch(SocketException e)
            {
                //Console.WriteLine(e.StackTrace);
                //Console.WriteLine("Server is closed");
                return false;
            }

        }// end method
      
        /*
         * read message from stdin
         */
        public string getStr()
        {
            string str = "";
            Console.Write("메세지를 입력하세요 : ");
            str = Console.ReadLine();
            return str;
        }
        
        /*
         * send message(header, body) to server
         */
        public Boolean send(short type, string msg)
        {
            // generate body
            Body body = new Body(msg);
            byte[] bodyBytes = Utils.ObjectToByte(body);

            // generate header
            Header header = new Header(type, bodyBytes.Length);

            try
            {
                Console.WriteLine("new header size : " + Utils.ObjectToByte(header).Length);
                Utils.sendMessage(peer, header);
                Utils.sendMessage(peer, body);
            }
            catch (SocketException se)
            {
                Console.WriteLine("send failed");
                return false;
            }

            Console.WriteLine("[Client] : "+ Encoding.UTF8.GetString(body.Msg));
            return true;
        }

        /*
         * generate message(header, body)
         * read message to server
         */
        public Boolean read()
        {
            Header echoHeader = null;
            Body echoBody = null;
            
            try
            {
                echoHeader = (Header)Utils.readMessage(this.Peer, MyConst.msgHeaderSerailizedSize);
                // read body from buff
                echoBody = (Body)Utils.readMessage(peer, echoHeader.BodySize);

                Console.WriteLine("[Echo Server] : " + Encoding.UTF8.GetString(echoBody.Msg));
                Console.WriteLine("----------------------------------------------\n\n");
            }
            catch (SocketException se)
            {
                //Console.WriteLine(se.StackTrace);
                return false;
            }

            return true;
        }
        
        /*
         * start client 
         */
        public void start()
        {
            while (true)
            {
                while (!connect())
                {
                    Console.WriteLine("Server is closed");
                    Console.WriteLine("try reconnect . . .");
                }
                Console.WriteLine("서버 연결 완료");

                while (true)
                {
                    // check tcp connection 
                    // poll 대신에 heart-beat을 보내야 한다 !!! 
                    //Header header
                    /*
                    if (!peer.Poll(MyConst.HEART_BEAT_TIME*1000, SelectMode.SelectWrite))
                    {
                        HEART_BEATS++;
                        Console.WriteLine("heart beat!!");
                        if (HEART_BEATS == 3)
                        {
                            break;
                        }
                        continue;
                    }
                    HEART_BEATS = 0;
                    */

                    String strMsg = getStr();

                    if (!send(MyConst.MSG_PLAIN, strMsg))
                    {
                        peer.Close();
                        break;
                    }
                        
                    if (!read())
                    {
                        peer.Close();
                        break;
                    }
                }// end while
            }
        }// end method
    }// end client class
}// end namespace
