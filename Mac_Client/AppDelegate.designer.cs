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
    partial class AppDelegate
    {
        [Outlet]
        AppKit.NSMenuItem connectDisconnectTitle { get; set; }

        [Outlet]
        AppKit.NSMenuItem connexionStatus { get; set; }

        [Action ("openCloseConnexion:")]
        partial void openCloseConnexion (Foundation.NSObject sender);
        
        void ReleaseDesignerOutlets ()
        {
            if (connexionStatus != null) {
                connexionStatus.Dispose ();
                connexionStatus = null;
            }

            if (connectDisconnectTitle != null) {
                connectDisconnectTitle.Dispose ();
                connectDisconnectTitle = null;
            }
        }
    }
}
