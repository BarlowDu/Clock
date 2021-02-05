
SET FEXE=%cd%\WarningClock.exe
echo %FEXE%

reg add  HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run /v WarningClock  /d %FEXE% 
echo reg success