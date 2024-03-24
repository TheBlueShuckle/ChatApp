using ChatClient.Net.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Net
{
    class Server
    {
        public bool IsConnected { get; set; }

        private TcpClient _client;
        private PacketBuilder _packetBuilder;

        public Server()
        {
            _client = new TcpClient();
            IsConnected = false;
        }

        public bool ConnectToServer(string ipAddress, string username)
        {
            IPAddress parsedIP;

            if (!_client.Connected && IPAddress.TryParse(ipAddress, out parsedIP))
            {
                _client.Connect(parsedIP, 7891);
                PacketBuilder connectPacket = new PacketBuilder();

                connectPacket.WriteOpCode(0);
                connectPacket.WriteMessage(username);
                _client.Client.Send(connectPacket.GetPacketBytes());

                IsConnected = _client.Connected;
            }

            return _client.Connected;
        }
    }
}
