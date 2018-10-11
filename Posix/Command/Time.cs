using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Posix
{
    partial class Command
    {
        static int Time(string command, string arguments, bool posixFormat = false)
        {
            int exitCode = 0;

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
                bool executed = toRun.Start();

                if (executed)
                {
                    toRun.WaitForExit();
                    exitCode = toRun.ExitCode;
                    TimeSpan totalProc = toRun.TotalProcessorTime;
                    TimeSpan userProc =  toRun.UserProcessorTime;
                    TimeSpan systemProc = toRun.PrivilegedProcessorTime;

                    if (posixFormat)
                    {
                        Posix.Stdio.perror("Real:\t" + totalProc.TotalSeconds
                            + "\nUser:\t" + userProc.TotalSeconds + "\nSystem:\t"
                            + systemProc.TotalSeconds + "\n");
                    }
                    else
                    {
                        Posix.Stdio.perror("Real:\t" + totalProc.Minutes + "m"
                            + totalProc.Seconds + "." + totalProc.Milliseconds
                            + "s\n" + "User:\t" + userProc.Minutes + "m"
                            + userProc.Seconds + "." + userProc.Milliseconds
                            + "s\n" + "System:\t" + systemProc.Minutes + "m"
                            + systemProc.Seconds + "." + systemProc.Milliseconds
                            + "s\n");
                    }
                }

            }


            return exitCode;
        }
    }
}
