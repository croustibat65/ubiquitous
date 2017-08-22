using System;
using Foundation;
using AppKit;

namespace Mac_Client
{
	[Register("AppController")]
	public partial class AppController : NSObject
	{

        static public NSStatusItem statusItem = NSStatusBar.SystemStatusBar.CreateStatusItem(30);

		public AppController()
		{
		}

		public override void AwakeFromNib()
		{
			statusItem.Menu = statusMenu;
			statusItem.Image = NSImage.ImageNamed("dino_icon");
			statusItem.HighlightMode = true;
		}

	}
}

