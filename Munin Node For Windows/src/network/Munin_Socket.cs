using System;
using System.Net;
using System.Net.Sockets;
using Munin_Node_For_Windows.Properties;

namespace Munin_Node_For_Windows.network
{
    public class MuninSocket
    {
        private readonly Socket _socket;
        private readonly int _timeout;
        
        public MuninSocket(int timeout)
        {
            // Store the socket timeout
            _timeout = timeout;

            // Create an endpoint with the bound IP and Port
            var address = new IPAddress(Settings.Default.BoundAddress);
            var port = Settings.Default.BoundPort;
            var endPoint = new IPEndPoint(address, port);

            // Create a TCP socket
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the endpoint
            _socket.Bind(endPoint);
        }

        public void ReadySocket(Action<Socket> callback)
        {
            // Listen with the socket timeout
            _socket.Listen(_timeout);
            
            // Run callback with the current socket
            callback(_socket);
            
            // Close the socket
            _socket.Close();
        }
    }
}