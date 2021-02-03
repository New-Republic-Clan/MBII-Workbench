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

        public static readonly DependencyProperty FoldersProperty = DependencyProperty.Register("Folders", typeof(string), typeof(TreeViewItemTextBox), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPropertyChanged));

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var i = d.GetValue(FoldersProperty);
            TreeViewItemTextBox c = (TreeViewItemTextBox)d;
            c.ValueTextBox.Text = (string) i;

        }

        public string Folders
        {
            get { return GetValue(FoldersProperty) as string; }
            set { SetValue(FoldersProperty, value); }
        }

        private TextBlock TitleTextBlock = new TextBlock();
        private TextBox ValueTextBox = new TextBox();

        public string Title
        {
            get { return (string) TitleTextBlock.Text; }
            set { TitleTextBlock.Text = value; }
        }


        public TreeViewItemTextBox()
        {
            InitializeComponent();
        }

        

        private void InitializeComponent()
        {

            TreeViewItem parent = new TreeViewItem();

            StackPanel stackPanel = new StackPanel();

            stackPanel.Children.Add(TitleTextBlock);

            stackPanel.Children.Add(new Button()
            {
                Content = "?",
                ToolTip = this.ToolTip
            });

           
            parent.Header = stackPanel;


            /* Binding */
           // BindingOperations.SetBinding(data, TextBox.TextProperty, new Binding(Dias));

            TreeViewItem child = new TreeViewItem();

            //ValueTextBox.Text = Value;

            child.Header = ValueTextBox;

            parent.Items.Add(child);

            Content = parent;

        }
    }
}
