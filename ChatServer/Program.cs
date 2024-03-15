using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    internal class Program
    {
        static private List<Client> users;
        static private TcpListener _listener;

        static void Main(string[] args)
        {
            users = new List<Client>();
            _listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 7891);
            _listener.Start();

            Client _client = new Client(_listener.AcceptTcpClient());
        }
    }
}
