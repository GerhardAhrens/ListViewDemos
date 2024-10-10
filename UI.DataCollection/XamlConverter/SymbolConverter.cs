
namespace UI.DataCollection.XAMLConverter
{
    using System.Collections;
    using System.Data;
    using System.Globalization;
    using System.Windows.Controls;
    using System.Windows.Data;

    public class SymbolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IEnumerable itemSource = ((ListView)parameter).ItemsSource;
            DataRow row = ((CollectionView)itemSource).CurrentItem as DataRow;
            int zahl = row.GetAs<int>("Zahlen");

            return value;
           
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Not Implemented");
        }
    }
}
