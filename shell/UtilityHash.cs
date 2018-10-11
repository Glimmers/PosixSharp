using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Posix.Shell;

namespace Posix
{
    namespace Shell
    {
        public class LocationCache
        {
            private struct hashInfo
            {
                public int hits;
                public string path;
            }

            private Dictionary<string, hashInfo> _files;

            public LocationCache()
            {
                _files = new Dictionary<string, hashInfo>();
            }

            public int Clear()
            {
                _files.Clear();
                return 0;
            }

            public string[] ListContents(bool printHits = false, bool printShort = false)
            {
                List<string> commands = new List<string>();
                    foreach (KeyValuePair<string, hashInfo> hashEntry in _files)
                    {
                        StringBuilder command = new StringBuilder();
                        if (printHits)
                        {
                            command.Append(hashEntry.Value.hits);
                            command.Append("\t");
                        }
                        if (printShort)
                        {
                            command.Append(hashEntry.Key);
                            command.Append("\t");
                        }
                        command.Append(hashEntry.Value.path);
                        commands.Add(command.ToString());
                    }

                return commands.ToArray();
            }

            public string GetEntry(string toGet, Path systemPaths)
            {
                string filePath;

                if (_files.ContainsKey(toGet))
                {
                    hashInfo fileEntry = _files[toGet];
                    filePath = fileEntry.path;
                    fileEntry.hits++;
                    _files[toGet] = fileEntry;
                }
                else
                {
                    filePath = Path.SearchForCommand(toGet);
                    if (filePath != "")
                    {
                        hashInfo fileEntry;
                        fileEntry.hits = 1;
                        fileEntry.path = filePath;
                        _files.Add(toGet, fileEntry);
                    }
                }

                return filePath;
            }

        }
    }
}
