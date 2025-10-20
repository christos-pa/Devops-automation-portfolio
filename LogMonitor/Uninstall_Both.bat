@echo on
setlocal ENABLEDELAYEDEXPANSION

echo === Uninstalling Log Monitor (all variants) ===

REM 1) Kill possible running processes (console/windowless/old names)
for %%P in (
  TrayHelper.exe
  log_monitor.exe
  ping_monitor.exe
  pythonw.exe
  python.exe
) do (
  taskkill /IM %%P /F >nul 2>&1
)

REM 2) Remove Scheduled Tasks (common names for user + SYSTEM)
for %%T in (
  "LogWatcher"
  "LogMonitor"
  "LogMonitor (UserTray)"
  "PingMonitor"
  "PingMonitor (UserTray)"
) do (
  schtasks /End    /TN %%T >nul 2>&1
  schtasks /Delete /TN %%T /F >nul 2>&1
)

REM 3) Also detect & delete any tasks with “log”, “monitor”, or “tray” in the name
for /f "skip=1 tokens=1 delims=," %%A in ('schtasks /Query /FO CSV /NH ^| findstr /I /C:"log" /C:"monitor" /C:"tray"') do (
  set "TASK=%%~A"
  if not "!TASK!"=="" (
    echo [Info] Deleting leftover task: !TASK!
    schtasks /End    /TN "!TASK!" >nul 2>&1
    schtasks /Delete /TN "!TASK!" /F >nul 2>&1
  )
)

REM 4) Remove per-user install folders (common)
rmdir /S /Q "%LOCALAPPDATA%\LogMonitor"  2>nul
rmdir /S /Q "%LOCALAPPDATA%\PingMonitor" 2>nul

REM 5) Remove Startup shortcuts (current user + all users)
for /f "delims=" %%S in ('powershell -NoProfile -Command "[Environment]::GetFolderPath('Startup')"') do set "STARTUP=%%S"
if "%STARTUP%"=="" set "STARTUP=%APPDATA%\Microsoft\Windows\Start Menu\Programs\Startup"

del /f /q "%STARTUP%\TrayHelper.lnk"     2>nul
del /f /q "%STARTUP%\LogMonitor.lnk"     2>nul
del /f /q "%STARTUP%\PingMonitor.lnk"    2>nul
del /f /q "C:\ProgramData\Microsoft\Windows\Start Menu\Programs\Startup\TrayHelper.lnk"  2>nul
del /f /q "C:\ProgramData\Microsoft\Windows\Start Menu\Programs\Startup\LogMonitor.lnk"  2>nul
del /f /q "C:\ProgramData\Microsoft\Windows\Start Menu\Programs\Startup\PingMonitor.lnk" 2>nul

REM 6) Remove Run registry keys (per-user + machine-wide)
reg delete "HKCU\Software\Microsoft\Windows\CurrentVersion\Run" /v "TrayHelper"   /f 2>nul
reg delete "HKCU\Software\Microsoft\Windows\CurrentVersion\Run" /v "LogMonitor"   /f 2>nul
reg delete "HKCU\Software\Microsoft\Windows\CurrentVersion\Run" /v "PingMonitor"  /f 2>nul

reg delete "HKLM\Software\Microsoft\Windows\CurrentVersion\Run" /v "TrayHelper"   /f 2>nul
reg delete "HKLM\Software\Microsoft\Windows\CurrentVersion\Run" /v "LogMonitor"   /f 2>nul
reg delete "HKLM\Software\Microsoft\Windows\CurrentVersion\Run" /v "PingMonitor"  /f 2>nul

echo.
echo [✓] Cleanup complete.
echo If you still see activity after reboot:
echo  - Open Task Scheduler and confirm no tasks remain.
echo  - Search C:\ for "log_monitor.exe" or "TrayHelper.exe" and delete stray copies.
echo.
pause
endlocal
