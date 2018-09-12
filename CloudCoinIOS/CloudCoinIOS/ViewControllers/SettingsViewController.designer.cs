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
