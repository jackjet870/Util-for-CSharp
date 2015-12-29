using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util.Pool
{
    /// <summary>
    /// Defines serveral methods to handle a resource in pool.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IResourceHandle<T>
    {
        /// <summary>
        /// Create a instance for current resource.
        /// </summary>
        /// <returns></returns>
        T CreateInstance();
        /// <summary>
        /// Dispose current resource.
        /// </summary>
        /// <param name="instance"></param>
        void DiposeInstance(T instance);
        /// <summary>
        /// Check whether the resource can be used.
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        bool IsAvailable(T instance);
    }
}
