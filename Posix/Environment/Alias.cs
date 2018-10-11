using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Posix.Env
{
    class AliasPool
    {
        Dictionary<string, string> _aliases;

        public AliasPool()
        {
            _aliases = new Dictionary<string, string>();
        }

        public AliasPool(Dictionary<string, string> aliases)
        {
            _aliases = new Dictionary<string,string>(aliases);
        }

        public string[] Alias()
        {
            return _aliases.Select((x) => ( x.Key + " = " + x.Value)).ToArray();
        }

        public Dictionary<string, string> Aliases()
        {
            return new Dictionary<string, string>(_aliases);
        }

        public int Alias(string key, string value)
        {
            if (_aliases.ContainsKey(key))
            {
                _aliases[key] = value;
            }
            else
            {
                _aliases.Add(key, value);
            }
            return 1;
        }

        public int Unalias(string key)
        {
            int retVal = 0;

            if (_aliases.ContainsKey(key))
            {
                _aliases.Remove(key);
            }
            else
            {
                Posix.Stdio.perror("Unalias: " + key + ": not found");
                retVal = -1;
            }

            return retVal;
        }

    }
}
