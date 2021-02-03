using MB2_Workbench.DataImport;
using MB2_Workbench.Classes.Helpers;
using MB2_Workbench.Classes;
using MB2_Workbench.Classes.Extensions;
using MB2_Workbench.DataTypes.Siege;
using System.IO;
using System.IO.Compression;
using System.Windows;
using System.Linq;
using Path = System.IO.Path;

using System.Collections.ObjectModel;
using MB2_Workbench.DataTypes.Team;
using MB2_Workbench.DataTypes.Character;

namespace MB2_Workbench.Windows.Dialogs
{
    /// <summary>
    /// Interaction logic for DialogNewProject.xaml
    /// </summary>
    public partial class DialogNewProject : Window
    {
        public DialogNewProject()
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
                MessageBox.Show("Your map name must contain no spaces, no capitals and not match an existing map");
                return;
            }

            if (Prefix.Text.Length == 0 || !Prefix.Text.ToLower().Equals(Prefix.Text) || !Prefix.Text.Replace(" ", "").Equals(Prefix.Text))
            {
                MessageBox.Show("Your prefix must contain no spaces, no capitals and not match an existing map");
                return;
            }

            if (Maps.SelectedItem == null || Maps.SelectedItem.ToString().Length == 0)
            {
                MessageBox.Show("Please select a valid base map");
                return;
            }

            if (Directory.Exists(Path.Combine(Settings.UsersProjectHome, MapName.Text)))
            {
                MessageBox.Show("A map with this name already exists in your projects folder");
                return;
            }

            if(ImportedData.importedMaps.Where(x => x.name == MapName.Text).Any())
            {
                MessageBox.Show("A map with this name already exists in game");
                return;
            }

            /* Append Underscore */
            if (!Prefix.Text.EndsWith("_"))
                Prefix.Text = Prefix.Text + "_";

            MainWindow mainWindow = WindowFinder.FindOpenWindowByType<MainWindow>();

            /* Setup Current Project */
            ActiveProject.Project = new Project();
            ActiveProject.Project.name = MapName.Text;
            ActiveProject.Project.map = ImportedData.importedMaps.Where(x => x.name == Maps.SelectedItem.ToString()).FirstOrDefault();
            ActiveProject.Project.directory = Path.Combine(Settings.UsersProjectHome, ActiveProject.Project.name);
            ActiveProject.Project.file = Path.Combine(Settings.UsersProjectHome, ActiveProject.Project.name, ActiveProject.Project.name + ".mbwb");
            ActiveProject.Project.prefix = Prefix.Text;

            /* Create the Project Directory */
            Directory.CreateDirectory(ActiveProject.Project.directory);

            /* Create Maps and ext_data */
            Directory.CreateDirectory(Path.Combine(ActiveProject.Project.directory, "maps"));
            Directory.CreateDirectory(Path.Combine(ActiveProject.Project.directory, "ext_data"));


            /* Extract the map from it's PK3 */
            using (ZipArchive zipFile = ZipFile.OpenRead(Path.Combine(Settings.MBIIDirectory, ActiveProject.Project.map.parentPK3)))
            {
                ZipArchiveEntry mapFile = zipFile.GetEntry(ActiveProject.Project.map.entryPath);
                mapFile.ExtractToFile(Path.Combine(ActiveProject.Project.directory, "maps", ActiveProject.Project.map.fileName));
            }

            /* Rename the BSP to that of current project name */
            File.Move(Path.Combine(ActiveProject.Project.directory, "maps", ActiveProject.Project.map.fileName), Path.Combine(ActiveProject.Project.directory, "maps", ActiveProject.Project.name + ".bsp"));

            /* Prefill teams, characters and siege info if this base map has available */
            Siege defaultSiege = ImportedData.importedSieges.Where(x => x.map == ActiveProject.Project.map.name).FirstOrDefault();

            if (defaultSiege != null && defaultSiege.team1 != null && defaultSiege.team2 != null)
            {
                ActiveProject.Project.siege = defaultSiege;

                foreach(Team team in ImportedData.importedTeams.Where(x => x.name == ActiveProject.Project.siege.team1.useTeam || x.name == ActiveProject.Project.siege.team2.useTeam).ToList())
                {
                    Team newTeam = team;
                    team.name = ActiveProject.Project.prefix + team.name;

                    foreach(Character character in team.Characters()){
                        Character newCharacter = character;
                        character.classinfo.name = ActiveProject.Project.prefix + character.classinfo.name;
                        ActiveProject.Project.characters.Add(newCharacter);
                    }

                    /* TODO: We still need to loop the team and change character names to include new prefix here */

                    ActiveProject.Project.teams.Add(newTeam);
                }
            }

            /* Save Project */
            ActiveProject.Save();

            /* Update the GUI */
            ActiveProject.TreeAddSources();

            this.Close();

        }

    }
}
