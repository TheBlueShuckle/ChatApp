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

namespace ChatClient.MVVM.ViewModel
{
    class MainViewModel
    {
        public ObservableCollection<string> Messages { get; set; }

        public MainViewModel()
        {
            Messages = new ObservableCollection<string>();
        }

        public void AddMessageToMessages(string message)
        {
            Application.Current.Dispatcher.Invoke(() => Messages.Add(message));
        }
    }
}
