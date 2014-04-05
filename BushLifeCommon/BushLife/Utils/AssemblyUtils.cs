/**
 * Copyright (C) 2012 Bush Life Pty Limited
 * 
 * All rights reserved.  No unauthorised copying or redistribution without the prior written 
 * consent of the management of Bush Life Pty Limited.
 * 
 * www.bushlife.com.au
 * sales@bushlife.com.au
 * 
 * PO Box 865, Redcliffe, QLD, 4020, Australia
 * 
 * 
 * @(#) AssemblyUtils.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace AU.Com.BushLife.Utils
{
	/// <summary>
	/// Class containing utilities for manipulation of the Assembly object
	/// </summary>
	public static class AssemblyUtils
	{
		/// <summary>
		/// Get the types within the assembly that match the predicate.
		/// <para>for example, to get all types within a namespace</para>
		/// <para>    typeof(SomeClassInAssemblyYouWant).Assembly.GetMatchingTypesInAssembly(item => "MyNamespace".Equals(item.Namespace))</para>
		/// </summary>
		/// <param name="assembly">The assembly to search</param>
		/// <param name="predicate">The predicate query to match against</param>
		/// <returns>The collection of types within the assembly that match the predicate</returns>
		public static ICollection<Type> GetMatchingTypesInAssembly(this Assembly assembly, Predicate<Type> predicate)
		{
			ICollection<Type> types = new List<Type>();
			try
			{
				types = assembly.GetTypes().Where(i => i != null && predicate(i) && i.Assembly == assembly).ToList();
			}
			catch (ReflectionTypeLoadException ex)
			{
				types = ProcessExceptionTypes(ex.Types, i => i != null && predicate(i) && i.Assembly == assembly);
			}
			return types;
		}

		/// <summary>
		/// Get all matching types in all assemblies that match the predicate
		/// </summary>
		/// <param name="predicate">The predicate function to match the types against</param>
		/// <returns>The list of all types that match the predicate</returns>
		public static ICollection<Type> GetMatchingTypes(Predicate<Type> predicate)
		{
			ICollection<Type> types = new List<Type>();
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				try
				{
					types.AddRange(assembly.GetTypes().Where(i => i != null && predicate(i)).ToList());
				}
				catch (ReflectionTypeLoadException ex)
				{
					types.AddRange(ProcessExceptionTypes(ex.Types, i => i != null && predicate(i)));
				}
			}
			return types;
		}

	
		/// <summary>
		/// Process each of the types in the list (usually called from within a ReflectionTypeLoadException
		/// to load the types that match the predicate.
		/// </summary>
		/// <param name="theTypes">The list of types to process (taken from ReflectionTypeLoadException.Types</param>
		/// <param name="predicate">The boolean predicate to compare the type against</param>
		/// <returns>The collection of types (that can be loaded) matching the predicate</returns>
		private static ICollection<Type> ProcessExceptionTypes(Type[] theTypes, Predicate<Type> predicate)
		{
			ICollection<Type> types = new List<Type>();
			foreach (Type theType in theTypes)
			{
                try
                {
                    if (predicate(theType))
                        types.Add(theType);
                }
                // This exception list is not exhaustive, modify to suit any reasons
                // you find for failure to parse a single assembly
                catch (BadImageFormatException)
                {
                    // Type not in this assembly - reference to elsewhere ignored
                }
                catch (FileNotFoundException)
                {
                    // Type not in this assembly - reference to elsewhere ignored
                }
                catch (TypeLoadException)
                {
                    // Type not in this assembly - reference to elsewhere ignored
                }
			}
			return types;
		}

		/// <summary>
		/// Find the assembly of a particular name
		/// </summary>
		/// <param name="name">The full name of the assembly to match against</param>
		/// <returns>The assembly with the matching name or null if the name is not matched</returns>
		public static Assembly GetAssemblyByName(string name)
		{
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
				if (assembly.FullName == name)
					return assembly;
			return null;
		}


		/// <summary>
		/// Find the assembly of a particular name
		/// </summary>
		/// <param name="name">The full name of the assembly to match against</param>
		/// <returns>The assembly with the matching name or null if the name is not matched</returns>
		public static Assembly GetAssemblyStartingWith(string name)
		{
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
				if (assembly.FullName.StartsWith(name))
					return assembly;
			return null;
		}


		/// <summary>
		/// Get the version of the calling assembly
		/// </summary>
		/// <returns></returns>
		public static Version GetAssemblyVersion()
		{
			return Assembly.GetCallingAssembly().GetName().Version;
		}

		/// <summary>
		/// Get the assembly build date of the calling assembly by extracting the linker time stamp
		/// from the portable executable header of the assembly
		/// </summary>
		/// <returns>The date and time (adjusted for local timezone) of the assembly</returns>
		public static DateTime GetAssemblyBuildDate()
		{
			string assemblyPath = Assembly.GetCallingAssembly().Location;
			return GetAssemblyBuildDate(assemblyPath);
		}

		/// <summary>
		/// Get the assembly build date of the calling assembly by extracting the linker time stamp
		/// from the portable executable header of the assembly
		/// </summary>
		/// <param name="assemblyPath">The path of the assembly to get the version for</param>
		/// <returns>The date and time (adjusted for local timezone) of the assembly</returns>
		public static DateTime GetAssemblyBuildDate(string assemblyPath)
		{
			const int BufferSize = 2048;
			const int PortableExecutablePointerOffset = 60;
			const int LinkerTimestampOffset = 8;

			byte[] buffer = new byte[BufferSize];
			using (Stream stream = new FileStream(assemblyPath, FileMode.Open, FileAccess.Read))
			{
				stream.Read(buffer, 0, BufferSize);
				Int32 portableHeaderOffset = BitConverter.ToInt32(buffer, PortableExecutablePointerOffset);
				Int32 secondsSince1970 = BitConverter.ToInt32(buffer, portableHeaderOffset + LinkerTimestampOffset);
				DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0);
				dt = dt.AddSeconds(secondsSince1970);
				dt = dt.AddHours(TimeZone.CurrentTimeZone.GetUtcOffset(dt).Hours);
				return dt;
			}
		}

        /// <summary>
        /// Output a list embedded resources in the specified assembly
        /// <para>The list is output to the provided stream</para>
        /// </summary>
        /// <param name="assembly">The assembly that the embedded resources are contained in</param>
        /// <param name="stream">The stream to output the list to</param>
        public static void ListResources(this Assembly assembly, System.IO.StreamWriter stream)
        {
            foreach (var resourceName in assembly.GetManifestResourceNames())
            {
                stream.WriteLine(resourceName);
            }
        }

        /// <summary>
        /// Extract an embedded resource from an assembly and save it to the specified path
        /// </summary>
        /// <param name="assembly">The assembly to extract the resource from</param>
        /// <param name="resourceName">The name of the embedded resource to extract</param>
        /// <param name="fileName">The path name of the file to create the embedded resource as</param>
        public static void WriteResourceToFile(this Assembly assembly, string resourceName, string fileName)
        {
            using (var resource = assembly.GetManifestResourceStream(resourceName))
            {
                using (var file = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    resource.CopyTo(file);
                }
            }
        }
    }
}
