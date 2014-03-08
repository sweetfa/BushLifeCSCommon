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
 * @(#) TransactionScopeAspect.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;


using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;
using PostSharp.Collections;
using PostSharp.Constraints;
using PostSharp.Extensibility;
using PostSharp.Reflection;


namespace AU.Com.BushLife.Aspects
{
	/// <summary>
	/// Aspect for wrapping functionality within a NEW transaction scope.
	/// </summary>
	[Serializable]
	class TransactionScopeAspect : OnMethodBoundaryAspect
	{
		[NonSerialized]
		TransactionScope TransactionScope;

		/// <summary>
		/// Create a new transaction
		/// </summary>
		/// <param name="args">The arguments for the method to be executed</param>
		public override void OnEntry(MethodExecutionArgs args)
		{
			this.TransactionScope = new TransactionScope(TransactionScopeOption.RequiresNew);
		}

		/// <summary>
		/// Complete the transaction on successful handling of the join-point
		/// </summary>
		/// <param name="args"></param>
		public override void OnSuccess(MethodExecutionArgs args)
		{
			this.TransactionScope.Complete();
		}

		/// <summary>
		/// Roll back the transaction on receipt of an exception during processing
		/// </summary>
		/// <param name="args"></param>
		public override void OnException(MethodExecutionArgs args)
		{
			args.FlowBehavior = FlowBehavior.Continue; 
			Transaction.Current.Rollback();
		}

		/// <summary>
		/// Dispose of the transaction when all is done and packed away
		/// </summary>
		/// <param name="args"></param>
		public override void OnExit(MethodExecutionArgs args)
		{
			this.TransactionScope.Dispose();
		}
	}
}
