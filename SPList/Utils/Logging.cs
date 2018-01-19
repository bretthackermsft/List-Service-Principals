using System;
using System.Diagnostics;
using System.Text;

namespace Utils
{
    public static class Logging
    {
        public static void WriteToAppLog(string message)
        {
            WriteToAppLog(message, EventLogEntryType.Error, null);
        }

        public static void WriteToAppLog(string message, Exception ex)
        {
            WriteToAppLog(message, EventLogEntryType.Error, ex);
        }

        /// <summary>
        /// Writes an error entry to the Application log, Application Source. This is a fallback error writing mechanism.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="errorType">Type of error.</param>
        /// <param name="ex">Original exception (optional)</param>
        public static void WriteToAppLog(string message, EventLogEntryType errorType, Exception ex)
        {
            if (ex != null)
            {
                message += message + " (original error: " + ex.Source + "/" + ex.Message + "\r\nStack Trace: " +
                                ex.StackTrace + ")";
                if (ex.InnerException != null)
                {
                    message += "\r\nInner Exception: " + ex.GetBaseException();
                }
            }
            EventLog.WriteEntry("Application", message, errorType, 0);
        }

        #region helpers
        private static string FormatAlertMessage(string message, string source)
        {
            var res = new StringBuilder();
            res.AppendFormat("<p>The following error occured at {0} (server time):</p>", DateTime.Now);
            res.AppendLine("    <blockquote>");
            res.AppendFormat("      Message: {0}<br>", message);
            res.AppendFormat("      Source: {0}<br>", source);
            res.AppendLine("    </blockquote>");
            return Utils.GetHtmlMessageWrapper("Application Alert", res.ToString());
        }
        #endregion
    }
}
