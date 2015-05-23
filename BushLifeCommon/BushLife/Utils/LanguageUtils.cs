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
 * @(#) LanguageUtils.cs
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.ObjectModel;

namespace AU.Com.BushLife.Utils
{
	/// <summary>
	/// Set of utility extension methods to assist in C# code
	/// </summary>
	public static class LanguageUtils
	{
		#region Collection Utils
		/// <summary>
		/// Shallow clone a collection
		/// </summary>
		/// <typeparam name="T">The type of the element in the collection</typeparam>
		/// <param name="cloneable">The collection to clone the contents of</param>
		/// <returns>The cloned collection or null if cloneable parameter is null</returns>
		public static List<T> CloneShallowCollection<T>(this ICollection<T> cloneable)
		{
			if (cloneable == null) return null;

			List<T> result = new List<T>(cloneable.Count);
			foreach (T item in cloneable)
			{
				result.Add((T)item);
			}
			return result;
		}

		/// <summary>
		/// Shallow clone a collection
		/// </summary>
		/// <typeparam name="T">The type of the element in the collection</typeparam>
		/// <param name="cloneable">The collection to clone the contents of</param>
		/// <returns>The cloned collection or null if cloneable parameter is null</returns>
		public static Stack<T> CloneShallowCollection<T>(this Stack<T> cloneable)
		{
			if (cloneable == null) return null;

			Stack<T> result = new Stack<T>(cloneable);
			return result;
		}

		/// <summary>
		/// Deep clone a collection provided the instances in the list 
		/// implement the ICloneable interface
		/// </summary>
		/// <typeparam name="T">The type of the element in the collection</typeparam>
		/// <param name="cloneable">The collection to clone the contents of</param>
		/// <returns>The cloned collection or null if cloneable parameter is null</returns>
		public static List<T> CloneDeepCollection<T>(this ICollection<T> cloneable)
			where T : ICloneable
		{
			if (cloneable == null) return null;

			List<T> result = new List<T>(cloneable.Count);
			foreach (T item in cloneable)
			{
				result.Add((T)item.Clone());
			}
			return result;
		}

		/// <summary>
		/// Deep clone a list provided the instances in the list 
		/// implement the ICloneable interface
		/// </summary>
		/// <typeparam name="T">The type of the element in the collection</typeparam>
		/// <param name="cloneable">The collection to clone the contents of</param>
		/// <returns>The cloned collection or null if cloneable parameter is null</returns>
		public static List<T> CloneDeepCollection<T>(this IList<T> cloneable)
			where T : ICloneable
		{
			if (cloneable == null) return null;

			List<T> result = new List<T>(cloneable.Count);
			foreach (T item in cloneable)
			{
				result.Add((T)item.Clone());
			}
			return result;
		}

		/// <summary>
		/// Array extension method to fill an array with the specified value
		/// </summary>
		/// <typeparam name="T">The type of item in the array</typeparam>
		/// <param name="array">The array to fill</param>
		/// <param name="value">The value to fill the array with</param>
		public static void Fill<T>(this T[] array, T value)
		{
			for (int i = 0; i < array.Length; ++i)
			{
				array[i] = value;
			}
		}

		/// <summary>
		/// Add a collection to another collection
		/// </summary>
		/// <typeparam name="T">The type contained within the collection</typeparam>
		/// <param name="collection">The original collection</param>
		/// <param name="additional">The collection to add to the original collection</param>
		/// <returns>The original collection with the additions added</returns>
		public static ICollection<T> AddRange<T>(this ICollection<T> collection, IEnumerable<T> additional)
		{
			if (additional != null)
			{
				foreach (T item in additional)
				{
					collection.Add(item);
				}
			}
			return collection;
		}

		/// <summary>
		/// Convert a collection to a hash set of another type</para>
		/// <para>The funtion provided must take an argument of type T and return a type of TResult</para>
		/// <para>Example: sourceList.ToHashSet&lt;XYZ, Point3D>(l => new Point3D(l))</para>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="collection"></param>
		/// <param name="ctor"></param>
		/// <returns></returns>
		public static ICollection<TResult> ToHashSet<T,TResult>(this ICollection<T> collection, Func<T,TResult> ctor)
		{
			HashSet<TResult> result = new HashSet<TResult>();
			foreach (T item in collection)
			{
				result.Add(ctor(item));
			}
			return result;
		}
		#endregion

		#region Hash Codes
		/// <summary>
		/// This is a simple hashing function from Robert Sedgwicks Hashing in C book.
		/// Also, some simple optimizations to the algorithm in order to speed up
		/// its hashing process have been added. from: www.partow.net
		/// </summary>
		/// <param name="objects">array of objects, parameters that you are used to make up the object hash code</param>
		/// <returns>the hash code for the objects</returns>
		public static int RSHash(params object[] objects)
		{
			int a = 378551;
			int b = 63689;
			int hash = 0;

			unchecked
			{
				foreach (object obj in objects)
				{
					if (obj != null)
					{
						hash = hash * a + obj.GetHashCode();
						a = a * b;
					}
				}
			}
			return hash;
		}
		#endregion

		#region Tuples
		/// <summary>
		/// Sort a tuple with the same elements by a natural sort order using operator<
		/// </summary>
		/// <typeparam name="T">The type contained in the tuple</typeparam>
		/// <param name="tuple">The tuple to sort</param>
		/// <returns>The sorted tuple in natural sort order</returns>
		public static Tuple<T, T> Sort<T>(this Tuple<T, T> tuple)
		{
			T v1 = tuple.Item1;
			T v2 = tuple.Item2;
			if ((dynamic)v1 > v2)
			{
				T temp = v2;
				v2 = v1;
				v1 = temp;
			}
			return new Tuple<T, T>(v1, v2);
		}

		/// <summary>
		/// Sort a tuple with a key field in the tuple object
		/// </summary>
		/// <typeparam name="T">The storage type in the tuple, must be same or derived for both types</typeparam>
		/// <typeparam name="K">The type of the key parameter</typeparam>
		/// <param name="tuple">The tuple to sort</param>
		/// <param name="keyExtractor">The function that extracts the key from the object</param>
		/// <returns>The sorted tuple</returns>
		public static Tuple<T, T> Sort<T, K>(this Tuple<T, T> tuple, Func<T,K> keyExtractor)
		{
			T v1 = tuple.Item1;
			T v2 = tuple.Item2;
			K key1 = keyExtractor(v1);
			K key2 = keyExtractor(v2);
			if ((dynamic)key1 > key2)
			{
				T temp = v2;
				v2 = v1;
				v1 = temp;
			}
			return new Tuple<T, T>(v1, v2);
		}
		#endregion

		/// <summary>
		/// Get the property name of a property using Linq
		/// </summary>
		/// <typeparam name="T">The type of the input type</typeparam>
		/// <param name="expression">The lambda expression to parse</param>
		/// <returns>The name of the property if it exists, otherwise null</returns>
		public static string GetPropertyName<T>(Expression<Func<T, object>> expression)
		{
			if (expression != null)
			{
				MemberExpression me = expression.Body as MemberExpression;
				if (me != null)
				{
					return me.Member.Name;
				}
				UnaryExpression ue = expression.Body as UnaryExpression;
				if (ue != null)
				{
					MemberExpression me2 = ue.Operand as MemberExpression;
					return me2.Member.Name;
				}
			}
			return null;
		}

		#region Types
		/// <summary>
		/// Convert a type name to a pretty name creating any generic
		/// parameters as required for the type
		/// </summary>
		/// <param name="type">The type to create the pretty name for</param>
		/// <returns>A string representing the correct type name including any recursive generics</returns>
		public static string PrettyName(this Type type)
		{
			if (type.GetGenericArguments().Length == 0)
			{
				return type.Name;
			}
			var genericArguments = type.GetGenericArguments();
			var typeDefeninition = type.Name;
			var unmangledName = typeDefeninition.Substring(0, typeDefeninition.IndexOf("`"));
			return unmangledName + "<" + String.Join(",", genericArguments.Select(PrettyName)) + ">";
		}		
		#endregion

        #region IEnumerable Functions
        /// <summary>
		/// Return a distinct set ordered by the key selector
		/// </summary>
		/// <typeparam name="TSource">The type of the source list</typeparam>
		/// <typeparam name="TKey">The type of the key</typeparam>
		/// <param name="source">The source list to select the distinct elements from</param>
		/// <param name="keySelector">The selector used to distinguish unique elements</param>
		/// <returns>The list of distinct elements</returns>
		public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			HashSet<TKey> knownKeys = new HashSet<TKey>();
			foreach (TSource element in source)
			{
				if (knownKeys.Add(keySelector(element)))
				{
					yield return element;
				}
			}
		}

        /// <summary>
        /// Partition an enumerable into multiple enumerables based on a size
        /// </summary>
        /// <typeparam name="T">The type in the enumerable</typeparam>
        /// <param name="source">The collection of items to partition</param>
        /// <param name="size">The number of items to include in each partition</param>
        /// <returns>The collection of partitioned collections</returns>
        public static IEnumerable<IEnumerable<T>> Partition<T>(this IEnumerable<T> source, int size)
        {
            return source.Partition((a, i) => a.Count() <= size);
        }

        /// <summary>
        /// Partition a collection of items into groups based on a predicate function
        /// </summary>
        /// <typeparam name="T">The type of the item to partition</typeparam>
        /// <param name="source">The list of items to group on</param>
        /// <param name="predicate">A predicate function that is supplied the current group, 
        /// and the next item to attempt to add. 
        /// <para>Should return true if the item can be added to the group, 
        /// and false if the item cannot be added to the group</para></param>
        /// <returns>The group of groups resulting from the application of the predicate function</returns>
        public static IEnumerable<IEnumerable<T>> Partition<T>(this IEnumerable<T> source, Func<IEnumerable<T>, T, bool> predicate)
        {
            IList<T> array = new List<T>();
            foreach (T item in source)
            {
                if (!predicate(array, item))
                {
                    yield return array;
                    array = new List<T>();
                }
                array.Add(item);
            }
            if (array.Count > 0)
            {
                yield return array;
            }
        }

        /// <summary>
        /// Group a set of items based on a set of rules.  This function works on 
        /// sequentially processing a list of items, using the canFitPredicate function
        /// to determine the number that should fit into the current group.
        /// </summary>
        /// <typeparam name="T">The type of the item in the list</typeparam>
        /// <param name="source">The source list</param>
        /// <param name="canFitPredicate">A predicate to sequentially check each of the items from the source 
        /// to determine how many of its number will fit into a group</param>
        /// <param name="fullCount">A function to determine if the T item has exhausted all its available items</param>
        /// <param name="splitFunction">A function to split the T item into two component parts based on a 
        /// count of items to shift into the first group</param>
        /// <returns>The collection of groups</returns>
        public static IEnumerable<IEnumerable<T>> Partition<T>(
            this IEnumerable<T> source,
            Func<IEnumerable<T>, T, int> canFitPredicate,
            Func<T, int, bool> fullCount,
            Func<T, int, T> splitFunction)
        {
            IList<T> array = new List<T>();
            foreach (T item in source)
            {
                var count = canFitPredicate(array, item);
                if (count == 0)
                {
                    yield return array;
                    array = new List<T>();
                }
                else if (!fullCount(item, count))
                {
                    do
                    {
                        var newItem = splitFunction(item, count);
                        array.Add(newItem);
                        yield return array;
                        array = new List<T>();
                        count = canFitPredicate(array, item);
                    }
                    while (!fullCount(item, count));
                }
                array.Add(item);
            }
            if (array.Count > 0)
            {
                yield return array;
            }
        }

        #endregion
    }
}
