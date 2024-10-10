//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Lifeprojects.de">
//     Class: Program
//     Copyright © Lifeprojects.de 2024
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>07.10.2024 08:01:20</date>
//
// <summary>
// Konsolen Applikation mit Menü
// </summary>
//-----------------------------------------------------------------------

namespace Console.LINQDemo
{
    using System;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Linq;

    public class Program
    {
        private static void Main(string[] args)
        {
            do
            {
                Console.Clear();
                Console.WriteLine("0. Demodaten");
                Console.WriteLine("1. LINQ mit Array");
                Console.WriteLine("2. LINQ mit Demodaten");
                Console.WriteLine("X. Beenden");

                Console.WriteLine("Wählen Sie einen Menüpunkt oder 'x' für beenden");
                ConsoleKey key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.X)
                {
                    Environment.Exit(0);
                }
                else
                {
                    if (key == ConsoleKey.D0)
                    {
                        AnzeigenDemoDaten();
                    }
                    else if (key == ConsoleKey.D1)
                    {
                        MenuPoint1();
                    }
                    else if (key == ConsoleKey.D2)
                    {
                        MenuPoint2();
                    }
                }
            }
            while (true);
        }

        private static void AnzeigenDemoDaten()
        {
            Console.Clear();
            
            DataTable dt = new DemoData().TableRows;
            ObservableCollection<ViewItem> items = new DemoData().Items;

            Console.ReadKey();
        }

        private static void MenuPoint1()
        {
            Console.Clear();

            int[] array1 = new int[] { };
            int count1 = array1.Count();

            int[] array2 = new int[] { 1, 2, 3, 4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20 };
            int count2 = array2.Count();
            int sum1 = array2.Sum();
            int min1 = array2.Min();
            int max1 = array2.Max();

            IEnumerable<int> oddValue = array2.ToList().Where((c, i) => i % 2 != 0);
            IEnumerable<int> evenValue = array2.ToList().Where((c, i) => i % 2 == 0);

            string[] array3 = new string[] {  };
            string[] array4 = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20" };
            IEnumerable<string> result1 = array4.Where(w => w.StartsWith("1") == true);

            Console.ReadKey();
        }

        private static void MenuPoint2()
        {
            Console.Clear();

            DataTable dt = new DemoData().TableRows;
            int countDT1 = dt.Rows.Count;

            IEnumerable<DataRow> liste = dt.AsEnumerable();
            int countIE1 = liste.Count();
            bool anyTrue = liste.Any(c => c.Field<string>("Developer") == "Access");
            int countIE0 = liste.Count(c => c.Field<string>("Developer") == "Access");
            int countIE2 = liste.Count(c => c.Field<string>("Developer") == "WPF");

            IEnumerable<DataRow> resultWhere = liste.Where(c => c.Field<string>("Developer") == "WPF");

            DataRow dr1 = liste.FirstOrDefault();
            DataRow dr2 = liste.FirstOrDefault(c => c.Field<string>("Developer") == "WPF");
            DataRow dr3 = liste.LastOrDefault();
            DataRow dr4 = liste.LastOrDefault(c => c.Field<string>("Developer") == "WPF");

            float summeDeveloper2 = liste.Where(c => c.Field<string>("Developer") == "WPF").Count();
            float summeDeveloper1 = liste.Where(c => c.Field<string>("Developer") == "WPF").Sum(s => s.Field<float>("Gehalt"));
            float minDeveloper1 = liste.Where(c => c.Field<string>("Developer") == "WPF").Min(s => s.Field<float>("Gehalt"));
            float maxDeveloper1 = liste.Where(c => c.Field<string>("Developer") == "WPF").Max(s => s.Field<float>("Gehalt"));

            IEnumerable<string> resultSelect = liste.Select(c => c.Field<string>("Name")).Distinct();

            Console.ReadKey();
        }
    }
}
