using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Performance
{
    /// <summary>
    /// Represents a collection of ExecutionItem objects
    /// </summary>
    public class ExecutionItemCollection : List<ExecutionItem>
    {
        /// <summary>
        /// creates an empty list
        /// </summary>
        public ExecutionItemCollection() { }

        /// <summary>
        /// creates a list using the items on the given list
        /// </summary>
        /// <param name="another">items to use when creating this list</param>
        public ExecutionItemCollection(ExecutionItemCollection another) : base(another) { }

        /// <summary>
        /// creates a list using the items on the given list
        /// </summary>
        /// <param name="another">items to use when creating this list</param>
        public ExecutionItemCollection(IList<ExecutionItem> another) : base(another) { }

        /// <summary>
        /// Adds an item to the list
        /// </summary>
        /// <param name="value">item to add</param>
        /// <returns>index of the newly added item</returns>
        public void Add(ExecutionItem value)
        {
            base.Add(value);
        }


        /// <summary>
        /// Get/set the execution item at a given index
        /// </summary>
        public new ExecutionItem this[int index]
        {
            get
            {
                return (ExecutionItem)base[index];
            }
            set
            {
                base[index] = value;
            }
        }

        /// <summary>
        /// Get a string representation of this collection
        /// </summary>
        /// <returns>string containing values</returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("\nExecution Item Summary\n");
            builder.Append("----------------------\n");
            builder.Append("Name\tHours\tMinutes\tSeconds\tMilliseconds\tCount\tAverage (ms)\n");
            foreach (ExecutionItem item in this)
            {
                builder.AppendFormat("{0}\n", item.ToString());
            }
            builder.AppendFormat("Total Items: {0}", this.Count);
            return builder.ToString();
        }


    }
}
