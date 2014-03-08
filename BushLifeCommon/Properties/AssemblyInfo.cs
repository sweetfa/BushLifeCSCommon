using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("BushLifeCommon")]
[assembly: AssemblyDescription("Common Function Support Library for Bush Life Development")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Bush Life Pty Limited")]
[assembly: AssemblyProduct("BushLifeCommon")]
[assembly: AssemblyCopyright("Copyright ©  2012-2014  Bush Life Pty Limited")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("9cddb20d-0135-4b45-ace3-0c9346992d8f")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

// Allow unit testing access to internal methods
[assembly: InternalsVisibleTo("BushLifeCommonTest,PublicKey="+
"002400000480000094000000060200000024000052534131000400000100010077a64d18f2d228" +
"1ae7e103ee77968666757ffbf185614a5f55eb0eee56185c598f1d462192ea7caa05b63f5ce63f" +
"c6819f40edc58cef6f6d31ac0da8d9c287b8f00e3ee08b306d055f300f6d495eafe8df14ee2447" +
"dc734d46f0e44fa29686a618bffc1d03fe646afd07eb4e4084ac3db2c0a3ebf74eab0a17054fbb" +
"8c3065a9")]

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.Config", Watch = true)]


