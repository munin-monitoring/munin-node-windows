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

using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using Munin_Node_For_Windows.core;
using Munin_Node_For_Windows.required;

namespace Munin_Node_For_Windows
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            // Initialize Log
            Logger.InitializeLog();
            
            if (args.Contains("-run"))
            {
                NativeMethods.AllocConsole();
            }

            // Different run arguments
            if (args.Length > 0)
            {
                string argList = "";
                foreach (string arg in args) {
                    argList += " '" + arg + "'";
                }
                Logger.GetLogger().LogText("Application run with Arguments -> " + argList, LogTypes.LogInformation);
            }

            // runs the service only once, does not require installing
            if (args.Contains("-run"))
            {
                string[] runArgs = {};
                Logger.UseConsole = true;
                MuninService service = new MuninService(true);
                service.RunOnce(runArgs);
                NativeMethods.FreeConsole();
                return;
            }


            var servicesToRun = new ServiceBase[]
            {
                new MuninService(false)
            };
            ServiceBase.Run(servicesToRun);
        }
    }

    internal sealed class NativeMethods
    {
        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();
        
        [DllImport("kernel32.dll")]
        public static extern bool FreeConsole();
        
    }
}
