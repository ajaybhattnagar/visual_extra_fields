using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Pipes;
using System.Threading;

namespace ExtraFields
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static Mutex mutex = new Mutex(true, "{SomeUniqueStringHere}");
        static NamedPipeServerStream pipeServer;

        [STAThread]
        static void Main()
        {
            string[] passInArgs = Environment.GetCommandLineArgs();

            bool result;
            var mutex = new System.Threading.Mutex(true, "UniqueAppId", out result);
            if (!result)
            {
                MessageBox.Show("Another instance is already running.");
                //SendArgumentsToRunningInstance(passInArgs);
                return;
            }

            GC.KeepAlive(mutex);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ExtraFields(passInArgs));


        }

    }
}
