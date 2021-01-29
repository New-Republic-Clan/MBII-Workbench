using MB2_Workbench.Classes;
using MB2_Workbench.Classes.SiegeDeserializer;
using MB2_Workbench.Popups;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MB2_Workbench
{
    public class ImportJob
    {

        public void Log(string line)
        {
            Application.Current.Dispatcher.Invoke(() => {
                var splashLoading = (SplashLoading) Application.Current.Windows.OfType<SplashLoading>().FirstOrDefault();
                splashLoading.Activate();
                splashLoading.StatusLabel.Content = line;
            });
        }

        public void IncrementProgress()
        {
            Application.Current.Dispatcher.Invoke(() => {
                var splashLoading = (SplashLoading)Application.Current.Windows.OfType<SplashLoading>().FirstOrDefault();
                splashLoading.Activate();
                splashLoading.LoadingProgressBar1.Value = splashLoading.LoadingProgressBar1.Value + 1;
            });
        }

        public void UpdateProgressMax(double value)
        {
            Application.Current.Dispatcher.Invoke(() => {
                var splashLoading = (SplashLoading)Application.Current.Windows.OfType<SplashLoading>().FirstOrDefault();
                splashLoading.Activate();
                splashLoading.LoadingProgressBar1.Maximum = value;
            });
        }

        /* All Imports Characters */
        public List<Character> importedCharacters = new List<Character>();

        /* All Imported Teams */
        public List<Team> importedTeams = new List<Team>();

        /* All Importer Map Sieges */
        public List<Siege> importedSieges = new List<Siege>();

        /* All Import Errors */
        public List<string> failedImports = new List<string>();

        /* PK3s available to base a new map from */
        public List<string> importedMaps = new List<string>();

        /* Handles importing of all files from existing PK3 Files */
        public void BeginImport(){

            string[] pk3s = Directory.GetFiles(Settings.MBIIDirectory, "*.pk3");

            SiegeDeserializer seigeDeserializer = new SiegeDeserializer();

            UpdateProgressMax(pk3s.Count());

            foreach (string pk3 in pk3s)
            {

                Log($"Importing {Path.GetFileName(pk3)}");

                using (ZipArchive zipFile = ZipFile.OpenRead(pk3))
                {

                    foreach (ZipArchiveEntry entry in zipFile.Entries)
                    {

                        // Import Character File (.MBCH) 
                        if (entry.FullName.Contains("ext_data/mb2/character/"))
                        {
                            if (entry.FullName.ToLower().Contains(".mbch"))
                            {
                                Log($"Importing {Path.GetFileName(entry.FullName)}");

                                using (var stream = entry.Open())
                                {
                                    StreamReader reader = new StreamReader(stream);
                                    string text = reader.ReadToEnd();

                                    try
                                    {
                                        importedCharacters.Add(seigeDeserializer.Deserialize<Character>(text));
                                    }
                                    catch (Exception e)
                                    {
                                        failedImports.Add($"{System.IO.Path.GetFileName(pk3)}/{entry.FullName}: {e.Message}");
                                    }
                                }
                            }
                        }

                        // Import Team File (.MBTC) 
                        if (entry.FullName.Contains("ext_data/mb2/teamconfig/"))
                        {
                            if (entry.FullName.ToLower().Contains(".mbtc"))
                            {
                                Log($"Importing {Path.GetFileName(entry.FullName)}");

                                using (var stream = entry.Open())
                                {
                                    StreamReader reader = new StreamReader(stream);
                                    string text = reader.ReadToEnd();

                                    try
                                    {

                                        importedTeams.Add(seigeDeserializer.Deserialize<Team>(text));
                                    }
                                    catch (Exception e)
                                    {
                                        failedImports.Add($"{System.IO.Path.GetFileName(pk3)}/{entry.FullName}: {e.Message}");
                                    }
                                }
                            }
                        }

                        // Import Siege File (.SIEGE) 
                        if (entry.FullName.Contains("maps/"))
                        {
                            if (entry.FullName.ToLower().Contains(".siege"))
                            {
                                Log($"Importing {Path.GetFileName(entry.FullName)}");

                                using (var stream = entry.Open())
                                {
                                    StreamReader reader = new StreamReader(stream);
                                    string text = reader.ReadToEnd();

                                    try
                                    {

                                        Siege siege = seigeDeserializer.Deserialize<Siege>(text);

                                        /* This allows us to keep track of what map this siege file belongs to */
                                        siege.map = entry.FullName.ToLower().Replace(".siege", "").Replace(@"maps/", "");

                                        importedSieges.Add(siege);
                                    }
                                    catch (Exception e)
                                    {
                                        failedImports.Add($"{System.IO.Path.GetFileName(pk3)}/{entry.FullName}: {e.Message}");
                                    }
                                }
                            }
                        }

                        // Import as Base map when a PK3 has a matching BSP file */
                        if (entry.FullName.Contains($"maps/{Path.GetFileName(pk3).Replace(".pk3","")}.bsp"))
                        {
                            importedMaps.Add(pk3);
                        }
                    }
                }

                IncrementProgress();

            }

            Log($"Importing Complete...");
            Thread.Sleep(1);

            Application.Current.Dispatcher.Invoke(() => {
                var splashLoading = (SplashLoading)Application.Current.Windows.OfType<SplashLoading>().FirstOrDefault();
                splashLoading.Close();
            });

            Application.Current.Dispatcher.Invoke(() => {
                var mainWindow = (MainWindow)Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                mainWindow.BasePK3s = importedMaps;
                mainWindow.Show();
                mainWindow.Activate();

            });

            Application.Current.Dispatcher.Invoke(() =>
            {

                var mainWindow = (MainWindow)Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

                TreeViewItem characters = new TreeViewItem();
                characters.Header = "Characters";
                characters.Tag = "characters";

                foreach (Character character in importedCharacters)
                {
                    if (character.description != null) {
                        characters.Items.Add(new TreeViewItem()
                        {
                            Header = character.name,
                            Tag = "character"
                        });
             
                    }
                    
                }

                TreeViewItem teams = new TreeViewItem();
                teams.Tag = "teams";
                teams.Header = "Teams";

                foreach (Team team in importedTeams)
                {
                    teams.Items.Add(new TreeViewItem()
                    {
                        Header = team.name,
                        Tag = "team"
                    });
                }

                mainWindow.ImportedDataTree.Items.Add(characters);
                mainWindow.ImportedDataTree.Items.Add(teams);

            });

        }

    }
}
