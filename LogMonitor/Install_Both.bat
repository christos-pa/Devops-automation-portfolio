@echo on
setlocal ENABLEEXTENSIONS ENABLEDELAYEDEXPANSION

rem ===== Admin check =====
net session >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
  echo [ERROR] Please run this installer as Administrator.
  pause
  exit /b 1
)

rem ===== Paths =====
set "APPDIR=%~dp0"
set "ENGINE=%APPDIR%LogWatcher.exe"
set "TRAY=%APPDIR%TrayHelper.exe"
set "TASK_ENGINE=LogWatcher"
set "LOGFILE=%APPDIR%Install_Both.log"

echo [%DATE% %TIME%] Starting install... > "%LOGFILE%"

rem ===== Defender Exclusion (so Defender won't quarantine these files) =====
echo Adding Windows Defender exclusion for "%APPDIR%" ...
powershell -NoProfile -Command "Add-MpPreference -ExclusionPath '%APPDIR%'" >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
  echo [WARN] Could not add Defender exclusion automatically. >> "%LOGFILE%"
  echo [WARN] If Defender blocks the EXEs, add an exclusion for "%APPDIR%" manually.
) else (
  echo Defender exclusion added for %APPDIR%.
  echo Defender exclusion added for %APPDIR%. >> "%LOGFILE%"
)

rem ===== Checks =====
if not exist "%ENGINE%" (
  echo [ERROR] %ENGINE% not found. >> "%LOGFILE%"
  echo [ERROR] %ENGINE% not found. Place this .bat next to LogWatcher.exe
  pause & exit /b 1
)
if not exist "%TRAY%" (
  echo [ERROR] %TRAY% not found. >> "%LOGFILE%"
  echo [ERROR] %TRAY% not found. Place this .bat next to TrayHelper.exe
  pause & exit /b 1
)

rem ===== 1) ENGINE as SYSTEM at startup =====
echo Creating SYSTEM task: %TASK_ENGINE% >> "%LOGFILE%"
schtasks /Create ^
  /TN "%TASK_ENGINE%" ^
  /TR "\"%ENGINE%\"" ^
  /SC ONSTART ^
  /RL HIGHEST ^
  /RU SYSTEM ^
  /F
if %ERRORLEVEL% NEQ 0 (
  echo [ERROR] Failed to create SYSTEM task. Code %ERRORLEVEL% >> "%LOGFILE%"
  echo Failed to create SYSTEM task. Error %ERRORLEVEL%
  pause & exit /b 1
)

echo Starting engine now... >> "%LOGFILE%"
schtasks /Run /TN "%TASK_ENGINE%" >nul 2>&1

rem ===== 2) TRAY at user logon via Startup shortcut =====
for /f "delims=" %%S in ('powershell -NoProfile -Command "[Environment]::GetFolderPath('Startup')"') do set "STARTUP=%%S"
if "%STARTUP%"=="" set "STARTUP=%APPDATA%\Microsoft\Windows\Start Menu\Programs\Startup"
set "LNK=%STARTUP%\TrayHelper.lnk"

echo Creating Startup shortcut: "%LNK%" >> "%LOGFILE%"

rem Create a small VBS to make the shortcut (no PowerShell dependency for shortcut creation)
set "VBS=%TEMP%\mk_tray_shortcut.vbs"
> "%VBS%" echo Set oWS = CreateObject("WScript.Shell")
>> "%VBS%" echo sLnk = "%LNK%"
>> "%VBS%" echo Set oLnk = oWS.CreateShortcut(sLnk)
>> "%VBS%" echo oLnk.TargetPath = "%TRAY%"
>> "%VBS%" echo oLnk.WorkingDirectory = "%APPDIR%"
>> "%VBS%" echo If CreateObject("Scripting.FileSystemObject").FileExists("%APPDIR%\logwatcher.ico") Then oLnk.IconLocation = "%APPDIR%\logwatcher.ico"
>> "%VBS%" echo oLnk.Save

cscript //nologo "%VBS%"
if %ERRORLEVEL% NEQ 0 (
  echo [WARN] Could not create Startup shortcut. Code %ERRORLEVEL% >> "%LOGFILE%"
  echo WARNING: Could not create Startup shortcut. You can add it manually via "shell:startup".
) else (
  echo Startup shortcut created at: %LNK% >> "%LOGFILE%"
  echo Tray shortcut added to Startup for user %USERNAME%.
)

rem ===== 3) Launch tray immediately for current user =====
start "" "%TRAY%"

echo [%DATE% %TIME%] Install complete. >> "%LOGFILE%"
echo SUCCESS: Engine runs at boot (SYSTEM). Tray starts at user logon and is launched now.
pause
