using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Performance
{
    /// <summary>
    /// Represetns a simple object that will keep a track of executions
    /// </summary>
    public class ExecutionItem : IComparable
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public ExecutionItem(string name)
        {
            _name = name;
            _count = 0;
            _totalTime = 0;
        }

        private ExecutionItem() { }


        /// <summary>
        /// holds a count of this item
        /// </summary>
        protected int _count;

        /// <summary>
        /// total time
        /// </summary>
        protected long _totalTime;

        /// <summary>
        /// Append execution time 
        /// </summary>
        /// <param name="executionTime">time in milliseconds</param>
        public void AppendTime(long executionTime)
        {
            _count++;
            _totalTime += executionTime;
        }

        /// <summary>
        /// Get the total number of milliseconds appended
        /// </summary>
        public long TotalTime
        {
            get { return _totalTime; }
        }

        /// <summary>
        /// Get the count this item got appended
        /// </summary>
        public int Count
        {
            get { return _count; }
        }

        /// <summary>
        /// name of the item
        /// </summary>
        protected string _name;

        /// <summary>
        /// Get name of the item
        /// </summary>
        public string Name
        {
            get { return _name; }
        }
        #region IComparable Members
        /// <summary>
        /// Compare this item to another
        /// </summary>
        /// <param name="obj">another item</param>
        /// <returns>less than zero if this is less than obj, zero if both
        /// are equal and greater than zero if this is greater than obj</returns>
        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                return -1;
            }
            ExecutionItem item = (ExecutionItem)obj;
            return this.Name.CompareTo(item.Name);
        }

        #endregion

        /// <summary>
        /// Get a string representation of this item
        /// </summary>
        /// <returns>string containting information about this</returns>
        public override string ToString()
        {
            TimeSpan total = new TimeSpan(TotalTime);
            return String.Format("{0,-30}\t{1,10}\t{2,10}\t{3,10}\t{4,10}\t{5,10}\t{6,10}",
                Name, total.TotalHours, total.TotalMinutes, total.TotalSeconds, total.TotalMilliseconds, Count, total.TotalMilliseconds / Count);
        }

    }
}