REM
REM	Batch file to run unit tests for BushLifeCommon library
REM
REM

set ASSEMBLYDIR=%WORKSPACE%\BushLifeCommonTest\bin\Debug

mkdir "%ASSEMBLYDIR%\TestResults"

set EXITCODE=0
REM rmdir /S /Q "%ASSEMBLYDIR%\TestResources"
REM xcopy /i /e "%WORKSPACE%\BushLifeCommonTest\TestResources" "%ASSEMBLYDIR%\TestResources"
call "C:\Program Files (x86)\Typemock\Isolator\7.1\mocking_on.bat"
"c:\Program Files\Gallio\bin\Gallio.Echo.exe" "%ASSEMBLYDIR%\BushLifeCommonTest.dll" /report-directory:"%ASSEMBLYDIR%\TestResults" /report-type:Xml /working-directory:"%ASSEMBLYDIR%"
IF ERRORLEVEL 1 SET EXITCODE=1
call "C:\Program Files (x86)\Typemock\Isolator\7.1\mocking_off.bat"
exit /b %EXITCODE%
