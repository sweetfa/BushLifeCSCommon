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
 * @(#) AntColonyContext.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AU.Com.BushLife.Framework.Clone;
using AU.Com.BushLife.Utils;

namespace AU.Com.BushLife.AntColony
{
	/// <summary>
	/// Context used to pass information to an ant colony and it's ants
	/// </summary>
	public class AntColonyContext : IAntColonyContext, ICloneable, IPolymorphicClone
	{
		/// <summary>
		/// The id of the ant that owns this context.
		/// <para>The initial context will have zero for this value</para>
		/// <para>All other context clones will have the id of the ant</para>
		/// </summary>
		public Int32 Id { get; set; }

		/// <summary>
		/// The available steps an ant can forage over
		/// </summary>
		public ICollection<IStep> Steps
		{
			get
			{
				if (m_Steps == null)
					m_Steps = new List<IStep>();
				return m_Steps;
			}
			set { m_Steps = value; }
		}
		private ICollection<IStep> m_Steps = null;

		public ICollection<IPath> BestPaths { get; set; }
		public IPath Path { get; set; }

		public virtual void Reset() 
		{
			Path.Reset();
		}

		#region ICloneable Members

		public virtual object Clone()
		{
			AntColonyContext result = new AntColonyContext();
			CopyInto(result);
			return result;
		}

		#endregion

		#region IPolymorphicClone Members

		public virtual void CopyInto<T>(T parentObject)
			where T : class
		{
			AntColonyContext context = parentObject as AntColonyContext;
			if (context == null)
				throw new InvalidCastException(string.Format("Cannot CopyInto type of [{0}]", parentObject.GetType().FullName));

			if (Path != null)
				context.Path = Path.Clone() as IPath;
			if (Steps != null)
				context.Steps = Steps.CloneDeepCollection();
			if (BestPaths != null)
				context.BestPaths = BestPaths.CloneDeepCollection();
		}

		#endregion
	}
}
