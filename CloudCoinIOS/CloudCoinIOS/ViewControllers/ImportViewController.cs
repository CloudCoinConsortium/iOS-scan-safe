// This file has been autogenerated from a class added in the UI designer.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CloudCoin_SafeScan;
using Foundation;
using GalaSoft.MvvmLight.Threading;
using UIKit;
using System.Linq;

namespace CloudCoinIOS
{
    public partial class ImportViewController : BaseFormSheet
    {
        private List<string> urlList;
        private const string confirmMsg = "Would you like to change ownership and import money in Safe?\n" +
                                 "Choosing 'No' will simply scan coins without changing passwords.";
        private int isPasswordForSafe;
        private CloudCoinFile coinFile;

        public EventHandler<bool> DetectHandler;
        public EventHandler<CloudCoinFile> ImportFilesHandler;

        public ImportViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            InitializeProperties();

            InitializeMethods();
        }

        private void InitializeProperties()
        {
            var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
            urlList = appDelegate.UrlList;

            lblImportedFiles.Text = string.Format("You have {0} files in your import directory", urlList.Count);

            //if (urlList.Count == 0)
            //    btnImport.Enabled = false;
            //else
                //btnImport.Enabled = true;

            btnFinished.Enabled = false;
            
        }

        public static CloudCoinFile ScanSelected(List<string> urlList)
        {
            CloudCoinFile coinFile = null;

            if (urlList != null && urlList.Count != 0)
            {
                coinFile = new CloudCoinFile(urlList);
            }
            return coinFile;
        }

        private void InitializeMethods()
        {
            btnImport.TouchUpInside += async (sender, e) =>
            {
                coinFile = ScanSelected(urlList);
                if (coinFile != null && coinFile.IsValidFile)
                {
                    isPasswordForSafe = await ShowAlert("Confirmation", confirmMsg, new string[] { "Yes", "No" });
                    btnCancel.Hidden = true;
                    btnImport.Hidden = true;

                    if (isPasswordForSafe == 0)
                    {
                        //RAIDA.Instance.Detect(coinFile.Coins, true);

                        DetectHandler.Invoke(this, true);
                        //will implement the Safe source.
                    }
                    else
                    {
                        //RAIDA.Instance.Detect(coinFile.Coins, false);
                        DetectHandler.Invoke(this, false);
                    }

                    var raida = RAIDA.Instance;
                    var tasks = raida.GetMultiDetectTasks(coinFile.Coins, 1000);

                    await Task.WhenAll(tasks.AsParallel().Select(async task => await task()));
                    raida.onStackScanCompleted(new StackScanCompletedEventArgs(coinFile.Coins, null, raida));

                    ImportFilesHandler.Invoke(this, coinFile);
                    RemoveAnimate();
                }
                else
                {
                    await ShowAlert("Message", "There are no coins to import. " +
                                    "You have to get coins in form of .stack or " +
                                    ".jpg/.jpeg files from email or AirDrop. Choose" +
                                    " CloudCoin Scan&Safe icon to get files.", new string[] { "Ok"});
                    RemoveAnimate();
                }
            };

            btnCancel.TouchUpInside += (sender, e) => 
            { 
                RemoveAnimate();
            };

            btnFinished.TouchUpInside += (sender, e) =>
            {
                //RemoveAnimate();
                //if (isPasswordForSafe == 0)
                    //CompletedWithPassword(coinFile);
            };
        }
    }
}
