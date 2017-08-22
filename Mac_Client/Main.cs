using AppKit;
using System;
using WebSocketSharp;
using Newtonsoft.Json;
using Foundation;
using System.IO;


namespace Mac_Client
{
	static class MainClass
	{

		public static string id, pwd;


		static void Main(string[] args)
		{
			NSApplication.Init();

            // Reading id and pwd
            StreamReader Sr = File.OpenText("id.txt");
            id  = Sr.ReadLine();
			pwd = Sr.ReadLine();


			NSApplication.Main(args);
		}	
	}


}
