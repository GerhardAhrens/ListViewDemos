namespace ListViewControl
{
    using System.Windows;

    using ListViewControl.ViewModel;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindowVM rootVM = null;

        public MainWindow()
        {
            this.InitializeComponent();

            if (rootVM == null)
            {
                rootVM = new MainWindowVM();
            }

            this.DataContext = this.rootVM;
        }
    }
}