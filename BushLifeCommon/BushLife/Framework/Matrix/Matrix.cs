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
 * @(#) Matrix.cs
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AU.Com.BushLife.Framework.Matrix
{
	/// <summary>
	/// A two dimensional matrix for storing of values.
	/// <para>Where the types of the horizontal and vertical
	/// indexes are the same the matrix will be populated
	/// with the reverse key values as well</para>
	/// </summary>
	[DebuggerDisplay("Matrix ({MatrixMap.Count}]")]
	public class Matrix<HType,VType,T> : ICloneable
		where HType : class
		where VType : class
	{
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		private IDictionary<MatrixKey<HType, VType>, T> MatrixMap = new Dictionary<MatrixKey<HType, VType>, T>();

		/// <summary>
		/// Index into matrix based on the horizontal index only
		/// </summary>
		/// <param name="hKey">The horizontal index value</param>
		/// <returns>The enumeration of vertical index keys and the associated value</returns>
		public IEnumerable<KeyValuePair<VType, T>> this[HType hKey]
		{
			get
			{
				return MatrixMap.Where(kvp => kvp.Key.HorizontalKey.Equals(hKey))
					.Select(kvp => new KeyValuePair<VType, T>(kvp.Key.VerticalKey, kvp.Value));
			}
		}

		/// <summary>
		/// Allow indexing into the matrix using the horizontal and vertical index
		/// references as parameters without requiring key construction
		/// </summary>
		/// <param name="hKey">The horizontal index value</param>
		/// <param name="vKey">The vertical index value</param>
		/// <returns>The value in the matrix at the particular reference point</returns>
		public T this[HType hKey, VType vKey]
		{
			get 
			{
				MatrixKey<HType, VType> key = new MatrixKey<HType, VType>(hKey, vKey);
				return this[key];
			}
			private set
			{
				MatrixKey<HType, VType> key = new MatrixKey<HType, VType>(hKey, vKey);
				this[key] = value;
			}
		}

		/// <summary>
		/// Allow indexing into the matrix based on the matrix key
		/// <para>i.e. Matrix<</para>
		/// <example>Matrix<T1,T2,V> mat = new Matrix<T1,T2,V>();
		/// var key = new MatrixKey<T1,T2>(hval, vval);
		/// V x = mat[key];
		/// mat[key] = x;</example>
		/// </summary>
		/// <param name="key">The key for the matrix</param>
		/// <returns>The value in the matrix at the particular key reference point</returns>
		public T this[MatrixKey<HType, VType> key]
		{
			get
			{
				return MatrixMap[key];
			}
			private set
			{
				MatrixMap[key] = value;
			}
		}

		/// <summary>
		/// Add a value into the matrix based on the supplied key values.
		/// <para>If both key types are of the same base type then
		/// a reverse key entry will also be added into the matrix</para>
		/// </summary>
		/// <param name="hKey">The key for the horizontal axis of the matrix</param>
		/// <param name="vKey">The key for the vertical axis of the matrix</param>
		/// <param name="value">The value to be stored in the matrix at the keyed position</param>
		public void Add(HType hKey, VType vKey, T value)
		{
			MatrixKey<HType, VType> key = new MatrixKey<HType, VType>(hKey, vKey);
			MatrixMap[key] = value;
			// If both keys are of the same type then also add them reversed into the matrix
			if (hKey is VType)
			{
				key = new MatrixKey<HType, VType>(vKey as HType, hKey as VType);
				MatrixMap[key] = value;
			}
		}


		public IEnumerable<HType> HorizontalKeys()
		{
			return MatrixMap.Keys.Select(k => k.HorizontalKey);
		}

		public IEnumerable<VType> VerticalKeys()
		{
			return MatrixMap.Keys.Select(k => k.VerticalKey);
		}

		public IEnumerable<MatrixKey<HType, VType>> Keys()
		{
			return MatrixMap.Keys;
		}

		public IEnumerable<T> SelectAll()
		{
			IEnumerable<T> result = MatrixMap.Select(f => f.Value);
			return result;
		}



		#region ICloneable Members

		/// <summary>
		/// Shallow clone of the object
		/// </summary>
		/// <returns></returns>
		public object Clone()
		{
			return MemberwiseClone();
		}

		#endregion

	}
}
