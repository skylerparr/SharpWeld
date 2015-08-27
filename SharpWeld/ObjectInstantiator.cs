using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SharpWeld
{
    /// <summary>
    /// Primarily used for testing to delegate the creation of the object 
    /// 
    /// </summary>
    public class ObjectInstantiator
    {
        public virtual Type GetTypeByName(string name)
        {
            return Type.GetType(name, true);
        }

        public virtual T GetInstanceByName<T>(string name, Object[] args)
        {
            return GetInstanceByType<T>(GetTypeByName(name), args);
        }

        public virtual T GetInstanceByType<T>(Type type, Object[] args)
        {
            return (T) System.Activator.CreateInstance(type, args);
        }

    }
}
