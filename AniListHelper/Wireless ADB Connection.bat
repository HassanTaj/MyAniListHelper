@REM ----------------------------- Read ME
@REM ----
@REM ----    BEFORE YOU RUN THIS SCRIPT
@REM ---- Please configure 'adb' & 'scrcpy' 
@REM ---- as environment variables for system
@REM ----
@REM ----------------------------- Read ME
@echo off
@REM list adb devices
adb devices
@REM get Ip And Port either way

set /p IP="Ip: "
set /p ipAddr="Ip[port]: "
set /p ipPass="Ip:[password]: "
echo "thanks for entering %ipAddr%"
@REM connect adb to android device
adb pair %IP%:%ipAddr% %ipPass%
adb connect %IP%
adb devices
@REM mirror screen
pause
scrcpy
