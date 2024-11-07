﻿namespace ListViewControl.Controls
{
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Shapes;

    public class ListViewEx : ListView
    {
        public static readonly DependencyProperty SelectedRowCommandProperty =
            DependencyProperty.Register(nameof(SelectedRowCommand), typeof(ICommand), typeof(ListViewEx), new PropertyMetadata(null));

        public ICommand SelectedRowCommand
        {
            get { return (ICommand)GetValue(SelectedRowCommandProperty); }
            set { SetValue(SelectedRowCommandProperty, value); }
        }

        public static readonly DependencyProperty SelectionChangedCommandProperty =
    DependencyProperty.Register(nameof(SelectionChangedCommand), typeof(ICommand), typeof(ListViewEx), new PropertyMetadata(null));

        public ICommand SelectionChangedCommand
        {
            get { return (ICommand)GetValue(SelectionChangedCommandProperty); }
            set { SetValue(SelectionChangedCommandProperty, value); }
        }

        static ListViewEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ListViewEx),
                        new FrameworkPropertyMetadata(typeof(ListViewEx)));
        }

        public ListViewEx()
        {
            this.Loaded += this.OnLoaded;
            this.MouseDoubleClick += this.OnMouseDoubleClick;
            this.KeyDown += this.OnKeyDown;
            this.AlternationCount = 2;
            this.SelectionChanged += this.OnSelectionChanged;
            this.SetValue(KeyboardNavigation.IsTabStopProperty, false);
            this.SetValue(ScrollViewer.IsDeferredScrollingEnabledProperty, false);
            this.SetValue(VirtualizingPanel.IsVirtualizingProperty, true);
            this.SetValue(VirtualizingPanel.VirtualizationModeProperty, VirtualizationMode.Recycling);
        }

        ~ListViewEx()
        {
            this.Loaded -= this.OnLoaded;
            this.MouseDoubleClick -= this.OnMouseDoubleClick;
            this.KeyDown -= this.OnKeyDown;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        private void OnMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            DependencyObject originalSource = (DependencyObject)e.OriginalSource;
            while ((originalSource != null) && !(originalSource is ListViewItem))
            {
                originalSource = VisualTreeHelper.GetParent(originalSource);
                if (originalSource != null && (originalSource.GetType() == typeof(Thumb) || originalSource.GetType() == typeof(ScrollViewer)))
                {
                    e.Handled = true;
                    return;
                }
            }

            if (this.SelectedRowCommand != null && this.SelectedRowCommand.CanExecute(this.SelectedItem) == true)
            {
                this.SelectedRowCommand.Execute(this.SelectedItem);
            }
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.SelectionChangedCommand != null && this.SelectionChangedCommand.CanExecute(this.SelectedItem) == true)
            {
                this.SelectionChangedCommand.Execute(this.SelectedItem);
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (this.SelectedRowCommand != null && this.SelectedRowCommand.CanExecute(this.SelectedItem) == true)
                {
                    this.SelectedRowCommand.Execute(this.SelectedItem);
                }
            }
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

    public class ListViewSort
    {
        public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached(
                "Command", typeof(ICommand), typeof(ListViewSort),
                new UIPropertyMetadata(
                    null,
                    (o, e) =>
                    {
                        ItemsControl listView = o as ItemsControl;
                        if (listView != null)
                        {
                            if (!GetAutoSort(listView)) // Don't change click handler if AutoSort enabled
                            {
                                if (e.OldValue != null && e.NewValue == null)
                                {
                                    listView.RemoveHandler(GridViewColumnHeader.ClickEvent, new RoutedEventHandler(ColumnHeader_Click));
                                }
                                if (e.OldValue == null && e.NewValue != null)
                                {
                                    listView.AddHandler(GridViewColumnHeader.ClickEvent, new RoutedEventHandler(ColumnHeader_Click));
                                }
                            }
                        }
                    }
                ));

        public static readonly DependencyProperty AutoSortProperty = DependencyProperty.RegisterAttached(
        "AutoSort", typeof(bool), typeof(ListViewSort),
        new UIPropertyMetadata(
            false,
            (o, e) =>
            {
                ListView listView = o as ListView;
                if (listView != null)
                {
                    if (GetCommand(listView) == null) // Don't change click handler if a command is set
                    {
                        bool oldValue = (bool)e.OldValue;
                        bool newValue = (bool)e.NewValue;
                        if (oldValue && !newValue)
                        {
                            listView.RemoveHandler(GridViewColumnHeader.ClickEvent, new RoutedEventHandler(ColumnHeader_Click));
                        }
                        if (!oldValue && newValue)
                        {
                            listView.AddHandler(GridViewColumnHeader.ClickEvent, new RoutedEventHandler(ColumnHeader_Click));
                        }
                    }
                }
            }
        ));

        public static readonly DependencyProperty PropertyNameProperty =
            DependencyProperty.RegisterAttached(
                "PropertyName", typeof(string), typeof(ListViewSort), new UIPropertyMetadata(null));

        public static readonly DependencyProperty ShowSortGlyphProperty =
            DependencyProperty.RegisterAttached("ShowSortGlyph", typeof(bool), typeof(ListViewSort), new UIPropertyMetadata(true));

        public static readonly DependencyProperty SortGlyphAscendingProperty =
            DependencyProperty.RegisterAttached("SortGlyphAscending", typeof(ImageSource), typeof(ListViewSort), new UIPropertyMetadata(null));

        public static readonly DependencyProperty SortGlyphDescendingProperty =
            DependencyProperty.RegisterAttached("SortGlyphDescending", typeof(ImageSource), typeof(ListViewSort), new UIPropertyMetadata(null));

        private static readonly DependencyProperty SortedColumnHeaderProperty =
            DependencyProperty.RegisterAttached("SortedColumnHeader", typeof(GridViewColumnHeader), typeof(ListViewSort), new UIPropertyMetadata(null));

        public static ICommand GetCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(CommandProperty);
        }

        public static void SetCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(CommandProperty, value);
        }


        public static bool GetAutoSort(DependencyObject obj)
        {
            return (bool)obj.GetValue(AutoSortProperty);
        }

        public static void SetAutoSort(DependencyObject obj, bool value)
        {
            obj.SetValue(AutoSortProperty, value);
        }


        public static string GetPropertyName(DependencyObject obj)
        {
            return (string)obj.GetValue(PropertyNameProperty);
        }

        public static void SetPropertyName(DependencyObject obj, string value)
        {
            obj.SetValue(PropertyNameProperty, value);
        }

        public static bool GetShowSortGlyph(DependencyObject obj)
        {
            return (bool)obj.GetValue(ShowSortGlyphProperty);
        }

        public static void SetShowSortGlyph(DependencyObject obj, bool value)
        {
            obj.SetValue(ShowSortGlyphProperty, value);
        }

        public static ImageSource GetSortGlyphAscending(DependencyObject obj)
        {
            return (ImageSource)obj.GetValue(SortGlyphAscendingProperty);
        }

        public static void SetSortGlyphAscending(DependencyObject obj, ImageSource value)
        {
            obj.SetValue(SortGlyphAscendingProperty, value);
        }

        public static ImageSource GetSortGlyphDescending(DependencyObject obj)
        {
            return (ImageSource)obj.GetValue(SortGlyphDescendingProperty);
        }

        public static void SetSortGlyphDescending(DependencyObject obj, ImageSource value)
        {
            obj.SetValue(SortGlyphDescendingProperty, value);
        }

        private static GridViewColumnHeader GetSortedColumnHeader(DependencyObject obj)
        {
            return (GridViewColumnHeader)obj.GetValue(SortedColumnHeaderProperty);
        }

        private static void SetSortedColumnHeader(DependencyObject obj, GridViewColumnHeader value)
        {
            obj.SetValue(SortedColumnHeaderProperty, value);
        }

        private static void ColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader headerClicked = e.OriginalSource as GridViewColumnHeader;
            if (headerClicked != null && headerClicked.Column != null)
            {
                string propertyName = GetPropertyName(headerClicked.Column);
                if (!string.IsNullOrEmpty(propertyName))
                {
                    ListView listView = GetAncestor<ListView>(headerClicked);
                    if (listView != null)
                    {
                        ICommand command = GetCommand(listView);
                        if (command != null)
                        {
                            if (command.CanExecute(propertyName))
                            {
                                command.Execute(propertyName);
                            }
                        }
                        else if (GetAutoSort(listView))
                        {
                            ApplySort(listView.Items, propertyName, listView, headerClicked);
                        }
                    }
                }
            }
        }

        private static T GetAncestor<T>(DependencyObject reference) where T : DependencyObject
        {
            DependencyObject parent = VisualTreeHelper.GetParent(reference);
            while (!(parent is T))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            if (parent != null)
            {
                return (T)parent;
            }
            else
            {
                return null;
            }
        }

        private static void ApplySort(ICollectionView view, string propertyName, ListView listView, GridViewColumnHeader sortedColumnHeader)
        {
            ListSortDirection direction = ListSortDirection.Ascending;
            if (view.SortDescriptions.Count > 0)
            {
                SortDescription currentSort = view.SortDescriptions[0];
                if (currentSort.PropertyName == propertyName)
                {
                    if (currentSort.Direction == ListSortDirection.Ascending)
                        direction = ListSortDirection.Descending;
                    else
                        direction = ListSortDirection.Ascending;
                }

                view.SortDescriptions.Clear();

                GridViewColumnHeader currentSortedColumnHeader = GetSortedColumnHeader(listView);
                if (currentSortedColumnHeader != null)
                {
                    RemoveSortGlyph(currentSortedColumnHeader);
                }
            }
            if (!string.IsNullOrEmpty(propertyName))
            {
                view.SortDescriptions.Add(new SortDescription(propertyName, direction));
                if (GetShowSortGlyph(listView))
                {
                    AddSortGlyph(
                        sortedColumnHeader,
                        direction,
                        direction == ListSortDirection.Ascending ? GetSortGlyphAscending(listView) : GetSortGlyphDescending(listView));
                }

                SetSortedColumnHeader(listView, sortedColumnHeader);
            }
        }

        private static void AddSortGlyph(GridViewColumnHeader columnHeader, ListSortDirection direction, ImageSource sortGlyph)
        {
            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(columnHeader);
            adornerLayer.Add(
                new SortGlyphAdorner(
                    columnHeader,
                    direction,
                    sortGlyph
                    ));
        }

        private static void RemoveSortGlyph(GridViewColumnHeader columnHeader)
        {
            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(columnHeader);
            Adorner[] adorners = adornerLayer.GetAdorners(columnHeader);
            if (adorners != null)
            {
                foreach (Adorner adorner in adorners)
                {
                    if (adorner is SortGlyphAdorner)
                        adornerLayer.Remove(adorner);
                }
            }
        }


        #region SortGlyphAdorner nested class

        private class SortGlyphAdorner : Adorner
        {
            private GridViewColumnHeader _columnHeader;
            private ListSortDirection _direction;
            private ImageSource _sortGlyph;

            public SortGlyphAdorner(GridViewColumnHeader columnHeader, ListSortDirection direction, ImageSource sortGlyph)
                : base(columnHeader)
            {
                _columnHeader = columnHeader;
                _direction = direction;
                _sortGlyph = sortGlyph;
            }

            private Geometry GetDefaultGlyph()
            {
                double x1 = _columnHeader.ActualWidth - 13;
                double x2 = x1 + 10;
                double x3 = x1 + 5;
                double y1 = _columnHeader.ActualHeight / 2 - 3;
                double y2 = y1 + 5;

                if (_direction == ListSortDirection.Ascending)
                {
                    double tmp = y1;
                    y1 = y2;
                    y2 = tmp;
                }

                PathSegmentCollection pathSegmentCollection = new PathSegmentCollection();
                pathSegmentCollection.Add(new LineSegment(new Point(x2, y1), true));
                pathSegmentCollection.Add(new LineSegment(new Point(x3, y2), true));

                PathFigure pathFigure = new PathFigure(
                    new Point(x1, y1),
                    pathSegmentCollection,
                    true);

                PathFigureCollection pathFigureCollection = new PathFigureCollection();
                pathFigureCollection.Add(pathFigure);

                PathGeometry pathGeometry = new PathGeometry(pathFigureCollection);
                return pathGeometry;
            }

            protected override void OnRender(DrawingContext drawingContext)
            {
                base.OnRender(drawingContext);

                if (_sortGlyph != null)
                {
                    double x = _columnHeader.ActualWidth - 13;
                    double y = _columnHeader.ActualHeight / 2 - 5;
                    Rect rect = new Rect(x, y, 10, 10);
                    drawingContext.DrawImage(_sortGlyph, rect);
                }
                else
                {
                    drawingContext.DrawGeometry(Brushes.LightGray, new Pen(Brushes.Gray, 1.0), GetDefaultGlyph());
                }
            }
        }

        #endregion
    }

    public class GridViewColumnLines : GridViewRowPresenter
    {
        public static readonly DependencyProperty SeparatorStyleProperty;
        private static readonly Style DefaultSeparatorStyle;
        private readonly List<FrameworkElement> lines = new List<FrameworkElement>();

        static GridViewColumnLines()
        {
            DefaultSeparatorStyle = new Style(typeof(Rectangle));
            DefaultSeparatorStyle.Setters.Add(new Setter(Shape.FillProperty, SystemColors.ControlLightBrush));
            SeparatorStyleProperty = DependencyProperty.Register("SeparatorStyle", typeof(Style), typeof(GridViewColumnLines),
                                                                    new UIPropertyMetadata(DefaultSeparatorStyle, SeparatorStyleChanged));
        }

        public Style SeparatorStyle
        {
            get { return (Style)GetValue(SeparatorStyleProperty); }
            set { this.SetValue(SeparatorStyleProperty, value); }
        }

        protected override int VisualChildrenCount
        {
            get { return base.VisualChildrenCount + this.lines.Count; }
        }

        private IEnumerable<FrameworkElement> Children
        {
            get { return LogicalTreeHelper.GetChildren(this).OfType<FrameworkElement>(); }
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            var size = base.ArrangeOverride(arrangeSize);
            var children = this.Children.ToList();
            this.EnsureLines(children.Count);
            for (var i = 0; i < this.lines.Count; i++)
            {
                var child = children[i];
                var x = child.TransformToAncestor(this).Transform(new Point(child.ActualWidth, 0)).X + child.Margin.Right;
                var rect = new Rect(x, -Margin.Top, 1, size.Height + Margin.Top + Margin.Bottom);
                var line = this.lines[i];
                line.Measure(rect.Size);
                line.Arrange(rect);
            }

            return size;
        }

        protected override Visual GetVisualChild(int index)
        {
            var count = base.VisualChildrenCount;
            return index < count ? base.GetVisualChild(index) : this.lines[index - count];
        }

        private static void SeparatorStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var presenter = (GridViewColumnLines)d;
            var style = (Style)e.NewValue;
            foreach (FrameworkElement line in presenter.lines)
            {
                line.Style = style;
            }
        }

        private void EnsureLines(int count)
        {
            count = count - this.lines.Count;
            for (var i = 0; i < count; i++)
            {
                var line = (FrameworkElement)Activator.CreateInstance(this.SeparatorStyle.TargetType);
                line = new Rectangle { Fill = Brushes.LightGray };
                line.Style = this.SeparatorStyle;
                this.AddVisualChild(line);
                this.lines.Add(line);
            }
        }
    }
}
