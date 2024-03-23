using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Net
{
    class Server
    {
        public bool IsConnected { get; set; }

        private TcpClient _client;

        public Server()
        {
            _client = new TcpClient();
            IsConnected = false;
        }

        public void ConnectToServer(string ipAdress)
        {
            if (!_client.Connected)
            {
                _client.Connect(ipAdress, 7891);

                IsConnected = _client.Connected;
            }
        }
    }
}
