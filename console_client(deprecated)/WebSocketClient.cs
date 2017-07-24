using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocket4Net;

namespace console_client
{
    class WebSocketClient
    {
        WebSocket websocket;
        string url = "ws://localhost:8080/";
        public List<Func<string, bool>> messageListeners;

        public WebSocketClient()
        {
            Connect();
        }
        public WebSocketClient(string url)
        {
            this.url = url;
            Connect();
        }
        public void SendMessage(string message)
        {
            websocket.Send(message);
        }
        private void Connect()
        {
            websocket = new WebSocket(url);
            websocket.Opened += new EventHandler(websocket_Opened);
            websocket.Error += new EventHandler<SuperSocket.ClientEngine.ErrorEventArgs>(websocket_Error);
            websocket.Closed += new EventHandler(websocket_Closed);
            websocket.MessageReceived += new EventHandler<MessageReceivedEventArgs>(websocket_MessageReceived);
            websocket.Open();
        }
        private void websocket_Opened(object sender, EventArgs e)
        {
            Console.WriteLine("Connection opened");
        }
        private void websocket_Closed(object sender, EventArgs e)
        {
            Console.WriteLine("Connection closed");
        }
        private void websocket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            Console.WriteLine("Message received: " + e.Message);
            messageListeners.ForEach(m => m(e.Message));
        }
        private void websocket_Error(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {
            throw e.Exception;
        }
    }
}
