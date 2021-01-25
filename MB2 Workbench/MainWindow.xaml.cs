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

namespace MB2_Workbench
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


            /* All Imported Characters */
            List<Character> characters = new List<Character>();

            /* All Imported Teams */
            List<Team> teams = new List<Team>();

            /* All Importer Map Sieges */
            List<Siege> sieges = new List<Siege>();

            string[] pk3s = Directory.GetFiles(@"E:\SteamLibrary\steamapps\common\Jedi Academy\GameData\MBII\", "*.pk3");

            SiegeDeserializer seigeDeserializer = new SiegeDeserializer();

            List<string> failedImports = new List<string>();

            foreach(string pk3 in pk3s)
            {
                using (ZipArchive zipFile = ZipFile.OpenRead(pk3))
                {
                    foreach (ZipArchiveEntry entry in zipFile.Entries)
                    {

                        // Import Character File (.MBCH) 
                        if (entry.FullName.Contains("ext_data/mb2/character/"))
                        {
                            if (entry.FullName.ToLower().Contains(".mbch"))
                            {
                                using (var stream = entry.Open())
                                {
                                    StreamReader reader = new StreamReader(stream);
                                    string text = reader.ReadToEnd();

                                    try
                                    {
                                        characters.Add(seigeDeserializer.Deserialize<Character>(text));
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
                                using (var stream = entry.Open())
                                {
                                    StreamReader reader = new StreamReader(stream);
                                    string text = reader.ReadToEnd();

                                    try
                                    {

                                        teams.Add(seigeDeserializer.Deserialize<Team>(text));
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
                                using (var stream = entry.Open())
                                {
                                    StreamReader reader = new StreamReader(stream);
                                    string text = reader.ReadToEnd();

                                    try
                                    {

                                        Siege siege = seigeDeserializer.Deserialize<Siege>(text);

                                        /* This allows us to keep track of what map this siege file belongs to */
                                        siege.map = entry.FullName.ToLower().Replace(".siege", "").Replace(@"maps/","");

                                        sieges.Add(siege);
                                    }
                                    catch (Exception e)
                                    {
                                        failedImports.Add($"{System.IO.Path.GetFileName(pk3)}/{entry.FullName}: {e.Message}");
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
