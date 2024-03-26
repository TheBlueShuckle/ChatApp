using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using ChatServer.Net.IO;

namespace ChatServer
{
    class Client
    {
        public string Username { get; set; }
        public Guid UID { get; set; }
        public TcpClient ClientSocket { get; set; }

        private PacketReader _packetReader;

        public Client(TcpClient client)
        {
            ClientSocket = client;
            UID = Guid.NewGuid();
            _packetReader = new PacketReader(ClientSocket.GetStream());
            
            byte opCode = _packetReader.ReadByte();
            Username = _packetReader.ReadMessage();

            Console.WriteLine($"[{DateTime.Now}]: Client has connected with the username: {Username}");

            Task.Run(Process);
        }

        // Runs until client disconnects
        private void Process()
        {
            while (true)
            {
                try
                {
                    byte opCode = _packetReader.ReadByte();
                    switch (opCode)
                    {
                        case 5:
                            string message = _packetReader.ReadMessage();
                            Console.WriteLine($"[{DateTime.Now}]: Message recieved: {message}");
                            Program.BroadcastMessage($"[{TimeOnly.FromDateTime(DateTime.Now)}] {Username}: {message}");
                            break;

                        default:
                            break;
                    }
                }

                catch (Exception)
                {
                    Console.WriteLine($"[{UID}]: Disconnected!");
                    ClientSocket.Close();
                    Program.BroadcastDisconnect(UID.ToString());
                    break;
                }
            }
        }
    }
}
