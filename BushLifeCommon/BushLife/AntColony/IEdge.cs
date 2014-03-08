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
 * @(#) IEdge.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace AU.Com.BushLife.AntColony
{
	/// <summary>
	/// An edge is a traversal from one point to another,
	/// a step from a starting step to the next reachable
	/// step from the starting step.
	/// <para>A score is associated with this edge indicating
	/// if it is a favourable or less favourable traversal. 
	/// Higher scores are considered less favourable</para>
	/// </summary>
	public interface IEdge : IPheremoneKey, IComparable
	{
		/// <summary>
		/// The step at which this traversal originates
		/// </summary>
		IStep StartStep { get; set; }
		/// <summary>
		/// The step at which this traversal reaches next
		/// </summary>
		IStep EndStep { get; set; }

		/// <summary>
		/// The equals method must be overridden for this object
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		bool Equals(object obj);

		/// <summary>
		/// The GetHashCode method must be overridden for this object
		/// </summary>
		/// <returns></returns>
		int GetHashCode();
	}
}
