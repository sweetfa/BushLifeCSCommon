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
 * @(#) Indexer.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AU.Com.BushLife.Utils
{
	/// <summary>
	/// An indexer that allows sequential number allocation with
	/// ability to return numbers to the pool as required for reuse
	/// </summary>
	public class Indexer
	{
		private bool ZeroBased;
		private Queue<int> FreePool = new Queue<int>();

		/// <summary>
		/// Standard default constructor
		/// </summary>
		/// <param name="zeroBased">If set to true first value returned is zero, 
		/// otherwise one is the first value returned by the indexer</param>
		public Indexer(bool zeroBased = false)
		{
			ZeroBased = zeroBased;
			if (ZeroBased)
				m_Index = -1;
		}

		/// <summary>
		/// Get the next index value
		/// </summary>
		public int NextIndex
		{
			get
			{
				if (FreePool.Count > 0)
					return FreePool.Dequeue();
				else
					return ++m_Index;
			}
		}
		private int m_Index;

		/// <summary>
		/// Get the current index value;
		/// </summary>
		public int CurrentIndex
		{
			get
			{
				return m_Index;
			}
		}

		/// <summary>
		/// Return an index value back to the free pool
		/// </summary>
		/// <param name="returnedIndex">The index value to return</param>
		public void Return(int returnedIndex)
		{
			FreePool.Enqueue(returnedIndex);
		}

		/// <summary>
		/// Reset the index counter to zero
		/// </summary>
		public void Reset()
		{
			m_Index = 0;
			if (ZeroBased)
				m_Index = -1;
			FreePool = new Queue<int>();
		}

		/// <summary>
		/// Set the next index value to use
		/// <para>Can be used to restart an index during operations</para>
		/// </summary>
		/// <param name="nextIndex"></param>
		public void SetNext(int nextIndex)
		{
			m_Index = nextIndex - 1;
		}

	}
}
