namespace UI.DataCollection
{
    using System.Windows;

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

            this.DataContext = rootVM;
        }

        private void ctxSortAbsteigend_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ctxSortAufsteigend_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}