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
        public ClientState State { get; set; }
        public RelayCommand ConnectToServerCommand { get; set; }

        private Server _server;
        private string _ipAddress, _username;

        public MainViewModel()
        {
            Messages = new ObservableCollection<string>();
            _server = new Server();
            State = ClientState.Disconnected;
            PrintMessage("Enter target IP Address");
        }

        public async void DirectToCorrectMethod(string input)
        {
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
                        PrintMessage($"Connecting to {_ipAddress} with the username {_username}");
                        ConnectToServer();
                    }
                    break;

                case ClientState.Connecting:
                    break;

                case ClientState.Connected:
                    break;

                default:
                    break;
            }
        }


        public void ConnectToServer()
        {
            if (!string.IsNullOrEmpty(_ipAddress) && !string.IsNullOrEmpty(_username))
            {
                _server.ConnectToServer(_ipAddress);
            }
        }

        public void PrintMessage(string message)
        {
            Application.Current.Dispatcher.Invoke(() => Messages.Add(message));
        }
    }
}
