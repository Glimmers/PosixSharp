using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Posix.Shell
{
    public partial class Shell
    {
        private LocationCache _seenPrograms;
        private List<System.IO.DirectoryInfo> _path;
        private Dictionary<string, string> _alias;
        private Stream _input, _output, _error;

        public Shell(Stream input, Stream output, Stream error)
        {
            _seenPrograms = new LocationCache();
            _path = new List<System.IO.DirectoryInfo>();
            _alias = new Dictionary<string, string>();

            // Set up our I/O buffers, making sure we can read/write from them
            if (!input.CanRead)
            {
                throw new ArgumentException("Cannot read from given input stream.");
            }
            if (!output.CanWrite)
            {
                throw new ArgumentException("Cannot write to output stream.");
            }
            if (!error.CanWrite)
            {
                throw new ArgumentException("Cannot write to error stream.");
            }
            _input = input;
            _output = output;
            _error = error;
        }

        public int SetEnv(string key, string value)
        {
            Environment.SetEnvironmentVariable(key, value);
            
            return 0;
        }

        public int UnsetEnv(string env)
        {
            SetEnv(env, "");
            return 0;
        }

        public string GetEnv(string variable)
        {
            string entry;

            if (Environment.GetEnvironmentVariables().Contains(variable))
            {
                entry = Environment.GetEnvironmentVariable(variable);
            }
            else
            {
                entry = "";
            }

            return entry;
        }

        public int Alias(string alias, string fullString)
        {
            if (_alias.ContainsKey(alias))
            {
                _alias[alias] = fullString;
            }
            else
            {
                _alias.Add(alias, fullString);
            }
            return 0;
        }

        public int Unalias(string alias)
        {
            int retval;
            if (_alias.ContainsKey(alias))
            {
                _alias.Remove(alias);
                retval = 0;
            }
            else
            {
                PError("Alias" + alias + " does not exist.");
                retval = 1;
            }
            return retval;
        }


        public void PError(string toPrint)
        {

        }

    }
}

