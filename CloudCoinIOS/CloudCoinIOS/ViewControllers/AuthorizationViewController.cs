// This file has been autogenerated from a class added in the UI designer.

using System;
using System.Collections.Generic;
using System.Linq;
using CloudCoin_SafeScan;
using Foundation;
using UIKit;

namespace CloudCoinIOS
{
    public partial class AuthorizationViewController : BaseFormSheet
    {
		public delegate void SetPasswordEventHandler();
		public event SetPasswordEventHandler CompletedWithPassword;
        private List<Coin4Display> checkingCoinList;

        public AuthorizationViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            checkingCoinList = new List<Coin4Display>();
            authTableView.Source = new AuthrizeTableSource(checkingCoinList);

            InitProperties();

            InitMethods();
        }

        private void InitProperties()
        {
            RAIDA.Instance.StackScanCompleted += StackScanCompleted;
            RAIDA.Instance.DetectCoinCompleted += CoinScanCompleted;

            btnCancel.SetTitle("Processing...", UIControlState.Normal);
            btnCancel.Enabled = false;

            authTableView.Layer.CornerRadius = 5f;
            authTableView.Layer.BorderColor = UIColor.LightGray.CGColor;
            authTableView.Layer.MasksToBounds = true;
        }

        private void InitMethods()
        {
            btnCancel.TouchUpInside += (sender, e) => {
				RemoveAnimate();
				CompletedWithPassword();                
            };
        }

        private void CoinScanCompleted(object o, DetectCoinCompletedEventArgs e)
        {
            InvokeOnMainThread(() => {
                checkingCoinList.Add(new Coin4Display(){
                    Serial = e.coin.sn,
                    Value = Utils.Denomination2Int(e.coin.denomination),
                    Check = e.coin.isPassed,
                    Comment = e.coin.percentOfRAIDAPass + "%"
                });

                authTableView.ReloadData();
            });
        }

        private void StackScanCompleted(object o, StackScanCompletedEventArgs e)
        {
            InvokeOnMainThread(() =>
            {
                int j = 0;
                foreach (var coin in e.stack) {
                    for (int k = 0; k < CloudCoinCore.Config.NodeCount; k++) {
                        if (e.raida.NodesArray[k].MultiResponse.responses!=null) {
                            coin.response[k] = e.raida.NodesArray[k].MultiResponse.responses[j];
                        } else {
                            coin.response[k] = new CloudCoinCore.Response();
                        }
                    }

                    int countp = coin.response.Where(x => x.outcome == "pass").Count();
                    int countf = coin.response.Where(x => x.outcome == "fail").Count();

                    checkingCoinList.Add(new Coin4Display()
                    {
                        Serial = coin.sn,
                        Value = Utils.Denomination2Int(coin.denomination),
                        Check = countp > RAIDA.MINTRUSTEDNODES4AUTH ? true : false,
                        Comment = (countp * 100 / RAIDA.NODEQNTY) + "%"
                    });

                    j++;

                }
                authTableView.ReloadData();

                var status = "Checking Result:\n";
                status += e.stack.cloudcoin.Count.ToString() + " coins scanned.\n";

                if (e.stack.AuthenticatedQuantity > 0)
                    status += "Authenticated: " + e.stack.AuthenticatedQuantity + " units\n";
                if (e.stack.CounterfeitedQuantity > 0)
                    status += "Counterfeited: " + e.stack.CounterfeitedQuantity + " units\n";
                if (e.stack.FractionedQuantity > 0)
                    status += "Fractioned: " + e.stack.FractionedQuantity + " units\n";

                status += "Total value of good coins: " + e.stack.SumOfGoodCoins;

                textStatus.Text = status;

				btnCancel.SetTitle("Cancel", UIControlState.Normal);
				btnCancel.Enabled = true;
            });
        }
    }

    public class Coin4Display
    {
        public int Serial { get; set; }
        public int Value { get; set; }
        public bool Check { get; set; }
        public string Comment { get; set; }
    }

    public class AuthrizeTableSource : UITableViewSource
    {
        List<Coin4Display> coinList;
        NSString CellIdentifier = new NSString("AuthViewCell");

        public AuthrizeTableSource(List<Coin4Display> coinList)
        {
            this.coinList = coinList;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return coinList.Count;
        }

        public override UIView GetViewForHeader(UITableView tableView, nint section)
        {
            var cell = (AuthViewCell)tableView.DequeueReusableCell(CellIdentifier);

            if (cell == null)
            {
                cell = new AuthViewCell(CellIdentifier);
            }

            cell.ContentView.BackgroundColor = UIColor.FromRGB(210, 210, 210);

            return cell;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        public override nfloat GetHeightForHeader(UITableView tableView, nint section)
        {
            return 30;
        }


        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = (AuthViewCell)tableView.DequeueReusableCell(CellIdentifier);
            var coin = coinList[indexPath.Row];

            //---- if there are no cells to reuse, create a new one
            if (cell == null)
            {
                cell = new AuthViewCell(CellIdentifier);
            }

            cell.ContentView.BackgroundColor = UIColor.White;
            cell.UpdateCell(coin);

            return cell;
        }
    }
}
