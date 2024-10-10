namespace UI.ListViewDemo
{
    using System.ComponentModel;
    using System.Data;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private DataTable listviewSource = null;

        public MainWindow()
        {
            this.InitializeComponent();
            this.ListViewSource = new DemoData().TableRows;
            this.ChangeView("GridView");

#if DEBUG
            PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Critical;
#endif
            this.DataContext = this;
        }

        public DataTable ListViewSource
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

        private void SwitchViewMenu(object sender, RoutedEventArgs args)
        {
            MenuItem mi = (MenuItem)sender;
            ChangeView(mi.Header.ToString());
        }

        private void ChangeView(string str)
        {
            if (str == "GridView")
            {
                lv.View = lv.FindResource("gridView") as ViewBase;
            }
            else if (str == "TileView")
            {
                lv.View = lv.FindResource("tileView") as ViewBase;
            }
            else if (str == "IconView")
            {
                lv.View = lv.FindResource("iconView") as ViewBase;
            }
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

    public class BindingErrorListener : TraceListener
    {
        private readonly Action<string> _errorHandler;

        public BindingErrorListener(Action<string> errorHandler)
        {
            _errorHandler = errorHandler;
            TraceSource bindingTrace = PresentationTraceSources.DataBindingSource;

            bindingTrace.Listeners.Add(this);
            bindingTrace.Switch.Level = SourceLevels.Error;
        }

        public override void WriteLine(string message)
        {
            _errorHandler?.Invoke(message);
        }

        public override void Write(string message)
        {
        }
    }
}