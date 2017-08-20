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
		static private WebSocket client;
		static nint timeCount;

        // Entry Point
		public override void DidFinishLaunching(NSNotification notification)
		{

			// Websocket Server
			//client = new WebSocket ("ws://34.248.0.158:8080"); 
			client = new WebSocket("ws://192.168.0.39:8080");

            // Connexion + send ID
            serverConnexion();

			// Pasteboard declaration
			pasteboard = NSPasteboard.GeneralPasteboard;

			// Paste/Reception
			client.OnMessage += PasteClipboard;

			// Copy/Sending
			timeCount = pasteboard.ChangeCount;
			var sampleTimer = NSTimer.CreateRepeatingScheduledTimer(TimeSpan.FromSeconds(0.25), delegate
			{
				if (timeCount != pasteboard.ChangeCount)
				{
					timeCount = pasteboard.ChangeCount;
					sendClipboard();
				}
			});

		}

        // Connect + Send ID
        public void serverConnexion()
        {
			client.Connect();
			MsgJSON message = new MsgJSON("id");
			string sMsgJson = JsonConvert.SerializeObject(message);
			client.Send(sMsgJson);

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
            string encryptedMsg = e.Data;
            string decryptedMsg = StringCipher.Decrypt(encryptedMsg, "pwd");

            // Pasteboard Management
            InvokeOnMainThread(() =>
            {
                pasteboard.SetStringForType(decryptedMsg, pboardTypes[0]);
                timeCount = pasteboard.ChangeCount;
            });

            // Debug
			Console.WriteLine(decryptedMsg + " received");


		}


		// Send Clipboard
		public static void sendClipboard()
		{
            // Encryption of the pasteboard
            string encryptedMsg = StringCipher.Encrypt(pasteboard.GetStringForType(pboardTypes[0]), "pwd");

            // JSON
			MsgJSON message = new MsgJSON("msg");
            message.msg = encryptedMsg;
			string sMsgJson = JsonConvert.SerializeObject(message);

			// Sending 
			client.Send(sMsgJson);
			Console.WriteLine("Clipboard: '" + pasteboard.GetStringForType(pboardTypes[0]) + "' sent");
		}

		// Open or close the connexion
        partial void openCloseConnexion(NSObject sender)
        {
            if (client.IsAlive)
            {
				client.Close();
                connexionStatus.Title = "Status: OffLine";
				connectDisconnectTitle.Title = "Log In";
			}
            else
            {
                serverConnexion();
			}

		}


	}
}
