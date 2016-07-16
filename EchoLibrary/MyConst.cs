using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoLibrary
{
    public class MyConst
    {
        public const String IP = "127.0.0.1";
        public const int PORT= 9998;
        public const int msgHeaderSize = 4;
        public const int maxPacketSize = 1500;
        public const int msgHeaderSerailizedSize = 137;
        public const short MSG_PLAIN = 100;
        public const short MSG_HEART_BEAT = 200;

        public const short HEART_BEAT_TIME = 1; // 2 SECONDS
    }
}
