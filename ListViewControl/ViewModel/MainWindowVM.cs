namespace ListViewControl.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Data;

    using EasyPrototypingNET.BaseClass;
    using EasyPrototypingNET.Core;
    using EasyPrototypingNET.Interface;
    using EasyPrototypingNET.WPF;

    public class MainWindowVM : ViewModelBase<MainWindowVM>, IViewModel
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

        public override void InitCommands()
        {
            this.CmdAgg.AddOrSetCommand("WindowCloseCommand", new RelayCommand(p1 => this.WindowCloseHandler(), p2 => true));
            this.CmdAgg.AddOrSetCommand("SelectedRowCommand", new RelayCommand(p1 => this.SelectedRowClick(p1), p2 => true));
        }

        public override void OnViewIsClosing(CancelEventArgs eventArgs)
        {
            if (this.CanSave == false)
            {
                var userResponse = MessageBoxEx.Show("Wirklich beenden?", "Bist Du Sicher?", string.Empty, MessageBoxButton.YesNo, InstructionIcon.Information, DialogResultsEx.No);

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

        private void SelectedRowClick(object selectedRow)
        {
            DialogResultsEx msg = MessageBoxEx.Show("ListView Demo", "Doppelklick auf gewälten Datensatz", $"{string.Join(';', ((DataRow)selectedRow).ItemArray)}", MessageBoxButton.OK, InstructionIcon.Information, DialogResultsEx.No);

        }


        private void LoadData()
        {
            List<DataRow> dataDource = new DemoData().BuildDataRow();
            this.DialogDataView = CollectionViewSource.GetDefaultView(dataDource);
            if (this.DialogDataView != null)
            {
                this.MaxRowCount = this.DialogDataView.Count<DataRow>();
            }

        }
    }
}
