using ChatServer.Net.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    class Program
    {
        static private List<Client> _users;
        static private TcpListener _listener;

        static void Main(string[] args)
        {
            _users = new List<Client>();
            _listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 7891);
            _listener.Start();

            while (true)
            {
                _users.Add(new Client(_listener.AcceptTcpClient()));
                BroadcastConnection();
            }
        }


        static void BroadcastConnection()
        {
            foreach (Client user in _users)
            {
                foreach (Client usr in _users)
                {
                    PacketBuilder broadcastPacket = new PacketBuilder();
                    broadcastPacket.WriteOpCode(1);
                    broadcastPacket.WriteMessage(usr.Username);
                    broadcastPacket.WriteMessage(usr.UID.ToString());
                    user.ClientSocket.Client.Send(broadcastPacket.GetPacketBytes());
                }
            }
        }

        public static void BroadcastMessage(string message)
        {
            foreach(Client user in _users)
            {
                PacketBuilder messagePacket = new PacketBuilder();
                messagePacket.WriteOpCode(5);
                messagePacket.WriteMessage(message);
                user.ClientSocket.Client.Send(messagePacket.GetPacketBytes());
            }
        }

        public static void BroadcastDisconnect(string uid)
        {
            Client disconnectedUser = _users.Where(x => x.UID.ToString() == uid).FirstOrDefault();
            _users.Remove(disconnectedUser);

            foreach (Client user in _users)
            {
                PacketBuilder disconnectPacket = new PacketBuilder();
                disconnectPacket.WriteOpCode(10);
                disconnectPacket.WriteMessage(uid);
                user.ClientSocket.Client.Send(disconnectPacket.GetPacketBytes());
            }

            BroadcastMessage($"{disconnectedUser.Username} has disconnected.");
        }
    }
}
