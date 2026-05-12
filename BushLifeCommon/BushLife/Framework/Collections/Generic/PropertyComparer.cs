// <copyright file="PropertyComparer.cs" company="Bush Life Pty Limited">
// Copyright (c) 2012 Bush Life Pty Limited. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;
using System.Collections;

namespace AU.Com.BushLife.Framework.Collections.Generic
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
    public class PropertyComparer<T> : IComparer<T>
    {
        private readonly IComparer Comparer;
		private PropertyDescriptor PropertyDescriptor { get; set; }
        private int Reverse;

        public PropertyComparer(PropertyDescriptor property, ListSortDirection direction)
        {
            this.PropertyDescriptor = property;
            Type comparerForPropertyType = typeof(Comparer<>).MakeGenericType(property.PropertyType);
            this.Comparer = (IComparer)comparerForPropertyType.InvokeMember("Default", BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.Public, null, null, null);
            this.SetListSortDirection(direction);
        }

        #region IComparer<T> Members

        public int Compare(T x, T y)
        {
            return this.Reverse * this.Comparer.Compare(this.PropertyDescriptor.GetValue(x), this.PropertyDescriptor.GetValue(y));
        }

        #endregion

        private void SetListSortDirection(ListSortDirection direction)
        {
            this.Reverse = direction == ListSortDirection.Ascending ? 1 : -1;
        }

        public void SetPropertyAndDirection(PropertyDescriptor descriptor, ListSortDirection direction)
        {
            this.PropertyDescriptor = descriptor;
            this.SetListSortDirection(direction);
        }
    }
}
