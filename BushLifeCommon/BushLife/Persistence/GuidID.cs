// <copyright file="GuidID.cs" company="Bush Life Pty Limited">
// Copyright (c) 2012 Bush Life Pty Limited. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AU.Com.BushLife.Aspects.Hibernate;
using AU.Com.BushLife.Utils;

namespace AU.Com.BushLife.Persistence
{
	/// <summary>
	/// Base class for persistence classes that utilise 
	/// a Guid as the unique identifier
	/// </summary>
	public abstract class GuidID : GuidIDInterface
	{
		#region GuidIDInterface Members

		/// <summary>
		/// Unique identifier for the record
		/// </summary>
		public virtual Guid Id { get; set; }

		#endregion

		public override bool Equals(object obj)
		{
			if (obj == null) 
				return false;
			if (obj is GuidIDInterface)
				return (obj as GuidIDInterface).Equals(this);
			return false;
		}

		public override int GetHashCode()
		{
			return LanguageUtils.RSHash(Id);
		}

		#region IEquatable<GuidIDInterface> Members

		public virtual bool Equals(GuidIDInterface other)
		{
			return this.Id.Equals(other.Id);
		}

		#endregion

		#region IEqualityComparer<GuidIDInterface> Members

		public virtual bool Equals(GuidIDInterface x, GuidIDInterface y)
		{
			return x.Equals(y);
		}

		public virtual int GetHashCode(GuidIDInterface obj)
		{
			return obj.GetHashCode();
		}

		#endregion


		#region ICloneable Members

		public virtual object Clone()
		{
			return MemberwiseClone();
		}

		#endregion
	}
}
