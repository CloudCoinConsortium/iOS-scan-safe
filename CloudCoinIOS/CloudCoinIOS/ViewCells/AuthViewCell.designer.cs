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
    [Register ("AuthViewCell")]
    partial class AuthViewCell
    {
        [Outlet]
        UIKit.UILabel lblAuthenticated { get; set; }


        [Outlet]
        UIKit.UILabel lblPercent { get; set; }


        [Outlet]
        UIKit.UILabel lblSerial { get; set; }


        [Outlet]
        UIKit.UILabel lblValue { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (lblAuthenticated != null) {
                lblAuthenticated.Dispose ();
                lblAuthenticated = null;
            }

            if (lblPercent != null) {
                lblPercent.Dispose ();
                lblPercent = null;
            }

            if (lblSerial != null) {
                lblSerial.Dispose ();
                lblSerial = null;
            }

            if (lblValue != null) {
                lblValue.Dispose ();
                lblValue = null;
            }
        }
    }
}