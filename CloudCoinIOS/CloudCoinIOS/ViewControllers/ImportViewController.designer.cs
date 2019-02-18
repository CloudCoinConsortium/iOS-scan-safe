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
    [Register ("ImportViewController")]
    partial class ImportViewController
    {
        [Outlet]
        UIKit.UIButton btnCancel { get; set; }


        [Outlet]
        UIKit.UIButton btnFinished { get; set; }


        [Outlet]
        UIKit.UIButton btnImport { get; set; }


        [Outlet]
        UIKit.UILabel lblImportedFiles { get; set; }


        [Action ("OnCancelTouched:")]
        partial void OnCancelTouched (Foundation.NSObject sender);

        void ReleaseDesignerOutlets ()
        {
            if (btnCancel != null) {
                btnCancel.Dispose ();
                btnCancel = null;
            }

            if (btnFinished != null) {
                btnFinished.Dispose ();
                btnFinished = null;
            }

            if (btnImport != null) {
                btnImport.Dispose ();
                btnImport = null;
            }

            if (lblImportedFiles != null) {
                lblImportedFiles.Dispose ();
                lblImportedFiles = null;
            }
        }
    }
}