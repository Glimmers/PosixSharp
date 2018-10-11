using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Posix
{
    partial class FileUtils
    {

        static void Touch(string fileName, bool setAccess = true, bool setModification = false, bool dontCreate = false)
        {
            // If no arg's given, set date and time to now.
            Touch(fileName, DateTime.Now, setAccess, setModification, dontCreate);
        }

        static void Touch(string fileName, DateTime toSet, bool setAccess = true, bool setModification = false, bool dontCreate = false)
        {
            if (File.Exists(fileName))
            {
                if (setAccess)
                {
                    File.SetLastAccessTime(fileName, toSet);
                }
                if (setModification)
                {
                    File.SetLastWriteTime(fileName, toSet);
                }
            }
            else
            {
                if (!dontCreate)
                {
                    File.Create(fileName);
                    Touch(fileName, toSet, setAccess, setModification, dontCreate);
                }
            }
        }

        static void Touch(string fileName, string fileToMatch, bool setAccess = true, bool setModification = false, bool dontCreate = false)
        {
            Touch(fileName, File.GetCreationTime(fileToMatch), setAccess, setModification, dontCreate);
        }
        
    }
}
