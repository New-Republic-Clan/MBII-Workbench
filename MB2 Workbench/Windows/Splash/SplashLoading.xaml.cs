using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MB2_Workbench.Windows.Splash
{
    /// <summary>
    /// Interaction logic for Loading.xaml
    /// </summary>
    public partial class SplashLoading : Window
    {
        public SplashLoading()
        {

            InitializeComponent();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
