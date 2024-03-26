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
                    connectPacket.WriteOpCode(0);
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
                    byte opCode = PacketReader.ReadByte();
                    switch (opCode)
                    {
                        case 1:
                            connectEvent?.Invoke();
                            break;

                        case 5:
                            messageRecievedEvent?.Invoke();
                            break;

                        case 10:
                            userDisconnectEvent?.Invoke();
                            break;

                        default:
                            Console.WriteLine("Ahh yes of course...");
                            break;
                    }
                }
            });
        }

        public void SendMessageToServer(string msg)
        {
            PacketBuilder messagePacket = new PacketBuilder();
            messagePacket.WriteOpCode(5);
            messagePacket.WriteMessage(msg);
            _client.Client.Send(messagePacket.GetPacketBytes());
        }
    }
}
