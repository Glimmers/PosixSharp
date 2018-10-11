using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Posix.Shell
{
    partial class Shell
    {
        static string[] Tokenize(string toTokenize, char[] ifs = null)
        {
            string[] tokens;

            tokens = toTokenize.Split(ifs);

            return tokens;
        }
    }
}
