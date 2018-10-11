using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Posix.Executable
{
    public class SignalableThread
    {
        public static Thread CreateSignalableThread(ISignalable toStart)
        {
            Thread threadToStart = new Thread(toStart.startup);
            threadToStart.Start();
            return threadToStart;
        }

        public static Thread CreateSignalableThread(ISignalable toStart, int stackSize)
        {
            Thread threadToStart = new Thread(toStart.startup, stackSize);
            threadToStart.Start();
            return threadToStart;
        }

        public static Thread CreateSignalableThread(ISignalable toStart, object parameters)
        {
            Thread threadToStart = new Thread(toStart.startup);
            threadToStart.Start(parameters);
            return threadToStart;
        }

        public static Thread CreateSignalableThread(ISignalable toStart, object parameters, int stackSize)
        {
            Thread threadToStart = new Thread(toStart.startup, stackSize);
            threadToStart.Start(parameters);
            return threadToStart;
        }

        public static Task CreateSignalableTask(ISignalable toStart, object stateObject = null)
        {
            if (stateObject == null)
            {
                return new Task(new Action(() => { toStart.startup(); }));
            }
            else
            {
                return new Task(new Action<object>((object obj) => { toStart.SetState(obj); toStart.startup(); }), stateObject);
            }
        }

        public static Task CreateSignalableTask(ISignalable toStart, TaskCreationOptions options, object stateObject = null)
        {
            if (stateObject == null)
            {
                return new Task(new Action(() => { toStart.startup(); }), options);
            }
            else
            {
                return new Task(new Action<object>((object obj) => { toStart.SetState(obj);  toStart.startup(); }), stateObject, options);
            }
        }

        public static Task CreateSignalableTask(ISignalable toStart, CancellationToken token, object stateObject = null)
        {
            if (stateObject == null)
            {
                return new Task(new Action(() => { toStart.startup(); }), token);
            }
            else
            {
                return new Task(new Action<object>((object obj) => { toStart.SetState(obj); toStart.startup(); }), token);
            }
        }

        public static Task CreateSignalableTask(ISignalable toStart, CancellationToken token, TaskCreationOptions options)
        {
            return new Task(new Action(() => { toStart.startup(); }), token, options);
        }

        public static Task CreateSignalableTask(ISignalable toStart, CancellationToken token)
        {
            return new Task(new Action(() => { toStart.startup(); }));
        }
    }
}
