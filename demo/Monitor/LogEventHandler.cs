using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor
{
    /// <summary>
    /// Log event handler.
    /// </summary>
    /// <param name="sender">Event creator.</param>
    /// <param name="eventArgs">Event data.</param>
    public delegate void LogEventHandler(object sender, LogEventArgs eventArgs);
}
