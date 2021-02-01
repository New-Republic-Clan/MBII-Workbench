using MB2_Workbench.DataTypes.Character;
using MB2_Workbench.DataTypes.Map;
using MB2_Workbench.DataTypes.Siege;
using MB2_Workbench.DataTypes.Team;
using System;
using System.Collections.Generic;
using System.Text;

namespace MB2_Workbench.Classes
{
    /* Represents the current loaded project, static as there can only ever be one loaded project */
    public static class ActiveProject
    {

        public static string name { get; set; }
        public static Map map { get; set; }
        public static string directory { get; set; }
        public static string file { get; set; }

        public static Siege siege { get; set; }
        public static List<Character> characters { get; set; }
        public static List<Team> teams { get; set; }

    }
}
