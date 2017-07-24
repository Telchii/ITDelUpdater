# ITDeleteUpdater

## About

This tool was designed to simplify the process of updating the "IT DELETE" folder that IT technicians use in my office. 

The IT Delete Updater tool does **not** scrape web pages and find the direct download link. This is due to the fact that websites update frequently and dynamic HTML parsing/scraping is not a skill I currently possess. As such, links are opened to the provided download page.

The program is stand-alone. Running it from a removable drive is possible!

## Development

The program was created in C#/.NET using the MVVM design pattern in Visual Studio 2015. The `Caliburn.Micro framework` was utilized to aide in creating the GUI. You may need to download the `Caliburn.Micro packages` via NuGet if Visual Studio does not restore them.

Be sure to create the `urls.txt` and `bundler.bat` files (see Setup/Required Files section below) in the same directory as the program executable.

## Requirements

* Modern Windows operating system (e.g., Windows 10)
* Up to date .NET Framework.

## Useage

This tool has three main functions:

1. Open links listed in the `urls.txt` file. This function will check for Google Chrome on the system, as direct download links will automatically begin downloading when the page is finished loading. Otherwise, it will default to the set default browser.
2. Run the `bundler.bat` script to group downloaded files together.
3. Compress (to a .zip) the grouped files folder, renamed to have the current date.

## Setup/Required Files

The IT Delete Updater requires two files (names are listed as what the program expects) to be present in the same directory as the ITDelUp.exe file. 

* `urls.txt` - This is the list of URLs to be opened. One URL per line. Preferably links to direct downloads for efficiency's sake. Comment lines are **not** supported. If you attempt to put one in, you will have a funky page (attempted to be) opened. Tools that do not have a direct download URL or do not have a webpage that automatically initiates a download should be placed at the bottom of the `urls.txt` file to easily find these manual-download pages.
* `Bundler.bat` - This batch file will group everything that was downloaded together into a named and dated file at the download location. See the example below for an easy setup. The directory name, "IT Delete", is currently hardcoded (insert eye rolls here) into the program.

### File examples
These files are set up in a way that programs can be added or removed easily. Below is a working example for two tools.

**urls.txt** - This is the list of links that the program will open. 

```
http://www.bleepingcomputer.com/download/junkware-removal-tool/dl/293/
http://www.bleepingcomputer.com/download/rkill/dl/10/
```
*(Bleeping Computer links are for example purposes only and are not a part of nor affiliated with this project.)*


**Bundler.bat**

```batch
@echo off

::Set and create the working directories.
CD %HOMEPATH%\Downloads\
SET DOWNLOADS="%HOMEPATH%\Downloads"
SET RESULTPATH="%HOMEPATH%\Downloads\IT Delete\"
MD "IT Delete"

::Add your programs below.
::Move files from downloads path to the result path.
MOVE "%DOWNLOADS%\JRT*.exe" "%RESULTPATH%"
MOVE "%DOWNLOADS%\rkill*.exe" "%RESULTPATH%"
::Add other MOVE commands here.
```

Change directory to the download location for the files. (Via browsers, so assumed to be the current user's Downloads folder.) Make the "bundle" directory
