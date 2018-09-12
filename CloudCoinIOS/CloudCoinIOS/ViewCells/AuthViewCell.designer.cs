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
			if (lblSerial != null) {
				lblSerial.Dispose ();
				lblSerial = null;
			}

			if (lblValue != null) {
				lblValue.Dispose ();
				lblValue = null;
			}

			if (lblAuthenticated != null) {
				lblAuthenticated.Dispose ();
				lblAuthenticated = null;
			}

			if (lblPercent != null) {
				lblPercent.Dispose ();
				lblPercent = null;
			}
		}
	}
}
