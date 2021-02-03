using MB2_Workbench.Classes.SiegeDeserializer;
using MB2_Workbench.DataImport;
using MB2_Workbench.DataTypes.Character;
using MB2_Workbench.DataTypes.Map;
using MB2_Workbench.DataTypes.Siege;
using MB2_Workbench.DataTypes.Team;
using MB2_Workbench.Classes.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Windows;
using MB2_Workbench.DataTypes.Skin;
using MB2_Workbench.DataTypes.Model;
using System.Collections.ObjectModel;
using MB2_Workbench.Windows.Splash;

namespace MB2_Workbench
{
    public class ImportJob
    {

        /* All Import Errors */
        public List<string> failedImports = new List<string>();

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
                                        ImportedData.importedCharacters.Add(seigeDeserializer.Deserialize<Character>(text));
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

                                        ImportedData.importedTeams.Add(seigeDeserializer.Deserialize<Team>(text));
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

                                        ImportedData.importedSieges.Add(siege);
                                    }
                                    catch (Exception e)
                                    {
                                        failedImports.Add($"{System.IO.Path.GetFileName(pk3)}/{entry.FullName}: {e.Message}");
                                    }
                                }
                            }
                        }

                        // Import a map file (.BSP) */
                        if (entry.FullName.ToLower().Contains(".bsp"))
                        {
                            Log($"Importing {Path.GetFileName(entry.FullName)}");

                            ImportedData.importedMaps.Add(new Map()
                            {
                                parentPK3 = pk3,
                                fileName = Path.GetFileName(entry.FullName),
                                name = Path.GetFileNameWithoutExtension(entry.FullName),
                                entryPath = entry.FullName
                            });
                        }

                        // Import a map file (.skin) */
                        if (entry.FullName.ToLower().Contains(".skin"))
                        {

                            Log($"Importing {Path.GetFileName(entry.FullName)}");

                            ImportedData.importedSkins.Add(new Skin()
                            {
                                parentPK3 = pk3,
                                fileName = Path.GetFileName(entry.FullName),
                                name = Path.GetFileNameWithoutExtension(entry.FullName),
                                entryPath = entry.FullName,
                                modelName = Directory.GetParent(entry.FullName).Name
                            });
                        }

                        // Import a player model file (.gml) */
                        if (entry.FullName.ToLower().Contains(".glm") && entry.FullName.ToLower().Contains("players"))
                        {
                            Log($"Importing {Path.GetFileName(entry.FullName)}");

                            ImportedData.importedPlayerModels.Add(new PlayerModel()
                            {
                                parentPK3 = pk3,
                                fileName = Path.GetFileName(entry.FullName),
                                name = Directory.GetParent(entry.FullName).Name,
                                entryPath = entry.FullName,
                            });
                        }

                        // Import a weapon model file (.gml) */
                        if (entry.FullName.ToLower().Contains(".glm") && entry.FullName.ToLower().Contains("weapon"))
                        {
                            Log($"Importing {Path.GetFileName(entry.FullName)}");

                            ImportedData.importedWeaponModels.Add(new WeaponModel()
                            {
                                parentPK3 = pk3,
                                fileName = Path.GetFileName(entry.FullName),
                                name = Directory.GetParent(entry.FullName).Name,
                                entryPath = entry.FullName,
                            });
                        }

                    }
                }

                IncrementProgress();

            }

            Log($"Importing Complete...");

            /* Write latest failed imports to a log file */
            if (File.Exists(Settings.FailedImportLog))
                File.Delete(Settings.FailedImportLog);

            using (StreamWriter sw = File.CreateText(Settings.FailedImportLog))
            {

                sw.WriteLine("Failed Imports from Base Game Files");
                sw.WriteLine($"Last Run: {DateTime.Now.ToString("MM/dd/yyyy H:mm")}");
                sw.WriteLine("-----------------------------------------------------");

                foreach (string failedImport in failedImports)
                {
                    sw.WriteLine(failedImport);
                }
            }

            /* Any Additional Filtering or manipulation of imported data */
            ImportedData.importedCharacters = new ObservableCollection<Character>(ImportedData.importedCharacters
                .Where(x => x.name != null &&  x.name.Trim() != "")
                .OrderBy(x => x.name).ToList());

            Application.Current.Dispatcher.Invoke(() => {

                /* Close Splash */
                SplashLoading splashLoading = WindowFinder.FindOpenWindowByType<SplashLoading>();
                splashLoading.Close();
       
                MainWindow mainWindow = WindowFinder.FindOpenWindowByType<MainWindow>();

                /* Setup Imported Data Treeview Datasources */
                mainWindow.ImportedCharacters.ItemsSource = ImportedData.importedCharacters;

                /* Show Window */
                mainWindow.Show();
                mainWindow.Activate();

            });

        }

    }
}
