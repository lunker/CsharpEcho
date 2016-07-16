using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpEchoClient
{
    class Program
    {
        static void Main(string[] args)
        {
            EchoClient client = new EchoClient();
            client.start();

            HeartCheck checker = new HeartCheck();
          
        }
    }
}
