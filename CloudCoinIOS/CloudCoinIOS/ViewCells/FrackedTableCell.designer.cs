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

			if (lblProgress != null) {
				lblProgress.Dispose ();
				lblProgress = null;
			}

			if (viewStatus != null) {
				viewStatus.Dispose ();
				viewStatus = null;
			}
		}
	}
}
