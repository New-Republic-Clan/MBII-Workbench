using MB2_Workbench.DataTypes.Character;
using MB2_Workbench.DataTypes.Map;
using MB2_Workbench.DataTypes.Siege;
using MB2_Workbench.DataTypes.Team;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Text.Json;

namespace MB2_Workbench.Classes
{
    public class Project
    {


        public Project()
        {
            siege = new Siege();
            characters = new ObservableCollection<Character>();
            teams = new ObservableCollection<Team>();
        }

        public string name { get; set; }
        public Map map { get; set; }
        public string prefix { get; set; }
        public string directory { get; set; }
        public string file { get; set; }

        public Siege siege { get; set; }
        public ObservableCollection<Character> characters { get; set; }
        public ObservableCollection<Team> teams { get; set; }

    }
}
