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
 * @(#) IPathPath.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AU.Com.BushLife.AntColony
{
	/// <summary>
	///
	/// </summary>
	public interface IPathPath : IPath, ICloneable
	{
		ICollection<IEdge> Edges { get; set; }

		/// <summary>
		/// Add another step to the existing path
		/// </summary>
		/// <param name="nextNode">The next step node of the path</param>
		/// <param name="score">The score for the node from the current node</param>
		void Add(IStep currentNode, IStep nextNode, decimal score);
	}
}
