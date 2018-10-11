using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Posix
{
    partial class FileUtils
    {
        static int MkDir(string toMake)
        {
            int retVal = 0;
            try
            {
                Directory.CreateDirectory(toMake);
            }
            catch (Exception e)
            {
                Posix.Stdio.perror("Unable to create directory: " + e.Message);
                retVal = -1;
            }
            return retVal;
        }

        static int Unlink(string filename)
        {
            int retVal = 0;
            try
            {
                File.Delete(filename);
            }
            catch (Exception e)
            {
                Posix.Stdio.perror("Unable to delete file: " + e.Message);
                retVal = -1;
            }
            return retVal;
        }

        static int RmDir(string toDelete, bool recursive = false)
        {
            int retVal = 0;

            try
            {
                Directory.Delete(toDelete, recursive);
            }
            catch (Exception e)
            {
                Posix.Stdio.perror("Unable to delete directory: " + e.Message);
                retVal = -1;
            }

            return retVal;
        }
    }
}
