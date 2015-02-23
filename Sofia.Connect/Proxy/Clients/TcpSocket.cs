using System;
using System.Net;
using System.Net.Sockets;

namespace Sofia.Connect.Proxy.Clients
{
    internal class TcpSocket : Socket
    {


        public TcpSocket()
            : base(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
        {
        }

        private static void ConnectCallBack(IAsyncResult asyncResult)
        {
            try
            {
                ((TcpSocket)asyncResult.AsyncState).EndConnect(asyncResult);

            }
            catch (Exception)
            {

            }
        }
        public static bool IsOpenPort(Uri url)
        {
            return IsOpenPort(url.Host, url.Port);
        }


        public static bool IsOpenPort(string host,int port)
        {
            return new TcpSocket().Connect(host, port, 2000);
        }

        public bool Connect(string ip, int port, int timeout)
        {
            return Connect(new IPEndPoint(IPAddress.Parse(ip), port), timeout);
        }

        public bool Connect(EndPoint endPoint, int timeout)
        {
            IAsyncResult asyncResult = base.BeginConnect(endPoint, new AsyncCallback(ConnectCallBack), this);

            if (!asyncResult.AsyncWaitHandle.WaitOne(timeout, false))
            {
                base.Close();
                return false;
            }

            return true;
        }

        public NetworkStream GetStream()
        {
            return new NetworkStream(this);
        }
    }
}