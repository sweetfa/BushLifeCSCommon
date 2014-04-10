REM
REM	Batch file to run FXcop for External Frame
REM
REM
set PROGRAMFILES=C:\Program Files (x86)
"%PROGRAMFILES%\Microsoft FxCop 1.36\FxCopCmd" /ignoregeneratedcode /f:"%WORKSPACE%\BushLifeCommon\bin\Debug\BushLifeCommon.dll" /out:fxcop.xml /fo /rule:"%PROGRAMFILES%\Microsoft FxCop 1.36\Rules" /rid:-"Microsoft.Design#CA1004" /rid:-"Microsoft.Design#CA1006" /rid:-"Microsoft.Design#CA1026" /rid:-"Microsoft.Design#CA1031" /rid:-"Microsoft.Globalization#CA1300" /rid:-"Microsoft.Globalization#CA1305" /rid:-"Microsoft.Performance#CA1800" /rid:-"Microsoft.Usage#CA2213" /d:"%PROGRAMFILES%\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0" /d:"%WORKSPACE%\SharedLibs\x64\Hibernate" /gac  /dictionary:"%WORKSPACE%\BushLifeCommon\FXCopDictionary.xml"
set RETVAL=%ERRORLEVEL%
if %RETVAL%==0 exit /b 1
if %RETVAL%==512 exit /b 1
exit /b 0

