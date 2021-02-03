using MB2_Workbench.DataTypes.Enums;
using MB2_Workbench.Classes.SiegeDeserializer;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MB2_Workbench.Classes;
using System.Threading;
using Microsoft.Win32;
using Path = System.IO.Path;
using MB2_Workbench.DataTypes.Character;
using MB2_Workbench.DataImport;
using MB2_Workbench.Windows.Dialogs;
using MB2_Workbench.Windows.Splash;

namespace MB2_Workbench
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        /* Application Load */
        public MainWindow()
        {
            InitializeComponent();

            /* Hide Main Window while we load */
            this.Hide();

            /* Check if OpenJK Directory has been set and if not ask for it */
            if (Settings.GetValue("OpenJKDirectory") == null)
                Settings.FindOpenJKDirectory();

            /* Create Home Directory for projects and settings */
            if (!Directory.Exists(Settings.UsersProjectHome))
                Directory.CreateDirectory(Settings.UsersProjectHome);

            /* Show Splash Screen While we load */
            SplashLoading splashLoading = new SplashLoading();
            splashLoading.Show();
            splashLoading.Activate();

            /* Runs Import Job which fetches our imported data */
            Thread ImportJob = new Thread(() => {
                ImportJob import = new ImportJob();
                import.BeginImport();
            });

            ImportJob.Start();

            /* Now hide these until a project is loaded / created */
            ProjectDataTree.Visibility = Visibility.Hidden;
            ImportedDataTree.Visibility = Visibility.Hidden;

        }

        /* ---------------------------------------*/
        /* TOOL BAR ACTIONS */
        /* ---------------------------------------*/

        /* Toolbar Menu - New Project Clicked */
        private void MenuItemNew_Click(object sender, RoutedEventArgs e)
        {
            DialogNewProject newProject = new DialogNewProject();
            newProject.Show();
            newProject.Activate();
        }

        /* Toolbar Menu - Save Project Clicked */
        private void MenuItemSave_Click(object sender, RoutedEventArgs e)
        {
            ActiveProject.Save();
        }

        /* Toolbar Menu - Open Project Clicked */
        private void MenuItemOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog projectFinder = new OpenFileDialog();

            projectFinder.Filter = "Workbench Project|*.mbwb|All files (*.*)|*.*";
            projectFinder.FilterIndex = 1;
            projectFinder.DefaultExt = "*.mbwb";
            projectFinder.CheckFileExists = true;
            projectFinder.Title = "Open Workbench Project";
            projectFinder.InitialDirectory = Settings.UsersProjectHome;

            if (projectFinder.ShowDialog() == true)
            {
                ActiveProject.Load(projectFinder.FileName);
            }
        }

        /* Handles right or left clicking selects a Tree View Item */
        private void TreeViewItem_SelectItemButtonDown(object sender, RoutedEventArgs e)
        {

            TreeViewItem treeViewItem = VisualUpwardSearch(e.OriginalSource as DependencyObject);

            if (treeViewItem != null)
            {
                treeViewItem.Focus();
                e.Handled = true;
            }


        }

        /* Handles right or left clicking selects a Tree View Item */
        static TreeViewItem VisualUpwardSearch(DependencyObject source)
        {
            while (source != null && !(source is TreeViewItem))
                source = VisualTreeHelper.GetParent(source);

            return source as TreeViewItem;
        }

        /* ---------------------------------------*/
        /* TREEVIEW CONTEXT BAR ACTIONS */
        /* ---------------------------------------*/

        /* Context Menu - Project, Edit Character */
        private void EditProjectCharacter_Click(object sender, RoutedEventArgs e)
        {

            MenuItem selectedMenuItem = e.Source as MenuItem;
            Character character = (Character)selectedMenuItem.DataContext;

            DialogEditCharacter editCharacter = new DialogEditCharacter(character);
            editCharacter.Show();
            editCharacter.Activate();
        }

        /* Context Menu - Characters, Clone Character to Project */
        private void CloneImportedCharacter_Click(object sender, RoutedEventArgs e)
        {
            MenuItem selectedMenuItem = e.Source as MenuItem;
            Character character = (Character)selectedMenuItem.DataContext;

            DialogCloneCharacter newProject = new DialogCloneCharacter(character);
            newProject.Show();
            newProject.Activate();

        }
        
    }


}
