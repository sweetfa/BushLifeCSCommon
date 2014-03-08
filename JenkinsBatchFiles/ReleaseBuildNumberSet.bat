REM
REM	Batch file to set the release number and version number for a production release
REM
REM
REM Change the version details
fart --svn -r AssemblyInfo.cs "[assembly: AssemblyVersion(\"1.0.0.0\")]" "[assembly: AssemblyVersion(\"%RELEASE_VERSION%.%BUILD_NUMBER%.%SVN_REVISION%\")]"
if %ERRORLEVEL%==0 exit /b 1
fart --svn -r AssemblyInfo.cs "[assembly: AssemblyFileVersion(\"1.0.0.0\")]" "[assembly: AssemblyFileVersion(\"%RELEASE_VERSION%.%BUILD_NUMBER%.%SVN_REVISION%\")]"
if %ERRORLEVEL%==0 exit /b 1
exit /b 0

