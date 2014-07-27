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
 * @(#) AbstractFactory.cs
 */

using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AU.Com.BushLife.Patterns
{
	/// <summary>
	/// <para>Factory pattern implementation based on GOF factory</para>
	/// <para>This class should be inherited by a singleton class 
	/// that provides the singletonness for this factory</para>
	/// <para>This class is thread safe</para>
	/// <para>K = the type of the key used in the factory</para>
	/// <para>V = the type of the value object stored in the factory</para>
	/// </summary>
	/// <typeparam name="K">The type of the key used in the factory</typeparam>
	/// <typeparam name="V">The type of object stored in the factory</typeparam>
	/// <typeparam name="C">The comparison object to use.  Use IEqualityComparer.Default if nothing special required</typeparam>
	public abstract class AbstractFactory<K, V>
	{
		private Dictionary<K, V> FactoryItems { get; set; }

        /// <summary>
        /// Default constructor initialises the dictionary object.  
        /// No locking is required here as the Singleton owner instance will
        /// have lock control over instantiation of the constructor.
        /// </summary>
        public AbstractFactory()
        {
            FactoryItems = new Dictionary<K, V>(EqualityComparer<K>.Default);
        }

        /// <summary>
        /// Default constructor initialises the dictionary object.  
        /// No locking is required here as the Singleton owner instance will
        /// have lock control over instantiation of the constructor.
        /// </summary>
        /// <param name="comparator">The comparison object to use.  Use IEqualityComparer.Default if nothing special required.
        /// <para>Remember to override GetHasCode() and Equals() methods of the K class</para></param>
        public AbstractFactory(IEqualityComparer<K> comparator)
        {
            FactoryItems = new Dictionary<K, V>(comparator);
        }

        /// <summary>
		/// Register an entry in the factory
		/// </summary>
		/// <param name="key">The key to register</param>
		/// <param name="item">The item to register</param>
		public virtual void Register(K key, V item)
		{
			FactoryItems.Add(key, item);
		}

		/// <summary>
		/// Remove an entry from the factory
		/// </summary>
		/// <param name="key">The key of the entry to remove</param>
		public virtual void Unregister(K key)
		{
			FactoryItems.Remove(key);
		}

		/// <summary>
		/// Determine if the key value is contained in the factory
		/// </summary>
		/// <param name="key">The key to check for the pre-existence of</param>
		/// <returns>true if the key is already in the factory, false otherwise</returns>
		public bool Contains(K key)
		{
			return FactoryItems.ContainsKey(key);
		}

		/// <summary>
		/// Fetch the entry from the factory.  An exception is thrown
		/// if no entry matches the key.
		/// </summary>
		/// <param name="key">The key for the entry to fetch</param>
		/// <returns>The value matching the key</returns>
		public V Obtain(K key)
		{
			return FactoryItems[key];
		}

		/// <summary>
		/// Empty all registered items from the factory registry
		/// </summary>
		public void UnregisterAll()
		{
			while (FactoryItems.Count > 0)
			{
				if (FactoryItems.Count > 0)
					Unregister(FactoryItems.ElementAt(0).Key);
			}
		}

        /// <summary>
        /// Search the values in the factory for the set of factory items
        /// that match the supplied predicate
        /// </summary>
        /// <param name="predicate">The predicate to match the search on.</param>
        /// <returns>An enumerable of matched values</returns>
        public IEnumerable<V> SearchValues(Func<V, bool> predicate)
        {
            return FactoryItems.Values.Where(predicate);
        }
	}
}
