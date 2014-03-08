REM
REM	Batch file to run unit tests for BushLifeCommon library
REM
REM
dir "%WORKSPACE%\TestResults"
set EXITCODE=0
mkdir "%WORKSPACE%\TestResults"
rmdir /S /Q "%WORKSPACE%\BushLifeCommonTest\bin\Debug\TestResources"
xcopy /i /e "%WORKSPACE%\BushLifeCommonTest\TestResources" "%WORKSPACE%\BushLifeCommonTest\bin\Debug\TestResources"
call "C:\Program Files (x86)\Typemock\Isolator\7.1\mocking_on.bat"
"c:\Program Files\Gallio\bin\Gallio.Echo.exe" "%WORKSPACE%\BushLifeCommonTest\bin\Debug\BushLifeCommonTest.dll" /report-directory:"%WORKSPACE%\TestResults" /report-type:Xml /working-directory:"%WORKSPACE%"
IF ERRORLEVEL 1 SET EXITCODE=1
call "C:\Program Files (x86)\Typemock\Isolator\7.1\mocking_off.bat"
exit /b %EXITCODE%

