namespace ListViewControl
{
    using System.Configuration;
    using System.Data;
    using System.Windows;

    using ListViewControl.Extension;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            List<string> strings = new List<string>() { "gerhard", "charlie" };
            DataTable stringsDT = strings.ToGetDataTable();

            List<int?> integers = new List<int?>() { 1, 2,3,4,5,6,7,8,9 };
            DataTable integersDT = integers.ToGetDataTable();

            List<Status> status = new List<Status>() {Status.None,Status.Aktive,Status.InAktive };
            DataTable statusDT = status.ToGetDataTable();

            List<DemoModel> demoModels = new DemoData().BuildData();
            DataTable demoModelDT = demoModels.ToGetDataTable();
        }
    }

    public enum Status : int
    {
        None = 0,
        Aktive = 1,
        InAktive = 2,
    }
}
