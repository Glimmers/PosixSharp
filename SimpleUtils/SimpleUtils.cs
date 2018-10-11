using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Posix
{
    public class SimpleUtils
    {

        static int True()
        {
            return 0;
        }
        static int False()
        {
            return 1;
        }
        public static string Logname()
        {
            return System.Environment.UserName;
        }
    }
}
