namespace ListViewControl
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.IO;
    using System.Windows;

    using ListViewControl.Extension;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            /*
            List<string> strings = new List<string>() { "gerhard", "charlie" };
            DataTable stringsDT = strings.ToGetDataTable();

            List<int?> integers = new List<int?>() { 1, 2,3,4,5,6,7,8,9 };
            DataTable integersDT = integers.ToGetDataTable();

            List<Status> status = new List<Status>() {Status.None,Status.Aktive,Status.InAktive };
            DataTable statusDT = status.ToGetDataTable();

            List<DemoModel> demoModels = new DemoData().BuildData();
            DataTable demoModelDT = demoModels.ToGetDataTable();

                         VersionChanger(new string[] { "$(ProjectDir)|1.yyyy.MMdd.*" });
        */
        }

        private int VersionChanger(string[] args)
        {
            if (args.Count() != 1)
            {
                Console.WriteLine("!!! Error args count !!!");
                return -1;
            }
            var items = args[0].Split("|", StringSplitOptions.RemoveEmptyEntries);
            if (items.Count() != 2)
            {
                Console.WriteLine("!!! Bad formed argument !!!");
                return -1;
            }

            string fullpath = Path.Combine(items[0], @"AssemblyInfo.cs");

            if (fullpath.Contains("$(ProjectDir)") == true)
            {
                string projectDir = $"{Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName}";
                fullpath = fullpath.Replace("$(ProjectDir)", projectDir);
            }

            if (File.Exists(fullpath) == false)
            {
                Console.WriteLine("!!! Not .NET project !!!");
                return -1;
            }
            Console.WriteLine(fullpath);
            var props = File.ReadAllText(fullpath).Split(@"AssemblyVersion(""");
            if (props.Count() != 2)
            {
                Console.WriteLine("!!! Properties file invalid format !!!");
                return -1;
            }

            var newtext = props[0];
            newtext += @"AssemblyVersion(""" + DateTime.Now.ToString(items[1]) + @""")]";

            newtext += string.Join(Environment.NewLine, props[1].Split(Environment.NewLine.ToCharArray()).Skip(1).ToArray());

            File.WriteAllText(fullpath, newtext);

            Console.WriteLine();
            return 0;
        }
    }


    public enum Status : int
    {
        None = 0,
        Aktive = 1,
        InAktive = 2,
    }
}
