using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Posix.Command
{
    public class Kill
    {
        public static int KillProcess(int pid, Posix.Executable.Signal sig = Executable.Signal.SIGHUP)
        {
            int returnValue = 0;

            if (pid == 0)
            {
                Posix.Stdio.perror("This OS does not support process groups.");
                returnValue = 1;
            }
            else
            {
                try
                {
                    System.Diagnostics.Process toKill = System.Diagnostics.Process.GetProcessById(pid);
                    switch (sig)
                    {
                        case Executable.Signal.SIGKILL:
                            toKill.Kill();
                            returnValue = toKill.ExitCode;
                            break;
                        case Executable.Signal.SIGHUP:
                            toKill.CloseMainWindow();
                            toKill.Close();
                            returnValue = toKill.ExitCode;
                            break;
                        default: // For now, all other signals will be treated as SIGHUP
                            toKill.CloseMainWindow();
                            toKill.Close();
                            returnValue = toKill.ExitCode;
                            break;
                    }
                }
                catch (Exception e)
                {
                    Posix.Stdio.perror("Unable to kill " + pid + ": " + e.Message);
                    returnValue = 1;
                }
            }

            return returnValue;
        }

        public static int KillThread(int tid)
        {
            int retval = 0;

            System.Diagnostics.ProcessThread toKill = null;

            try
            {

                foreach (System.Diagnostics.Process pid in System.Diagnostics.Process.GetProcesses())
                {
                    foreach (System.Diagnostics.ProcessThreadCollection threads in pid.Threads)
                    {
                        foreach (System.Diagnostics.ProcessThread thread in threads)
                        {
                            if (thread.Id == tid)
                            {
                                toKill = thread;
                                break;
                            }
                        }
                        if (toKill != null) break;
                        
                    }
                    if (toKill != null) break;
                }

                if (toKill != null)
                {
                    toKill.Dispose();
                }
                else
                {
                    Posix.Stdio.perror("Unable to find thread ID: " + tid);
                    retval = 1;
                }
            }
            catch (Exception e)
            {
                Posix.Stdio.perror("Unable to kill thread: " + tid + ": " + e.Message);
                retval = 1;
            }

            return retval;
        }

        public void KillAll(string toKill, Executable.Signal sig)
        {
            foreach (System.Diagnostics.Process pid in System.Diagnostics.Process.GetProcesses())
            {
                if (pid.ProcessName == toKill)
                {
                    switch (sig)
                    {
                        case Executable.Signal.SIGKILL:
                            pid.Kill();
                            break;
                        case Executable.Signal.SIGHUP:
                            pid.CloseMainWindow();
                            pid.Close();
                            break;
                        default: // For now, all other signals will be treated as SIGHUP
                            pid.CloseMainWindow();
                            pid.Close();
                            break;
                    }
                }
            }
        }
    }
}
