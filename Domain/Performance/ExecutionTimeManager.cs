using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Domain.Performance
{
    /// <summary>
    /// Summary description for ExecutionTimeManager.
    /// </summary>
    public sealed class ExecutionTimeManager
    {
        private static SortedList<string, ExecutionItem> ExecutionTimes;
        private static bool RecordExecutionTimes = false;
        private static bool SeparateExecutionTimesByThread = false;
        private static string EmptyThreadName = "NoThreadName";

        /// <summary>
        /// static constructor -- loads all settings
        /// </summary>
        static ExecutionTimeManager()
        {
            lock (typeof(ExecutionTimeManager))
            {
                RecordExecutionTimes = IsPropertyTrue("recordExecutionTimes");
                if (RecordExecutionTimes)
                {
                    ExecutionTimes = new SortedList<string, ExecutionItem>();
                    SeparateExecutionTimesByThread = IsPropertyTrue("separateExecutionTimesByThread");

                    string value = ConfigurationManager.AppSettings["emptyThreadName"];
                    if (value != null && value.Length > 0)
                    {
                        EmptyThreadName = value.Trim();
                    }
                }
            }
        }


        private static bool IsPropertyTrue(string propertyName)
        {
            string value = ConfigurationManager.AppSettings[propertyName];
            if (value != null && value.Length > 0)
            {
                return value.Trim().ToUpper().Equals("Y");
            }
            return false;
        }

        /// <summary>
        /// Cannot instantiate.
        /// </summary>
        private ExecutionTimeManager() { }

        /// <summary>
        /// Record execution to the list
        /// </summary>
        /// <param name="method">method name</param>
        /// <param name="begin">begin ticks</param>
        public static void RecordExecutionTime(string method, long begin)
        {
            RecordExecutionTime(method, begin, DateTime.Now.Ticks);
        }

        /// <summary>
        /// Record execution time to a list
        /// </summary>
        /// <param name="method">executed method</param>
        /// <param name="begin">being ticks</param>
        /// <param name="end">end ticks</param>
        public static void RecordExecutionTime(string method, long begin, long end)
        {
            if (ExecutionTimes != null)
            {
                lock (typeof(ExecutionTimeManager))
                {
                    if (SeparateExecutionTimesByThread)
                    {
                        string threadName = System.Threading.Thread.CurrentThread.Name;
                        if (threadName == null || threadName.Length == 0)
                        {
                            threadName = EmptyThreadName;
                        }
                        method = String.Format("{0} ({1})", method, threadName);
                    }

                    if (!ExecutionTimes.ContainsKey(method))
                    {
                        ExecutionTimes.Add(method, new ExecutionItem(method));
                    }
                    ((ExecutionItem)ExecutionTimes[method]).AppendTime(end - begin);
                }
            }
        }

        /// <summary>
        /// Get list of execution times key'd by the method name
        /// </summary>
        /// <returns>SortedList with method name as the key(string) and a list of ExecutionItem objects</returns>
        public static ExecutionItemCollection GetExecutionTimes()
        {
            if (ExecutionTimes != null)
            {
                lock (typeof(ExecutionTimeManager))
                {
                    return new ExecutionItemCollection(ExecutionTimes.Values);
                }
            }
            return new ExecutionItemCollection();
        }


        /// <summary>
        /// Notify whether or not Execution times are getting recorded
        /// </summary>
        /// <returns>true if application is using the feature, false otherwise</returns>
        public static bool IsRecording()
        {
            return RecordExecutionTimes;
        }

        /// <summary>
        /// Clear the execution items list
        /// </summary>
        public static void Clear()
        {
            if (ExecutionTimes != null)
            {
                ExecutionTimes.Clear();
            }
        }

    }
}
