using AutoUpdaterDotNET;
using System.ComponentModel;
using Apollo.Forms;
using System.Windows.Controls;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Apollo
{
    internal static class Program
    {


        public static string Version = "1.11.0";

        private static Mutex? _mutex = null;
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {

            //auto updater
            AutoUpdater.InstalledVersion = new Version(Version);

  

            //associate apollo soundboard with the .asb file extension
            FileAssociations.EnsureAssociationsSet();

            //mutex to make sure only one instance runs at a time
            const string appName = "Apollo Soundboard";

            _mutex = new Mutex(true, appName, out bool createdNew);

            if (!createdNew)
            {
                return;
            }


            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            if (args.Length == 0)
            {
                Application.Run(new MainForm(null));
            }
            else
            {
                var FileToOpenWIth = args[0];
                Application.Run(new MainForm(FileToOpenWIth));
            }

        }

        //putting this here to get it out of the way of the rest of the code
        //https://stackoverflow.com/questions/2575592/moving-a-member-of-a-list-to-the-front-of-the-list
        public static void MoveItemAtIndexToFront<T>(this List<T> list, int index)
        {
            T item = list[index];
            for (int i = index; i > 0; i--)
                list[i] = list[i - 1];
            list[0] = item;
        }



    }

    public class OptimizedBindingList<I> : BindingList<I>
    {
        private readonly List<I> _baseList;

        public OptimizedBindingList() : this(new List<I>())
        {

        }

        public OptimizedBindingList(List<I> baseList) : base(baseList)
        {
            _baseList = baseList ?? throw new ArgumentNullException();
        }

        public void AddRange(IEnumerable<I> vals)
        {
            if (vals is ICollection<I> collection)
            {
                int requiredCapacity = Count + collection.Count;
                if (requiredCapacity > _baseList.Capacity)
                    _baseList.Capacity = requiredCapacity;
            }

            bool restore = RaiseListChangedEvents;
            try
            {
                RaiseListChangedEvents = false;
                foreach (I v in vals)
                    Add(v);
            }
            finally
            {
                RaiseListChangedEvents = restore;
                if (RaiseListChangedEvents)
                    ResetBindings();
            }
        }

    }
}