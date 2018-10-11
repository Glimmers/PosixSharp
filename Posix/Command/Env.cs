using System;
using System.Collections.Generic;
using System.Linq;
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
                System.Diagnostics.Process toRun = new System.Diagnostics.Process();
                toRun.StartInfo.FileName = command;
                toRun.StartInfo.Arguments = arguments;
                bool executed;

                
                
                // Save our old environment for restoration later
                System.Collections.Specialized.StringDictionary oldEnvironment = (System.Collections.Specialized.StringDictionary)Environment.GetEnvironmentVariables();

                // Lock the environment until the process is started
                lock (typeof(System.Environment))
                {

                    // Build our temporary environment, keeping track of environment variables we need to unset
                    
                    foreach (KeyValuePair<string, string> procVar in vars)
                    {
                        // Add an empty key to our environment if it doesn't exist. When we restore the
                        // environment, these empty keys will be deleted.
                        if (!oldEnvironment.ContainsKey(procVar.Key))
                        {
                            oldEnvironment.Add(procVar.Key, "");
                        }
                        Environment.SetEnvironmentVariable(procVar.Key, procVar.Value);
                    }
                    
                    executed = toRun.Start(); // Run file, save whether we started up or not

                    // File started, it has the environment, clean ourselves up
                    foreach (KeyValuePair<string, string> restoreVars in oldEnvironment)
                    {
                        Environment.SetEnvironmentVariable(restoreVars.Key, restoreVars.Value);
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
