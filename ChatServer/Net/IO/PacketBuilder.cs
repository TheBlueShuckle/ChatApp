using System;
using System.IO;
using System.Text;

namespace ChatServer.Net.IO
{
    class PacketBuilder
    {
        MemoryStream _memStream;

        public PacketBuilder()
        {
            _memStream = new MemoryStream();
        }

        public void WriteOpCode(byte opCode)
        {
            _memStream.WriteByte(opCode);
        }

        public void WriteMessage(string msg)
        {
            int msgLength = msg.Length;
            _memStream.Write(BitConverter.GetBytes(msgLength));
            _memStream.Write(Encoding.ASCII.GetBytes(msg));
        }

        public byte[] GetPacketBytes()
        {
            return _memStream.ToArray();
        }
    }
}
