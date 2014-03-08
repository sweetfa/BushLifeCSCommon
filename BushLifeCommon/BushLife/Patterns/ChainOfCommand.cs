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
 * @(#) ChainOfCommand.cs
 */

using System.Collections.Generic;

namespace AU.Com.BushLife.Patterns
{
	/// <summary>
	/// <para>Provide an implementation of the GOF Chain Of Command pattern</para>
	/// <para> Execute an ordered list of ICommand class instances aborting early if a command generates a failure</para>
	/// </summary>
	/// <typeparam name="T">The type of the context passed between the commands</typeparam>
	public sealed class ChainOfCommand<T> : ICommand<T>
		where T : ICommandContext
    {
		/// <summary>
		/// The list of commands applicable to this chain
		/// </summary>
        ICollection<ICommand<T>> commands { get; set; }

		/// <summary>
		/// Constructor requiring the list of commands to Execute
		/// </summary>
		/// <param name="commands">The list of commands for this particular command chain</param>
        public ChainOfCommand(ICollection<ICommand<T>> commands)
        {
            this.commands = commands;
        }

		/// <summary>
		/// <para>Execute an ordered list of commands without allowing the context object to be updated</para>
		/// <para>Execute each command in the ordered list unless one of the commands indicates a failure has occurred, in which case return early.</para>
		/// <para>The context object may be updated in this instance to provide detailed information on the nature of the failure</para>
		/// </summary>
		/// <param name="context">The context used to pass information to the separate commands and return</param>
		/// <returns>IResult.Successful if all commands executed without returning failure indication (IResult.Failed), otherwise IResult.Failed</returns>
        public IResult Execute(T context)
        {
            foreach (ICommand<T> command in commands)
            {
				if (context != null)
					context.CurrentProcessingStep = command.GetType();
                IResult result = command.Execute(context);
                if (result != IResult.Successful)
                {
                    return result;
                }
            }
            return IResult.Successful;
        }
    }
	
}
