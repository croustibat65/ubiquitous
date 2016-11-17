using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WebSocket4Net;


namespace WPF_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WebSocket client;
        private string text;
        delegate void SetClipBoard(string content);
        public MainWindow()
        {
            InitializeComponent();
            client = new WebSocket("ws://192.168.1.19:8080", "", WebSocketVersion.None);
            client.Open();
            client.MessageReceived += OnMessageReceived;
            //client.Opened += new EventHandler(SendSomething);

        }

        private void OnMessageReceived(object o, MessageReceivedEventArgs args)
        {
            if (this.text != args.Message)
            {
                Dispatcher.BeginInvoke((SetClipBoard) delegate(string message) {
                   Clipboard.SetText(args.Message);
                }, args.Message);
            }
        }
        private void SendSomething(object o, EventArgs args)
        {
            client.Send("hey joe");
        }
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            // Initialize the clipboard now that we have a window soruce to use
            var windowClipboardManager = new ClipboardManager(this);
            windowClipboardManager.ClipboardChanged += ClipboardChanged;
        }
        private void ClipboardChanged(object sender, EventArgs e)
        {
            // Handle your clipboard update here, debug logging example:
            if (Clipboard.ContainsText())
            {
                this.text = Clipboard.GetText();
                client.Send(this.text);
            }
        }
    }
}
