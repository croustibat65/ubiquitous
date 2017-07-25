using AppKit;
using System;
using WebSocketSharp;
using WebSocketSharp.Net;
using Newtonsoft.Json;
using Foundation;
using System.IO;


namespace Mac_Client
{

	class msgJson
	{
		public string name;// = File.OpenText("id.txt").ReadToEnd();
		public string msg;
		public string type;

		// constructor
		public msgJson(string s)
		{
            name = File.OpenText("../../../../../id.txt").ReadToEnd();
			msg = null;
			type = s;
		}

	}


	static class MainClass
	{

        // Private Variables
        static public NSPasteboard pasteboard;// = NSPasteboard.GeneralPasteboard;
		private static string[] pboardTypes = new string[] { "NSStringPboardType" };
		static private WebSocket client;



		static void Main(string[] args)
		{
			NSApplication.Init();

			// Websocket Server
			//client = new WebSocket ("ws://34.248.0.158:8080"); 
			client = new WebSocket("ws://192.168.1.64:8080");

			// Connexion + send ID
			client.Connect();
			msgJson message = new msgJson("id");
			string sMsgJson = JsonConvert.SerializeObject(message);
			client.Send(sMsgJson);

			// Pasteboard declaration
			pasteboard = NSPasteboard.GeneralPasteboard;
			pasteboard.SetStringForType("test",pboardTypes[0]);

			// Paste/Reception
			client.OnMessage += PasteClipboard;

			// Copy/Sending
			var timeCount = pasteboard.ChangeCount;
			var sampleTimer = NSTimer.CreateRepeatingScheduledTimer(TimeSpan.FromSeconds(0.5), delegate
			{
				if (timeCount != pasteboard.ChangeCount)
				{
					timeCount = pasteboard.ChangeCount;
					sendClipboard();
				}
			});


			NSApplication.Main(args);
		}

		private static void PasteClipboard(object sender, MessageEventArgs e)
		{
			Console.WriteLine(e.Data);
			//pasteboard.DeclareTypes(pboardTypes, null);
			pasteboard.SetStringForType(e.Data, pboardTypes[0]);
		}


		// Send Clipboard
		static void sendClipboard()
		{

			// Json
			msgJson message = new msgJson("msg");
			//message.type = "msg";
            message.msg = pasteboard.GetStringForType(pboardTypes[0]);
			string sMsgJson = JsonConvert.SerializeObject(message);

			// Sending JSON
			client.Send(sMsgJson);
            Console.WriteLine("Clipboard: '" + pasteboard.GetStringForType(pboardTypes[0]) + "' sent");
		}





	}
}
