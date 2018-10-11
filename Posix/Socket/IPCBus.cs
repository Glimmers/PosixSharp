using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Posix.Socket
{
    class IPCBus
    {
        System.IO.Pipes.NamedPipeClientStream _downstream;
        System.IO.Pipes.NamedPipeServerStream _upstream;

        public IPCBus(string path)
        {
            _downstream = new System.IO.Pipes.NamedPipeClientStream("com.h3m3.posix.sockets." + System.Diagnostics.Process.GetCurrentProcess().Id);
            _upstream = new System.IO.Pipes.NamedPipeServerStream("com.h3m3.posix.sockets.SockServ");
            System.IO.Pipes.PipeSecurity downstreamACL = new System.IO.Pipes.PipeSecurity();
            
            downstreamACL.AddAccessRule(new System.IO.Pipes.PipeAccessRule(Environment.UserName, System.IO.Pipes.PipeAccessRights.FullControl, System.Security.AccessControl.AccessControlType.Allow));

            _downstream.SetAccessControl(downstreamACL);
            
        }
    }
}
