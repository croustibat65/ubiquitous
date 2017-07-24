using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace console_client
{
    class Program
    {
        static void Main(string[] args)
        {
            bool quit = false;
            string port = "8000";
            string srvUrl = "ws://localhost:8080";
            if (args.Length > 0)
            {
                Console.WriteLine("Port: " + args[0]);
                port = args[0];
            }

            WebSocketClient wsc = new WebSocketClient(srvUrl);
            wsc.messageListeners.Add( m => {
                Console.WriteLine(m);
                return true;
                });


            while (!quit)
            {
            }
        }

        private async void awaitClipboardEvent(DataPackageView dpv)
        {
            if (dpv.Contains(StandardDataFormats.Text))
            {
                string text = await dpv.GetTextAsync();
                // To output the text from this example, you need a TextBlock control
            }
        }

    }
}
