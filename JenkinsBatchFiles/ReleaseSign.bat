REM
REM	Batch file to sign release files
REM
REM
"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\signtool.exe" sign /f c:\Certificates\CodeSign.pfx /p %SigningPassword% BushLifeCommon\bin\Debug\BushLifeCommon.dll
IF ERRORLEVEL 1 exit /b 1
exit /b 0

