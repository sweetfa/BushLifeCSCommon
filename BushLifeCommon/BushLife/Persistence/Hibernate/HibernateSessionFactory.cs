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
 * @(#) HibernateSessionFactory.cs
 */

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.ExceptionServices;
using System.Reflection;
using System.Reflection.Emit;

using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Impl;

using AU.Com.BushLife.Aspects.Hibernate;
using AU.Com.BushLife.Utils;

namespace AU.Com.BushLife.Persistence.Hibernate
{
	/// <summary>
	/// Session factory for configuring and managing a session with
	/// a NHibernate controlled database.  This is a singleton instance.
	/// </summary>
	public class HibernateSessionFactory : IDisposable
	{
		// Configure this using any class that is in the mapping namespace that you wish to configure
		// via AssemblyInfo.cs in the utilising Assembly
		// NOTE: PostSharp to inject the namespace from the parent assembly configuration

        #region Singleton
        // The singleton instance of this class
        static HibernateSessionFactory m_instance = new HibernateSessionFactory();

        // Hidden constructor to enforce singleton pattern
		private HibernateSessionFactory() { }

        // Fetch the singleton instance of this class
		public static HibernateSessionFactory Instance
        {
            get { return m_instance; }
        }
        #endregion

		/// <summary>
		/// Set this flag to dump the configuration to XML files in the bin/mapping directory
		/// </summary>
		public bool DumpToXML { private get; set; }

		private IHibernateConfigurationExtension m_ExtensionConfig;
		private Configuration m_Configuration;
		public Configuration Configuration
		{
			get
			{
				if (m_Configuration == null)
				{
					m_ExtensionConfig = FetchExtensionConfig();
					m_Configuration = new Configuration();
					string assemblyDirectory = Path.GetDirectoryName(m_ExtensionConfig.Assembly.Location);
					m_Configuration.Configure(Path.Combine(assemblyDirectory, "hibernate.cfg.xml"));
					m_Configuration = LoadHBMXML(m_Configuration);
					m_Configuration = LoadMappings(m_Configuration);
				}
				return m_Configuration;
			}
		}

		private ISessionFactory m_SessionFactory;
		public ISessionFactory SessionFactory
		{
			get
			{
				if (m_SessionFactory == null)
				{
					m_SessionFactory = Configuration.BuildSessionFactory();
				}
				return m_SessionFactory;
			}
		}

		#region NHibernate Configuration
		/// <summary>
		/// Load the assemblies that are configured with the *.hbm.xml configuration
		/// </summary>
		/// <param name="configuration"></param>
		/// <returns></returns>
		private Configuration LoadHBMXML(Configuration configuration)
		{
			configuration.AddAssembly(m_ExtensionConfig.Assembly);
			return configuration;
		}

		/// <summary>
		/// Load the configurations that are configured with the ClassMap convention (NHibernate 3.2+)
		/// </summary>
		/// <param name="configuration"></param>
		/// <returns></returns>
		private Configuration LoadMappings(Configuration configuration)
		{
			// This configuration for mapper files
			ModelMapper mapper = new ModelMapper();
			mapper.BeforeMapProperty += new PropertyMappingHandler(mapper_BeforeMapProperty);
			List<Type> typeList = m_ExtensionConfig.Assembly.GetMatchingTypesInAssembly(item => m_ExtensionConfig.MappingsNamespace.Equals(item.Namespace)).ToList();
			mapper.AddMappings(typeList);
			IEnumerable<HbmMapping> mappings = mapper.CompileMappingForEachExplicitlyAddedEntity();
			mappings.ForEach(m => configuration.AddMapping(m));
			if (DumpToXML)
				mappings.WriteAllXmlMapping();
			return configuration;
		}

		/// <summary>
		/// Event handler to check each persistent class and determine if there is an
		/// override mapping between object and persistent types.  Mappings are defined
		/// in IHibernateConfigurationExtension implementation.
		/// </summary>
		/// <param name="modelInspector"></param>
		/// <param name="member"></param>
		/// <param name="propertyCustomizer"></param>
		private void mapper_BeforeMapProperty(IModelInspector modelInspector, PropertyPath member, IPropertyMapper propertyCustomizer)
		{
			foreach (Type objectType in m_ExtensionConfig.UserConversionMappings.Keys)
			{
				// if object type name is the same as the current type name override the type conversion
				if (((PropertyInfo)(member.GetRootMember())).PropertyType.FullName.Equals(objectType.FullName))
					propertyCustomizer.Type(m_ExtensionConfig.UserConversionMappings[objectType], null);
			}
		}

		#endregion

		/// <summary>
		/// Fetch the class that implements the IHibernateConfigurationExtension interface
		/// <para>Support is only provided for a single implementation of this class
		/// through all loaded assemblies</para>
		/// </summary>
		/// <returns>The class that implements the extension</returns>
		private IHibernateConfigurationExtension FetchExtensionConfig()
		{
			ICollection<Type> interfaces = AssemblyUtils.GetMatchingTypes(item => item.GetInterfaces().Contains(typeof(IHibernateConfigurationExtension)));
			if (interfaces.Count == 0)
				throw new ArgumentException("No implementation of IHibernateConfigurationExtension found");
			if (interfaces.Count > 1)
				throw new ArgumentException("Only support for a single implementation if IHibernateConfigurationExtension available.  Please raise support request");
			return Activator.CreateInstance(interfaces.First()) as IHibernateConfigurationExtension;
		}


		#region IDisposable Members

		[HandleProcessCorruptedStateExceptions]
		public virtual void Dispose()
		{
			if (m_SessionFactory != null)
			{
				m_SessionFactory.Close();
				m_SessionFactory.Dispose();
				m_SessionFactory = null;
			}
		}

		#endregion
	}
}
