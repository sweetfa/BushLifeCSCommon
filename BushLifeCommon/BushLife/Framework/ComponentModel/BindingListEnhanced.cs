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
 * @(#) BindingListEnhanced.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace AU.Com.BushLife.Framework.ComponentModel
{
	/// <summary>
	///
	/// </summary>
	public class BindingListEnhanced<T> : BindingList<T>
	{
		public BindingListEnhanced()
		{
		}

		public BindingListEnhanced(IList<T> list)
			: base(list)
		{
		}

		public BindingListEnhanced(IEnumerable<T> enumeration)
			: base(new List<T>(enumeration))
		{
		}

		private ListChangingEventHandler onListChanging;

		public event ListChangingEventHandler ListChanging
		{
			add
			{
				onListChanging += value;
			}
			remove
			{
				onListChanging -= value;
			}
		}

		protected void OnListChanging(ListChangedEventArgs e)
		{
			if (onListChanging != null)
			{
				onListChanging(this, e);
			}
		}

		protected override void RemoveItem(int index)
		{
			OnListChanging(new ListChangedEventArgs(ListChangedType.ItemDeleted, index));
			base.RemoveItem(index);
		}
	}

	// Summary:
	//     Represents the method that will handle the AU.Com.BushLife.Framework.ComponentModel.ListChanging
	//     event of the AU.Com.BushLife.Framework.ComponentModel.BindingListEnhanced class.
	//
	// Parameters:
	//   sender:
	//     The source of the event.
	//
	//   e:
	//     A System.ComponentModel.ListChangedEventArgs that contains the event data.
	public delegate void ListChangingEventHandler(object sender, ListChangedEventArgs e);
}
