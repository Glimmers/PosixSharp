using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Posix.Executable
{
    public enum Signal
    {
        SIGZERO, SIGHUP, SIGINT, SIGQUIT, SIGILL, SIGTRAP, SIGABRT, SIGEMT,
        SIGFPE, SIGKILL, SIGBUS, SIGSEGV, SIGSYS, SIGPIPE, SIGALRM, SIGTERM,
        SIGURG, SIGSTOP, SIGTSTP, SIGCONT, SIGCHLD, SIGTTIN, SIGTTOU, SIGIO,
        SIGXCPU, SIGXFSZ, SIGVTALRM, SIGPROF, SIGWINCH, SIGINFO, SIGUSR1,
        SIGUSR2, SIGTHR, SIGLIBRT
    };

    public interface ISignalable
    {
        void HandleSignal(Signal toSend);
    }

}
