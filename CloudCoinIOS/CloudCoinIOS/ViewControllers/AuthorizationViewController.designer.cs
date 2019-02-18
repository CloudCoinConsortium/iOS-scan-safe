// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
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