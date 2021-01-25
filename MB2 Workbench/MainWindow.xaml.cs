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


            List<Character> characters = new List<Character>();
            int g = 0;

            //This just loads when the window loads, we will eventually use the GUI but as of yet, lets just put our code here

            // We want to test on this PK3, eventually, we want to scan all PK3's in MBII dir and Base Dir and process them all
            var pk3 = "E:\\SteamLibrary\\steamapps\\common\\Jedi Academy\\GameData\\MBII\\MBAssets3.pk3";

            using (ZipArchive zipFile = ZipFile.OpenRead(pk3))
            {
                foreach (ZipArchiveEntry entry in zipFile.Entries)
                {

                    // Character File (.MBCH) 
                    if (entry.FullName.Contains("ext_data/mb2/character/"))
                    {
                        if (entry.FullName.Contains(".mbch"))
                        {
                            using (var stream = entry.Open())
                            {
                                StreamReader reader = new StreamReader(stream);
                                string text = reader.ReadToEnd();

                                if (!text.Contains("RC_rep_Commander") && !text.Contains("xmas_Bobble") && !text.Contains("YA_Desann") && !text.Contains("_h_s_NSold"))
                                {
                                    var s = new SiegeDeserializer();



                                    characters.Add(s.Deserialize<Character>(text));

                                    g++;

                                }

   

                            }

                        }
                    }
                    // Also want to do stuff when we find
                    // a map file, a .siege file, a team .mctc file
                }

                var i = g;

            }

        }

    }



}
