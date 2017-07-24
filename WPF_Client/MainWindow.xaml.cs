using System;
using System.Collections.Generic;
using System.IO;
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
        private String id;
        delegate void SetClipBoard(string content);
        public MainWindow()
        {
            InitializeComponent();

            // id file read attempt
             try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader("id.txt"))
                {
                    // get id from text file
                    id = sr.ReadToEnd();
                    Console.WriteLine("Opening clipboard session with user " + id);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Id file missing !");
                throw e;
            }



            client = new WebSocket("ws://34.248.0.158:8080", "", WebSocketVersion.None);
            client.Open();
            client.MessageReceived += OnMessageReceived;
            client.Closed += Client_Closed;
            client.Error += Client_Error;
            client.Opened += Client_Opened;

            // sending id to server for prior identification

            SendId();

            //client.Opened += new EventHandler(SendSomething);

        }

        private void SendId()
        {
            client.Send( "{'type':'id','name':'" + id + "'}" );
        }

        private void SendMessage(String msg)
        {
            client.Send( "{'type':'msg','name':'" + id + "','msg':'" + msg + "'}" );
        }

        private void Client_Opened(object sender, EventArgs e)
        {
            Console.WriteLine("Client Opened");
        }

        private void Client_Error(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {
            Console.WriteLine("Client Error: " + e.Exception.Message);
            //throw e.Exception;
        }

        private void Client_Closed(object sender, EventArgs e)
        {
            Console.WriteLine("Client Closed");
        }

        private void OnMessageReceived(object o, MessageReceivedEventArgs args)
        {
            Console.WriteLine("Message received");
            if (this.text != args.Message)
            {
                Console.WriteLine("Text message: " + args.Message);
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
                SendMessage(this.text);
            }
        }
    }
}
