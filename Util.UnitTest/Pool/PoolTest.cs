using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using Util.Pool;

namespace Util.UnitTest.Pool
{
    [TestClass]
    public class PoolTest
    {
        private Socket ListeningSocket = null;
        private string ListeningSocket_Host = "127.0.0.1";
        private int ListeningSocket_Port = 13965;

        [TestInitialize]
        public void Startup()
        {
            this.ListeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint _listeningEndPoint = new IPEndPoint(IPAddress.Parse(this.ListeningSocket_Host), this.ListeningSocket_Port);
            this.ListeningSocket.Bind(_listeningEndPoint);
            this.ListeningSocket.Listen(100);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (this.ListeningSocket != null)
            {
                this.ListeningSocket.Dispose();
                this.ListeningSocket = null;
            }
        }

        [TestMethod]
        public void TestPoolManagerInitializeSuccess()
        {
            int _poolSize = 5;
            //Create PoolManager instance and start it.
            PoolManager<Socket> _poolManager = new PoolManager<Socket>(_poolSize);
            _poolManager.Start(new SocketResourceHandle(this.ListeningSocket_Host, this.ListeningSocket_Port));

            //Get all resources
            List<Resource<Socket>> _resourceList = new List<Resource<Socket>>();
            Resource<Socket> _resource = _poolManager.GetResource();
            while (_resource != null)
            {
                _resourceList.Add(_resource);
                _resource = _poolManager.GetResource();
            }

            //Check whether the count of resources match the PoolSize           
            Assert.AreEqual(_poolSize, _resourceList.Count, string.Format("The PoolSize is {0}, but the count of resources I got was {1}", _poolSize, _resourceList.Count));
            _poolManager.Dispose();
        }

        [TestMethod]
        public void TestPoolManagerMonitorSuccess()
        {
            int _poolSize = 5, _connectTimeout = 300, _monitorInterval = 1000;
            string _resourceID = string.Empty;

            //Create PoolManager instance and start it.
            PoolManager<Socket> _poolManager = new PoolManager<Socket>(_poolSize, _connectTimeout, _monitorInterval);
            _poolManager.Start(new SocketResourceHandle(this.ListeningSocket_Host, this.ListeningSocket_Port));

            //Get a resource and store its ID, then close this resource
            Resource<Socket> _resource = _poolManager.GetResource();
            _resourceID = _resource.ID;
            _resource.Current.Close();

            //Delay current thread with MonitorInterval 
            System.Threading.Thread.Sleep(_monitorInterval + 30);

            //Reacquire the resource which matches the ID
            Resource<Socket> _resource2 = _poolManager.GetResource();
            while (_resource2 != null && _resource2.ID != _resourceID)
            {
                _resource2 = _poolManager.GetResource();
            }

            //Check whether the resource is open
            Assert.AreEqual(true, _resource2 == null ? false : _resource2.Available, string.Format("The monitor didn't work successfully."));
            _poolManager.Dispose();
        }
    }
}
