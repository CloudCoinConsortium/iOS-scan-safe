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
    [Register ("SettingsViewController")]
    partial class SettingsViewController
    {
        [Outlet]
        UIKit.UIButton btnClose { get; set; }


        [Outlet]
        UIKit.UISwitch switchFracked { get; set; }


        [Outlet]
        UIKit.UISwitch switchZip { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnClose != null) {
                btnClose.Dispose ();
                btnClose = null;
            }

            if (switchFracked != null) {
                switchFracked.Dispose ();
                switchFracked = null;
            }

            if (switchZip != null) {
                switchZip.Dispose ();
                switchZip = null;
            }
        }
    }
}