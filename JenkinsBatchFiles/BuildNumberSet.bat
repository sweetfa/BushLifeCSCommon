REM
REM	Batch file to configure version number prior to build
REM	based on the subversion revision number and the Jenkins
REM	build number
REM
REM
fart --svn -r AssemblyInfo.cs "[assembly: AssemblyFileVersion(\"1.0.0.0\")]" "[assembly: AssemblyFileVersion(\"1.0.%BUILD_NUMBER%.%SVN_REVISION%\")]"
if %ERRORLEVEL%==0 exit /b 1
exit /b 0

