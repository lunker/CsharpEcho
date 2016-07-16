using EchoLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;


namespace EchoLibrary
{
    public static class Utils
    {
        public static Header readHeader(Socket peer, int headerSize)
        {

            byte[] buff = new byte[MyConst.maxPacketSize];

            int cnt = headerSize;

            for (; cnt > 0; cnt -= peer.Receive(buff, cnt, 0)) {; }

            Header header = null;
            header = (Header) Utils.ByteToObject(buff);

            return header;
        }

        public static Body readBody(Socket peer, int bodySize)
        {
            byte[] buff = new byte[bodySize];
            int cnt = bodySize;

            for (; cnt > 0; cnt -= peer.Receive(buff, cnt, 0)) ;
            {
                ;
            }

            Body body = null;
            body = (Body)Utils.ByteToObject(buff);

            return body;
        }

        public static int sendHeader(Socket peer, Header header)
        {

            int byteSent = 0;
            byteSent = peer.Send(ObjectToByte(header));

            return byteSent;
        }

        public static int sendBody(Socket peer, Body body)
        {

            int byteSent = 0;
            byteSent = peer.Send(ObjectToByte(body));

            return byteSent;
        }

        public static Object readMessage(Socket peer, int size)
        {
            byte[] buff = new byte[size];
            Object msg = null;
            int cnt = 0;

            //for (; cnt > 0; cnt -= peer.Receive(buff, cnt, 0)) {; }
            cnt = peer.Receive(buff, size, 0);
            
            msg = Utils.ByteToObject(buff);

            return msg;
        }

        public static int sendMessage(Socket peer, Object obj)
        {
            int byteSent = 0;
            byteSent = peer.Send(ObjectToByte(obj));

            return byteSent;
        }
        
        public static byte[] ObjectToByte(object obj)
        {
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    binaryFormatter.Serialize(stream, obj);
                    return stream.ToArray();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }

            return null;
        }

        public static object ByteToObject(byte[] buffer)
        {
            try
            {
                using (MemoryStream stream = new MemoryStream(buffer))
                {

                    BinaryFormatter binForm = new BinaryFormatter();
                    stream.Write(buffer, 0, buffer.Length);
                    stream.Seek(0, SeekOrigin.Begin);
                    Object obj = (Object)binForm.Deserialize(stream);
                    return obj;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }

            return null;

        }

    }
}
