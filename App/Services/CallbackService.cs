using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arcards.Services
{
    public class CallbackAction
    {
        public Action Action { get; set; }

        /// <summary>
        /// Gets deleted after next run
        /// </summary>
        public bool Once { get; set; }

        public CallbackAction(Action action, bool once)
        {
            Action = action;
            Once = once;
        }
    }

    public class AsyncCallbackAction
    {
        public Func<Task> Action { get; set; }

        /// <summary>
        /// Gets deleted after next run
        /// </summary>
        public bool Once { get; set; }

        public AsyncCallbackAction(Func<Task> action, bool once)
        {
            Action = action;
            Once = once;
        }
    }

    public class Callback
    {
        public List<CallbackAction> Callbacks = new List<CallbackAction>();

        public CallbackAction Register(Action action, bool once = false)
        {
            if(action != null)
            {
                CallbackAction cb = new CallbackAction(action, once);
                Callbacks.Add(cb);
            }

            return null;
        }

        public void Run()
        {
            foreach(CallbackAction ca in Callbacks)
            {
                if (ca == null)
                {
                    Delete(ca);
                    continue;
                }

                ca.Action();
                if (ca.Once) Delete(ca);
            }
        }

        public void Delete(CallbackAction cb)
        {
            Callbacks.Remove(cb);
        }
    }

    public class AsyncCallback
    {
        public List<AsyncCallbackAction> Callbacks = new List<AsyncCallbackAction>();

        public AsyncCallbackAction Register(Func<Task> action, bool once = false)
        {
            if (action != null)
            {
                AsyncCallbackAction cb = new AsyncCallbackAction(action, once);
                Callbacks.Add(cb);
            }

            return null;
        }

        public async Task Run()
        {
            foreach (AsyncCallbackAction ca in Callbacks)
            {
                if (ca == null)
                {
                    Delete(ca);
                    continue;
                }

                await ca.Action();
                if (ca.Once) Delete(ca);
            }
        }

        public void Delete(AsyncCallbackAction cb)
        {
            Callbacks.Remove(cb);
        }
    }

    public class CallbackService
    {
        public Callback ErrorCallbacks = new Callback();
        public static AsyncCallback LocUpdateCallbacks = new AsyncCallback();
        public static AsyncCallback SettingsUpdateCallbacks = new AsyncCallback();

        public static async Task InvokeInterval(TimeSpan timeSpan, Action action, bool now = false, CancellationToken cts = default)
        {
            if (now) action();

            var periodicTimer = new PeriodicTimer(timeSpan);
            while (await periodicTimer.WaitForNextTickAsync() && !cts.IsCancellationRequested)
            {
                if (cts.IsCancellationRequested) return;
                action();
            }
        }

        public static async Task InvokeIntervalAsync(TimeSpan timeSpan, Func<Task> action, bool now = false, CancellationToken cts = default)
        {
            Task _;
            if (now) _ = action();

            var periodicTimer = new PeriodicTimer(timeSpan);
            while (await periodicTimer.WaitForNextTickAsync() && !cts.IsCancellationRequested)
            {
                if (cts.IsCancellationRequested) return;
                _ = action();
            }
        }
    }
}
