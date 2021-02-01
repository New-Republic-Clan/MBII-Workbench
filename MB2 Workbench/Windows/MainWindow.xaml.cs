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
using MB2_Workbench.Popups;
using Path = System.IO.Path;
using MB2_Workbench.DataTypes.Character;
using MB2_Workbench.DataImport;

namespace MB2_Workbench
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        public Thread ImportJob;

        public MainWindow()
        {
            InitializeComponent();

            /* Hide Main Window */
            this.Hide();

            /* Check if OpenJK Directory has been set and if not ask for it */
            if (Settings.GetValue("OpenJKDirectory") == null)
            {
                Settings.FindOpenJKDirectory();
            }


            /* Create Home Directory */
            if (!Directory.Exists(Settings.UsersProjectHome))
            {
                Directory.CreateDirectory(Settings.UsersProjectHome);
            }

            /* Show Splash Screen While we load */
            SplashLoading splashLoading = new SplashLoading();
            splashLoading.Show();
            splashLoading.Activate();

            /* Runs Import Job which builds our Imported Data TreeView */
            ImportJob = new Thread(() => {
                ImportJob import = new ImportJob();
                import.BeginImport();
            });

            ImportJob.Start();

            /* Now hide it until a new project is started or one has been loaded */
            //CustomDataTree.Visibility = Visibility.Hidden;
            //ImportedDataTree.Visibility = Visibility.Hidden;

        }

        /* Runs when a Imported Game Data Tree Item is selected */
        private void ImportedDataTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewItem SelectedItem = ImportedDataTree.SelectedItem as TreeViewItem;
            if (SelectedItem == null || SelectedItem.Tag == null)
                return;

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

        /* Ensures right clicking a item (ie for context menu) selects the item first */
        private void TreeViewItem_PreviewMouseRightButtonDown(object sender, MouseEventArgs e)
        {
            TreeViewItem item = sender as TreeViewItem;
            if (item != null)
            {
                //item.Focus();
                //e.Handled = true;
            }
        }

        /* Runs when a Custom Game Data Tree Item is selected */
        private void CustomDataTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewItem SelectedItem = CustomDataTree.SelectedItem as TreeViewItem;
            if (SelectedItem == null || SelectedItem.Tag == null)
                return;

            switch (SelectedItem.Tag.ToString())
            {
                case "character":
                    CustomDataTree.ContextMenu = CustomDataTree.Resources["CharacterContext"] as System.Windows.Controls.ContextMenu;
                    break;

                case "team":
                    CustomDataTree.ContextMenu = CustomDataTree.Resources["TeamContext"] as System.Windows.Controls.ContextMenu;
                    break;

                case "siege":
                    CustomDataTree.ContextMenu = CustomDataTree.Resources["SiegeContext"] as System.Windows.Controls.ContextMenu;
                    break;

                case "playerModels":
                    CustomDataTree.ContextMenu = CustomDataTree.Resources["PlayerModelsContext"] as System.Windows.Controls.ContextMenu;
                    break;

                case "playerModel":
                    CustomDataTree.ContextMenu = CustomDataTree.Resources["PlayerModelContext"] as System.Windows.Controls.ContextMenu;
                    break;

                case "weaponModels":
                    CustomDataTree.ContextMenu = CustomDataTree.Resources["WeaponModelsContext"] as System.Windows.Controls.ContextMenu;
                    break;

                case "weaponModel":
                    CustomDataTree.ContextMenu = CustomDataTree.Resources["WeaponModelContext"] as System.Windows.Controls.ContextMenu;
                    break;
            }
        }

        /* When Menu Item NEW Project has been clicked */
        private void MenuItemNew_Click(object sender, RoutedEventArgs e)
        {
            NewProject newProject = new NewProject();
            newProject.Show();
            newProject.Activate();
        }

        /* When a user wants to edit a project character */
        private void EditProjectCharacter_Click(object sender, RoutedEventArgs e)
        {

            TreeViewItem SelectedItem = ImportedDataTree.SelectedItem as TreeViewItem;

            /* Identify the character we want to edit our project */
            Character character = ActiveProject.characters.Where(x => x.classinfo.name == SelectedItem.Header.ToString()).FirstOrDefault();


            /* Send character to the editor */

                            
        }


        /* When a user wants to clone a character from imported data to their project */
        private void AddImportedCharacterToProject_Click(object sender, RoutedEventArgs e)
        {

            TreeViewItem SelectedItem = ImportedDataTree.SelectedItem as TreeViewItem;

            /* Identify the character we want to clone into our project */
            Character character = ImportedData.importedCharacters.Where(x => x.classinfo.name == SelectedItem.Header.ToString()).FirstOrDefault();

            /* Add to our project */
            ActiveProject.characters.Add(character);

        }









        
    }


}
