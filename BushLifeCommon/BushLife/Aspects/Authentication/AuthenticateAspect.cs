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
 * @(#) AuthenticateAspect.cs
 */

using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using PostSharp.Aspects;
using AU.Com.BushLife.Utils;

namespace AU.Com.BushLife.Authentication
{
	/// <summary>
	/// Aspect to ensure role is available for current signed on windows user
	/// </summary>
	[Serializable]
	[AuthenticateAspect(AttributeExclude = true)]
	public class AuthenticateAspect : OnMethodBoundaryAspect
	{
		public override sealed void OnSuccess(MethodExecutionArgs args)
		{
			// TODO: Determine way of injecting this from AssemblyInfo.cs
			const string RoleName = "MonarchBSConfiguration";
			if (!WinUtils.IsUserInRole(WinUtils.GetPrincipal(), RoleName))
			{
				MessageBox.Show("No authorisation for " + WinUtils.GetPrincipal().Identity.Name + ", not in role " + RoleName, "Authorisation Failed");
				args.FlowBehavior = FlowBehavior.Return;
				args.ReturnValue = 1;
			}
			else
			{
				args.FlowBehavior = FlowBehavior.Continue;
				args.ReturnValue = 0;
			}
		}
	}
}
