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
 * @(#) NodeTravellingSalesmanPath.cs
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using AU.Com.BushLife.Utils;

namespace AU.Com.BushLife.AntColony
{
	/// <summary>
	/// Test class for a AntColony test using the travelling salesman
	/// algorithm
	/// </summary>
	[DebuggerDisplay("{DebuggerDisplay,nq}")]
	public class NodeTravellingSalesmanPath : INodePath
	{
		private bool InvalidPath = false;

		#region IPath<int> Members

		public decimal Score 
		{ 
			get 
			{ 
				return InvalidPath ? decimal.MaxValue : m_Score; 
			}
			set
			{
				m_Score = value;
			}
		}
		private decimal m_Score = 0;

		public ICollection<IStep> Steps { get; set; }

		public bool Contains(IStep step)
		{
			return Steps.Where(e => e.Equals(step)).Count() > 0;
		}

		public void Add(IStep step, decimal score)
		{
			Steps.Add(step);
			Score += score;
		}

		[Obsolete("Not required", true)]
		public void Add(IStep startPoint, IStep endPoint, decimal score)
		{
			throw new NotImplementedException();
		}

		public void Reset()
		{
			Steps = new List<IStep>();
			InvalidPath = false;
		}

		public void Validate(object contextObject)
		{
			IAntColonyContext context = contextObject as IAntColonyContext;
			if (Steps.Count != (context.Steps.Count - 1))
				InvalidPath = true;
		}

		#endregion

		#region ICloneable Members

		public object Clone()
		{
			return MemberwiseClone();
		}

		#endregion

		#region Debugger Display
		internal string DebuggerDisplay
		{
			get
			{
				if (Steps == null || Steps.Count() == 0)
					return this.GetType().Name;
				return string.Format("{0} [{3}] {1},{2}", this.GetType().Name, string.Join(",", Steps.Select(e => (e as IStep).StepName)), Score);
			}
		}
		#endregion


		public override bool Equals(object obj)
		{
			if (obj == null || !(obj is NodeTravellingSalesmanPath))
				return false;
			NodeTravellingSalesmanPath rhs = obj as NodeTravellingSalesmanPath;
			return Score == rhs.Score &&
				Steps.SequenceEqual(rhs.Steps);
		}

		public override int GetHashCode()
		{
			return LanguageUtils.RSHash(Score, Steps);
		}
	}
}
