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
        private const int UserConnected = 1, MessageRecieved = 5, UserDisconnected = 10;

        public bool IsConnected { get; set; }

        private TcpClient _client;
        public PacketReader PacketReader;

        public event Action connectEvent;
        public event Action messageRecievedEvent;
        public event Action userDisconnectEvent;

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
                PacketReader = new PacketReader(_client.GetStream());

                if (!string.IsNullOrEmpty(username))
                {
                    PacketBuilder connectPacket = new PacketBuilder();
                    connectPacket.WriteOpcode(0);
                    connectPacket.WriteMessage(username);
                    _client.Client.Send(connectPacket.GetPacketBytes());

                    IsConnected = _client.Connected;
                }

                ReadPackets();
            }

            return _client.Connected;
        }

        private void ReadPackets()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    byte opcode = PacketReader.ReadByte();
                    switch (opcode)
                    {
                        case UserConnected:
                            connectEvent?.Invoke();
                            break;

                        case MessageRecieved:
                            messageRecievedEvent?.Invoke();
                            break;

                        case UserDisconnected:
                            userDisconnectEvent?.Invoke();
                            break;

                        default:
                            Console.WriteLine("Ahh yes of course...");
                            break;
                    }
                }
            });
        }

        // Sends message to server. 
        // Specifically for messages from the user, not disconnect or connect messages.
        public void SendMessageToServer(string msg)
        {
            PacketBuilder messagePacket = new PacketBuilder();
            messagePacket.WriteOpcode(5);
            messagePacket.WriteMessage(msg);
            _client.Client.Send(messagePacket.GetPacketBytes());
        }
    }
}
