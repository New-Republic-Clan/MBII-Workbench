using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MB2_Workbench
{
    public static class Settings
    {

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

        public static string OpenJKDirectory = GetValue("OpenJkDirectory");
        public static string OpenJKGameDataDirectory = Path.Combine(OpenJKDirectory, "GameData");
        public static string MBIIDirectory = Path.Combine(OpenJKGameDataDirectory, "MBII");

    }
}
