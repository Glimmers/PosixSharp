using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Posix
{
    namespace Shell
    {
        public class Path
        {
            public static string SearchForCommand(string toSearchFor)
            {
                string fullPath = "";

                // Strings windows will pretty much assuredly run
                string[] executableExtensions = 
                    new string[] { ".exe", ".bat", ".cmd", ".com", ".pif"};
                
                string[] fileWithExecutableExtensions = new string[9];
                for (int i = 0; i < 7; i++)
                {
                    fileWithExecutableExtensions[i] = toSearchFor + executableExtensions[i];
                }
                // They may have passed a file with an extension, we'll cheat and just add it to the list
                fileWithExecutableExtensions[8] = toSearchFor;

                char[] dirSeparator = new char[] { ';' };
                string[] pathEntries = Environment.GetEnvironmentVariable("PATH").
                    Split(dirSeparator);
                foreach (string pathMember in pathEntries)
                {
                    bool foundFile = false;
                    foreach (string possibleFilename in fileWithExecutableExtensions)
                    {
                        string toTest = pathMember + possibleFilename;
                        if (System.IO.File.Exists(toTest))
                        {
                            fullPath = toTest;
                            foundFile = true; // Set our fallthrough variable
                            break; // Fall out of iterating over extensions
                        }
                    }
                    if (foundFile) // Fall out of search loop
                    {
                        break;
                    }
                }
                return fullPath;
            }
        }
    }
}
