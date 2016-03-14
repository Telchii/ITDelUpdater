﻿using System;
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
		private int shortSleep = 500;
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
				if (ShowConfirmation("You opened or attempted to open all these links once already. Do you like browser spam that much?") == true)
				{
					OpenLinksDialog();
				}
			} else
			{
				OpenLinksDialog();
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

		private void OpenLinksChrome()
		{
			int limitThree = 0;
			try {
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
		}

		private void OpenLinks()
		{
			try	{
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
				lastThreeNote();
			} catch (Exception e)
			{
				ShowError(e.Message);
			}
		}

		private void OpenLinksDialog()
		{
			if (checkChromeExists() == true)
			{
				MessageBoxResult msgres = MessageBox.Show("Google Chrome has been detected! If you want to continue without Chrome, select \"No\" below or \"Cancel\" to stop this procedure.\n\nThis program is going to open a bunch of downloads via Google Chrome. This program may seem inactive while this is occurring. Give the program a minute or two, as the downloads are staggered.", "Brace thine self for the downloads approacheth!", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

				if (msgres == MessageBoxResult.Yes)
				{
					OpenLinksChrome();
				} else if (msgres == MessageBoxResult.No)
				{
					OpenLinks();
				}

			} else
			{
				MessageBoxResult res = MessageBox.Show("Incoming downloads! If you want to stop this procedure before the downloads begin, select \"No\" below.\n\nThis program is going to open a bunch of downloads via your default browser. This program may seem inactive while this is occurring. Give the program a minute or two, as the downloads are staggered.", "Oh Downloadeo, Downloadeo. Where art thou Downloadeo?", MessageBoxButton.YesNo, MessageBoxImage.Warning);

				if (res == MessageBoxResult.Yes)
				{
					OpenLinks();
				}
			}
		}

		private bool checkChromeExists()
		{
			return File.Exists(ChromePath); ;
		}

		private void lastThreeNote()
		{
			//Leave as "end of process message" for simplicity's sake. That way we don't have to deal with counting which part we're at. 
			MessageBoxResult res = MessageBox.Show("The automatic download pages should be done opening. The last three links (Spybot and two Comodo links) need to be manually downloaded from that point.\nThe two Comodo links are opened to download the 64 and 32 bit versions easily.", "Note", MessageBoxButton.OK, MessageBoxImage.Information);
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
			MessageBoxResult res = MessageBox.Show("An error occurred: " + msg, "Error! Error!", MessageBoxButton.OK, MessageBoxImage.Error);
		}

		private bool ShowConfirmation(string msg)
		{
			MessageBoxResult res = MessageBox.Show(msg, "Your confirmation is requested.", MessageBoxButton.YesNo, MessageBoxImage.Information);

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