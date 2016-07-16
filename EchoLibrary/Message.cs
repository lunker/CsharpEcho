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

}
