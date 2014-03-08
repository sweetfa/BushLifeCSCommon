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
 * @(#) BindingListUtils.cs
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace AU.Com.BushLife.Utils
{
	/// <summary>
	/// Extension methods for the BindingList class
	/// </summary>
	public static class BindingListUtils
	{
		public static void SortAscending<T, P>(this BindingList<T> bindingList, Func<T, P> sortProperty)
		{
			bindingList.Sort(null, (a, b) => ((IComparable<P>)sortProperty(a)).CompareTo(sortProperty(b)));
		}
		public static void SortDescending<T, P>(this BindingList<T> bindingList, Func<T, P> sortProperty)
		{
			bindingList.Sort(null, (a, b) => ((IComparable<P>)sortProperty(b)).CompareTo(sortProperty(a)));
		}
		public static void Sort<T>(this BindingList<T> bindingList)
		{
			bindingList.Sort(null, null);
		}
		public static void Sort<T>(this BindingList<T> bindingList, IComparer<T> comparer)
		{
			bindingList.Sort(comparer, null);
		}
		public static void Sort<T>(this BindingList<T> bindingList, Comparison<T> comparison)
		{
			bindingList.Sort(null, comparison);
		}
		private static void Sort<T>(this BindingList<T> bindingList, IComparer<T> p_Comparer, Comparison<T> p_Comparison)
		{

			//Extract items and sort separately
			List<T> sortList = new List<T>();
			bindingList.ForEach(item => sortList.Add(item));//Extension method for this call
			if (p_Comparison == null)
			{
				sortList.Sort(p_Comparer);
			}
			else
			{
				sortList.Sort(p_Comparison);
			}

			//Disable notifications, rebuild, and re-enable notifications
			bool oldRaise = bindingList.RaiseListChangedEvents;
			bindingList.RaiseListChangedEvents = false;
			try
			{
				bindingList.Clear();
				sortList.ForEach(item => bindingList.Add(item));
			}
			finally
			{
				bindingList.RaiseListChangedEvents = oldRaise;
				bindingList.ResetBindings();
			}

		}

		/// <summary>
		/// Iterate over each item in the source enumeration performing the action on it
		/// </summary>
		/// <typeparam name="T">The type contained in the collection</typeparam>
		/// <param name="source">The source collection</param>
		/// <param name="action">The action to perform</param>
		[DebuggerHidden]
		public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
		{
			if (source == null) throw new ArgumentNullException("source");
			if (action == null) throw new ArgumentNullException("action");

			foreach (T item in source)
			{
				action(item);
			}
		}

		/// <summary>
		/// Remove all items from the list that match the predicate
		/// </summary>
		/// <typeparam name="T">The type of the object in the list</typeparam>
		/// <param name="list">The list to remove the items from</param>
		/// <param name="selector">The predicate function to select the items to remove</param>
		public static void RemoveAll<T>(this IList<T> list, Func<T, bool> selector)
		{
			IList<T> itemsToRemove = list.Where(selector).ToList();
			itemsToRemove.ForEach(item => list.Remove(item));
		}
	}
}
