/* This file is part of Munin Node for Windows
 * Copyright (C) 2020 Lourens Ros (lourensros@gmail.com)
 *
 * Modified By: No-One (Template: "Name : Date")
 *
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License v3.0
 * along with this program;
 */

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Timers;
using Munin_Node_For_Windows.required;

namespace Munin_Node_For_Windows.network
{
    public class MuninListener
    {
        private readonly TcpListener _connectionListener;
        private Socket _socket;
        private readonly int _timeout;
        private Timer _timer;
        private bool _shouldPend = false;

        // Create an instance of MuninListener
        public MuninListener(int timeout)
        {
            // The IP address to bind to
            IPAddress ip = IPAddress.Parse(Properties.Settings.Default.bound_ip);
            // The port to bind to
            int port = Properties.Settings.Default.bound_port;
            // The timeout of the socket
            _timeout = timeout;
            // Creates a new instance of the network socket
            _connectionListener = new TcpListener(ip, port);
            // Reset the timer object
            ResetTimer(true);
        }

        // Start to listen for connections
        public void StartListeningForConnection()
        {
            // Reset the timer
            ResetTimer();
            // Set the socket to listen mode, pass the backlog size as integer
            _connectionListener.Start(100);

            _shouldPend = true;
            while (_shouldPend)
            {
                if (_connectionListener.Pending())
                {
                    _socket = _connectionListener.AcceptSocket();
                    _shouldPend = false;
                    _connectionListener.Stop();
                    Logger.GetLogger().LogText("Connection from: " + _socket.RemoteEndPoint.ToString(), LogTypes.LogInformation);
                    ResetTimer();
                }
            }

            // Start listening for commands if the socket connection was established
            if (_socket.Connected)
            {
                StartListeningForCommands();
            }
        }

        private void StartListeningForCommands()
        {
            while (_socket.Connected)
            {
                if (_socket.Available > 0)
                {
                    // Receive data from the socket when available
                    byte[] receive = new byte[256];
                    int k = _socket.Receive(receive);
                    // Decode the received string
                    string str = Encoding.ASCII.GetString(receive, 0, k);
                    Logger.GetLogger().LogText("Received From: " + _socket.RemoteEndPoint.ToString() + " | " + str, LogTypes.LogInformation);
                    ResetTimer();
                }                
            }
        } 

        // Called when the socket listen timeout is reached
        private void TimeoutElapsed(Object sender, ElapsedEventArgs e)
        {
            // Stop the timer
            _timer.Stop();
            // Tell the program to stop waiting for pending connections
            _shouldPend = false;
            // Close the socket, since the timout has been elapsed
            _connectionListener.Stop();
            _socket.Disconnect(false);
            _socket.Close();
        }

        // Renews the timer
        private void ResetTimer(bool initial = false)
        {
            if (!initial)
            {
                // Dispose timer object
                _timer.Dispose();
                _timer.Close();
            }
            // Create a new timer object with the passed timeout
            _timer = new Timer(_timeout);
            // Set the Elapsed event to the TimeoutElapsed Method
            _timer.Elapsed += TimeoutElapsed;
            // Start the timer
            _timer.Start();
        }

        // Destroy all necessary Objects
        public void Destroy()
        {
            try
            {
                _timer.Dispose();
                _timer.Close();
            }
            catch (Exception e)
            {
                Logger.GetLogger().LogText("Timer Cannot Be Destroyed, may already be dead", LogTypes.LogError);
                throw;
            }
            try
            {
                _connectionListener.Stop();
            }
            catch (Exception e)
            {
                Logger.GetLogger().LogText("ConnectionListener cannot be Stopped, may already be dead", LogTypes.LogError);
                throw;
            }
            try
            {
                if (_socket.Connected)
                {
                    _socket.Disconnect(false);                    
                }
                _socket.Close();
                _socket.Dispose();
            }
            catch (Exception e)
            {
                Logger.GetLogger().LogText("Socket Cannot Be Destroyed, may already be dead", LogTypes.LogError);
                throw;
            }
        }
    }
}