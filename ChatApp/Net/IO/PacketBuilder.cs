using System.IO;
using System.Text;

namespace ChatClient.Net.IO
{
    class PacketBuilder
    {
        MemoryStream _memStream;

        public PacketBuilder()
        {
            _memStream = new MemoryStream();
        }

        public void WriteOpcode(byte opCode)
        {
            _memStream.WriteByte(opCode);
        }

        public void WriteMessage(string msg)
        {
            int msgLength = msg.Length;
            _memStream.Write((ReadOnlySpan<byte>)BitConverter.GetBytes(msgLength));
            _memStream.Write((ReadOnlySpan<byte>)Encoding.ASCII.GetBytes(msg));
        }

        public byte[] GetPacketBytes()
        {
            return _memStream.ToArray();
        }
    }
}
