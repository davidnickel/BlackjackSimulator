using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Performance
{
    /// <summary>
    /// Knows information about execution time
    /// </summary>
    public class ExecutionTimeEventArgs : EventArgs
    {
        private string _className;

        /// <summary>
        /// Get/set the class name executing
        /// </summary>
        public string ClassName
        {
            get { return _className; }
            set { _className = value; }
        }

        private string _methodName;

        /// <summary>
        /// Get/set the executing method name
        /// </summary>
        public string MethodName
        {
            get { return _methodName; }
            set { _methodName = value; }
        }

        private long _begin;

        /// <summary>
        /// Get/set the time span at begining of execution
        /// </summary>
        public long Begin
        {
            get { return _begin; }
            set { _begin = value; }
        }

        private long _end;

        /// <summary>
        /// Get/set the time span at the end of execution
        /// </summary>
        public long End
        {
            get { return _end; }
            set { _end = value; }
        }


        /// <summary>
        /// Get the elapsed time between Begin and End
        /// </summary>
        /// <returns>elapsed time</returns>
        public TimeSpan GetElapsed()
        {
            return new TimeSpan(_end - _begin);
        }

        /// <summary>
        /// Creates events arguments object with given information
        /// </summary>
        /// <param name="className">executing class name</param>
        /// <param name="methodName">executing method name</param>
        /// <param name="begin">ticks at begining</param>
        /// <param name="end">ticks at end</param>
        public ExecutionTimeEventArgs(string className, string methodName, long begin, long end)
        {
            _className = className;
            _methodName = methodName;
            _begin = begin;
            _end = end;
        }

        /// <summary>
        /// Creates an empty argument object
        /// </summary>
        public ExecutionTimeEventArgs()
        {
        }
    }
}
