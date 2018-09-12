// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace CloudCoinIOS
{
    [Register ("AuthorizationViewController")]
    partial class AuthorizationViewController
    {
        [Outlet]
        UIKit.UITableView authTableView { get; set; }

        [Outlet]
        UIKit.UIButton btnCancel { get; set; }

        [Outlet]
        UIKit.UITextView textStatus { get; set; }
        
        void ReleaseDesignerOutlets ()
        {
            if (authTableView != null) {
                authTableView.Dispose ();
                authTableView = null;
            }

            if (btnCancel != null) {
                btnCancel.Dispose ();
                btnCancel = null;
            }

            if (textStatus != null) {
                textStatus.Dispose ();
                textStatus = null;
            }
        }
    }
}
