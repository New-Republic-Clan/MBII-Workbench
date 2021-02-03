using MB2_Workbench.Classes.Helpers;
using MB2_Workbench.DataTypes.Character;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace MB2_Workbench.Classes
{
    public static class ActiveProject
    {
        public static Project Project = new Project();

        public static void Save()
        {

            if (File.Exists(Project.file))
                File.Delete(Project.file);

            string jsonString = JsonConvert.SerializeObject(Project);

            // Create a file to write to.
            using (StreamWriter sw = File.CreateText(Project.file))
            {
                sw.WriteLine(jsonString);
            }
        }

        public static void Load(string file)
        {

            if (File.Exists(file))
            {
                string jsonString = File.ReadAllText(file);
                ActiveProject.Project = JsonConvert.DeserializeObject<Project>(jsonString);

                TreeAddSources();
            }

        }

        public static void TreeAddSources()
        {

            MainWindow mainWindow = WindowFinder.FindOpenWindowByType<MainWindow>();

            mainWindow.ProjectCharacters.ItemsSource = ActiveProject.Project.characters;

            /* Show the Tree Views */
            mainWindow.ProjectDataTree.Visibility = Visibility.Visible;
            mainWindow.ImportedDataTree.Visibility = Visibility.Visible;
        }


    }
}
