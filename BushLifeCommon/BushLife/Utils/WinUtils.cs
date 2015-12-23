﻿/**
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
 * @(#) WinUtils.cs
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Drawing.Printing;

using System.Security.Principal;

using AU.Com.BushLife.Patterns;
using System.DirectoryServices.AccountManagement;
using System.Net.NetworkInformation;

namespace AU.Com.BushLife.Utils
{
    public class WinUtils
    {
		/// <summary>
		/// Context passed between dialogs when utilising
		/// the action based processing provided by 
		/// the DisplayDialog methods
		/// </summary>
		/// <typeparam name="T">The dialog type being displayed</typeparam>
        public class DialogActionContext<T>  : ICommandContext
			where T : CommonDialog
        {
			/// <summary>
			/// The instantiated dialogue instance currently beingused
			/// </summary>
            public T dialogue { get; set; }

			/// <summary>
			/// Any additional information that needs to be passed to the dialog
			/// </summary>
            public object additional { get; set; }

			/// <summary>
			/// The current stage of processing
			/// </summary>
            public Stage currentStage { get; set; }

			/// <summary>
			/// The current stage of the processing
			/// </summary>
            public enum Stage
            {
				/// <summary>
				/// Before the dialog has been displayed
				/// </summary>
                PreDisplay,

				/// <summary>
				/// After the dialog has been displayed
				/// </summary>
                PostDisplay
            }

			#region ICommandContext Members
			/// <summary>
			/// The current processing step
			/// </summary>
			public Type CurrentProcessingStep { get; set; }
			#endregion
		}

		/// <summary>
		/// Context passed between forms when utilising
		/// the action based processing provided by
		/// the DisplayForm methods
		/// </summary>
		/// <typeparam name="T">The form type being displayed</typeparam>
        public class FormActionContext<T> : ICommandContext
			where T : Form
        {
            public T dialogue { get; set; }
            public object additional { get; set; }
            public Stage currentStage { get; set; }

            public enum Stage
            {
                PreDisplay,
                PostDisplay
            }
			#region ICommandContext Members
			/// <summary>
			/// The current processing step
			/// </summary>
			public Type CurrentProcessingStep { get; set; }
			#endregion
		}

		#region Form Assistance
		/// <summary>
		/// Display a form whilst appropriately handling
		/// the result code.
		/// </summary>
		/// <param name="dialogType">The type of the form to display</param>
		/// <returns>The dialog result as generated by the dialog</returns>
		/// <exception cref="System.InvalidOperationException">Method does not exist for this object</exception>
		/// <exception cref="System.ArgumentNullException">Something is not quite right with the arguments being processed</exception>
		public static DialogResult DisplayForm(Type formType)
		{
			MethodInfo method = typeof(WinUtils).GetMethod("DisplayForm", System.Type.EmptyTypes);
			MethodInfo generic = method.MakeGenericMethod(formType);
			return (DialogResult)generic.Invoke(null, null);
		}

		/// <summary>
		/// Display a form and provide an action handler to be executed before and after the 
		/// form display.  Provides an interceptor pattern approach to screen design
		/// </summary>
		/// <param name="formType">The type of the form to display</param>
		/// <param name="action">The command object to execute before and after form display</param>
		/// <returns>The result from the form</returns>
		/// <exception cref="System.InvalidOperationException">Method does not exist for this object</exception>
		/// <exception cref="System.ArgumentNullException">Something is not quite right with the arguments being processed</exception>
		public static DialogResult DisplayForm(Type formType, ICommand<ICommandContext> action)
		{
			// Get the generic method that matches the number of arguments for this method
			MethodInfo method = typeof(WinUtils).GetMethods().Where(item => item.IsGenericMethod && item.GetParameters().Count() == 1 && item.Name.Equals("DisplayForm")).Single();
			MethodInfo generic = method.MakeGenericMethod(formType);
			return (DialogResult)generic.Invoke(null, new object[] { action });
		}

		/// <summary>
		/// Display a form and provide an action handler to be executed before and after the 
		/// form display.  Provides an interceptor pattern approach to screen design
		/// </summary>
		/// <param name="formType">The type of the form to display</param>
		/// <param name="action">The command object to execute before and after form display</param>
		/// <param name="executionContext">A context to pass information to the action</param>
		/// <returns>The result from the form</returns>
		/// <exception cref="System.InvalidOperationException">Method does not exist for this object</exception>
		/// <exception cref="System.ArgumentNullException">Something is not quite right with the arguments being processed</exception>
		public static DialogResult DisplayForm(Type formType, object action, object executionContext)
		{
			// Get the generic method that matches the number of arguments for this method
			MethodInfo method = typeof(WinUtils).GetMethods().Where(item => item.IsGenericMethod && item.GetParameters().Count() == 2 && item.Name.Equals("DisplayForm")).Single();
			MethodInfo generic = method.MakeGenericMethod(formType);
			return (DialogResult)generic.Invoke(null, new object[] { action, executionContext });
		}

		/// <summary>
		/// Display a form whilst appropriately handling
		/// the result code.
		/// <typeparam name="T">The type of the form to display</typeparam>
		/// <returns>The dialog result as generated by the dialog</returns>
		public static DialogResult DisplayForm<T>() where T : Form, new()
        {
            return DisplayForm<T>(null, null);
        }

		/// <summary>
		/// Display a form and provide an action handler to be executed before and after the 
		/// form display.  Provides an interceptor pattern approach to screen design
		/// </summary>
		/// <typeparam name="T">The type of the form to display</typeparam>
		/// <param name="action">The command object to execute before and after form display</param>
		/// <returns>The dialog result as generated by the dialog</returns>
		public static DialogResult DisplayForm<T>(ICommand<FormActionContext<T>> action) where T : Form, new()
        {
            return DisplayForm<T>(action, new FormActionContext<T>());
        }

		/// <summary>
		/// Display a form and provide an action handler to be executed before and after the 
		/// form display.  Provides an interceptor pattern approach to screen design
		/// </summary>
		/// <typeparam name="T">The type of the form to display</typeparam>
		/// <param name="action">The command object to execute before and after form display</param>
		/// <param name="executionContext">A context to pass information to the action</param>
		/// <returns>The dialog result as generated by the dialog</returns>
		public static DialogResult DisplayForm<T>(ICommand<FormActionContext<T>> action, FormActionContext<T> executionContext) where T : Form, new()
        {
            using (T sfDialog = (T)(object)new T())
            {
				if (executionContext != null)
					executionContext.dialogue = sfDialog;
				
				// Run the pre display hooks if any
                if (action != null)
                {
                    executionContext.currentStage = FormActionContext<T>.Stage.PreDisplay;
                    IResult actionResult = action.Execute(executionContext);
                    if (actionResult != IResult.Successful)
                    {
						if (actionResult == IResult.Cancelled)
							return DialogResult.Cancel;
                        return DialogResult.Abort;
                    }
                }
                // Show the real form to the user
                DialogResult dlgResult = sfDialog.ShowDialog();
                // Check the user response to the form
                if (dlgResult == DialogResult.OK)
                {
                    // Run the post display hooks if any
                    if (action != null)
                    {
                        executionContext.currentStage = FormActionContext<T>.Stage.PostDisplay;
                        IResult actionResult = action.Execute(executionContext);
						if (actionResult == IResult.Successful)
							return DialogResult.OK;
						else if (actionResult == IResult.Cancelled)
							return DialogResult.Cancel;
						else
							return DialogResult.Abort;
                    }
                    else
                    {
                        return dlgResult;
                    }
                }
                else
                {
                    return dlgResult;
                }
            }
        }
		#endregion

		#region Dialog Assistance
		/// <summary>
		/// Display a dialog whilst appropriately handling
		/// the result code.
		/// </summary>
		/// <param name="dialogType">The type of the dialog to display</param>
		/// <returns>The dialog result as generated by the dialog</returns>
		public static DialogResult DisplayDialog(Type dialogType)
		{
			MethodInfo method = typeof(WinUtils).GetMethod("DisplayDialog",System.Type.EmptyTypes);
			MethodInfo generic = method.MakeGenericMethod(dialogType);
			return (DialogResult) generic.Invoke(null, null);
		}

		public static DialogResult DisplayDialog<T>() where T : CommonDialog, new()
        {
            return DisplayDialog<T>(null, null);
        }

        public static DialogResult DisplayDialog<T>(ICommand<DialogActionContext<T>> action) where T : CommonDialog, new()
        {
            return DisplayDialog<T>(action, new DialogActionContext<T>());
        }

        public static DialogResult DisplayDialog<T>(ICommand<DialogActionContext<T>> action, DialogActionContext<T> executionContext) where T : CommonDialog, new()
        {
            using (T sfDialog = (T)(object)new T())
            {
                // Run the pre display hooks if any
                if (action != null)
                {
                    executionContext.dialogue = sfDialog;
                    executionContext.currentStage = DialogActionContext<T>.Stage.PreDisplay;
                    IResult actionResult = action.Execute(executionContext);
                    if (actionResult != IResult.Successful)
                    {
                        return DialogResult.Abort;
                    }
                }
                // Show the real form to the user
                DialogResult dlgResult = sfDialog.ShowDialog();
                // Check the user response to the form
                if (dlgResult == DialogResult.OK)
                {
                    // Run the post display hooks if any
                    if (action != null)
                    {
                        executionContext.currentStage = DialogActionContext<T>.Stage.PostDisplay;
                        IResult actionResult = action.Execute(executionContext);
                        if (actionResult == IResult.Successful)
                        {
                            return DialogResult.OK;
                        }
                        else
                        {
                            return DialogResult.Abort;
                        }
                    }
                    else
                    {
                        return dlgResult;
                    }
                }
                else
                {
                    return dlgResult;
                }
            }
        }
		#endregion

		#region Security

		#region Authentication
        /// <summary>
        /// Get the authentication principal for the current user
        /// </summary>
        /// <returns>The principal for the user</returns>
		public static IPrincipal GetPrincipal()
		{
			return GetPrincipal(WindowsIdentity.GetCurrent());
		}

        /// <summary>
        /// Get the authentication principal for the specified identity
        /// </summary>
        /// <param name="identity">The identity to get the principal for</param>
        /// <returns>The principal for the user</returns>
		public static IPrincipal GetPrincipal(WindowsIdentity identity)
		{
			IPrincipal principal = new WindowsPrincipal(identity);
			return principal;
		}

		/// <summary>
		/// Get the name of the current user in the form DOMAINNAME\UserName
		/// </summary>
		/// <returns></returns>
		public static string GetCurrentUserName()
		{
			IPrincipal principal = GetPrincipal();
			IIdentity identity = principal.Identity;
			return identity.Name;
		}

		/// <summary>
		/// Get the name of the current windows user
		/// without the domain component
		/// </summary>
		/// <returns></returns>
		public static string GetCurrentUserNameWithoutDomain()
		{
			string[] parts = GetCurrentUserName().Split('\\');
			if (parts.Length > 1)
				return parts[1];
			return parts[0];
		}

        /// <summary>
        /// Get the current user email address
        /// </summary>
        /// <returns></returns>
        public static string CurrentUserEmailAddress()
        {
            var userPrincipal = WinUtils.GetPrincipal() as UserPrincipal;
            if (userPrincipal != null)
            {
                if (userPrincipal.EmailAddress != null && userPrincipal.EmailAddress.Length > 0)
                    return userPrincipal.EmailAddress;
            }
            var username = WinUtils.GetCurrentUserNameWithoutDomain();
            var domain = IPGlobalProperties.GetIPGlobalProperties();
            return string.Format("{0}@{1}.{2}", username, domain.HostName, domain.DomainName);
        }

        #endregion

		#region Authorisation
		public static bool IsUserInRole(IPrincipal principal, string rolename)
		{
			return principal.IsInRole(rolename);
		}
		#endregion

		#endregion

		#region Directory Utilities
		public static ICollection<string> GetFileNames(string path)
		{
			string[] files = Directory.GetFiles(path);
			return files.ToList();
		}
		#endregion

        #region Printer Utilities
        /// <summary>
        /// Get the list of printer names that the system has access to
        /// </summary>
        /// <returns>The list of printers</returns>
        public static IEnumerable<String> GetPrinterNames()
        {
            foreach (var printer in PrinterSettings.InstalledPrinters)
                yield return printer.ToString();
        }
        #endregion
    }
}
