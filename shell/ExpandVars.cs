using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Posix.Shell
{
    partial class Shell
    {
        public static string ExpandVars(string toExpand)
        {
            string result = "";

            // If we have spaces, expand tokens first, because variables can't have spaces
            if (toExpand.Contains(' '))
            {
                foreach (string expansion in ExpandVars(Tokenize(toExpand)))
                {
                    result += expansion;
                }
            }
            else
            {
                string[] expanded = toExpand.Split(new char[] { '$' });

                // We're expanding either two aggregated variables, or a fixed string
                // with a variable appended
                if (expanded.Length > 1)
                {
                    StringBuilder finishedString = new StringBuilder();

                    // Easiest way to see if we need to expand the first substring
                    if (toExpand.StartsWith("$"))
                    {
                        foreach (string substring in expanded)
                        {
                            // If we find the substring, substitute and add, if not, just drop the var
                            if (Environment.GetEnvironmentVariables().Contains(substring))
                            {
                                finishedString.Append(Environment.GetEnvironmentVariable(substring));
                            }
                        }
                    }
                    // Starting with a non-variable, put that at the beginning, then iterate through
                    // the rest of the keys.
                    else
                    {
                        finishedString.Append(expanded[0]);
                        for (int i = 1; i < expanded.Length; i++)
                        {
                            if (Environment.GetEnvironmentVariables().Contains(expanded[i]))
                            {
                                finishedString.Append(Environment.GetEnvironmentVariable(expanded[i]));
                            }
                        }
                    }
                    result = finishedString.ToString();
                }
                // This is either a non-variable, or a single variable to expand
                else
                {
                    // If the lengths are different, then we must have started with a $ and we're looking at a variable
                    if (expanded[0].Length != toExpand.Length)
                    {
                        if (Environment.GetEnvironmentVariables().Contains(expanded[0]))
                        {
                            result = Environment.GetEnvironmentVariable(expanded[0]);
                        }
                        else result = "";
                    }
                    // No expansion needed, just return the whole string.
                    else
                    {
                        result = toExpand;
                    }
                }
            }
            return result;
        }
        public static string[] ExpandVars(string[] toExpand)
        {
            string[] result = new string[toExpand.Length];

            for (int i = 0; i < toExpand.Length; i++)
            {
                result[i] = ExpandVars(toExpand[i]);
            }

            return result;
        }
    }
}
