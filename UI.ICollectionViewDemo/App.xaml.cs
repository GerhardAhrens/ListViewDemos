namespace UI.ICollectionViewDemo
{
    using System.Collections;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Windows;
    using System.Windows.Markup;
    using System.Windows.Threading;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const string DEFAULTLANGUAGE = "de-DE";
        private static readonly string MessageBoxTitle = "ICollectionViewDemo Application";
        private static readonly string UnexpectedError = "An unexpected error occured.";
        string exePath = string.Empty;
        string exeName = string.Empty;

        public App()
        {
            exeName = Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.FriendlyName);
            exePath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
            FrameworkCompatibilityPreferences.KeepTextBoxDisplaySynchronizedWithTextProperty = false;
            Application.Current.DispatcherUnhandledException += OnDispatcherUnhandledException;

            try
            {

                InitializeCultures(DEFAULTLANGUAGE);
            }
            catch (Exception ex)
            {
                ex.Data.Add("UserDomainName", Environment.UserDomainName);
                ex.Data.Add("UserName", Environment.UserName);
                ex.Data.Add("exePath", exePath);
                ErrorMessage(ex, "General Error: ");
                Application.Current.Shutdown(0);
            }
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

        public static void ErrorMessage(Exception ex, string message = "")
        {
            string expMsg = ex.Message;
            var aex = ex as AggregateException;

            if (aex != null && aex.InnerExceptions.Count == 1)
            {
                expMsg = aex.InnerExceptions[0].Message;
            }

            if (string.IsNullOrEmpty(message) == true)
            {
                message = UnexpectedError;
            }

            StringBuilder errorText = new StringBuilder();
            if (ex.Data != null && ex.Data.Count > 0)
            {
                foreach (DictionaryEntry item in ex.Data)
                {
                    errorText.AppendLine($"{item.Key} : {item.Value}");
                }
            }

            MessageBox.Show(
                message + $"{expMsg}\n{ex.Message}\n{errorText.ToString()}",
                MessageBoxTitle,
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Debug.WriteLine($"{exeName}-{(e.Exception as Exception).Message}");
        }

        private void ApplicationExit()
        {
            Application.Current.Shutdown();
            Process.GetCurrentProcess().Kill();
        }
    }
}
