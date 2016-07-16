using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpEchoServer
{
    class Program
    {
        static void Main(string[] args)
        {
            EchoServer echoServer = new EchoServer();
            echoServer.start();
        }
    }
}
