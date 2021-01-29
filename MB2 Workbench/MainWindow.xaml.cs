using MB2_Workbench.Enums;
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
using MB2_Workbench.Popups;
using Path = System.IO.Path;

namespace MB2_Workbench
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        public Thread importJob;

        public List<String> BasePK3s;

        public MainWindow()
        {
            InitializeComponent();

            /* Hide Main Window */
            this.Hide();

            /* Check if OpenJK Directory has been set and if not ask for it */
            if (Settings.GetValue("OpenJKDirectory") == null)
            {

                MessageBox.Show("Please Locate your JediAcademy.exe" + Environment.NewLine + Environment.NewLine + @"Typically this will be in C:\Program Files (x86)\Steam\steamapps\common\Jedi Academy");
                OpenFileDialog jaFinder = new OpenFileDialog();
                jaFinder.Filter = "JediAcademy.exe|*.exe|All files (*.*)|*.*";
                jaFinder.FilterIndex = 1;
                jaFinder.DefaultExt = "*.exe";
                jaFinder.CheckFileExists = true;
                jaFinder.Title = "JediAcademy.exe";

                if (jaFinder.ShowDialog() == true)
                {
                    Settings.SetValue("OpenJKDirectory", Path.GetDirectoryName(jaFinder.FileName));
                }

            }

            /* Show splash screen */
            SplashLoading splashLoading = new SplashLoading();
            splashLoading.Show();
            splashLoading.Activate();

            /* Runs Import Job which builds our imported data treeview */
            importJob = new Thread(() => {
                ImportJob import = new ImportJob();
                import.BeginImport();
            });

            importJob.Start();

            TreeViewItem characters = new TreeViewItem();
            characters.Header = "Characters";
            CustomDataTree.Items.Add(characters);

            TreeViewItem teams = new TreeViewItem();
            teams.Header = "Teams";
            CustomDataTree.Items.Add(teams);

        }


        private void NewProject_Click(object sender, RoutedPropertyChangedEventArgs<object> e)
        {


            NewProject newProject = new NewProject();


            foreach(string pk3 in BasePK3s)
            {
                newProject.BaseMaps.Items.Add(Path.GetFileName(pk3).ToLower().Replace(".pk3",""));
            }

            newProject.Show();
            newProject.Activate();

        }
            

        /* Runs when a Imported Game Data Tree Item is selected */
        private void ImportedDataTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewItem SelectedItem = ImportedDataTree.SelectedItem as TreeViewItem;
            switch (SelectedItem.Tag.ToString())
            {
                case "character":
                    ImportedDataTree.ContextMenu = ImportedDataTree.Resources["CharacterContext"] as System.Windows.Controls.ContextMenu;
                    break;
                case "team":
                    ImportedDataTree.ContextMenu = ImportedDataTree.Resources["TeamContext"] as System.Windows.Controls.ContextMenu;
                    break;
            }
        }

        /* Runs when a Custom Game Data Tree Item is selected */
        private void CustomDataTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewItem SelectedItem = ImportedDataTree.SelectedItem as TreeViewItem;
            switch (SelectedItem.Tag.ToString())
            {
                case "character":
                    ImportedDataTree.ContextMenu = ImportedDataTree.Resources["CharacterContext"] as System.Windows.Controls.ContextMenu;
                    break;
            }
        }

    }


}
