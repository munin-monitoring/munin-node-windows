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
using System.Runtime.InteropServices;
using System.ServiceProcess;
using Munin_Node_For_Windows.network;
using Munin_Node_For_Windows.required;

namespace Munin_Node_For_Windows.core
{
    public partial class MuninService : ServiceBase
    {
        private readonly MuninListener _muninListener;
        
        // Initialization of the service
        public MuninService(bool runOnce)
        {
            InitializeComponent();
            int timeout = Properties.Settings.Default.socket_timeout;
            if (runOnce)
            {
                timeout = 100000;
            }
            Logger.GetLogger().LogText("Service initiated with timeout: " + timeout, LogTypes.LogInformation);
            _muninListener = new MuninListener(timeout);
        }

        // This runs the service only Once
        public void RunOnce(string[] args)
        {
            Console.WriteLine(@"Running service only once in console window. Socket timeout will be reduced");
            OnStart(args);
        }

        // Run when the service is commanded to start
        protected override void OnStart(string[] args)
        {
            Logger.GetLogger().LogText("Service Started", LogTypes.LogInformation);
            _muninListener.StartListeningForConnection();
        }

        // Run when the service is commanded to stop
        protected override void OnStop()
        {
        }
    }

    // Enums to describe Service Status
    public enum ServiceState
    {
        ServiceStopped = 0x00000001,
        ServiceStartPending = 0x00000002,
        ServiceStopPending = 0x00000003,
        ServiceRunning = 0x00000004,
        ServiceContinuePending = 0x00000005,
        ServicePausePending = 0x00000006,
        ServicePaused = 0x00000007,
    }

    // This can be changed from readonly if needed
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct ServiceStatus
    {
        private readonly int dwServiceType;
        private readonly ServiceState dwCurrentState;
        private readonly int dwControlsAccepted;
        private readonly int dwWin32ExitCode;
        private readonly int dwServiceSpecificExitCode;
        private readonly int dwCheckPoint;
        private readonly int dwWaitHint;
    };
}
