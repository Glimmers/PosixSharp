using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Posix
{
    partial class Sys
    {
        static string Uname(bool hardware, bool hostname, bool osVersion, bool osName, bool releaseLevel)
        {
            StringBuilder info = new StringBuilder();

            if (hardware)
            {
                info.Append(System.Environment.OSVersion.Platform.ToString());
            }
            if (hostname)
            {
                info.Append(System.Environment.MachineName);
            }
            if (osVersion)
            {
                info.Append(System.Environment.OSVersion.Version.Major + "." + System.Environment.OSVersion.Version.Minor);
            }
            if (osName)
            {
                info.Append(System.Environment.OSVersion.Platform);
            }
            if (releaseLevel)
            {
                info.Append(Environment.OSVersion.VersionString);
            }

            return info.ToString();
        }
    }
}
