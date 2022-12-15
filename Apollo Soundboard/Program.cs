using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Threading;

namespace Apollo_Soundboard
{
    internal static class Program
    {
        private static Mutex _mutex = null;
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            FileAssociations.EnsureAssociationsSet();

            const string appName = "Apollo Soundboard";
            bool createdNew;

            _mutex = new Mutex(true, appName, out createdNew);

            if (!createdNew)
            {
                return;
            }


            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            if(args.Length == 0 )
            {
                Application.Run(new Soundboard(null));
            } else
            {
                Application.Run(new Soundboard(args[0]));
            }
            
        }


    }
}