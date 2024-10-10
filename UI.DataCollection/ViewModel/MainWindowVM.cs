namespace UI.DataCollection
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Data;
    using System.Diagnostics;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    using EasyPrototypingNET.BaseClass;
    using EasyPrototypingNET.Core;
    using EasyPrototypingNET.Interface;
    using EasyPrototypingNET.WPF;

    [ViewModel]
    internal sealed class MainWindowVM : ViewModelBase<MainWindowVM>, IViewModel
    {
        public MainWindowVM()
        {
            this.InitCommands();
            this.LoadData();
        }

        [PropertyBinding]
        public ICollectionView DialogDataView
        {
            get { return this.Get<ICollectionView>(); }
            private set { this.Set(value); }
        }

        [PropertyBinding]
        public DataRow CurrentSelectedItem
        {
            get { return this.Get<DataRow>(); }
            set { this.Set(value); }
        }

        [PropertyBinding]
        public string FilterDefaultSearch
        {
            get { return this.Get<string>(); }
            set { this.Set(value, this.RefreshDefaultFilter); }
        }

        [PropertyBinding]
        public int SelectedCheckBoxRows
        {
            get { return this.Get<int>(); }
            set { this.Set(value); }
        }

        [PropertyBinding]
        public string SelectedCheckBoxHeader
        {
            get { return this.Get<string>(); }
            set { this.Set(value); }
        }

        [PropertyBinding]
        public bool? AllItemsChecked
        {
            get { return this.Get<bool?>(); }
            set { this.Set(value, this.ShowAllItemsChecked); }
        }

        [PropertyBinding]
        public int SelectedRows
        {
            get { return this.Get<int>(); }
            set { this.Set(value); }
        }

        [PropertyBinding]
        public string SelectedRowHeader
        {
            get { return this.Get<string>(); }
            set { this.Set(value); }
        }

        public override void InitCommands()
        {
            this.CmdAgg.AddOrSetCommand("WindowCloseCommand", new RelayCommand(p1 => this.WindowCloseHandler(), p2 => true));

            this.CmdAgg.AddOrSetCommand("ClearFilterCommand", new RelayCommand(p1 => this.ClearFilterHandler(), p2 => this.CanClearFilterHandler()));
            this.CmdAgg.AddOrSetCommand("EditEntryCommand", new RelayCommand(p1 => this.EditEntryHandler(), p2 => this.CanEditEntryHandler()));
            this.CmdAgg.AddOrSetCommand("EditEntryCommand", new RelayCommand(p1 => this.EditEntryHandler(), p2 => this.CanEditEntryHandler()));
            this.CmdAgg.AddOrSetCommand("SelectionChangedCommand", new RelayCommand(p1 => this.SelectionChangedHandler(p1), p2 => true));
            this.CmdAgg.AddOrSetCommand("CheckBoxCheckedCommand", new RelayCommand(p1 => this.CheckBoxCheckedHandler(p1), p2 => true));
            this.CmdAgg.AddOrSetCommand("AllRowsMarked", new RelayCommand(p1 => this.ShowAllItemsChecked(null), p2 => true));
            this.CmdAgg.AddOrSetCommand("ContextMenuCommand", new RelayCommand(p1 => this.ContextMenuHandler(p1), p2 => true));
        }

        public override void OnViewIsClosing(CancelEventArgs eventArgs)
        {
            if (this.CanSave == false)
            {
                var userResponse = MessageBoxEx.Show("Wirklich beenden?", "Bist Du Sicher?",string.Empty, MessageBoxButton.YesNo, InstructionIcon.Information, DialogResultsEx.No);

                if (userResponse == DialogResultsEx.No)
                {
                    eventArgs.Cancel = true;
                }
            }

            base.OnViewIsClosing(eventArgs);
        }

        private void WindowCloseHandler()
        {
            Window currentWindow = Application.Current.Windows.LastActiveWindow();

            if (currentWindow != null)
            {
                currentWindow.Close();
            }
        }

        private void LoadData()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            List<DataRow> dataDource = new DemoData().BuildDataRow();
            this.DialogDataView = CollectionViewSource.GetDefaultView(dataDource);
            if (this.DialogDataView != null)
            {
                this.DialogDataView.Filter = rowItem => this.DataDefaultFilter(rowItem as DataRow);

                using (this.DialogDataView.DeferRefresh())
                {
                    this.DialogDataView.SortDescriptions.Clear();
                    SortDescription sd = new SortDescription("[Zahlen]", ListSortDirection.Ascending);
                    this.DialogDataView.SortDescriptions.Add(sd);
                }

                this.MaxRowCount = this.DialogDataView.Count<DataRow>();

                if (this.MaxRowCount > 0)
                {
                    stopwatch.Stop();
                    long loadingTime = stopwatch.ElapsedMilliseconds;
                    this.SelectedRowHeader = $"Ausgewählt: {0} von {this.MaxRowCount.ToString("N0")}; {loadingTime.ToString("N0")} ms";
                }
            }
        }

        private bool DataDefaultFilter(DataRow rowItem)
        {
            bool wordFound = false;

            if (rowItem == null)
            {
                return false;
            }

            string textFilterString = (this.FilterDefaultSearch ?? string.Empty).ToUpper();
            if (string.IsNullOrEmpty(textFilterString) == false)
            {
                string fullRow = rowItem.ItemArrayToString().ToUpper();
                if (string.IsNullOrEmpty(fullRow) == true)
                {
                    return true;
                }

                string[] words = textFilterString.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string word in words.AsParallel<string>())
                {
                    wordFound = fullRow.Contains(word);

                    if (wordFound == false)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private void RefreshDefaultFilter(string value)
        {
            if (value != null && this.DialogDataView != null)
            {
                this.DialogDataView.Refresh();
                this.MaxRowCount = this.DialogDataView.Cast<DataRow>().Count();
                this.SelectedRowHeader = $"Ausgewählt: {0} von {this.MaxRowCount.ToString("N0")}";
                this.DialogDataView.MoveCurrentToFirst();
            }
        }

        private bool CanClearFilterHandler()
        {
            return string.IsNullOrEmpty(this.FilterDefaultSearch) == false;
        }

        private void ClearFilterHandler()
        {
            this.FilterDefaultSearch = string.Empty;
        }

        private bool CanEditEntryHandler()
        {
            if (this.MaxRowCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void EditEntryHandler()
        {
            DialogResultsEx msg = MessageBoxEx.Show("ListView Demo", "Doppelklick auf gewälten Datensatz", $"{string.Join(';',this.CurrentSelectedItem.ItemArray)}", MessageBoxButton.OK, InstructionIcon.Information, DialogResultsEx.No);
        }

        private void SelectionChangedHandler(object commandParameter)
        {
            if (commandParameter != null)
            {
                IEnumerable<DataRow> itemsCollection = ((Collection<object>)commandParameter).OfType<DataRow>();
                if (itemsCollection.Count() == 0)
                {
                }
                else if (itemsCollection.Count() == 1)
                {
                }
                else if (itemsCollection.Count() > 1)
                {
                }

                this.SelectedRowHeader = $"Ausgewählt: {itemsCollection.Count()} von {this.MaxRowCount.ToString("N0")}";
            }
        }

        private void CheckBoxCheckedHandler(object argsItem)
        {
            if (argsItem is Collection<object>)
            {
                this.SelectedCheckBoxRows = this.DialogDataView.OfType<DataRow>().Count(c => c.GetAs<bool>("IsSelected") == true);
                this.SelectedCheckBoxHeader = $"{this.SelectedCheckBoxRows}";
            }
            else if (argsItem is DataRow)
            {
                if (this.CurrentSelectedItem.GetAs<bool>("IsSelected") == true)
                {
                    this.CurrentSelectedItem.SetField<bool>("IsSelected",false);
                }
                else
                {
                    this.CurrentSelectedItem.SetField<bool>("IsSelected", true);
                }

                this.CurrentSelectedItem.AcceptChanges();
            }

            this.DialogDataView.Refresh();
            this.SelectedCheckBoxRows = this.DialogDataView.OfType<DataRow>().Count(c => c.GetAs<bool>("IsSelected") == true);
            this.SelectedCheckBoxHeader = $"{this.SelectedCheckBoxRows}";
            this.SelectedRowHeader = $"Ausgewählt: {this.SelectedCheckBoxRows} von {this.MaxRowCount.ToString("N0")}";

        }

        private void ShowAllItemsChecked(bool? obj)
        {
            if (this.DialogDataView?.OfType<DataRow>().Count() > 0)
            {
                int selectedRows = 0;
                if (obj == null)
                {
                    int currentCheckedCount = this.DialogDataView.OfType<DataRow>().Count(c => c.GetAs<bool>("IsSelected") == true);
                    if (currentCheckedCount == 0)
                    {
                        foreach (DataRow item in this.DialogDataView.OfType<DataRow>())
                        {
                            item.SetField<bool>("IsSelected", true);
                            selectedRows++;
                        }

                        this.DialogDataView.Refresh();
                    }
                    else
                    {
                        foreach (DataRow item in this.DialogDataView.OfType<DataRow>())
                        {
                            item.SetField<bool>("IsSelected", false);
                        }

                        this.DialogDataView.Refresh();
                        this.AllItemsChecked = false;
                    }
                }
                else
                {
                    foreach (DataRow item in this.DialogDataView.OfType<DataRow>())
                    {
                        item.SetField<bool>("IsSelected", (bool)this.AllItemsChecked);
                        if (this.AllItemsChecked == true)
                        {
                            selectedRows++;
                        }
                    }

                    this.DialogDataView.Refresh();
                }

                this.SelectedCheckBoxHeader = $"{selectedRows}";
                this.SelectedRowHeader = $"Ausgewählt: {selectedRows} von {this.MaxRowCount.ToString("N0")}";
            }
        }

        private void ContextMenuHandler(object auswahl)
        {
            DataRow row = this.CurrentSelectedItem as DataRow;
            if (auswahl != null && auswahl.ToString() == "A")
            {
                int zahl = row.GetAs<int>("Zahlen");
                DialogResultsEx msg = MessageBoxEx.Show("ListView Demo", "Contextmenü-A auf gewählten Datensatz", $"{string.Join(';', this.CurrentSelectedItem.ItemArray)}", MessageBoxButton.OK, InstructionIcon.Information, DialogResultsEx.No);
            }
            else if (auswahl != null && auswahl.ToString() == "B")
            {
                DialogResultsEx msg = MessageBoxEx.Show("ListView Demo", "Contextmenü-B auf gewählten Datensatz", $"{string.Join(';', this.CurrentSelectedItem.ItemArray)}", MessageBoxButton.OK, InstructionIcon.Information, DialogResultsEx.No);
            }
        }
    }

    public class MyListView : ListView
    {

    }
}