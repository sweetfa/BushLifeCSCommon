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
 * @(#) IHibernateConfigurationExtension.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace AU.Com.BushLife.Persistence.Hibernate
{
	/// <summary>
	/// An interface to be implemented within the assembly using the 
	/// HibernateDAO class.  This determines what gets configured
	/// for hibernate automatically.
	/// <para>NOTE: Currently only supports a single implementation
	/// within all loaded assemblies</para>
	/// </summary>
	public interface IHibernateConfigurationExtension
	{
		/// <summary>
		/// The namespace containing the mapping files.
		/// <para>Only a single namespace is currently supported</para>
		/// </summary>
		string MappingsNamespace { get; }

		/// <summary>
		/// The assembly to search for .hbm.xml hibernate configuration files
		/// </summary>
		Assembly Assembly { get; }

		/// <summary>
		/// Dictionary of conversion mappings
		/// <para>Type is the object type, the type used in object definitions</para>
		/// <para>Type of IUserType is the NHIbernate IUserType implementation to convert 
		/// between the Type and the persisted representation</para>
		/// </summary>
		IDictionary<Type, Type> UserConversionMappings { get; }
	}
}
