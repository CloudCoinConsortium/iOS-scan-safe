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
    [Register ("MainViewController")]
    partial class MainViewController
    {
        [Outlet]
        UIKit.UIView bankView { get; set; }

        [Outlet]
        UIKit.UIView exportView { get; set; }

        [Outlet]
        UIKit.UIView importView { get; set; }

        [Outlet]
        UIKit.UIView mainView { get; set; }

        [Action ("OnBankTouched:")]
        partial void OnBankTouched (Foundation.NSObject sender);

        [Action ("OnExportTouched:")]
        partial void OnExportTouched (Foundation.NSObject sender);

        [Action ("OnHelpTouched:")]
        partial void OnHelpTouched (Foundation.NSObject sender);

        [Action ("OnImportTouched:")]
        partial void OnImportTouched (Foundation.NSObject sender);

        [Action ("OnSettingTouched:")]
        partial void OnSettingTouched (Foundation.NSObject sender);
        
        void ReleaseDesignerOutlets ()
        {
            if (bankView != null) {
                bankView.Dispose ();
                bankView = null;
            }

            if (exportView != null) {
                exportView.Dispose ();
                exportView = null;
            }

            if (importView != null) {
                importView.Dispose ();
                importView = null;
            }

            if (mainView != null) {
                mainView.Dispose ();
                mainView = null;
            }
        }
    }
}
