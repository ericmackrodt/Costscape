using Microsoft.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Costscape.Common.Behaviors
{
    [TypeConstraint(typeof(TextBox))]
    public class TextBoxUpdateBehavior : DependencyObject, IBehavior
    {
        public ICommand LostFocusCommand
        {
            get { return (ICommand)GetValue(LostFocusCommandProperty); }
            set { SetValue(LostFocusCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LostFocusCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LostFocusCommandProperty =
            DependencyProperty.Register("LostFocusCommand", typeof(ICommand), typeof(TextBoxUpdateBehavior), new PropertyMetadata(null));

        public ICommand KeyUpCommand
        {
            get { return (ICommand)GetValue(KeyUpCommandProperty); }
            set { SetValue(KeyUpCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for KeyUpCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty KeyUpCommandProperty =
            DependencyProperty.Register("KeyUpCommand", typeof(ICommand), typeof(TextBoxUpdateBehavior), new PropertyMetadata(default(ICommand)));
        
        public DependencyObject AssociatedObject { get; set; }

        public TextBox TextBoxObject { get; set; }

        public void Attach(DependencyObject associatedObject)
        {
            AssociatedObject = TextBoxObject = associatedObject as TextBox;

            TextBoxObject.KeyUp += TextBoxObject_KeyUp;
            TextBoxObject.LostFocus += TextBoxObject_LostFocus;
        }

        public void Detach()
        {
            TextBoxObject.KeyUp -= TextBoxObject_KeyUp;
            TextBoxObject.LostFocus -= TextBoxObject_LostFocus;
        }

        private void TextBoxObject_LostFocus(object sender, RoutedEventArgs e)
        {
            var binding = TextBoxObject.GetBindingExpression(TextBox.TextProperty);
            TextBoxObject.SetBinding(TextBox.TextProperty, binding.ParentBinding);

            if (LostFocusCommand != null && LostFocusCommand.CanExecute(TextBoxObject.DataContext))
                LostFocusCommand.Execute(TextBoxObject.DataContext);
        }

        private void TextBoxObject_KeyUp(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            var binding = TextBoxObject.GetBindingExpression(TextBox.TextProperty);
            binding.UpdateSource();

            if (KeyUpCommand != null && KeyUpCommand.CanExecute(TextBoxObject.DataContext))
                KeyUpCommand.Execute(TextBoxObject.DataContext);
        }
    }
}
