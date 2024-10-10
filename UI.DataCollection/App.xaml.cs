namespace UI.DataCollection
{
    using System.Globalization;
    using System.Windows;
    using System.Windows.Markup;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const string DEFAULTLANGUAGE = "de-DE";

        public App()
        {
            InitializeCultures(DEFAULTLANGUAGE);
        }

        private static void InitializeCultures(string language)
        {
            if (string.IsNullOrEmpty(language) == false)
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(language);
            }
            else
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(DEFAULTLANGUAGE);
            }

            if (string.IsNullOrEmpty(language) == false)
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
            }
            else
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(DEFAULTLANGUAGE);
            }

            FrameworkPropertyMetadata frameworkMetadata = new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(new CultureInfo(language).IetfLanguageTag));
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), frameworkMetadata);
        }
    }
}
