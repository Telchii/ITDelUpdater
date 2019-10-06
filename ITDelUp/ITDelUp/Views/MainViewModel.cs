using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.IO.Compression;
using System.Diagnostics;			//For the opening of every friggin' link
using System.Threading;				//Threading & thread methods
using Microsoft.Win32;				//File Dialog Box
using System.Windows;				//Message Dialog Box
using ITDelUp;

namespace ITDelUp.Views
{
	class MainViewModel : Screen
	{
		//Helper Props
		private string[] urls { get; set; }
		private bool ClickedOpenOnce { get; set; }
		private bool ClickedBundleOnce { get; set; }
		private bool ClickedZipOnce { get; set; }
		private bool ClickedMoveOnce { get; set; }

		private string ZipBasePath { get; set; }
		private string ZipResultPath { get; set; }
		private string NewITDFolderName { get; set; }

        private string NewDirPath { get; set; }
		private string FileSafeDate { get; set; }

		//Status Props + Window Props
		private string _BackgroundColor;
		public string BackgroundColor
		{
			get { return _BackgroundColor; }
			set { _BackgroundColor = value; NotifyOfPropertyChange(() => BackgroundColor); }
		}

		private string _BusyStatus;
		public string BusyStatus
		{
			get { return _BusyStatus; }
			set { _BusyStatus = value; NotifyOfPropertyChange(() => BusyStatus); }
		}

		private bool _ButtonsEnabled;
		public bool ButtonsEnabled
		{
			get { return _ButtonsEnabled; }
			set { _ButtonsEnabled = value; NotifyOfPropertyChange(() => ButtonsEnabled); }
		}
		
		//Fields
		private const int shortSleep = 500;
		private const int longSleep = 8000;

		//Constructors
		public MainViewModel()
		{
			ReadLinksFile();
			
			ClickedOpenOnce = false;
			ClickedBundleOnce = false;
			ClickedZipOnce = false;
			ClickedMoveOnce = false;

			GenerateFileSafeDate();
			SetReady();
		}

		//Buttons and Bound Methods
		public void Button_Open()
		{
			SetBusy();

			if (ClickedOpenOnce == true)
			{
                //Intentionally two levels of "if" to allow no-action if they select "no" to the confirmation.
				if (ShowConfirmation("You opened all these links once already. Do you like browser spam that much?"))
				{
					OpenLinksDialog();
					Thread.Sleep(1000);
				}
			} else
			{
				OpenLinksDialog();
				ClickedOpenOnce = true;
				Thread.Sleep(1000);

			}
			SetReady();
		}

		public void Button_Bundle()
		{
			SetBusy();
			RunBundler();
			SetReady();
		}

		public void Button_Zip()
		{
			SetBusy();
			FileZipper();
			SetReady();
		}

		public void Button_Bop()
		{
			Process.Start("https://upload.wikimedia.org/wikipedia/commons/b/bc/Bop_it.jpg");
		}

        public void Button_Whip()
        {
            Process.Start("https://www.youtube.com/watch?v=IIEVqFB4WUo&t=9");
        }

        //Helper Methods
        private void ReadLinksFile()
		{
			try
			{
				urls = File.ReadAllLines(Settings.UrlsFile);
			} catch (Exception e)
			{
				ShowError(e.Message);
			}
		}

		private void VerifyBundlerBatFileExists()
		{
			if (!File.Exists(Settings.BundlerScriptFile))
			{
				ShowError($"File \"{Settings.BundlerScriptFile}\" does not exist. Please verify it is in the same directory as this program.");
			}
		}

		private void OpenLinksChrome()
		{
			int limitThree = 0;
			try
			{
				foreach (string url in urls)
				{
					Process.Start(Settings.ChromePath, url);
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
				LastThreeNote();
			} catch (Exception e)
			{
				ShowError(e.Message);
			}
		}

		private void OpenLinks()
		{
			try
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
				LastThreeNote();
			} catch (Exception e)
			{
				ShowError(e.Message);
			}
		}

		private void OpenLinksDialog()
		{
            bool UseChrome = CheckChromeExists();
            string BrowserMessage = (UseChrome) 
                ? "Google Chrome"
                : "your default browser";

			if (CheckChromeExists())
			{
                MessageBoxResult MessageResult = MessageBox.Show($"Incoming downloads!\n\nThis program is going to open a bunch of download URLs via {BrowserMessage}. This program may seem inactive while this is occurring. Give the program a minute or two, as the downloads are staggered.\n\nContinue with downloads?", "Incoming Downloads", MessageBoxButton.YesNo, MessageBoxImage.Warning);


                if (MessageResult == MessageBoxResult.Yes)
                {
                    if (UseChrome)
                        OpenLinksChrome();
                    else
                        OpenLinks();
                }
			}
		}

		private bool CheckChromeExists()
		{
            return File.Exists(Settings.ChromePath);
		}

		private void LastThreeNote()
		{
			//Leave as "end of process message" for simplicity's sake. That way we don't have to deal with counting which part we're at. 
			MessageBox.Show("The automatic download pages should be done opening. The last few links need to be manually downloaded from that point. If a page was opened twice, check if both 32 and 64 bit need to be downloaded.", "Task Completed!", MessageBoxButton.OK, MessageBoxImage.Information);
		}

		private void RunBundler()
		{
			//Intended to run a batch file included in the directory.
			//  Rather than hard-code the expected files here, the script can be edited
            //  as needed, rather than editing + recompiling this program.

			try
			{
                //1 - Create the output directory (not yet timestamped)
                NewDirPath = $@"{Settings.OutputDirectory}\{Settings.OutputFileName}";

                //2 - Run the script
				Process.Start(Settings.BundlerScriptFile);
				Thread.Sleep(2000); //Give it a couple seconds to run. 

                //3 - Rename/Timestamp the bundler's output directory
				if(Settings.TimestampOutput)
                    Directory.Move(NewDirPath, $@"{NewDirPath} {FileSafeDate}");
                //else, do nothing
            } catch (Exception e)
			{
				ShowError(e.Message);
			}
		}

		private void FileZipper()
		{
			try
			{
				string ZipBasePath = $@"{NewDirPath} {FileSafeDate}";
                string ZipResultPath = $"{Settings.ZipOutputDirectory}\\{Settings.OutputFileName} {FileSafeDate}.zip";

				ZipFile.CreateFromDirectory(ZipBasePath, ZipResultPath);
				Directory.Delete(ZipBasePath, true);
			} catch (Exception e)
			{
				ShowError(e.Message);
			}
		}

		//Message Box Helpers
		private void ShowError(string msg)
		{
			MessageBoxResult res = MessageBox.Show($"An error occurred: {msg}", "Error! Error!", MessageBoxButton.OK, MessageBoxImage.Error);
		}

		private bool ShowConfirmation(string msg)
		{
			MessageBoxResult res = MessageBox.Show(msg, "Confirm.", MessageBoxButton.YesNo, MessageBoxImage.Information);
            return (res == MessageBoxResult.Yes);
		}

        //Other helper methods.
		private void GenerateFileSafeDate()
		{
            FileSafeDate = $"{DateTime.Today.Month}-{DateTime.Today.Day}-{DateTime.Today.Year}";
		}

		//private void FileDialog()
		//{
		//	OpenFileDialog fd = new OpenFileDialog();
		//	fd.Filter = "All Files (*.*)|*.*";
		//	fd.FilterIndex = 1;
		//	fd.Multiselect = false;
		//	fd.InitialDirectory = @"C:\";
		//	var result = fd.ShowDialog();
		//	String result2 = fd.SafeFileName;
		//}

		//Status Helpers -- These are only implemented in the bound button methods ("button_X"), for simplicity and upkeep's sake. 
		private void SetBusy()
		{
			BackgroundColor = "Orange";
			BusyStatus = "Working...";
			ButtonsEnabled = false;
		}

		private void SetReady()
		{
			BackgroundColor = "White";
			BusyStatus = "Ready";
			ButtonsEnabled = true;
		}
	}
}