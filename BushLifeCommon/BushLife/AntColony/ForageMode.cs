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
 * @(#) ForageMode.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AU.Com.BushLife.AntColony
{
	/// <summary>
	/// The mode that the ant uses to forage around the graph
	/// </summary>
	public enum ForageMode
	{
		/// <summary>
		/// Randomly wander around the graph.  All graph nodes
		/// can be reached from all other graph nodes
		/// </summary>
		RandomWandering,
		/// <summary>
		/// Randomly wander around the graph.  Each graph node
		/// has specific nodes it can travel to next
		/// </summary>
		DirectedGraph
	}

}
