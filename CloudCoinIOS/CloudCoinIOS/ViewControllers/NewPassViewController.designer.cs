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
    [Register ("NewPassViewController")]
    partial class NewPassViewController
    {
        [Outlet]
        UIKit.UIButton btnCancel { get; set; }


        [Outlet]
        UIKit.UIButton btnOk { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint textConstraintHeight { get; set; }


        [Outlet]
        UIKit.UITextField txtCfrPassword { get; set; }


        [Outlet]
        UIKit.UITextView txtExplain { get; set; }


        [Outlet]
        UIKit.UITextField txtPassword { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnCancel != null) {
                btnCancel.Dispose ();
                btnCancel = null;
            }

            if (btnOk != null) {
                btnOk.Dispose ();
                btnOk = null;
            }

            if (txtCfrPassword != null) {
                txtCfrPassword.Dispose ();
                txtCfrPassword = null;
            }

            if (txtPassword != null) {
                txtPassword.Dispose ();
                txtPassword = null;
            }
        }
    }
}