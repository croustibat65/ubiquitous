using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WebSocket4Net;


namespace WPF_Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// 

    public partial class App : Application
    {
        private WebSocket client;
        public App()
        {
            client = new WebSocket("ws://127.0.0.1:8080", "", WebSocketVersion.None);
            client.Open();
            client.Opened += new EventHandler(SendSomething);
        }
        private void SendSomething(object o, EventArgs args)
        {
           client.Send("hey joe");
        }
    }
}
