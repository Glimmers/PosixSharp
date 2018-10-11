using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;

namespace Posix
{
    partial class Command
    {
        static int Env(Dictionary<string, string> vars, string command, string arguments)
        {
            int exitCode;

            if (!Test.TestOne(Test.TestType.isExtant, command))
            {
                exitCode = 127; // File not found
            }
            else if (!Test.TestOne(Test.TestType.isExecutable, command))
            {
                exitCode = 126; // File isn't executable;
            }
            else
            {
                Process toRun = new Process();
                toRun.StartInfo.FileName = command;
                toRun.StartInfo.Arguments = arguments;
                bool executed;

                
                
                // Save our old environment for restoration later
                var oldEnvironment = Environment.GetEnvironmentVariables();
               

                // Lock the environment until the process is started
                lock (typeof(Environment))
                {
                    // Build our temporary environment

                    // Track which keys we need to delete after the process runs
                    var toDelete = vars.Keys.Where((t) => !oldEnvironment.Contains(t)).ToArray();
                    
                    // Delete the environment values which don't exist in the new environment
                    foreach (string toCheck in Environment.GetEnvironmentVariables().Keys)
                    {
                        if (!vars.ContainsKey(toCheck)) Environment.SetEnvironmentVariable(toCheck, "");
                    }

                    // Dump in the new environment variables

                    foreach (var pair in vars)
                    {
                        Environment.SetEnvironmentVariable(pair.Key, pair.Value);
                    }

                    executed = toRun.Start(); // Run file, getting a process descriptor

                    // Program started, it has the environment, clean ourselves up

                    // Set all the variables from our old environment
                    foreach (KeyValuePair<string, string> restoreVars in oldEnvironment)
                    {
                        Environment.SetEnvironmentVariable(restoreVars.Key, restoreVars.Value);
                    }

                    // Clear the environment variables which were in our temporary environment which
                    // aren't in the original environment
                    foreach (var deleting in toDelete)
                    {
                        Environment.SetEnvironmentVariable(deleting, "");
                    }

                }

                if (executed)
                {
                    // Wait until the specified file is done.
                    toRun.WaitForExit();
                    exitCode = toRun.ExitCode;
                }
                else
                {
                    exitCode = 126; // Error executing file
                }
            }

            return exitCode;
        }
    }
}
