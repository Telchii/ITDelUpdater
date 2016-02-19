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

		private void Button_Move(object sender, RoutedEventArgs e)
		{
			MoveFolder();
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
			MessageBoxResult res = MessageBox.Show("Incoming links! Are you ready??? This program may seem inactive while the links are opening.", "Incoming! Incoming! Incoming!", MessageBoxButton.YesNo, MessageBoxImage.Warning);

			try
			{
				if (res == MessageBoxResult.Yes)
				{
					foreach (string url in urls)
					{
						Process.Start(url);
						Thread.Sleep(350);
					}
				}
			} catch (Exception e)
			{
				ShowError(e.Message);
			}
			
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
				Thread.Sleep(1500); //Give it a couple seconds to run. 

				string newITDFolderName = newDirPath;
				newITDFolderName += " "; //Gotta get that space, yo...
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
				zipBasePath += @"\Downloads";

				string zipResultPath = zipBasePath;
				zipResultPath += @"\IT Delete";
				zipResultPath += " ";
				zipResultPath += TodaysDate;

				ZipFile.CreateFromDirectory(zipBasePath, zipResultPath);

			} catch (Exception e)
			{
				ShowError(e.Message);
			}
		}

		private void MoveFolder()
		{

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

		//Status Helpers
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
