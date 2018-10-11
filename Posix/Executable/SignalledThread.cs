using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Posix.Executable
{
    public abstract class SignalledThread : ISignalable
    {
        private System.Threading.Thread _thread;
        Action<Signal> _signalHandler;

        public SignalledThread()
        {
            _signalHandler = (Signal toSend) =>
            {
                switch (toSend)
                {
                    case Signal.SIGHUP:
                        try
                        {
                            _thread.Abort();
                        }
                        catch (System.Threading.ThreadStateException e)
                        {
                            System.Console.Error.WriteLine("Can't kill thread: " + e.Message);
                        }
                        break;
                    case Signal.SIGKILL:
                        try
                        {
                            _thread.Abort();
                        }
                        catch (System.Threading.ThreadStateException e)
                        {
                            System.Console.Error.WriteLine("Can't kill thread: " + e.Message);
                        }
                        break;
                    default:
                        break;
                }
            };


        }

        public virtual System.Threading.Thread Thread
        {
            get
            {
                return _thread;
            }
        }

        public Action<Signal> Handler
        {
            get
            {
                return _signalHandler;
            }
            set
            {
                _signalHandler = value;
            }
        }

        public abstract void HandleSignal(Signal toSend);

        public virtual void Start(object parameter)
        {
            _thread.Start(parameter);
        }
    }
}
