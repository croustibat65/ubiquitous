using System;
using Foundation;
using AppKit;

namespace Mac_Client
{
	[Register("AppController")]
	public partial class AppController : NSObject
	{
		public AppController()
		{

		}

		public override void AwakeFromNib()
		{
			var statusItem = NSStatusBar.SystemStatusBar.CreateStatusItem(30);
			statusItem.Menu = statusMenu;
			statusItem.Image = NSImage.ImageNamed("dino_icon");
			statusItem.HighlightMode = true;
		}

		partial void HelloWorld(NSObject sender)
		{
			Console.WriteLine("hello world");
		}
	}
}

