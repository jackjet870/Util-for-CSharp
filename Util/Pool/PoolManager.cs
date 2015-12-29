using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace Util.Pool
{
    public class PoolManager<T> : IDisposable where T : class
    {
        private List<Resource<T>> AllAllocatedResources = null;
        private int PoolSize = 1;
        private int ConnectTimeout = 30;
        private int MonitorInterval = 500;
        private bool Disposed = false;
        delegate T CreateInstanceMethod();
        private Timer MonitorTimer = null;
        private IResourceHandle<T> ResourceHandle = null;

        /// <summary>
        /// Initialize the default settings for pool manager
        /// </summary>
        /// <param name="poolSize">Max count of resources for pool manager</param>
        /// <param name="connectTimeout">Timeout for a resource's connection, default 300, in milliseconds</param>
        /// <param name="monitorInterval">Interval for monitor timer, default 300000, in milliseconds</param>
        public PoolManager(int poolSize, int connectTimeout = 300, int monitorInterval = 300000)
        {
            if (poolSize > 0) this.PoolSize = poolSize;
            if (connectTimeout > 0) this.ConnectTimeout = connectTimeout;
            if (monitorInterval > 0) this.MonitorInterval = monitorInterval;
            this.AllAllocatedResources = new List<Resource<T>>(this.PoolSize);
        }
        /// <summary>
        /// Start the current pool manager instance.
        /// </summary>
        public void Start(IResourceHandle<T> resourceHandle)
        {
            if (resourceHandle == null)
                throw new Exception("The resource handle was not supplied.");

            this.ResourceHandle = resourceHandle;

            #region Create all resources instance by PoolSize field

            lock (this.AllAllocatedResources)
            {
                for (int _resIndex = 0; _resIndex < this.PoolSize; _resIndex++)
                {
                    var _resource = new Resource<T>();
                    _resource.Current = this.CreateResourceInstance();
                    _resource.ResourceHandle = this.ResourceHandle;
                    this.AllAllocatedResources.Add(_resource);
                }
            }

            #endregion

            #region Traversal all resources and recreate resources that were disconnected or had exception

            this.MonitorTimer = new Timer(new TimerCallback((state) =>
            {
                lock (this.AllAllocatedResources)
                {
                    this.AllAllocatedResources.ForEach(item =>
                    {
                        item.InUse = false;
                        if (!item.Available)
                        {
                            item.Current = this.CreateResourceInstance();
                        }
                    });
                }
            }), null, this.MonitorInterval, this.MonitorInterval);

            #endregion
        }
        /// <summary>
        /// Get an avaliable resource.
        /// </summary>
        /// <returns></returns>
        public Resource<T> GetResource()
        {
            lock (this.AllAllocatedResources)
            {
                var _resource = this.AllAllocatedResources.Find(r => r.InUse == false && r.Available == true);
                if (_resource != null)
                    _resource.InUse = true;

                return _resource;
            }
        }

        #region Dispose current pool manager instance
        ~PoolManager()
        {
            Dispose(false);
        }
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!this.Disposed)
            {
                if (disposing)
                {
                    //Release managed resources
                    lock (this.AllAllocatedResources)
                    {
                        this.AllAllocatedResources.ForEach(item =>
                        {
                            try
                            {
                                if (this.ResourceHandle != null)
                                    this.ResourceHandle.DiposeInstance(item.Current);
                            }
                            catch { }
                        });
                    }
                    this.MonitorTimer.Dispose();
                }

                //Release unmanaged resources

                this.Disposed = true;
            }
        }

        #endregion

        #region Private Methods

        private T CreateResourceInstance()
        {
            try
            {
                CreateInstanceMethod _createInstanceCaller = new CreateInstanceMethod(this.ResourceHandle.CreateInstance);
                IAsyncResult _asyncResult = _createInstanceCaller.BeginInvoke(null, null);
                if (_asyncResult.AsyncWaitHandle.WaitOne(this.ConnectTimeout))
                {
                    return _createInstanceCaller.EndInvoke(_asyncResult);
                }
            }
            catch { }

            return null;
        }

        #endregion
    }
}
