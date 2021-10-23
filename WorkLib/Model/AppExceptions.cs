using System;
using System.Runtime.Serialization;

namespace WorkLib.Model
{

    /// <summary>
    /// Base exception for this app.
    /// </summary>
    /// <seealso cref="System.Exception" />
    [Serializable]
    public class AppException : Exception
    {
        #region Constructors

        /// <summary>
        ///   <para>
        /// Initializes a new instance of the <see cref="AppException" /> class.
        /// </para>
        /// </summary>
        public AppException() { }
        /// <summary>Initializes a new instance of the <see cref="AppException" /> class.</summary>
        /// <param name="message">The message that describes the error.</param>
        public AppException(string message) : base(message) { }
        /// <summary>Initializes a new instance of the <see cref="AppException" /> class.</summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="inner">The inner exception.</param>
        public AppException(string message, Exception inner) : base(message, inner) { }
        protected AppException(
          SerializationInfo info,
          StreamingContext context) : base(info, context) { }

        #endregion

        #region Message manipulations

        /// <summary>Gets the full message and includes all inner exceptions separated by new lisne.</summary>
        /// <value>The full message.</value>
        public string FullMessage => GetFullMessage();

        /// <summary>
        /// Gets the full message.
        /// </summary>
        /// <param name="rowSeparator">The row separator (default new line).</param>
        /// <param name="ex">The exception (default null)</param>
        /// <returns></returns>
        public string GetFullMessage(string rowSeparator = "\r\n", Exception ex = null)
        {
            if (ex == null)
                ex = this;
            var msg = ex.Message;
            if (ex.InnerException != null)
                msg += rowSeparator + GetFullMessage(rowSeparator, ex.InnerException);
            return msg;
        }
        #endregion
    }

}