using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Pipes;

namespace ExtraFields
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
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

        static void SendArgumentsToRunningInstance(string[] args)
        {
            using (var client = new NamedPipeClientStream(".", "your_pipe_name_here", PipeDirection.Out))
            using (var writer = new StreamWriter(client))
            {
                client.Connect(1000); // Timeout in milliseconds
                writer.WriteLine(string.Join(" ", args));
            }
        }
    }
}
