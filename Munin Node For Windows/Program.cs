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
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Munin_Node_For_Windows.core;

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

            // Different run arguments
            if (args.Length > 0)
            {
                string argList = "";
                foreach (string arg in args) {
                    argList += " '" + arg + "'";
                }
                Logger.LogText("Application run with Arguments -> " + argList, Logger.LogTypes.LogInformation);
            }

            // runs the service only once, does not require installing
            if (args.Contains("-run"))
            {
                string[] runArgs = {};
                MuninService service = new MuninService();
                service.runOnce(runArgs);
                return;
            }


            ServiceBase[] servicesToRun;
            servicesToRun = new ServiceBase[]
            {
                new MuninService()
            };
            ServiceBase.Run(servicesToRun);
        }
    }
}
