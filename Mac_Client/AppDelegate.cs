using AppKit;
using System;
using WebSocketSharp;
using Newtonsoft.Json;
using Foundation;
using System.IO;


namespace Mac_Client
{





    [Register("AppDelegate")]
    public partial class AppDelegate : NSApplicationDelegate
    {
        // Constructor
		public AppDelegate()
		{
		}

		// Private Variables
		static public NSPasteboard pasteboard;// = NSPasteboard.GeneralPasteboard;
		private static string[] pboardTypes = new string[] { "NSStringPboardType" };
		static nint timeCount;
		static private WebSocket client;


        // Entry Point
		public override void DidFinishLaunching(NSNotification notification)
		{
			// Websocket Server
			//client = new WebSocket ("ws://34.248.0.158:8080"); 
			client = new WebSocket("ws://192.168.1.64:8080");

            // Connexion + send ID
            serverConnexion();

			// Pasteboard declaration
			pasteboard = NSPasteboard.GeneralPasteboard;

			// Paste/Reception
			client.OnMessage += PasteClipboard;

			// Copy/Sending
			timeCount = pasteboard.ChangeCount;
			var sampleTimer = NSTimer.CreateRepeatingScheduledTimer(TimeSpan.FromSeconds(0.5), delegate
			{
				if (timeCount != pasteboard.ChangeCount)
				{
					timeCount = pasteboard.ChangeCount;
					sendClipboard();
				}
			});

            // Losing connexion Management
            client.OnClose += ConnexionLost;
			client.OnError += ConnexionLost;


		}



        // Connect + Send ID
        public void serverConnexion()
        {
			client.Connect();
			MsgJSON message = new MsgJSON("id");
			string sMsgJson = JsonConvert.SerializeObject(message);
			client.Send(sMsgJson);

            // Connection test
            if (client.IsAlive)
            {
				connexionStatus.Title = "Status: OnLine";
				connectDisconnectTitle.Title = "Log Out";
            }
            else
            {
				connexionStatus.Title = "Status: OffLine";
				connectDisconnectTitle.Title = "Log In";
            }

        }



        // Receive clipboard
        public void PasteClipboard(object sender, MessageEventArgs e)
		{
            // Decryption
            string decryptedMsg = StringCipher.Decrypt(e.Data, MainClass.pwd);

            // Pasteboard Management
            InvokeOnMainThread(() =>
            {
				pasteboard.ClearContents();
				pasteboard.SetStringForType(decryptedMsg, pboardTypes[0]);
                timeCount = pasteboard.ChangeCount;
            });


		}


		// Send Clipboard
		public static void sendClipboard()
		{
            // Encryption of the pasteboard
            string encryptedMsg = StringCipher.Encrypt(pasteboard.GetStringForType(pboardTypes[0]), MainClass.pwd);

            // JSON
			MsgJSON message = new MsgJSON("msg");
            message.msg = encryptedMsg;
			string sMsgJson = JsonConvert.SerializeObject(message);

			// Sending 
			client.Send(sMsgJson);
		}


		// Open or close the connexion
        partial void openCloseConnexion(NSObject sender)
        {
            if (client.IsAlive)
            {
				client.Close();
			}
            else
            {
                serverConnexion();
			}
		}

        // When Connexion is Lost
        public void ConnexionLost(object sender, EventArgs e)
        {
			connexionStatus.Title = "Status: OffLine";
			connectDisconnectTitle.Title = "Log In";
		}


	}
}
