using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.IO;					//For file ops
using System.IO.Compression;		//For the zippers
using System.Diagnostics;			//To open every friggin' link
using System.Threading;				//For sleepy time
using System.ComponentModel;		//To notify of prop changes. Yay.
using Microsoft.Win32;				//For the File Dialog Box, according to the intellisense

namespace ITDelUp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		//Helper Props
		private string[] urls { get; set; }
		private bool ClickedOpenOnce { get; set; }
		private bool ClickedBundleOnce { get; set; }
		private bool ClickedZipOnce { get; set; }
		private bool ClickedMoveOnce { get; set; }

		private string zipBasePath { get; set; }
		private string zipResultPath { get; set; }
		private string newITDFolderName { get; set; }

		private string TodaysDate { get; set; }
		private string ChromePath { get; set; }

		//Sleep times, used in the link opening.
		private int shortSleep = 400;
		private int longSleep = 8000;

		//Ctors
		public MainWindow()
		{
			InitializeComponent();

			ReadLinksFile();
			ClickedOpenOnce = false;
			ClickedBundleOnce = false;
			ClickedZipOnce = false;
			ClickedMoveOnce = false;

			GenerateFileSafeDate();
			ChromePath = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe";
			//SetReady();
		}

		//Button Handlers
		private void Button_Open(object sender, RoutedEventArgs e)
		{
			if (ClickedOpenOnce == true)
			{
				if (ShowConfirmation("You opened the links once already. Do you like browser spam that much?") == true)
				{
					OpenLinks();
				}
			} else
			{
				OpenLinks();
				ClickedOpenOnce = true;
			}
		}

		private void Button_Bundle(object sender, RoutedEventArgs e)
		{
			RunBundler();
		}

		private void Button_Zip(object sender, RoutedEventArgs e)
		{
			FileZipper();
		}

		//Helper Methods
		private void ReadLinksFile()
		{
			string urlPath = "urls.txt";
			try
			{
				urls = File.ReadAllLines(urlPath);
			} catch (Exception e)
			{
				ShowError(e.Message);
			}
		}

		private void OpenLinks()
		{
			if (checkChromeExists() == true)
			{
				MessageBoxResult msgres = MessageBox.Show("Google Chrome has been detected!\n\nThis program is going to open a bunch of downloads via Google Chrome.\n\nThis program may seem inactive while this is occurring. Don't worry! It will return to normal when the links are finished opening.", "Brace thine self for the downloads!", MessageBoxButton.YesNo, MessageBoxImage.Warning);

				try
				{
					int limitThree = 0;
					foreach (string url in urls)
					{
						Process.Start(ChromePath, url);
						limitThree++;

						if (limitThree < 3)
						{
							Thread.Sleep(shortSleep);
						} else
						{
							Thread.Sleep(longSleep);
							limitThree = 0;
						}
					}
					lastThreeNote();
				} catch (Exception e)
				{
					ShowError(e.Message);
				}
			} else
			{
				MessageBoxResult res = MessageBox.Show("!! Incoming links! Are you ready?? !!\n\nThis program may seem inactive while the links are opening. Just give it a minute. If it fails to respond, restart it and continue on where you left off.\n\nTo make life easier, set Chrome as your default browser during this process.", "Brace thine self for the downloads!", MessageBoxButton.YesNo, MessageBoxImage.Warning);

				try
				{
					if (res == MessageBoxResult.Yes)
					{
						int limitThree = 0;
						foreach (string url in urls)
						{
							Process.Start(url);
							limitThree++;

							if (limitThree < 3)
							{
								Thread.Sleep(shortSleep);
							} else
							{
								Thread.Sleep(longSleep);
								limitThree = 0;
							}
						}
					}
					lastThreeNote();
				} catch (Exception e)
				{
					ShowError(e.Message);
				}
			}
		}

		private bool checkChromeExists()
		{
			return File.Exists(ChromePath); ;
		}

		private void lastThreeNote()
		{
			MessageBoxResult res = MessageBox.Show("The download pages should be done opening. The last three links (Spybot and two Comodo links) need to be manually downloaded from that point.\nThe two Comodo links are opened to download the 64 and 32 bit versions easily.", "Note", MessageBoxButton.OK, MessageBoxImage.Information);
		}

		private void RunBundler()
		{
			//Intended to run a bat/cmd file included in the directory.
			//Simply due to the need to add/remove items on the fly.

			try
			{
				string username = Environment.UserName;

				string newDirPath = @"C:\Users\";
				newDirPath += username;
				newDirPath += @"\Downloads\IT Delete";

				Process.Start(@"Bundler.bat");
				Thread.Sleep(2000); //Give it a couple seconds to run. 

				string newITDFolderName = @"C:\Users\";
				newITDFolderName += username;
				newITDFolderName += @"\Desktop\IT Delete "; //Keep that training space, yo.
				newITDFolderName += TodaysDate;

				Directory.Move(newDirPath, newITDFolderName);
			} catch (Exception e)
			{
				ShowError(e.Message);
			}
		}

		private void FileZipper()
		{
			try
			{
				string username = Environment.UserName;
				string zipBasePath = @"C:\Users\";
				zipBasePath += username;
				zipBasePath += @"\Desktop\";

				string zipResultPath = zipBasePath;
				zipResultPath += @"\IT Delete";
				zipResultPath += " ";
				zipResultPath += TodaysDate;
				zipResultPath += ".zip";

				ZipFile.CreateFromDirectory(zipBasePath, zipResultPath);

			} catch (Exception e)
			{
				ShowError(e.Message);
			}
		}

		//Message Box Helpers
		private void ShowError(string msg)
		{
			//SetError();
			MessageBoxResult res = MessageBox.Show("An error occurred: " + msg, "ITDelUp Error", MessageBoxButton.OK, MessageBoxImage.Error);
		}

		private bool ShowConfirmation(string msg)
		{
			MessageBoxResult res = MessageBox.Show(msg, "Please confirm.", MessageBoxButton.YesNo, MessageBoxImage.Information);

			if (res == MessageBoxResult.Yes)
			{
				return true;
			} else
			{
				return false;
			}
		}

		//Helper helper helpers.
		private void GenerateFileSafeDate()
		{
			string ret = "";
			ret += DateTime.Today.Month;
			ret += "-";
			ret += DateTime.Today.Day;
			ret += "-";
			ret += DateTime.Today.Year;

			TodaysDate = ret;
		}

		private void FileDialog()
		{
			OpenFileDialog fd = new OpenFileDialog();
			fd.Filter = "All Files (*.*)|*.*";
			fd.FilterIndex = 1;
			fd.Multiselect = false;
			fd.InitialDirectory = @"C:\";
			var result = fd.ShowDialog();
			String result2 = fd.SafeFileName;
		}

		//Status Helpers
		private void SetError()
		{
			//StatusMessage = "Error";
		}

		private void SetWorking() 
		{
			//StatusMessage = "Working";
		}

		private void SetReady()
		{
			//StatusMessage = ""; //No news is good news. 
		}

	}//Class & NS end
}