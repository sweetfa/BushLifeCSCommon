REM
REM	Batch file to run unit tests for BushLifeCommon library
REM
REM

set ASSEMBLYDIR=%WORKSPACE%\BushLifeCommonTest\bin\Debug

mkdir "%WORKSPACE%\TestResults"

set EXITCODE=0
rmdir /S /Q "%ASSEMBLYDIR%\TestResources"
xcopy /i /e "%WORKSPACE%\src\NOESTest\TestResources" "%ASSEMBLYDIR%\TestResources"
call "C:\Program Files (x86)\Typemock\Isolator\7.1\mocking_on.bat"
"c:\Program Files\Gallio\bin\Gallio.Echo.exe" "%ASSEMBLYDIR%\BushLifeCommonTest.dll" /report-directory:"%WORKSPACE%\TestResults" /report-type:Xml /working-directory:"%WORKSPACE%"
IF ERRORLEVEL 1 SET EXITCODE=1
call "C:\Program Files (x86)\Typemock\Isolator\7.1\mocking_off.bat"
exit /b %EXITCODE%
