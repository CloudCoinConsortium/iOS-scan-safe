﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Newtonsoft.Json;
using Microsoft.Win32;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Foundation;
using UIKit;
using CoreGraphics;
using CloudCoinIOS;
using CoreText;
using MiniZip.ZipArchive;

namespace CloudCoin_SafeScan
{
    #region
    public static class FileSystem
    {
        //internal static void InitializePaths()
        //{
        //    string homedir = Environment.ExpandEnvironmentVariables(Properties.Settings.Default.UserCloudcoinDir);
        //    string importdir = Environment.ExpandEnvironmentVariables(Properties.Settings.Default.UserCloudcoinImportDir);
        //    string exportdir = Environment.ExpandEnvironmentVariables(Properties.Settings.Default.UserCloudcoinExportDir);
        //    string backupdir = Environment.ExpandEnvironmentVariables(Properties.Settings.Default.UserCloudcoinBackupDir);
        //    string logdir = Environment.ExpandEnvironmentVariables(Properties.Settings.Default.UserCloudcoinLogDir);

        //    foreach (string path in new string[] { homedir, importdir, exportdir, backupdir, logdir })
        //    {
        //        DirectoryInfo DI = new DirectoryInfo(path);
        //        if (!DI.Exists)
        //        {
        //            DI.Create();
        //        }
        //    }

        //    Logger.Initialize();
        //}

        //internal static void CopyOriginalFileToImported(FileInfo FI)
        //{
        //    string dt = DateTime.Now.ToString("dd-MM-yy_HH-mm");
        //    string importdir = Environment.ExpandEnvironmentVariables(Properties.Settings.Default.UserCloudcoinImportDir);
        //    var newFileName = importdir + FI.Name + ".imported-" + dt;
        //    File.Copy(FI.FullName, newFileName);
        //}

        //public static string[] ChooseInputFile()
        //{
        //    OpenFileDialog FD = new OpenFileDialog();
        //    FD.Multiselect = true;
        //    FD.Title = "Choose file with Cloudcoin(s)";
        //    FD.InitialDirectory = Environment.ExpandEnvironmentVariables(Properties.Settings.Default.UserCloudcoinImportDir);
        //    if (FD.ShowDialog() == true)
        //    {
        //        return FD.FileNames;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
    }
    #endregion
    public class CloudCoinFile
    {
        public enum Type { json, jpeg, unknown }
        public Type Filetype;
        string Filename;
        FileInfo FI;
        public CoinStack Coins = new CoinStack();
		public bool IsValidFile { get; set; }

		public CloudCoinFile()
		{
			
		}

		public CloudCoinFile(List<string> urlList)
        {
            foreach (var path in urlList)
            {
				ParseCloudCoinFile(path);
            }
        }

        private void ParseCloudCoinFile(string fullPath)
        {
			try
			{
				FI = new FileInfo(fullPath);
				if (FI.Exists)
				{
					Filename = fullPath;
					using (Stream fsSource = FI.Open(FileMode.Open, FileAccess.Read))
					{
						byte[] signature = new byte[20];
						fsSource.Read(signature, 0, 20);
						string sig = Encoding.UTF8.GetString(signature);
						var reg = new Regex(@"{[.\n\t\s\x09\x0A\x0D]*""cloudcoin""");
						IsValidFile = false;

						if (Enumerable.SequenceEqual(signature.Take(3), new byte[] { 255, 216, 255 })) //JPEG
						{
							Filetype = Type.jpeg;
							var coin = ReadJpeg(fsSource);
							IsValidFile = coin.Validate();

							if (coin != null && IsValidFile)
							{
								Coins.Add(new CoinStack(coin));
							}
						}
						else if (reg.IsMatch(sig)) //JSON
						{
							Filetype = Type.json;
							var json = ReadJson(fsSource);

							if (json != null)
							{
								foreach (var coin in json)
								{
									if (!coin.Validate())
									{
										IsValidFile = false;
										break;
									}
									else
									{
										IsValidFile = true;
									}
								}
							}

							if (IsValidFile)
							{
								Coins.Add(json);
							}
						}
                        else if(Path.GetExtension(fullPath).Equals((".zip")))
                        {

                            var zip = new ZipArchive();
                            var documentDirectory = NSFileManager.DefaultManager.GetUrls(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User)[0].Path;

                            zip.UnzipOpenFile(fullPath, "1");

                            zip.UnzipFileTo(documentDirectory + "/unzip", true);

                            zip.OnError += (sender, e) => {
                                Console.WriteLine("Error while unzipping: {0}", e);
                            };

                            Console.WriteLine(zip.UnzippedFiles.Count());
                            zip.UnzipCloseFile();
                        }
					}

					//***
					//if (IsValidFile)
					//FileSystem.CopyOriginalFileToImported(FI);
				}
				else
				{
					IsValidFile = false;
					//throw new FileNotFoundException();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
        }

        private CloudCoin ReadJpeg(Stream jpegFS)
        {
            // TODO: catch exception for wrong file format
            //            filetype = Type.jpeg;
            byte[] fileByteContent = new byte[455];
            int numBytesToRead = fileByteContent.Length;
            int numBytesRead = 0;
            string[] an = new string[RAIDA.NODEQNTY];
            string[] aoid = new string[1];
            int sn;
            int nn;
            string ed;

            jpegFS.Position = 0;
            while (numBytesToRead > 0)
            {
                // Read may return anything from 0 to numBytesToRead.
                int n = jpegFS.Read(fileByteContent, numBytesRead, numBytesToRead);

                // Break when the end of the file is reached.
                if (n == 0)
                    break;

                numBytesRead += n;
                numBytesToRead -= n;
            }

            string jpegHexContent = "";
            jpegHexContent = Utils.ToHexString(fileByteContent);

            for (int i = 0; i < RAIDA.NODEQNTY; i++)
            {
                an[i] = jpegHexContent.Substring(40 + i * 32, 32);
            }
            aoid[0] = jpegHexContent.Substring(840, 55);
            ed = jpegHexContent.Substring(898, 4);
            nn = Int16.Parse(jpegHexContent.Substring(902, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
            sn = Int32.Parse(jpegHexContent.Substring(904, 6), System.Globalization.NumberStyles.AllowHexSpecifier);

            CloudCoin coin = new CloudCoin(nn, sn, an, ed, aoid);
			if (coin.isValidated)
            {
                return coin;
            }
            return null;
        }

        private CoinStack ReadJson(Stream jsonFS)
        {
            jsonFS.Position = 0;
            StreamReader sr = new StreamReader(jsonFS);
            CoinStack stack = null;
            try
            {
                stack = JsonConvert.DeserializeObject<CoinStack>(sr.ReadToEnd());
            }
            catch (JsonException ex)
            {
                throw;
            }
            return stack;
        }

		/* Writes a JPEG To the Export Folder */
		public bool WriteJpeg(CloudCoin cc, string tag, ref string path)
		{
			// Console.Out.WriteLine("Writing jpeg " + cc.sn);

			bool fileSavedSuccessfully = true;

			/* BUILD THE CLOUDCOIN STRING */
			String cloudCoinStr = "01C34A46494600010101006000601D05"; //THUMBNAIL HEADER BYTES
			for (int i = 0; (i < 25); i++)
			{
				cloudCoinStr = cloudCoinStr + cc.an[i];
			} // end for each an

			cloudCoinStr += "204f42455920474f4420262044454645415420545952414e545320";// Hex for " OBEY GOD & DEFEAT TYRANTS "
			cloudCoinStr += "00"; // HC: Has comments. 00 = No
			cloudCoinStr += "00"; // LHC = 100%
								  //Calculate Expiration date 
			DateTime date = DateTime.Today;
			int YEARSTILEXPIRE = 2; //The rule is coins expire in two years. 
			date.AddYears(YEARSTILEXPIRE);
			// this.ed = (date.Month + "-" + date.Year);
			cloudCoinStr += date.Month.ToString("X1");
			cloudCoinStr += date.Year.ToString("X3");// 0x97E2;//Expiration date Sep. 2018

			cloudCoinStr += "01";//  cc.nn;//network number
			String hexSN = cc.sn.ToString("X6");
			String fullHexSN = "";
			switch (hexSN.Length)
			{
				case 1: fullHexSN = ("00000" + hexSN); break;
				case 2: fullHexSN = ("0000" + hexSN); break;
				case 3: fullHexSN = ("000" + hexSN); break;
				case 4: fullHexSN = ("00" + hexSN); break;
				case 5: fullHexSN = ("0" + hexSN); break;
				case 6: fullHexSN = hexSN; break;
			}
			cloudCoinStr = (cloudCoinStr + fullHexSN);
			/* BYTES THAT WILL GO FROM 04 to 454 (Inclusive)*/
			byte[] ccArray = this.hexStringToByteArray(cloudCoinStr);


			///* READ JPEG TEMPLATE*/
			//byte[] jpegBytes = null;
			//switch (getDenomination(cc.sn))
			//{
			//	case 1: jpegBytes = readBytesFromJpg(Environment.ExpandEnvironmentVariables(Properties.Settings.Default.UserCloudcoinTemplateDir) + "jpeg1.jpg"); break;
			//	case 5: jpegBytes = readBytesFromJpg(Environment.ExpandEnvironmentVariables(Properties.Settings.Default.UserCloudcoinTemplateDir) + "jpeg5.jpg"); break;
			//	case 25: jpegBytes = readBytesFromJpg(Environment.ExpandEnvironmentVariables(Properties.Settings.Default.UserCloudcoinTemplateDir) + "jpeg25.jpg"); break;
			//	case 100: jpegBytes = readBytesFromJpg(Environment.ExpandEnvironmentVariables(Properties.Settings.Default.UserCloudcoinTemplateDir) + "jpeg100.jpg"); break;
			//	case 250: jpegBytes = readBytesFromJpg(Environment.ExpandEnvironmentVariables(Properties.Settings.Default.UserCloudcoinTemplateDir) + "jpeg250.jpg"); break;

			//}// end switch


			///* WRITE THE SERIAL NUMBER ON THE JPEG */

			//Bitmap bitmapimage;
			//using (var ms = new MemoryStream(jpegBytes))
			//{
			//	bitmapimage = new Bitmap(ms);
			//	// bitmapimage.Save(fileName2, ImageFormat.Jpeg);
			//}
			//Graphics graphics = Graphics.FromImage(bitmapimage);
			//graphics.SmoothingMode = SmoothingMode.AntiAlias;
			//graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
			//PointF drawPointAddress = new PointF(10.0F, 10.0F);
			//graphics.DrawString(String.Format("{0:N0}", cc.sn) + " of 16,777,216 on Network: 1", new Font("Arial", 20), System.Drawing.Brushes.White, drawPointAddress);

			//ImageConverter converter = new ImageConverter();
			//byte[] snBytes = (byte[])converter.ConvertTo(bitmapimage, typeof(byte[]));

			UIImage jpgImage = null;
			switch (getDenomination(cc.sn))
			{
				case 1: jpgImage = UIImage.FromFile("jpeg1.jpg"); break;
				case 5: jpgImage = UIImage.FromFile("jpeg5.jpg"); break;
				case 25: jpgImage = UIImage.FromFile("jpeg25.jpg"); break;
				case 100: jpgImage = UIImage.FromFile("jpeg100.jpg"); break;
				case 250: jpgImage = UIImage.FromFile("jpeg250.jpg"); break;
			}

			var newImage = DrawFont(jpgImage, String.Format("{0:N0}", cc.sn) + " of 16,777,216 on Network: 1", new CGPoint(30f, 25f));

			Byte[] snBytes;
			using (NSData imageData = newImage.AsJPEG())
			{
				snBytes = new Byte[imageData.Length];
				System.Runtime.InteropServices.Marshal.Copy(imageData.Bytes, snBytes, 0, Convert.ToInt32(imageData.Length));
			}

			List<byte> b1 = new List<byte>(snBytes);
			List<byte> b2 = new List<byte>(ccArray);
			b1.InsertRange(4, b2);

			if (tag == "random")
			{
				Random r = new Random();
				int rInt = r.Next(100000, 1000000); //for ints
				tag = rInt.ToString();
			}

			var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
			string folderPath = appDelegate.ExportDir + tag + "_" + string.Format("{0:ddMMyyyy}", date) + "/";
			DirectoryInfo DI = new DirectoryInfo(folderPath);
			if (!DI.Exists)
			{
				DI.Create();
			}

			string fileName = getDenomination(cc.sn) + ".CloudCoin." + cc.nn + "." + cc.sn + ".";
			string jpgfileName = folderPath + fileName + tag + ".jpg";

			File.WriteAllBytes(jpgfileName, b1.ToArray());
			path = jpgfileName;

			return fileSavedSuccessfully;
		}
		//end write JPEG

		private UIImage DrawFont(UIImage image, string text, CGPoint point)
		{
			var font = UIFont.FromName("Arial", 10f);
			UIGraphics.BeginImageContext(image.Size);
			image.Draw(new CGRect(0, 0, image.Size.Width, image.Size.Height));

			var rect = new CGRect(point.X, point.Y, image.Size.Width, image.Size.Height);
			UIColor.White.SetFill();

			//NSDictionary stringAttrs = new NSDictionary("NSFontAttributeName", font, "NSForegroundColorAttributeName", UIColor.White);

			NSMutableAttributedString attString = new NSMutableAttributedString(text);
			NSRange range = new NSRange(0, attString.Length);

			attString.AddAttributes(new UIStringAttributes
			{
				Font = font,
				ForegroundColor = UIColor.White
			}, range);

			attString.DrawString(rect);

			var newImage = UIGraphics.GetImageFromCurrentImageContext();
			return newImage;
		}

		/* OPEN FILE AND READ ALL CONTENTS AS BYTE ARRAY */
		public byte[] readAllBytes(string fileName)
		{
			byte[] buffer = null;
			using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
			{
				buffer = new byte[fs.Length];
				int fileLength = Convert.ToInt32(fs.Length);
				fs.Read(buffer, 0, fileLength);
			}
			return buffer;
		}//end read all bytes

		//public byte[] readBytesFromJpg(string fileName)
		//{
		//	Image img = Image.FromFile(fileName);
		//	byte[] jpgBytes;
		//	using (MemoryStream ms = new MemoryStream())
		//	{
		//		img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
		//		jpgBytes = ms.ToArray();
		//	}

		//	return jpgBytes;
		//}

		public byte[] hexStringToByteArray(String HexString)
		{
			int NumberChars = HexString.Length;
			byte[] bytes = new byte[NumberChars / 2];
			for (int i = 0; i < NumberChars; i += 2)
			{
				bytes[i / 2] = Convert.ToByte(HexString.Substring(i, 2), 16);
			}
			return bytes;
		}//End hex string to byte array

		public int getDenomination(int sn)
		{
			int nom = 0;
			if (sn < 1)
			{
				nom = 0;
			}
			else if (sn < 2097153)
			{
				nom = 1;
			}
			else if (sn < 4194305)
			{
				nom = 5;
			}
			else if (sn < 6291457)
			{
				nom = 25;
			}
			else if (sn < 14680065)
			{
				nom = 100;
			}
			else if (sn < 16777217)
			{
				nom = 250;
			}
			else
			{
				nom = '0';
			}
			return nom;
		}//end get denomination
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class CloudCoinOut
    {
        [JsonProperty(Order = 1)]
        public string nn { get; set; }
        [JsonProperty(Order = 2)]
        public string sn { get; set; }
        [JsonProperty(Order = 3)]
        public string[] an = new string[RAIDA.NODEQNTY];
        [JsonProperty(Order = 4)]
        public string ed; //expiration in the form of Date expressed as a hex string like 97e2 Sep 2018
        [JsonProperty(Order = 5)]
        public string[] aoid = new string[1];//Account or Owner ID
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class CoinStackOut
    {
        public CoinStackOut(CoinStack stack)
        {
            cloudcoin = new List<CloudCoinOut>();
            foreach (CloudCoin coin in stack.cloudcoin)
            {
                cloudcoin.Add(new CloudCoinOut() { sn = coin.sn.ToString(), nn = coin.nn.ToString(), an = coin.an, aoid = coin.aoid, ed = coin.ed });
            }
        }
        [JsonProperty]
        public List<CloudCoinOut> cloudcoin { get; set; }
        public void SaveInFile(string filename)
        {
            FileInfo fi = new FileInfo(filename);
            
            using (StreamWriter sw = fi.CreateText())
            {
                string json = null;
                try
                {
                    json = JsonConvert.SerializeObject(this);
                    sw.Write(json);
                }
                catch (JsonException ex)
                {
					Console.WriteLine("CloudStackOut.SaveInFile Serialize exception: " + ex.Message);
                }
                catch (IOException ex)
                {
					Console.WriteLine("CloudStackOut.SaveInFile IO exception: " + ex.Message);
                }
            }
        }
    }
}
