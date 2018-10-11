using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Posix
{
    partial class FileUtils
    {

        // If no arg's given, set date and time to now.
        static void Touch(string FileName, bool SetAccess = true, 
            bool SetModification = false, bool DontCreate = false) =>
            Touch(FileName, DateTime.Now, SetAccess, SetModification, DontCreate);

        static void Touch(string FileName, DateTime ToSet,
            bool SetAccess = true, bool SetModification = false,
            bool DontCreate = false)
        {
            if (File.Exists(FileName))
            {
                if (SetAccess)
                {
                    File.SetLastAccessTime(FileName, ToSet);
                }
                if (SetModification)
                {
                    File.SetLastWriteTime(FileName, ToSet);
                }
            }
            else
            {
                if (!DontCreate)
                {
                    File.Create(FileName);
                    Touch(FileName, ToSet, SetAccess, SetModification, DontCreate);
                }
            }
        }

        static void Touch(string FileName, string FileToMatch, 
            bool SetAccess = true, bool SetModification = false,
            bool DontCreate = false)
        {
            Touch(FileName, File.GetCreationTime(FileToMatch), SetAccess,
                SetModification, DontCreate);
        }
        
    }
}
