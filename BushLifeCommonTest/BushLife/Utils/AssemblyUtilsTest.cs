using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.IO;
using System.Text;
using NUnit.Framework;

namespace AU.Com.BushLife.Utils
{
    [TestFixture]
    public class AssemblyUtilsTest
    {
        [Test]
        [Ignore("Only used for manual testing")]
        public void ListResourcesTest()
        {
            Assembly assembly = Assembly.LoadFrom(@"C:\Users\Frank Adcock\AppData\Local\NOES.exe");
            assembly.GetManifestResourceNames().ForEach(n => Console.WriteLine(n));
        }

        [Test]
        [Ignore("Only used for manual testing")]
        public void ExtractResourceToFileTest()
        {
            Assembly assembly = Assembly.LoadFrom(@"C:\Users\Frank Adcock\AppData\Local\NOES.exe");
            assembly.ExtractResourceToFile("NOES.exe.licenses", @"C:\Users\Frank Adcock\AppData\Local\NOES.exe.licenses");
        }
    }
}
