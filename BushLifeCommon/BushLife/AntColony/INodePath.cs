// <copyright file="IPath.cs" company="Bush Life Pty Limited">
// Copyright (c) 2012 Bush Life Pty Limited. All rights reserved.
// </copyright>

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
