using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Posix
{
    partial class Command
    {
        static void Wait(int pid)
        {
            try
            {
                Process.GetProcessById(pid).WaitForExit();
            }
            catch (ArgumentException e)
            {
                Stdio.perror("Can't wait on process: " + e.Message);
            }
        }

        static void Wait(int[] pids)
        {
            Dictionary<int, Process> toWaitFor = new Dictionary<int, Process>();

            // sort through the list, getting actual processes, sending to error 
            // processes that don't exist, etc. 

            foreach (int pid in pids) 
            {
                try
                {
                    Process p = Process.GetProcessById(pid);
                    p.EnableRaisingEvents = true;
                    toWaitFor.Add(pid, p);

                    // When the process' exit event triggers, remove it from the list of procs we're waiting on
                    p.Exited += (object o, EventArgs e) =>
                        {
                            if (toWaitFor.ContainsKey(pid))
                            {
                                toWaitFor.Remove(pid);
                            }
                        };

                    // Handle potential race condition for when the process finishes after we add it to the
                    // dict, but before we add the handler.
                    if (p.HasExited)
                    {
                        if (toWaitFor.ContainsKey(pid))
                        {
                            toWaitFor.Remove(pid);
                        }
                    }

                }
                catch (ArgumentException e)
                {
                    Stdio.perror("Can't wait on process " + pid + ": " + e.Message);

                    // Something had gone wrong, make sure we aren't going to be locking
                    // on a dead process
                    if (toWaitFor.ContainsKey(pid))
                    {
                        toWaitFor.Remove(pid);
                    }
                }
            }

            // Spinwait until all processes finish and remove themselves from the list

            System.Threading.SpinWait.SpinUntil(() => { return toWaitFor.Count == 0; } );

        }

    }
}
