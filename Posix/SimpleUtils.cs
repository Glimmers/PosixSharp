using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Posix
{
    public class SimpleUtils
    {

        public static int True()
        {
            return 0;
        }

        public static int False()
        {
            return 1;
        }

        public static string Logname()
        {
            return Environment.UserName;
        }

        public static string Hostname()
        {
            return System.Net.Dns.GetHostName();
        }

        public static int Sleep(int seconds)
        {
            Thread.Sleep(seconds * 1000);
            return 0;
        }

        public static string Pwd()
        {
            return Environment.CurrentDirectory;
        }

        public static int Cd(string directory)
        {
            int retval = 0;
            
            try
            {
                Environment.CurrentDirectory = directory;
            }
            catch (Exception e)
            {
                Stdio.perror("Invalid directory:" + e.Message);
                retval = 1;
            }

            return retval;
        }
    }
}
