using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace EchoLibrary
{

    /**
     * Message Header
     */
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public class Header
    {
        short type;
        int bodySize;//in bytes

        public Header()
        {

        }

        public Header(int bodySize)
        {
            this.bodySize = bodySize;
        }

        public Header(short type, int bodySize)
        {
            this.type = type;
            this.bodySize = bodySize;
        }

        public int BodySize
        {
            get { return this.bodySize; }
            set { this.bodySize = value; }
        }
        
        public short Type
        {
            get { return this.type; }
            set { this.type = value; }
        }  
        
    }

}
