using System;

namespace Monitor
{
    /// <summary>
    /// Application entry point class.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Application entry point method.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            try
            {
                MonitorAgent monitor = new MonitorAgent();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }

            Console.ReadLine();
        }
    }
}
