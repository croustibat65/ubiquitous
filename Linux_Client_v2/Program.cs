using System;
using Gtk;
using WebSocketSharp;
using WebSocketSharp.Net;
using Newtonsoft.Json;
using System.IO;



namespace Linux_Client
{

	class msgJson
	{
		public string name;// = File.OpenText("id.txt").ReadToEnd();
		public string msg;
		public string type;

		// constructor
		public msgJson(string s)
		{
			name = File.OpenText("id.txt").ReadToEnd();
			msg = null;
			type = s;
		}

	}

	class MainClass
	{


		static private WebSocket client;

		static public void Main (string[] args)
		{
			Application.Init ();

		
			// Websocket Server
			client = new WebSocket ("ws://34.248.0.158:8080"); 
			// client = new WebSocket ("ws://134.206.22.71:8080"); 


			// Connexion + send ID
			client.Connect ();
			msgJson message = new msgJson("id");
			string sMsgJson = JsonConvert.SerializeObject(message);
			client.Send (sMsgJson);


			// ClipboardEvents
			// Receive Clipboard
			client.OnMessage += (sender, e) =>
				Gtk.Clipboard.Get (Gdk.Selection.Clipboard).Text = e.Data;
						
			// Send Clipboard
			Gtk.Clipboard.Get (Gdk.Selection.Clipboard).OwnerChange += onClipboardOwnerChange;



			// Run
			Application.Run ();

		}



		// Clipboard Changed
		static protected void onClipboardOwnerChange (object sender, OwnerChangeArgs e)
		{
			//Gtk.Clipboard.Get (Gdk.Selection.Clipboard);
			Gtk.Clipboard.Get (Gdk.Selection.Clipboard).RequestText(sendClipboard);

		}



		// Send Clipboard
		static void sendClipboard(Gtk.Clipboard clipboard, string s) {

			// Json
			msgJson message = new msgJson("msg");
			//message.type = "msg";
			message.msg = s;
			string sMsgJson = JsonConvert.SerializeObject(message);

			// Sending JSON
			client.Send (sMsgJson);
			Console.WriteLine("Clipboard: '" + s +"' sent");
			Console.ReadKey (true);
		}




	}
}
