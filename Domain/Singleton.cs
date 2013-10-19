using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain
{
    public class Singleton<T>
    {
        public delegate T CreateSingleton();

        private static T _instance;
        private static object _syncRoot = new Object();

        /// <summary>
        /// Gets the instance of the type parameter.
        /// </summary>
        public static T GetInstance(CreateSingleton createSingleton)
        {
            if (_instance == null)
            {
                lock (_syncRoot)
                {
                    if (_instance == null)
                    {
                        T temp = createSingleton();
                        System.Threading.Thread.MemoryBarrier();
                        _instance = temp;
                    }
                }
            }
            return _instance;
        }
    } 
}
