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
    [Register ("FrackedTableCell")]
    partial class FrackedTableCell
    {
        [Outlet]
        UIKit.UILabel lblProgress { get; set; }


        [Outlet]
        UIKit.UILabel lblTitle { get; set; }


        [Outlet]
        UIKit.UIView viewStatus { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (lblTitle != null) {
                lblTitle.Dispose ();
                lblTitle = null;
            }

            if (viewStatus != null) {
                viewStatus.Dispose ();
                viewStatus = null;
            }
        }
    }
}