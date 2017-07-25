using AppKit;
using System;
using WebSocketSharp;
using WebSocketSharp.Net;
using Foundation;


namespace Mac_Client
{
	static class MainClass
	{

		static public NSPasteboard pasteboard;

		static void Main(string[] args)
		{
			NSApplication.Init();

			// Websocket Server
			//client = new WebSocket ("ws://34.248.0.158:8080"); 
			var client = new WebSocket("ws://192.168.1.64:8080");
			client.Connect();


			// Pasteboard declaration
			pasteboard = NSPasteboard.GeneralPasteboard;
			//pasteboard.SetStringForType("test",pboardTypes[0]);

			// Paste/Reception
			client.OnMessage += PasteClipboard;

			// Copy/Sending
			var timeCount = pasteboard.ChangeCount;
			var sampleTimer = NSTimer.CreateRepeatingScheduledTimer(TimeSpan.FromSeconds(0.5), delegate
			{
				if (timeCount != pasteboard.ChangeCount)
				{
					timeCount = pasteboard.ChangeCount;
					client.Send(pasteboard.GetStringForType(pboardTypes[0]));
				}
			});


			NSApplication.Main(args);
		}

		private static void PasteClipboard(object sender, MessageEventArgs e)
		{
			Console.WriteLine(e.Data);
			pasteboard.DeclareTypes(pboardTypes, null);
			pasteboard.SetStringForType(e.Data, pboardTypes[0]);
		}

		private static string[] pboardTypes = new string[] { "NSStringPboardType" };




	}
}
