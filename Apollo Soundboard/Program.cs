using System.ComponentModel;

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
            if (args.Length == 0)
            {
                Application.Run(new Soundboard(null));
            }
            else
            {
                Application.Run(new Soundboard(args[0]));
            }

        }
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
            if (baseList == null)
                throw new ArgumentNullException();
            _baseList = baseList;
        }

        public void AddRange(IEnumerable<I> vals)
        {
            ICollection<I> collection = vals as ICollection<I>;
            if (collection != null)
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
                    Add(v); // We cant call _baseList.Add, otherwise Events wont get hooked.
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