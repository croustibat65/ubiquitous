using System;
using Gtk;
using WebSocketSharp;
using Newtonsoft.Json;
using System.IO;



namespace Linux_Client
{


	class MainClass
	{


		static private WebSocket client;
		static string pwd;



		static public void Main (string[] args)
		{
			Application.Init ();

			// Open Password Information
			StreamReader Sr = File.OpenText("id.txt");
			Sr.ReadLine(); pwd = Sr.ReadLine();

		
			// Websocket Server
			// client = new WebSocket ("ws://34.248.0.158:8080");
			client = new WebSocket("ws://192.168.0.39:8080");
            // client = new WebSocket ("ws://134.206.22.71:8080"); 

			// Connexion + send ID
			serverConnexion();

            // ClipboardEvents
            // Receive Clipboard
            //client.OnMessage += (sender, e) =>
            //Gtk.Clipboard.Get (Gdk.Selection.Clipboard).Text = e.Data;
            client.OnMessage += receiveClipboard;
						

			// Send Clipboard
			Gtk.Clipboard.Get (Gdk.Selection.Clipboard).OwnerChange += onClipboardOwnerChange;

			// Run
			Application.Run ();

		}


        // Server Connexion
        static void serverConnexion()
        {
			client.Connect();
			msgJSON message = new msgJSON("id");
			string sMsgJson = JsonConvert.SerializeObject(message);
			client.Send(sMsgJson);
		}



		// Clipboard Changed
		static protected void onClipboardOwnerChange (object sender, OwnerChangeArgs e)
		{
			//Gtk.Clipboard.Get (Gdk.Selection.Clipboard);
			Gtk.Clipboard.Get (Gdk.Selection.Clipboard).RequestText(sendClipboard);

		}



		// Send Clipboard
		static void sendClipboard(Gtk.Clipboard clipboard, string s) {

			// Encryption
			string encryptedMsg = StringCipher.Encrypt(s, pwd);

			// Json
			msgJSON message = new msgJSON("msg");
            message.msg = encryptedMsg;
			string sMsgJson = JsonConvert.SerializeObject(message);

			// Sending JSON
			client.Send (sMsgJson);
			Console.WriteLine("Clipboard: '" + s +"' sent");
			Console.ReadKey (true);
		}


        // Receive Clipboard
        static protected void receiveClipboard(object sender, MessageEventArgs e)
        {
			// Decrypt Message
			string decryptedMsg = StringCipher.Decrypt(e.Data, pwd);

            // Set decrypted message as clipboard
            Gtk.Clipboard.Get(Gdk.Selection.Clipboard).Text = decryptedMsg;
        }



	}
}
