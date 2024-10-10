//-----------------------------------------------------------------------
// <copyright file="TextBoxEnterKeyUpdateBehavior.cs" company="Lifeprojects.de">
//     Class: TextBoxEnterKeyUpdateBehavior
//     Copyright © Lifeprojects.de 2024
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>18.01.2024 11:23:37</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace UI.ICollectionViewDemo.Behavior
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Microsoft.Xaml.Behaviors;

    public class TextBoxEnterKeyUpdateBehavior : Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            if (this.AssociatedObject != null)
            {
                base.OnAttached();
                this.AssociatedObject.KeyDown += AssociatedObject_KeyDown;
            }
        }

        protected override void OnDetaching()
        {
            if (this.AssociatedObject != null)
            {
                this.AssociatedObject.KeyDown -= AssociatedObject_KeyDown;
                base.OnDetaching();
            }
        }

        private void AssociatedObject_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                if (e.Key == Key.Return)
                {
                    if (e.Key == Key.Enter)
                    {
                        textBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                    }
                }
            }
        }
    }

    public class ComboBoxEnterKeyUpdateBehavior : Behavior<ComboBox>
    {
        protected override void OnAttached()
        {
            if (this.AssociatedObject != null)
            {
                base.OnAttached();
                this.AssociatedObject.KeyDown += AssociatedObject_KeyDown;
            }
        }

        protected override void OnDetaching()
        {
            if (this.AssociatedObject != null)
            {
                this.AssociatedObject.KeyDown -= AssociatedObject_KeyDown;
                base.OnDetaching();
            }
        }

        private void AssociatedObject_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                if (e.Key == Key.Return)
                {
                    if (e.Key == Key.Enter)
                    {
                        comboBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                    }
                }
            }
        }
    }

    public class SelectAllTextOnFocusMultiBehavior : Behavior<UIElement>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.GotFocus += HandleGotFocus;
            AssociatedObject.GotKeyboardFocus += HandleKeyboardFocus;
            AssociatedObject.GotMouseCapture += HandleMouseCapture;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.GotFocus -= HandleGotFocus;
            AssociatedObject.GotKeyboardFocus -= HandleKeyboardFocus;
            AssociatedObject.GotMouseCapture -= HandleMouseCapture;
        }

        private void HandleGotFocus(object sender, RoutedEventArgs e)
        {
            var txt = e.OriginalSource as TextBox;
            if (txt != null)
            {
                txt.Focus();
                txt.SelectAll();
            }
        }

        private static void HandleKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            var txt = e.NewFocus as TextBox;
            if (txt != null)
            {
                txt.Focus();
                txt.SelectAll();
            }
        }

        private static void HandleMouseCapture(object sender, MouseEventArgs e)
        {
            var txt = e.OriginalSource as TextBox;
            if (txt != null)
            {
                txt.Focus();
                txt.SelectAll();
            }
        }
    }
}
