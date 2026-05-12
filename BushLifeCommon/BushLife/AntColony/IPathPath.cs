// <copyright file="IPathPath.cs" company="Bush Life Pty Limited">
// Copyright (c) 2012 Bush Life Pty Limited. All rights reserved.
// </copyright>

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
