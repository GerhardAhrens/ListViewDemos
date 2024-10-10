
namespace UI.ICollectionViewDemo.XAMLConverter
{
    using System.Windows;
    using System.Windows.Data;

    public class BooleanVisiblityConverter :IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool convParameter = this.GetConverterParameter(parameter);
            bool selected = (bool)value;

            return convParameter==selected ? Visibility.Visible : Visibility.Collapsed;
           
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("Not Implemented");
        }

        private bool GetConverterParameter(object parameter)
        {
            try
            {
                bool convParameter = true;
                if (parameter != null)
                {
                    convParameter = System.Convert.ToBoolean(parameter);
                }

                return convParameter;
            }
            catch
            { 
                return false; 
            }
        }
    }
}
