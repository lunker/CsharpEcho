using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace EchoLibrary
{

    /*
     * Message Body 
     */
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public class Body
    {
        //int msgSize;
        byte[] msg;

        public Body(string msg)
        {
            this.msg = Encoding.UTF8.GetBytes(msg);
            //this.msgSize = msg.Length;
        }

        public byte[] Msg
        {
            get { return this.msg; }
        }
    }
}
