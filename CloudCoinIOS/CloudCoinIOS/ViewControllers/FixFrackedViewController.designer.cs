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
	[Register ("FixFrackedViewController")]
	partial class FixFrackedViewController
	{
		[Outlet]
		UIKit.UIButton btnClose { get; set; }

		[Outlet]
		UIKit.UITableView frackedTableView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (frackedTableView != null) {
				frackedTableView.Dispose ();
				frackedTableView = null;
			}

			if (btnClose != null) {
				btnClose.Dispose ();
				btnClose = null;
			}
		}
	}
}
