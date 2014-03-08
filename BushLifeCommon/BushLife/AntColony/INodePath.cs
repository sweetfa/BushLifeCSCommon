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
 * @(#) IPath.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AU.Com.BushLife.AntColony
{
	/// <summary>
	/// The path is the path of a single ant journey
	/// </summary>
	public interface INodePath : IPath, ICloneable
	{
		ICollection<IStep> Steps { get; set; }

		/// <summary>
		/// Add another step to the existing path
		/// </summary>
		/// <param name="nextNode">The next step node of the path</param>
		/// <param name="score">The score for the node from the current node</param>
		void Add(IStep nextNode, decimal score);

	}
}
