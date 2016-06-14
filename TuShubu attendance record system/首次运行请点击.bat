cd /d %~dp0
@echo off 
XCOPY /s /c /y "%~dp0Resource\PresentationFramework.Aero2" %SYSTEMDRIVE%\Windows\Microsoft.NET\assembly\GAC_MSIL\
XCOPY /s /c /y "%~dp0Resource\Link\*"  %USERPROFILE%\desktop\
XCOPY /s /c /y "%~dp0Resource\Link\*"  %USERPROFILE%\桌面\
pause 