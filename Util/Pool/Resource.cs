using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util.Pool
{
    public class Resource<T> : IDisposable where T : class
    {
        private string _id = string.Empty;
        /// <summary>
        /// ID
        /// </summary>
        public string ID
        {
            get
            {
                if (string.IsNullOrEmpty(this._id))
                    this._id = Guid.NewGuid().ToString();
                return _id;
            }
        }
        /// <summary>
        /// Indicate whether the resource is connected.
        /// </summary>
        public bool Available
        {
            get
            {
                if (this.ResourceHandle != null)
                    return this.ResourceHandle.IsAvailable(this.Current);

                return false;
            }
        }
        /// <summary>
        /// Indicate whether the resources is in use.
        /// </summary>
        public bool InUse { get; set; }
        /// <summary>
        /// Indicate the current resource
        /// </summary>
        public T Current { get; set; }
        /// <summary>
        /// Indicate the handle class for current resource.
        /// </summary>
        public IResourceHandle<T> ResourceHandle { get; set; }
        /// <summary>
        /// Dispose current instance, make the IsUsing status to false.
        /// </summary>
        public void Dispose()
        {
            this.InUse = false;
        }
    }
}
