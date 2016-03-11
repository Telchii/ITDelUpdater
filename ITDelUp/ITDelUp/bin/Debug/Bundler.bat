@echo off
cd %HOMEPATH%\Downloads\

md "IT Delete"

::The Bleeping Downloads: ADW, JRT, RKill, TDSS, Combofix, autoruns
move "%HOMEPATH%\Downloads\AdwCleaner*.exe" "%HOMEPATH%\Downloads\IT Delete\";
move "%HOMEPATH%\Downloads\JRT*.exe" "%HOMEPATH%\Downloads\IT Delete\";
move "%HOMEPATH%\Downloads\rkill*.exe" "%HOMEPATH%\Downloads\IT Delete\";
move "%HOMEPATH%\Downloads\tdss*.exe" "%HOMEPATH%\Downloads\IT Delete\";
move "%HOMEPATH%\Downloads\Combofix.exe" "%HOMEPATH%\Downloads\IT Delete\";
move "%HOMEPATH%\Downloads\autoruns.exe" "%HOMEPATH%\Downloads\IT Delete\";

::Comodo/CCE
move "%HOMEPATH%\Downloads\cce*.zip" "%HOMEPATH%\Downloads\IT Delete\";

::Stinger
move "%HOMEPATH%\Downloads\stinger*.exe" "%HOMEPATH%\Downloads\IT Delete\";

::Spybot
move "%HOMEPATH%\Downloads\spybot*.exe" "%HOMEPATH%\Downloads\IT Delete\";

::Should I Remove IT
move "%HOMEPATH%\Downloads\ShouldIRemoveIt_Setup.exe" "%HOMEPATH%\Downloads\IT Delete\"

::HerdProtect/Reason Core
move "%HOMEPATH%\Downloads\reason-core*.exe" "%HOMEPATH%\Downloads\IT Delete\"
move "%HOMEPATH%\Downloads\herd*.exe" "%HOMEPATH%\Downloads\IT Delete\"

::Revo
move "%HOMEPATH%\Downloads\revouninstaller.zip" "%HOMEPATH%\Downloads\IT Delete\
::CCleaner
move "%HOMEPATH%\Downloads\ccsetup*.zip" "%HOMEPATH%\Downloads\IT Delete\"

::Commented out to let the CMD shell release the hold on the file. 
::pause;