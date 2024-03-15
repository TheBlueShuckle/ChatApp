using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace ChatServer
{
    class Client
    {
        public string Username { get; set; }
        public Guid UID { get; set; }
        public TcpClient ClientSocket { get; set; }

        public Client(TcpClient client)
        {
            ClientSocket = client;
            UID = new Guid();

            Console.WriteLine($"[{DateTime.Now}]: Client has connected with the username: {Username}");
        }
    }
}
