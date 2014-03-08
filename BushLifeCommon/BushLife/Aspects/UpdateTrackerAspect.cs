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
 * @(#) UpdateTrackerAspect.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;

using AU.Com.BushLife.Attributes;
using AU.Com.BushLife.Persistence;
using AU.Com.BushLife.Utils;

namespace AU.Com.BushLife.Aspects
{
	/// <summary>
	/// Aspect to update instances of ITrackerUpdate with the
	/// current date and user
	/// </summary>
	[Serializable]
	[UpdateTrackerAspect(AttributeExclude= true)]
	[ProvideAspectRole(StandardRoles.Persistence)]
	[AspectRoleDependency(AspectDependencyAction.Conflict, AspectDependencyPosition.Before, StandardEffects.Custom)]
	public sealed class UpdateTrackerAspect : OnMethodBoundaryAspect
	{
		public override void OnEntry(MethodExecutionArgs args)
		{
			// Only do this if there is a single argument
			if (args.Arguments.Count == 1)
			{
				object obj = args.Arguments[0];
				Process(obj);
			}
		}

		/// <summary>
		/// Process the object to determine if it has the UpdateTrackerAttribute set
		/// and if so update the tracking information.  Also check if any properties 
		/// within the object have the UpdateTracker attributes set
		/// </summary>
		/// <param name="obj">The object to process</param>
		private void Process(object obj)
		{
			// Update the object in question if required first
			if (obj.GetType().IsDefined(typeof(UpdateTrackerAttribute), false))
				UpdateTrackingInformation(obj as IUpdaterTracking);

			IList<PropertyInfo> properties = obj.GetType().GetProperties();
			foreach (PropertyInfo property in properties)
			{
				if (property.IsDefined(typeof(UpdateTrackerAttribute), true))
				{
					if (obj is IUpdaterTracking)
						UpdateTrackingInformation(obj as IUpdaterTracking);
					else
						throw new ArgumentException(
							string.Format("Property defined with {0} does not implement the {1} interface",
							typeof(UpdateTrackerAttribute).Name,
							typeof(IUpdaterTracking).Name));
				}
				if (property.IsDefined(typeof(UpdateTrackerCollectionAttribute), true))
				{
					object propertyObject = property.GetValue(obj, null);
					foreach (object o in (IEnumerable<object>)propertyObject)
						Process(o);
				}
			}
		}

		/// <summary>
		/// Update the tracking information if the object supplied is not null
		/// </summary>
		/// <param name="trackedObject"></param>
		private void UpdateTrackingInformation(IUpdaterTracking trackedObject)
		{
			if (trackedObject != null)
			{
				trackedObject.UpdatedAt = DateTime.Now;
				trackedObject.UpdatedBy = WinUtils.GetPrincipal().Identity.Name;
			}
		}
	}
}
