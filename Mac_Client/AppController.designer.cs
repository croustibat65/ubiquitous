// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Mac_Client
{
	partial class AppController
	{
		[Outlet]
		AppKit.NSMenu statusMenu { get; set; }

		[Action ("HelloWorld:")]
		partial void HelloWorld (Foundation.NSObject sender);

		[Action ("RUIR:")]
		partial void RUIR (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (statusMenu != null) {
				statusMenu.Dispose ();
				statusMenu = null;
			}
		}
	}
}
