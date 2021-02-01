using MB2_Workbench.DataImport;
using MB2_Workbench.Classes.Helpers;
using MB2_Workbench.Classes;
using MB2_Workbench.Classes.Extensions;
using MB2_Workbench.DataTypes.Map;
using MB2_Workbench.DataTypes.Siege;

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Linq;
using Path = System.IO.Path;

using System.Collections.ObjectModel;
using MB2_Workbench.DataTypes.Team;
using MB2_Workbench.DataTypes.Character;

namespace MB2_Workbench.Popups
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class NewProject : Window
    {
        public NewProject()
        {
            InitializeComponent();

            ObservableCollection<string> availableMaps = new ObservableCollection<string>(ImportedData.importedMaps.Select(x => x.name).ToList());
            Maps.ItemsSource = availableMaps;

            
        }

        /* User clicks "create project" button */
        private void CreateProjectButton_Click(object sender, RoutedEventArgs e)
        {

            if(MapName.Text.Length == 0 || !MapName.Text.ToLower().Equals(MapName.Text) || !MapName.Text.Replace(" ","").Equals(MapName.Text) )
            {
                MessageBox.Show("Please Check your new map name is valid");
                return;
            }

            if (Maps.SelectedItem == null || Maps.SelectedItem.ToString().Length == 0)
            {
                MessageBox.Show("Please select a valid base map");
                return;
            }

            if (Directory.Exists(Path.Combine(Settings.UsersProjectHome, MapName.Text)))
            {
                MessageBox.Show("A project with this name already exists");
                return;
            }

            MainWindow mainWindow = WindowFinder.FindOpenWindowByType<MainWindow>();

            /* Show the Tree Views */
            mainWindow.ImportedDataTree.Visibility = Visibility.Visible;
            mainWindow.CustomDataTree.Visibility = Visibility.Visible;

            /* Setup Current Project */
            ActiveProject.name = MapName.Text;
            ActiveProject.map = ImportedData.importedMaps.Where(x => x.name == Maps.SelectedItem.ToString()).FirstOrDefault();
            ActiveProject.directory = Path.Combine(Settings.UsersProjectHome, ActiveProject.name);
            ActiveProject.file = Path.Combine(Settings.UsersProjectHome, ActiveProject.name, ActiveProject.name + ".mbwb");
            ActiveProject.characters = new List<Character>();
            ActiveProject.teams = new List<Team>();

            /* Create the Project Directory */
            Directory.CreateDirectory(ActiveProject.directory);

            /* Create a project file */
            if (!File.Exists(ActiveProject.file))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(ActiveProject.file))
                {
                    sw.WriteLine("Hello");
                }
            }

            /* Create Maps and ext_data */
            Directory.CreateDirectory(Path.Combine(ActiveProject.directory, "maps"));
            Directory.CreateDirectory(Path.Combine(ActiveProject.directory, "ext_data"));


            /* Extract the map from it's PK3 */
            using (ZipArchive zipFile = ZipFile.OpenRead(Path.Combine(Settings.MBIIDirectory, ActiveProject.map.parentPK3)))
            {
                ZipArchiveEntry mapFile = zipFile.GetEntry(ActiveProject.map.entryPath);
                mapFile.ExtractToFile(Path.Combine(ActiveProject.directory, "maps", ActiveProject.map.fileName));
            }

            /* Rename the BSP to that of current project name */
            File.Move(Path.Combine(ActiveProject.directory, "maps", ActiveProject.map.fileName), Path.Combine(ActiveProject.directory, "maps", ActiveProject.name + ".bsp"));

            /* Prefill teams, characters and siege info if this base map has available */
            Siege defaultSiege = ImportedData.importedSieges.Where(x => x.map == ActiveProject.map.name).FirstOrDefault();

            if (defaultSiege != null)
            {
                ActiveProject.siege = defaultSiege;
                ActiveProject.teams = ImportedData.importedTeams.Where(x => x.name == ActiveProject.siege.team1.useTeam || x.name == ActiveProject.siege.team2.useTeam).ToList();


                foreach(Team team in ActiveProject.teams)
                {
                    ActiveProject.characters.AddRange(team.TeamToCharacterList());
                }
            }
               
            /* Build our Project TreeView */
            mainWindow.CustomDataTree.Items.Clear();

            /* Add Characters */
            TreeViewItem characters = new TreeViewItem();
            characters.Header = "Characters";

            foreach(Character character in ActiveProject.characters)
            {
                characters.Items.Add(new TreeViewItem()
                {
                    Header = character.name,
                    Tag = "character"
                });
            }

            /* Add Teams */
            TreeViewItem teams = new TreeViewItem();
            teams.Header = "Teams";

            foreach (Team team in ActiveProject.teams)
            {
                teams.Items.Add(new TreeViewItem()
                {
                    Header = team.name,
                    Tag = "team",
                });
            }

            /* Add Models */
            TreeViewItem models = new TreeViewItem();
            models.Header = "Models";
            models.Tag = "";

            models.Items.Add(new TreeViewItem()
            {
                Header = "Player Models",
                Tag = "playerModels",
            });

            models.Items.Add(new TreeViewItem()
            {
                Header = "Weapon Models",
                Tag = "weaponModels",
            });

            /* Add Siege */
            TreeViewItem siege = new TreeViewItem();
            siege.Header = "Siege";
            siege.Tag = "siege";

            /* Add These Items to Tree */
            mainWindow.CustomDataTree.Items.Add(characters);
            mainWindow.CustomDataTree.Items.Add(teams);
            mainWindow.CustomDataTree.Items.Add(models);
            mainWindow.CustomDataTree.Items.Add(siege);

            this.Close();

        }

    }
}
