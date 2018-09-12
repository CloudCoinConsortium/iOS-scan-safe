using System;
using System.Threading.Tasks;
using Foundation;
using UIKit;

namespace CloudCoinIOS
{
	public class BaseFormSheet : UIViewController
	{
		
		public BaseFormSheet()
		{
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, KeyboardDidHide);
			NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, KeyboardDidShow);
		}

		private void KeyboardDidShow(NSNotification notification)
		{
			var rect = UIKeyboard.FrameEndFromNotification(notification);
			var diff = (View.Frame.Height - View.Subviews[0].Frame.Height) / 2f;

			if (diff < rect.Height)
				View.Frame = new CoreGraphics.CGRect(0, diff - rect.Height, View.Frame.Size.Width, View.Frame.Size.Height);
		}

		private void KeyboardDidHide(NSNotification notification)
		{
			View.Frame = new CoreGraphics.CGRect(0, 0, View.Frame.Size.Width, View.Frame.Size.Height);
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);
		}

		public BaseFormSheet(IntPtr handle) : base(handle)
		{
		}

		public void ShowInView(UIView parentView, bool animated)
		{
			parentView.AddSubview(View);
			if (animated)
				ShowAnimate();
		}

		public void ShowAnimate()
		{
			View.Transform.Scale(1.3f, 1.3f);
			View.Alpha = 0;

			UIView.Animate(0.25f, () =>
			{
				View.Alpha = 1;
				View.Transform.Scale(1, 1);
			}, null);
		}

		public void RemoveAnimate()
		{
			UIView.Animate(0.25f, () =>
			{
				View.Alpha = 0;
				View.Transform.Scale(1.3f, 1.3f);
			}, () =>
			{
				View.RemoveFromSuperview();
			});
		}

		public static Task<int> ShowAlert(string title, string message, params string[] buttons)
		{
			var tcs = new TaskCompletionSource<int>();
			var alert = new UIAlertView
			{
				Title = title,
				Message = message
			};
			foreach (var button in buttons)
				alert.AddButton(button);

			alert.Clicked += (sender, e) =>
			{
				tcs.TrySetResult((int)e.ButtonIndex);
			};
			alert.Show();
			return tcs.Task;
		}
	}
}
