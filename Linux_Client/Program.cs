using System;
using Gtk;
using WebSocketSharp;
using WebSocketSharp.Net;



namespace Linux_Client
{
	class MainClass
	{

		static private WebSocket client;

		static public void Main (string[] args)
		{
			Application.Init ();

			// Websocket Server
			client = new WebSocket ("ws://192.168.0.24:8080"); //192.168.43.107
			client.Connect ();

			// Events
			// Receive Clipboard
			client.OnMessage += (sender, e) =>
				Gtk.Clipboard.Get (Gdk.Selection.Clipboard).Text = e.Data;
			//Console.WriteLine("Test reception: " + e.Data); // Test reception


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
			Console.WriteLine("Clipboard: '" + s +"' sent");
			client.Send (s);
			Console.ReadKey (true);
		}

		// // Websocket Changed
		// static protected void onWebSocketChange (object sender, OwnerChangeArgs e)
		// {
		// 	//Gtk.Clipboard.Get (Gdk.Selection.Clipboard);
		// 	Gtk.Clipboard.Get (Gdk.Selection.Clipboard).Text = "prout";
		// }





	}
}
