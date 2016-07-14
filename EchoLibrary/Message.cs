using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace EchoLibrary
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    // not used
    public class Message
    {

        Header header;
        Body body;

        public Message(String msg)
        {
            body = new Body(msg);
            header = new Header(Marshal.SizeOf(body));
        }

        public Header Header
        {
            get { return this.header; }
        }
        public Body Body
        {
            get { return this.body; }
        }
        public byte[] Serialize()
        {
            // allocate a byte array for the struct data
            var buffer = new byte[Marshal.SizeOf(typeof(Message))];

            // Allocate a GCHandle and get the array pointer
            var gch = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            var pBuffer = gch.AddrOfPinnedObject();

            // copy data from struct to array and unpin the gc pointer
            Marshal.StructureToPtr(this, pBuffer, false);
            gch.Free();

            return buffer;
        }

    }// end message



    /**
     * Message Header
     */
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public class Header
    {
        int bodySize;//in bytes

        public Header(int bodySize)
        {
            this.bodySize = bodySize;
        }

        public int BodySize
        {
            get { return this.bodySize; }
            set { this.bodySize = value; }
        }
       
    }

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
