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
 * @(#) ExpressionCompiler.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.CodeDom.Compiler;
using Microsoft.CSharp;

using AU.Com.BushLife.Utils;

namespace AU.Com.BushLife.Framework.Expressions
{
	/// <summary>
	/// Class to create a lambda function from a string
	/// <para>the string invokes a compiler on the string so performance
	/// is not great.  Use judiciously</para>
	/// </summary>
	public class ExpressionCompiler<T>
	{
		private static readonly string[] AssemblyNames = 
		{
			"System.Core.dll"
		};

		private static readonly IDictionary<string, string> providerOptions = new Dictionary<string, string>()
		{
			{ "CompilerVersion",		"v4.0"	}
		};

		/// <summary>
		/// Template used to control the code for the class to be generated
		/// <para>Argument 1: the generic T of the expression function</para>
		/// <para>Argument 2: The lambda expression string</para>
		/// <para>Argument 3: Additional using statements</para>
		/// </summary>
		private static readonly string ExpressionTemplate = 
@"using System;
using System.Linq.Expressions;
{2}

class foo
{{
    public static Expression<{0}> bar()
    {{
        return ({1});
    }}
}}";

		/// <summary>
		/// Compile a string into a lambda function that represents the function type
		/// of the class T type parameter
		/// <para>See ExpressionCompilerTest for usage examples</para>
		/// </summary>
		/// <param name="functionString">The string for the lambda function</param>
		/// <returns>The compiled function</returns>
		public T Compile(string functionString, string[] additionalAssemblyNames = null, string[] usingNamespaces = null)
		{
			Expression<T> expression = Translate(functionString, additionalAssemblyNames, usingNamespaces);
			return expression.Compile();
		}

		/// <summary>
		/// Translate a string literal inta a lambda expression that represents an
		/// expression of the provided T type parameter
		/// </summary>
		/// <param name="functionString">The string for the lambda function</param>
		/// <returns>The string as an expression</returns>
		public Expression<T> Translate(string functionString, string[] additionalAssemblyNames = null, string[] usingNamespaces = null)
		{
			Tuple<string, IList<string>> assemblyDetails = GetAssemblyDetails(additionalAssemblyNames);

			CompilerParameters cParameters = new CompilerParameters(assemblyDetails.Item2.ToArray());
//			cParameters.GenerateInMemory = true;
//			cParameters.TempFiles.KeepFiles = true;
//			cParameters.CompilerOptions = string.Format("/lib:{0}", assemblyDetails.Item1);
			CSharpCodeProvider provider = new CSharpCodeProvider(providerOptions);

			Type t = typeof(T);
			// Add any additional namespaces into the Usings part of the code block
			string usingsString = "";
			if (usingNamespaces != null)
				foreach (string usingString in usingNamespaces)
					usingsString += string.Format("using {0};\n", usingString);
			string expressionCode = string.Format(ExpressionTemplate, typeof(T).PrettyName(), functionString, usingsString);

			CompilerResults results = provider.CompileAssemblyFromSource(cParameters, expressionCode);

			// Check if the compilation had errors
			if (results.Errors.HasErrors)
			{
				string message = "";
				foreach (CompilerError error in results.Errors)
				{
					message += error.ToString() + "\n";
				}
				throw new ArgumentException(message);
			}

			Expression<T> expression = (Expression<T>)results.CompiledAssembly.GetType("foo").GetMethod("bar").Invoke(null, null);
			return expression;
		}

		/// <summary>
		/// Get the assembly details required to configure the compiler
		/// for the names and search path of the required assemblies
		/// </summary>
		/// <param name="additionalAssemblyNames">The additional 
		/// assembly names to be referenced by the code being compiled</param>
		/// <returns>A tuple containing the comma separated directory 
		/// search path for each of the assemblies, and a list of 
		/// each of the assembly names</returns>
		private Tuple<string, IList<string>> GetAssemblyDetails(string[] additionalAssemblyNames)
		{
			string ver = string.Format("{0}.{1}.{2}", Environment.Version.Major, Environment.Version.MajorRevision, Environment.Version.Build);
            string exDir = string.Format(@"C:\WINDOWS\Microsoft.NET\Framework\v{0}", ver);

			Int32 additionalCount = additionalAssemblyNames == null ? 0 : additionalAssemblyNames.Count();
			IList<string> assemblyNames = new List<string>();
			assemblyNames.AddRange(AssemblyNames);
			if (additionalCount > 0)
				assemblyNames.AddRange(additionalAssemblyNames);

			string assemblyPath = exDir;
			if (additionalAssemblyNames != null)
			{
				foreach (string assemblyName in additionalAssemblyNames)
				{
					Assembly ass = AssemblyUtils.GetAssemblyByName(assemblyName);
					if (ass != null)
						assemblyPath += "," + ass.Location;
				}
			}
			return new Tuple<string, IList<string>>(assemblyPath, assemblyNames);
		}
	}
}
