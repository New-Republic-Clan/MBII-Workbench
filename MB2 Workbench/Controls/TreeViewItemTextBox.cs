using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MB2_Workbench.Controls
{

    public class TreeViewItemTextBox : UserControl
    {


        public static DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(string), typeof(TreeViewItemTextBox), new PropertyMetadata(null, Value_PropertyChanged));

        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        private static void Value_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TreeViewItemTextBox customTextBox = d as TreeViewItemTextBox;
            customTextBox.ValueTextBox.Text = (string) e.NewValue; 
        }

        private void ValueTextBox_OnTextChanged(object sender, RoutedEventArgs e)
        {

            if((string) GetValue(TreeViewItemTextBox.ValueProperty) != ValueTextBox.Text )
                SetValue(TreeViewItemTextBox.ValueProperty, ValueTextBox.Text);


        }


        public TextBox ValueTextBox = new TextBox();

        public TreeViewItemTextBox()
        {
            InitializeComponent();
            ValueTextBox.TextChanged += ValueTextBox_OnTextChanged;
        }

        private void InitializeComponent()
        {
            Content = ValueTextBox;
        }




    }
}
