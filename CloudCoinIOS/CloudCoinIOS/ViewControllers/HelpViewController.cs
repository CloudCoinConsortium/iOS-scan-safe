// This file has been autogenerated from a class added in the UI designer.

using System;

using Foundation;
using UIKit;

namespace CloudCoinIOS
{
	public partial class HelpViewController : BaseFormSheet
	{
		public HelpViewController (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            btnClose.TouchUpInside += (sender, e) => {
                RemoveAnimate();
            };
        }
	}
}