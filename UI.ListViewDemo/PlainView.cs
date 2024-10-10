namespace UI.ListViewDemo
{
    using System.Windows;
    using System.Windows.Controls;

    public class PlainView : ViewBase
    {
        #region ItemContainerStyle
        public static readonly DependencyProperty ItemContainerStyleProperty = ItemsControl.ItemContainerStyleProperty.AddOwner(typeof(PlainView));

        public Style ItemContainerStyle
        {
            get { return (Style)GetValue(ItemContainerStyleProperty); }
            set { SetValue(ItemContainerStyleProperty, value); }
        }
        #endregion ItemContainerStyle

        #region ItemTemplate
        public static readonly DependencyProperty ItemTemplateProperty = ItemsControl.ItemTemplateProperty.AddOwner(typeof(PlainView));

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }
        #endregion ItemTemplate

        #region ItemWidth
        public static readonly DependencyProperty ItemWidthProperty = WrapPanel.ItemWidthProperty.AddOwner(typeof(PlainView));

        public double ItemWidth
        {
            get { return (double)GetValue(ItemWidthProperty); }
            set { SetValue(ItemWidthProperty, value); }
        }
        #endregion ItemWidth

        #region ItemHeight
        public static readonly DependencyProperty ItemHeightProperty = WrapPanel.ItemHeightProperty.AddOwner(typeof(PlainView));

        public double ItemHeight
        {
            get { return (double)GetValue(ItemHeightProperty); }
            set { SetValue(ItemHeightProperty, value); }
        }
        #endregion ItemHeight

        protected override object DefaultStyleKey
        {
            get
            {
                return new ComponentResourceKey(GetType(), "CustomPlainView");
            }
        }
    }
}
