using Foundation;


namespace Mac_Client
{
	partial class AppController
	{
		[Outlet]
		AppKit.NSMenu statusMenu { get; set; }

		[Action("HelloWorld:")]
		partial void HelloWorld(Foundation.NSObject sender);

		void ReleaseDesignerOutlets()
		{
			if (statusMenu != null)
			{
				statusMenu.Dispose();
				statusMenu = null;
			}
		}
	}
}
