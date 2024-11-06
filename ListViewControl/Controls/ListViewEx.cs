namespace ListViewControl.Controls
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;

    public class ListViewEx : ListView
    {
        static ListViewEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ListViewEx),
                        new FrameworkPropertyMetadata(typeof(ListViewEx)));
        }
        public ListViewEx()
        {
            this.Loaded += this.OnLoaded;
            this.MouseDoubleClick += this.OnMouseDoubleClick;
        }

        ~ListViewEx()
        {
            this.Loaded -= this.OnLoaded;
            this.MouseDoubleClick -= this.OnMouseDoubleClick;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
        }

        private void OnMouseDoubleClick(object sender, RoutedEventArgs e)
        {
        }
    }

    public class GridViewColumnHeaderEx : GridViewColumnHeader
    {
        public GridViewColumnHeaderEx()
        {
            this.Click += this.OnClickHeader;
        }

        ~ GridViewColumnHeaderEx()
        {
            this.Click -= this.OnClickHeader;
        }

        private void OnClickHeader(object sender, RoutedEventArgs e)
        {
            string headerText = (string)((System.Windows.Controls.ContentControl)sender).Content;
            string column = (string)((System.Windows.Controls.ContentControl)sender).Tag;
        }
    }

    public class SortAdorner : Adorner
    {
        private static Geometry ascGeometry = Geometry.Parse("M 0 4 L 3.5 0 L 7 4 Z");
        private static Geometry descGeometry = Geometry.Parse("M 0 0 L 3.5 4 L 7 0 Z");

        public ListSortDirection Direction { get; private set; }

        public SortAdorner(UIElement element, ListSortDirection dir) : base(element)
        {
            this.Direction = dir;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (AdornedElement.RenderSize.Width < 20)
            {
                return;
            }

            TranslateTransform transform = new TranslateTransform
                (
                    AdornedElement.RenderSize.Width - 15,
                    (AdornedElement.RenderSize.Height - 5) / 2
                );
            drawingContext.PushTransform(transform);

            Geometry geometry = ascGeometry;
            if (this.Direction == ListSortDirection.Descending)
                geometry = descGeometry;
            drawingContext.DrawGeometry(Brushes.Black, null, geometry);

            drawingContext.Pop();
        }
    }
}
