#ITDelUpdater
Short for *IT Delete Updater*.

##About
This tool was designed to simplify the process of updating the "IT DELETE" folder that IT technicians use in my office. 

The IT Delete Updater tool does **not** scrape web pages and find the direct download link. This is due to the fact that websites update frequently. As such, links are opened to the provided download page.

The program is stand-alone, meaning that it can be ran from a removable drive. 

##Useage
This tool has three main functions:

1. Open links listed in the `urls.txt` file. This function will check for Google Chrome on the system, as direct download links will automatically begin downloading when the page is finished loading. Otherwise, it will default to the set default browser.
2. Run the `bundler.bat` script to group downloaded files together.
3. Compress (zip) the grouped files folder, renamed to have the current date.

##Required Files:

The IT Delete Updater requires two files (names are listed as what the program expects) to be present in the same directory as the ITDelUp.exe file:

* `urls.txt` - This is the list of links to be updated. Preferably links to direct downloads for efficiency's sake.
* `Bundler.bat` - This batch file will group everything that was downloaded together into a named and dated file at the download location.

These files are set up in a way that programs can be added or removed easily.

###File examples:
**urls.txt** - The links below are examples only. They are in no way affiliated with this project and belong/are copyright of their respective owners:
```
http://www.bleepingcomputer.com/download/junkware-removal-tool/dl/293/
http://www.bleepingcomputer.com/download/rkill/dl/10/
```

If a file does not have an easily-obtained or permanent direct download link, it is recommended to put those links at the end.

**Bundler.bat** - This is the script that will bundle everything together. Below is how my Bundler.bat is set up.
```
@echo off
CD %HOMEPATH%\Downloads\
SET DOWNLOADS="%HOMEPATH%\Downloads"
SET RESULTPATH="%HOMEPATH%\Downloads\IT Delete\"
MD "IT Delete"
MOVE "%DOWNLOADS%\JRT*.exe" "%RESULTPATH%"
MOVE "%DOWNLOADS%\rkill*.exe" "%RESULTPATH%"
::Add other MOVE commands here.
```
