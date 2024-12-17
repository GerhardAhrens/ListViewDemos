namespace UI.ICollectionViewDemo
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Data;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Text.Json;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    using UI.ICollectionViewDemo.Core;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private IEnumerable<string> developerList = null;
        private ICollectionView listviewSource = null;

        public MainWindow()
        {
            this.InitializeComponent();
            WeakEventManager<Window, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            this.DataModel = new Model();

            if (File.Exists("ListViewDemo.json") == true)
            {
                string jsonText = File.ReadAllText("ListViewDemo.json");
                ObservableCollection<ViewItem> list = JsonSerializer.Deserialize<ObservableCollection<ViewItem>>(jsonText);
                this.ListViewSource = CollectionViewSource.GetDefaultView(list);
            }
            else
            {
                this.ListViewSource = CollectionViewSource.GetDefaultView(this.DataModel.Items);
            }

            this.DeveloperList = this.DataModel.AvailableDevelopment;
            this.grdMain.DataContext = this.DataModel;
            this.lvItems.ItemsSource = this.ListViewSource;
            this.DataContext = this;
        }

        private Model DataModel { get; set; }

        public ICollectionView ListViewSource
        {
            get { return this.listviewSource; }
            set
            {
                if (this.listviewSource != value)
                {
                    this.listviewSource = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public IEnumerable<string> DeveloperList
        {
            get { return this.developerList; }
            set
            {
                if (this.developerList != value)
                {
                    this.developerList = value;
                    this.OnPropertyChanged();
                }
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {

            DataTable dt = this.ListViewSource.ToDataTable<ViewItem>();
            List<ViewItem> aa = new List<ViewItem>();
            IEnumerable<ViewItemExport> ignor = aa.Select(s => new ViewItemExport {Id = s.Id, Name = s.Name });
            this.lvItems.Focus();
        }

        private void ListView_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader currentHeader = e.OriginalSource as GridViewColumnHeader;
            if (currentHeader != null && currentHeader.Role != GridViewColumnHeaderRole.Padding)
            {
                using (this.ListViewSource.DeferRefresh())
                {
                    Func<SortDescription, bool> lamda = item => item.PropertyName.Equals(currentHeader.Column.Header.ToString());
                    if (this.ListViewSource.SortDescriptions.Count(lamda) > 0)
                    {
                        SortDescription currentSortDescription = this.ListViewSource.SortDescriptions.First(lamda);
                        ListSortDirection sortDescription = currentSortDescription.Direction == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;


                        currentHeader.Column.HeaderTemplate = currentSortDescription.Direction == ListSortDirection.Ascending ?
                            this.Resources["HeaderTemplateArrowDown"] as DataTemplate : this.Resources["HeaderTemplateArrowUp"] as DataTemplate;

                        this.ListViewSource.SortDescriptions.Remove(currentSortDescription);
                        this.ListViewSource.SortDescriptions.Insert(0, new SortDescription(currentHeader.Column.Header.ToString(), sortDescription));
                    }
                    else
                        this.ListViewSource.SortDescriptions.Add(new SortDescription(currentHeader.Column.Header.ToString(), ListSortDirection.Ascending));
                }
            }
        }

        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            this.ListViewSource.Filter = item =>
            {
                ViewItem vitem = item as ViewItem;
                if (vitem == null)
                {
                    return false;
                }

                PropertyInfo info = item.GetType().GetProperty(cmbProperty.Text);
                if (info == null)
                {
                    return false;
                }

                return info.GetValue(vitem, null).ToString().ToLower().Contains(this.txtFilter.Text.ToLower());
            };
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            this.ListViewSource.Filter = item => true;
        }

        private void btnGroup_Click(object sender, RoutedEventArgs e)
        {
            this.ListViewSource.GroupDescriptions.Clear();

            PropertyInfo pinfo = typeof(ViewItem).GetProperty(cmbGroups.Text);
            if (pinfo != null)
            {
                this.ListViewSource.GroupDescriptions.Add(new PropertyGroupDescription(pinfo.Name));
            }

        }

        private void btnClearGr_Click(object sender, RoutedEventArgs e)
        {
            this.ListViewSource.GroupDescriptions.Clear();
        }

        private void btnNavigation_Click(object sender, RoutedEventArgs e)
        {
            Button CurrentButton = sender as Button;

            switch (CurrentButton.Tag.ToString())
            {
                case "0":
                    this.ListViewSource.MoveCurrentToFirst();
                    break;
                case "1":
                    this.ListViewSource.MoveCurrentToPrevious();
                    break;
                case "2":
                    this.ListViewSource.MoveCurrentToNext();
                    break;
                case "3":
                    this.ListViewSource.MoveCurrentToLast();
                    break;
            }

        }

        private void btnEvaluate_Click(object sender, RoutedEventArgs e)
        {
            ViewItem item = this.lvItems.SelectedItem as ViewItem;

            string msg = $"Hallo {item.Name}, Developer für {item.Developer} mit Gehalt von {item.Gehalt.ToString("C2")}; Status={item.Status}";
            MessageBox.Show(msg);
        }

        private void MouseDoubleClickHandler(object sender, MouseButtonEventArgs e)
        {
            ListViewItem lvItem = sender as ListViewItem;
            ViewItem item = lvItem.Content as ViewItem;
            string msg = $"Hallo {item.Name}, Developer für {item.Developer} mit Gehalt von {item.Gehalt.ToString("C2")}; Status={item.Status}";
            MessageBox.Show(msg);
        }

        private void CurrentListViewItemClick(object sender, RoutedEventArgs e)
        {
            ViewItem item = this.lvItems.SelectedItem as ViewItem;

            string msg = $"Hallo {item.Name}, Developer für {item.Developer} mit Gehalt von {item.Gehalt.ToString("C2")}; Status={item.Status}";
            MessageBox.Show(msg);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string jsonText = JsonSerializer.Serialize<ICollectionView>(this.ListViewSource);
            File.WriteAllText("ListViewDemo.json",jsonText);
            MessageBox.Show("Aktuelle Ansich nach 'ListViewDemo.json' gespeichert", "Aktuelle Ansicht");
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler == null)
            {
                return;
            }

            var e = new PropertyChangedEventArgs(propertyName);
            handler(this, e);
        }
    }
}