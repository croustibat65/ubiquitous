using AppKit;
using System;
using WebSocketSharp;
using Newtonsoft.Json;
using Foundation;
using System.IO;


namespace Mac_Client
{


	public class msgJson
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
		static void Main(string[] args)
		{
			NSApplication.Init();
   			NSApplication.Main(args);
		}	
	}


}
