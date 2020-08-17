﻿/* This file is part of Munin Node for Windows
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Munin_Node_For_Windows.src.required
{
    class Logger
    {
        private static string dir = "\\log\\";
        private static string name = "munin";

        // Standard Log Categories, Although any string can be passed to the logText Method
        // It is strongly encouraged to use these standards
        public class LogTypes
        {
            public const string LOG_INFORMATION = "Information";
            public const string LOG_DEBUG = "Debug";
            public const string LOG_TRACE = "Trace";
            public const string LOG_WARNING = "Warning";
            public const string LOG_ERROR = "Error";
        }

        // Logs the text passed with the given Log Category
        public static void LogText(String message, string logCategory)
        {
            using (StreamWriter w = GetStreamWriter())
            {
                Log(DateTime.Now.ToString() + " : " + logCategory + " : " + message + "  : <END> ", w);
            }
        }

        // Builds a header for the log file containing necessary information
        public static void InitializeLog()
        {
            // Check if a newest log file already exists
            if (File.Exists(Newest()))
            {
                // If a newest log file already exists, rename it to the code in the top of the file
                String oldFile = File.ReadLines(Newest()).First();

                // Rename the newest log file to an older one
                File.Move(Newest(), Old(oldFile));
            }

            using (StreamWriter w = GetStreamWriter())
            {
                // Generate a code that defines the log file by removing all instances of "/" and ":", and replacing space with "-".
                Log(DateTime.Now.ToString().Replace("/", "").Replace(" ", "-").Replace(":", ""), w);
            }
        }

        // Logs a string with a given TextWriter
        private static void Log(string logMessage, TextWriter w)
        {
            w.Write(logMessage + "\n");
        }

        // Returns the full path of the logging Directory
        private static string Dir()
        {
            return BaseDir() + dir;
        }

        // Returns the base directory of the app
        private static string BaseDir()
        {
            return Path.GetDirectoryName(
                     Assembly.GetAssembly(typeof(Program)).CodeBase).Replace("file:\\", "");
        }

        // Returns the path of the newest log file
        private static string Newest()
        {
            return Dir() + name + ".newest.log";
        }

        // Returns the path of the old log file defined by passed string
        private static string Old(string old)
        {
            return Dir() + name + "." + old + ".log";
        }

        // Get the StreamWriter for the current log file, this also creates/renames the necessary log files
        private static StreamWriter GetStreamWriter()
        {
            // Check if the log directory exists
            Console.WriteLine(Dir());
            if (!Directory.Exists(Dir()))
            {
                // Create Loggin Directory
                Directory.CreateDirectory(Dir());
            }

            // Check if a newest log file already exists
            if ( File.Exists(Newest())) {
                // Create a newest log file
                return File.AppendText(Newest());
            } else
            {
                // If no newest log file exists create one
                return File.CreateText(Newest());
            }
        }
    }
}
