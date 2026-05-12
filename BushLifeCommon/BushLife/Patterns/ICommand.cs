// <copyright file="Command.cs" company="Bush Life Pty Limited">
// Copyright (c) 2012 Bush Life Pty Limited. All rights reserved.
// </copyright>

namespace AU.Com.BushLife.Patterns
{
	/// <summary>
	/// Possible result values from the command pattern
	/// </summary>
	public enum IResult
    {
		/// <summary>
		/// The command failed completely
		/// </summary>
        Failed,

		/// <summary>
		/// The command succeeded
		/// </summary>
        Successful,

		/// <summary>
		/// The command has been cancelled by the user or for some other reason
		/// </summary>
		Cancelled
    }

	/// <summary>
	/// A strategic command pattern to support GOF command pattern.
	/// <para>T is used to define the context information to pass to/fro and between commands</para>
	/// </summary>
	/// <typeparam name="T">the context class to use</typeparam>
	public interface ICommand<T>
		where T : ICommandContext
	{
		/// <summary>
		/// The main entry point for a concrete client class to invoke
		/// this command.
		/// <para>Use the context to pass information between commands</para>
		/// </summary>
		/// <param name="executionContext">The information required for the command to process.  This would be relative to the instance where the command processor was invoked</param>
		/// <returns>A success or failure result</returns>
        IResult Execute(T executionContext);
	}
	
}
