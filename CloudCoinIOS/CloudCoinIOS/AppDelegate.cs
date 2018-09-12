using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Foundation;
using UIKit;

namespace CloudCoinIOS
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
	[Register("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations
		public List<string> UrlList{get; set;}
		public string SafeDir { get; set; }
		public string BackupDir { get; set; }
		public string ExportDir { get; set; }
		public string ImportDir { get; set; }
		public string LogDir { get; set; }
		public string TemplatesDir { get; set; }
		public string InboxDir { get; set; }
		public string Password { get; set; }

        private NSUserDefaults userDefaults;

        public const string frackedBackground = "FrackedBackground";
        public const string supportZip = "SupportZip";

		public override UIWindow Window
		{
			get;
			set;
		}

		public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
		{
			// Override point for customization after application launch.
			// If not required for your application you can safely delete this method
			UrlList = new List<string>();
			var documentDirectory = NSFileManager.DefaultManager.GetUrls(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User)[0].Path;
			var pathList = NSFileManager.DefaultManager.Subpaths(documentDirectory);

			SafeDir = documentDirectory + "/" + "safe";
			IsExistDirectory(SafeDir);
            BackupDir = documentDirectory + "/" + "Backup";
            IsExistDirectory(BackupDir);
			ExportDir = documentDirectory + "/" + "Export";
            IsExistDirectory(ExportDir);
			ImportDir = documentDirectory + "/" + "Import";
            IsExistDirectory(ImportDir);
			LogDir = documentDirectory + "/" + "Log";
            IsExistDirectory(LogDir);
			TemplatesDir = documentDirectory + "/" + "Templates";
            IsExistDirectory(TemplatesDir);
			InboxDir = documentDirectory + "/" + "Inbox";

			foreach (var path in pathList)
			{
				var fullPath = documentDirectory + "/" + path;

				if (!UrlList.Contains(path) && !IsSameWithFolder(fullPath))
				{
  					UrlList.Add(fullPath);
				}
			}

			Password = "";
            userDefaults = NSUserDefaults.StandardUserDefaults;

			return true;
		}

        public void SetFrackedBackground(bool isBack)
        {
            userDefaults.SetBool(isBack, frackedBackground);
        }

        public bool IsFrackedBackground() {
            return userDefaults.BoolForKey(frackedBackground);
        }

        public void SetZip(bool isZip)
        {
            userDefaults.SetBool(isZip, supportZip);
        }

        public bool IsSupportZip() {
            return userDefaults.BoolForKey(supportZip);
        }

		private bool IsSameWithFolder(string path)
		{
			if (path.Contains(SafeDir) ||
			    path.Contains(BackupDir) ||
			    path.Contains(ExportDir) ||
			    path.Contains(ImportDir) ||
			    path.Contains(LogDir) ||
			    path.Contains(TemplatesDir) ||
			    path == InboxDir)
				return true;
			else
				return false;
		}

		private void IsExistDirectory(string dir)
		{
			var dirInfo = new FileInfo(dir);
			if (!dirInfo.Exists)
			{
				NSFileManager.DefaultManager.CreateDirectory(dir, true, null);
			}
		}

		public override void OnResignActivation(UIApplication application)
		{
			// Invoked when the application is about to move from active to inactive state.
			// This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) 
			// or when the user quits the application and it begins the transition to the background state.
			// Games should use this method to pause the game.
		}

		public override void DidEnterBackground(UIApplication application)
		{
			// Use this method to release shared resources, save user data, invalidate timers and store the application state.
			// If your application supports background exection this method is called instead of WillTerminate when the user quits.
		}

		public override void WillEnterForeground(UIApplication application)
		{
			// Called as part of the transiton from background to active state.
			// Here you can undo many of the changes made on entering the background.

		}

		public override void OnActivated(UIApplication application)
		{
			// Restart any tasks that were paused (or not yet started) while the application was inactive. 
			// If the application was previously in the background, optionally refresh the user interface.
		}

		public override void WillTerminate(UIApplication application)
		{
			// Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
		}

		public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
		{
			//ApplicationLogic.OpenFileUrl(url.AbsoluteString);

			NSNotificationCenter.DefaultCenter.PostNotificationName("OpenUrl", url);

			if (!UrlList.Contains(url.Path))
			{
				UrlList.Add(url.Path);
			}

            Debug.WriteLine("imported url = " + url.ToString());

			return true;
		}
	}
}

