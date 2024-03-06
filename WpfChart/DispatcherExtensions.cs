using System;
using System.Windows.Threading;

namespace WpfChart.GUI
{
    internal static class DispatcherExtensions
    {
        public static void InvokeIfRequired(this Dispatcher dispatcher, Action action, DispatcherPriority priority= DispatcherPriority.Send)
        {
            if (!dispatcher.CheckAccess())
            {
                dispatcher.Invoke(priority, action);
            }
            else
            {
                action();
            }
        }
    }
}
