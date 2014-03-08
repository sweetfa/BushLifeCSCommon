REM
REM	Batch file to build release version of software
REM
REM
REM SET PostSharpTargetProcessor=x64
SET Configuration="Debug"
SET PROGRAMFILES=C:\Program Files (x86)
"%PROGRAMFILES%\Microsoft Visual Studio 10.0\Common7\IDE\devenv.com" BushLifeCommon.sln /Rebuild %Configuration% /Project BushLifeCommon /ProjectConfig %Configuration% /out STDOUT
IF ERRORLEVEL 1 exit /b 1
exit /b 0

