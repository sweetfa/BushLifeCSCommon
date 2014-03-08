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
 * @(#) HibernateDAO.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;

using AU.Com.BushLife.Aspects;
using AU.Com.BushLife.Aspects.Hibernate;

namespace AU.Com.BushLife.Persistence.Hibernate
{
    /// <summary>
    /// Data Access Object pattern implementation for access via a 
	/// hibernate persistence store providing basic CRUD support
	/// in a generic manner.
	/// <para>To use:  </para>
	/// <para>     using (HibernateDAO<KEYTYPE,OBJECTTYPE> dao = new HibernateDAO<KEYTYPE,OBJECTTYPE>())</para>
	/// <para>     { dao.Create(newObject); }</para>
	/// <para>Requires the PostSharp.dll, NHibernate.dll and Iesi.Collections</para>
	/// <para>Note that the dao should not maintained longer than you require 
	/// to manipulate the current set of data as any database changes
	/// at the back-end will not be recognised until after this session closes</para>
    /// </summary>
    /// <typeparam name="PK">The primary key type</typeparam>
    /// <typeparam name="T">The object type to access</typeparam>
	[HibernateTransactionAspect(AttributeTargetMembers = "regex:Create|Update|Delete",AspectPriority=50)]
	[UpdateTrackerAspect(AttributeTargetMembers = "regex:Create|Update",AspectPriority=20)]
	public class HibernateDAO<PK, T> : GenericDAOInterface<PK, T>
		where T : class
    {
		/// <summary>
		/// Session attribute for the current persistence session.
		/// <para>This property is injected via the HibernateTransactionAspect</para>
		/// </summary>
		public ISession Session { get; protected set;  }

		#region GenericDAOInterface<PK,T> Members

		public PK Create(T newItem)
        {
            return (PK) Session.Save(newItem);
        }

        public T Update(T updatedItem)
        {
			T result = Session.Merge(updatedItem);
			Session.Flush();
			return result;
        }

        public void Delete(T itemToRemove)
        {
			Session.Delete(itemToRemove);
			Session.Flush();
        }


		public void Delete(IEnumerable<T> itemsToRemove)
		{
			if (itemsToRemove != null)
			{
				foreach (T item in itemsToRemove)
				{
					Session.Delete(item);
				}
				Session.Flush();
			}
		}

		public T GetById(PK primaryKey)
        {
			return Session.Get<T>(primaryKey);
        }

		public ICollection<T> GetAll()
		{
			return Session.CreateCriteria<T>().List<T>();
		}

		/// <summary>
		/// Delete any entries that are persisted in the persistent store
		/// but are not contained in the modified list
		/// </summary>
		/// <param name="modifiedList">The list of items that are to be retained/placed
		/// in the persistent store</param>
		public void DeleteXOREntries(IEnumerable<T> modifiedList)
		{
			IEnumerable<T> itemsToRemove = GetAll().Where(item => !modifiedList.Contains(item));
			Delete(itemsToRemove.ToList());
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			if (Session.IsOpen)
				Session.Close();
			Session.Dispose();
		}

		#endregion
	}
}
