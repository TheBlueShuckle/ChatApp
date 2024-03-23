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

        private Server _server;
        private string ipAddress, username;

        public MainViewModel()
        {
            Messages = new ObservableCollection<string>();
            _server = new Server();
            State = ClientState.Disconnected;
        }

        public async void DirectToCorrectMethod(string input)
        {
            switch (State)
            {
                case ClientState.Disconnected:
                    if (string.IsNullOrEmpty(ipAddress))
                    {
                        ipAddress = input;
                    }

                    else if (string.IsNullOrEmpty(username))
                    {
                        username = input;
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

        }

        public void AddMessageToMessages(string message)
        {
            Application.Current.Dispatcher.Invoke(() => Messages.Add(message));
        }
    }
}
