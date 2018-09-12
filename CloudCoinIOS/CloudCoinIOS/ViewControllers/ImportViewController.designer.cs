// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
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
			if (btnFinished != null) {
				btnFinished.Dispose ();
				btnFinished = null;
			}

			if (btnCancel != null) {
				btnCancel.Dispose ();
				btnCancel = null;
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
