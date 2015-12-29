using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.Pool;
using System.Net.Sockets;

namespace Util.UnitTest.Pool
{
    class SocketResourceHandle : IResourceHandle<Socket>
    {
        private string Host = string.Empty;
        private int Port = 0;

        public SocketResourceHandle(string host, int port)
        {
            this.Host = host;
            this.Port = port;
        }

        public Socket CreateInstance()
        {
            if (string.IsNullOrEmpty(this.Host))
                throw new Exception("The socket host was not supplied.");
            if (this.Port <= 0 || this.Port > 65535)
                throw new Exception("The socket port was invalid.");

            Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.Connect(this.Host, this.Port);
            if (_socket != null && _socket.Connected)
                return _socket;

            return null;
        }

        public void DiposeInstance(Socket instance)
        {
            if (instance != null)
            {
                if (instance.Connected)
                    instance.Close();

                instance.Dispose();
            }
        }

        public bool IsAvailable(Socket instance)
        {
            return instance != null && instance.Connected;
        }
    }
}
