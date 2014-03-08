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
 * @(#) HibernateTransactionAspect.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.ExceptionServices;

using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Aspects.Dependencies;

using NHibernate;

using AU.Com.BushLife.Persistence.Hibernate;

namespace AU.Com.BushLife.Aspects.Hibernate
{
	/// <summary>
	/// An aspect to provide hibernate session and transaction management.
	/// <para>Remember that the duration of a session implies that you 
	/// will not see any other changes to the database until a new
	/// session is created</para>
	/// </summary>
	[Serializable]
	[HibernateTransactionAspect(AttributeExclude=true)]
	[ProvideAspectRole(StandardRoles.Persistence)]
	public sealed class HibernateTransactionAspect : OnMethodBoundaryAspect, IDisposable
	{
		ISession m_session;

		[IntroduceMember(OverrideAction=MemberOverrideAction.OverrideOrFail)]
		public ISession Session 
		{
			get
			{
				if (m_session == null || !m_session.IsOpen)
					m_session = HibernateSessionFactory.Instance.SessionFactory.OpenSession();
				return m_session;
			}
		}

		ITransaction Transaction { get; set; }

		public override void OnEntry(MethodExecutionArgs args)
		{
			Transaction = Session.BeginTransaction();
		}

		public override void OnSuccess(MethodExecutionArgs args)
		{
			if (Transaction.IsActive)
				Transaction.Commit();
		}

		public override void OnException(MethodExecutionArgs args)
		{
			if (Transaction.IsActive)
				Transaction.Rollback();
		}

		public override void OnExit(MethodExecutionArgs args)
		{
			if (Transaction.IsActive)
			{
				Transaction.Rollback();
				throw new Exception("Transaction not completed correctly");
			}
		}

		#region IDisposable Members

		[HandleProcessCorruptedStateExceptions]
		public void Dispose()
		{
			if (Transaction != null)
			{
				if (Transaction.IsActive)
				{
					Transaction.Rollback();
					throw new Exception("Transaction not completed correctly");
				}
				Transaction.Dispose();
				Transaction = null;
			}
			if (m_session != null)
			{
				if (m_session.IsOpen)
					m_session.Close();
				m_session.Dispose();
				m_session = null;
			}
		}

		#endregion
	}
}
