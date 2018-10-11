using System;
using System.IO;

namespace Posix
{
    public class Yes
    {
        string _toRepeat;
        StreamWriter _outputStream;
        bool _repeat = true;
        bool _closeOnStop = false;


        public Yes(string toRepeat, Stream toWriteTo)
        {
            _toRepeat = toRepeat;
            _outputStream = new StreamWriter(toWriteTo);
            _outputStream.AutoFlush = true;
        }

        public Yes(Stream toWriteTo) 
            : this("y", toWriteTo) { }

        public Yes(string toRepeat, Stream toWriteTo, bool closeOnStop)
            : this(toRepeat, toWriteTo)
        {
            _closeOnStop = closeOnStop;
        }

        public Yes(Stream toWriteTo, bool closeOnStop)
            : this(toWriteTo)
        {
            _closeOnStop = closeOnStop;
        }

        public void Go()
        {
            while (_repeat)
            {
                try
                {
                    _outputStream.WriteLine(_toRepeat);
                }
                catch (Exception e)
                {
                    Posix.Stdio.perror("Stream error: " + e.Message);
                    _repeat = false;  // Something went wrong, stop sending
                }
                
            }
        }

        public EventHandler GetStopHandler()
        {
            return (object o, EventArgs e) => { Stop(); };
       } 

        public void Stop()
        {
            _repeat = false;
            if (_closeOnStop)
            {
                _outputStream.Close();
            }
        } 

    }
}
