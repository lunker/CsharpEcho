using EchoLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CsharpEchoClient
{
    class HeartCheck
    {
        private Socket clientSocket = null;
        public void start()
        {
            Thread th = new Thread(new ThreadStart(heartBeat));


        }

        public void heartBeat()
        {
            // 5초마다 보낸다.
            Header header = new Header();


            clientSocket.Send(Utils.sendMessage());
        }
    
    }
}
