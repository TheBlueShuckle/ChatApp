using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Net.IO
{
    class PacketReader : BinaryReader
    {
        private NetworkStream _netStream;

        public PacketReader(NetworkStream netStream) : base(netStream)
        {
            _netStream = netStream;
        }

        public string ReadMessage()
        {
            byte[] msgBuffer;
            int length = ReadInt32();
            msgBuffer = new byte[length];

            _netStream.Read(msgBuffer, 0, length);

            string msg = Encoding.ASCII.GetString(msgBuffer);

            return msg;
        }
    }
}
