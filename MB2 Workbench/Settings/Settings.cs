using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;

namespace MB2_Workbench
{
    public static class Settings
    {

        /* Settings Properties */
        public static string OpenJKDirectory = GetValue("OpenJkDirectory");
        public static string OpenJKGameDataDirectory = Path.Combine(OpenJKDirectory, "GameData");
        public static string MBIIDirectory = Path.Combine(OpenJKGameDataDirectory, "MBII");
        public static string UsersProjectHome = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MBII Workbench");
        public static string FailedImportLog = Path.Combine(UsersProjectHome, "FailedImports.log");


        /* Namespaces used by the Siege Deserialiser */

        public static List<string> NamespaceDataTypes = new List<string>() {
            "MB2_Workbench.DataTypes.Team",
            "MB2_Workbench.DataTypes.Siege",
            "MB2_Workbench.DataTypes.Character",
            "MB2_Workbench.DataTypes.Enums"
        };


        /* Fetching and Saving Data to Registry */
        public static string GetValue(string key)
        {
            CreateKey();
            RegistryKey regKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\MBIIWorkbench", true);

            if (regKey.GetValue(key) == null)
                return null;

            return regKey.GetValue(key).ToString();
        }

        public static void SetValue(string key, string value)
        {
            CreateKey();
            RegistryKey regKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\MBIIWorkbench", true);

            regKey.SetValue(key, value);
        }

        public static void CreateKey()
        {
            RegistryKey regKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\MBIIWorkbench", true);

            if(regKey == null)
                Registry.CurrentUser.CreateSubKey(@"SOFTWARE\MBIIWorkbench");

        }

        /* Save + Load Functions */
        public static void FindOpenJKDirectory()
        {

            /* Checks if OpenJK is installed to some standard directory */
            if(File.Exists(@"C:\Program Files (x86)\Steam\steamapps\common\Jedi Academy\JediAcademy.exe"))
                Settings.SetValue("OpenJKDirectory", Path.GetDirectoryName(@"C:\Program Files (x86)\Steam\steamapps\common\Jedi Academy\"));

            if (File.Exists(@"C:\Program Files\Steam\steamapps\common\Jedi Academy\JediAcademy.exe"))
                Settings.SetValue("OpenJKDirectory", Path.GetDirectoryName(@"C:\Program Files\Steam\steamapps\common\Jedi Academy\"));

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
            else
            {
                Environment.Exit(0);
            }
        }
    }
}
