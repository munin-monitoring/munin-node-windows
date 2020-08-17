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
 * You should have received a copy of the GNU General Public License Version 2
 * along with this program;
 */

using Munin_Node_For_Windows.src.required;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Munin_Node_For_Windows
{
    public partial class Munin_Service : ServiceBase
    {
        // Initialization of the service
        public Munin_Service()
        {
            InitializeComponent();
        }

        // This runs the service only Once
        public bool runOnce(string[] args)
        {
            OnStart(args);
            return true;
        }

        // Run when the service is commanded to start
        protected override void OnStart(string[] args)
        {
            Logger.LogText("Service Started", Logger.LogTypes.LOG_INFORMATION);
        }

        // Run when the service is commanded to stop
        protected override void OnStop()
        {
        }
    }

    // Enums to describe Service Status
    public enum ServiceState
    {
        SERVICE_STOPPED = 0x00000001,
        SERVICE_START_PENDING = 0x00000002,
        SERVICE_STOP_PENDING = 0x00000003,
        SERVICE_RUNNING = 0x00000004,
        SERVICE_CONTINUE_PENDING = 0x00000005,
        SERVICE_PAUSE_PENDING = 0x00000006,
        SERVICE_PAUSED = 0x00000007,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ServiceStatus
    {
        public int dwServiceType;
        public ServiceState dwCurrentState;
        public int dwControlsAccepted;
        public int dwWin32ExitCode;
        public int dwServiceSpecificExitCode;
        public int dwCheckPoint;
        public int dwWaitHint;
    };
}
