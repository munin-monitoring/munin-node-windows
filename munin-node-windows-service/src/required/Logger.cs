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
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Munin_Node_For_Windows.required
{
    // Standard Log Categories, Although any string can be passed to the logText Method
    // It is strongly encouraged to use these standards
    public class LogTypes
    {
        public const string LogInformation = "Information";
        public const string LogDebug = "Debug";
        public const string LogTrace = "Trace";
        public const string LogWarning = "Warning";
        public const string LogError = "Error";
    }
    internal class Logger
    {
        private static string _dir = "\\log\\";
        private static string _name = "munin";
        private static Logger _instance;

        public static bool UseConsole = false;

        public static Logger GetLogger()
        {
            return _instance;
        }
        
        // Builds a header for the log file containing necessary information
        public static void InitializeLog()
        {
            _instance = new Logger();
        }

        private Logger()
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
                Log(DateTime.Now.ToString(CultureInfo.InvariantCulture).Replace("/", "").Replace(" ", "-").Replace(":", ""), w);
            }
        }

        // Logs the text passed with the given Log Category
        public void LogText(String message, string logCategory)
        {
            using (StreamWriter w = GetStreamWriter())
            {
                Log(DateTime.Now.ToString(CultureInfo.InvariantCulture) + " : " + logCategory + " : " + message + "  : <END> ", w);
            }
        }

        // Logs a string with a given TextWriter
        private void Log(string logMessage, TextWriter w)
        {
            string messageWithNewLine = logMessage + "\n";
            w.Write(messageWithNewLine);
            if (UseConsole)
            {
                Console.WriteLine(messageWithNewLine);
            }
        }

        // Returns the full path of the logging Directory
        private string Dir()
        {
            return BaseDir() + _dir;
        }

        // Returns the base directory of the app
        private string BaseDir()
        {
            return Path.GetDirectoryName(
                     Assembly.GetAssembly(typeof(Program)).CodeBase)
                ?.Replace("file:\\", "");
        }

        // Returns the path of the newest log file
        private string Newest()
        {
            return Dir() + _name + ".newest.log";
        }

        // Returns the path of the old log file defined by passed string
        private string Old(string old)
        {
            return Dir() + _name + "." + old + ".log";
        }

        // Get the StreamWriter for the current log file, this also creates/renames the necessary log files
        private StreamWriter GetStreamWriter()
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
            }

            // If no newest log file exists create one
            return File.CreateText(Newest());
        }
    }
}
