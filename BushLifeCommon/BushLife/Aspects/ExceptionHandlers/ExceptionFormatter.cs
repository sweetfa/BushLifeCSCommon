using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AU.Com.BushLife.Aspects.ExceptionHandlers
{
    /// <summary>
    /// Interface to provide a function reference for the exception logging attributes
    /// </summary>
    public interface ExceptionFormatter
    {
        /// <summary>
        /// Format an exception message as determined by your own specific requirements
        /// </summary>
        /// <param name="exception">The exception to have a message generated for</param>
        /// <param name="showExceptionDetail">If true the descriptive details of the exception will be shown</param>
        /// <returns>A string containing the message to display</returns>
        string FormatException(Exception exception, bool showExceptionDetail);
    }
}
