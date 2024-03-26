using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows;
using ChatClient.MVVM.Core;
using ChatClient.Net;
using System.Globalization;
using System.Security.Cryptography;
using ChatClient.MVVM.Model;

namespace ChatClient.MVVM.ViewModel
{
    public enum ClientState
    {
        Disconnected,
        Connecting,
        Connected,
    }

    class MainViewModel
    {
        public ObservableCollection<string> Messages { get; set; }
        public ObservableCollection<UserModel> Users { get; set; }
        public ClientState State { get; set; }
        public RelayCommand ConnectToServerCommand { get; set; }

        private Server _server;
        private string _ipAddress, _username;

        public MainViewModel()
        {
            Messages = new ObservableCollection<string>();
            Users = new ObservableCollection<UserModel>();
            _server = new Server();

            _server.connectEvent += UserConnected;
            State = ClientState.Disconnected;
            PrintMessage("Enter target IP Address");
        }

        public async void DirectToCorrectMethod(string input)
        {
            PrintMessage(input);

            switch (State)
            {
                case ClientState.Disconnected:
                    if (string.IsNullOrEmpty(_ipAddress))
                    {
                        _ipAddress = input;
                        PrintMessage("Enter Username:");
                    }

                    else if (string.IsNullOrEmpty(_username))
                    {
                        _username = input;
                    }

                    if (!string.IsNullOrEmpty(_ipAddress) && !string.IsNullOrEmpty(_username))
                    {
                        if (_server.ConnectToServer(_ipAddress, _username))
                        {
                            PrintMessage($"Connecting to {_ipAddress} with the username {_username}");
                            State = ClientState.Connected;
                        }

                        else
                        {
                            PrintMessage($"Could not connect to server with the IP Address {_ipAddress}");
                        }
                    }

                    break;

                case ClientState.Connected:
                    break;

                default:
                    break;
            }
        }

        public void PrintMessage(string message)
        {
            Application.Current.Dispatcher.Invoke(() => Messages.Add(message));
        }

        private void UserConnected()
        {
            var user = new UserModel
            {
                Username = _server.PacketReader.ReadMessage(),
                UID = _server.PacketReader.ReadMessage(),
            };

            if (!Users.Any(x => x.UID == user.UID))
            {
                Application.Current.Dispatcher.Invoke(() => Users.Add(user));

                PrintMessage($"User {user.Username} has connected.");
            }
        }
    }
}
