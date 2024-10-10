namespace UI.ICollectionViewDemo.Core
{
    using System.Collections.ObjectModel;

    public class Model
    {
        public ObservableCollection<ViewItem> Items
        {
            get
            {
                return this.LoadItems();
            }
        }

        public IEnumerable<string> Columns
        {
            get
            {
                return from prop in typeof(ViewItem).GetProperties() select prop.Name;
            }
        }

        public IEnumerable<string> AvailableDevelopment
        {
            get
            {
                return (from developer in this.Items select developer.Developer).Distinct();
            }
        }

        private ObservableCollection<ViewItem> LoadItems()
        {
            ObservableCollection<ViewItem> items = new ObservableCollection<ViewItem>();

            items.Add(new ViewItem { Id = "1", Name = "Horst", Developer = "WPF", Gehalt = 50000.20f, Status = true });
            items.Add(new ViewItem { Id = "2", Name = "Horst", Developer = "ASP.NET", Gehalt = 89000.20f, Status = true });
            items.Add(new ViewItem { Id = "3", Name = "Gerhard", Developer = "ASP.NET", Gehalt = 95000.20f, Status = true });
            items.Add(new ViewItem { Id = "4", Name = "Kunal", Developer = "Silverlight", Gehalt = 26000.20f , Status = false });
            items.Add(new ViewItem { Id = "5", Name = "Hanselman", Developer = "ASP.NET", Gehalt = 78000.20f , Status = true });
            items.Add(new ViewItem { Id = "6", Name = "Peter", Developer = "WPF", Gehalt = 37000.20f , Status = true });
            items.Add(new ViewItem { Id = "7", Name = "Tim", Developer = "Silverlight", Gehalt = 45000.20f , Status = false });
            items.Add(new ViewItem { Id = "8", Name = "John", Developer = "ASP.NET", Gehalt = 70000.20f , Status = true });
            items.Add(new ViewItem { Id = "9", Name = "Jamal", Developer = "ASP.NET", Gehalt = 40000.20f , Status = false });
            items.Add(new ViewItem { Id = "10", Name = "Gerhard", Developer = "C#", Gehalt = 40000.20f , Status = true });
            items.Add(new ViewItem { Id = "11", Name = "Gerhard", Developer = "WPF", Gehalt = 40000.20f , Status = true });
            items.Add(new ViewItem { Id = "12", Name = "Peter", Developer = "C#", Gehalt = 37000.20f, Status = true });
            items.Add(new ViewItem { Id = "13", Name = "Peter", Developer = "ASP.Net", Gehalt = 37000.20f, Status = true });
            items.Add(new ViewItem { Id = "14", Name = "Gerhard", Developer = "Fortran", Gehalt = 40000.20f, Status = true });
            items.Add(new ViewItem { Id = "15", Name = "Gerhard", Developer = "VB.NET", Gehalt = 40000.20f, Status = false });
            items.Add(new ViewItem { Id = "16", Name = "Charlie", Developer = "Fortran", Gehalt = 40000.20f, Status = true });
            items.Add(new ViewItem { Id = "17", Name = "Charlie", Developer = "VB.NET", Gehalt = 40000.20f, Status = false });
            items.Add(new ViewItem { Id = "18", Name = "Stefan", Developer = "C#", Gehalt = 40000.20f, Status = true });
            items.Add(new ViewItem { Id = "19", Name = "Stefan", Developer = "WPF", Gehalt = 40000.20f, Status = true });

            return items;
        }
    }
    public class ViewItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Developer { get; set; }
        public float Gehalt { get; set; }
        public bool Status { get; set; }
    }

    public class ViewItemExport
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
