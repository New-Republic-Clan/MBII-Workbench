using System;
using System.Collections.Generic;
using System.Text;

namespace MB2_Workbench.Classes
{
    class Siege
    {
        /* This is just a reference field, not in original spec */
        public string map { get; set; }

        public Teams teams { get; set; }

        public string mapgraphic { get; set; }
        public string missionname { get; set; }
        public string radartopleft { get; set; }
        public string radarbottomright { get; set; }
        public string roundbegin_target { get; set; }

        public AutoMaps automaps { get; set; }

        public HelpIcons helpicons { get; set; }

        public TeamDetails team1 { get; set; }
        public TeamDetails team2 { get; set; }

        public LevelshotDesc levelShotDesc { get; set; }

        public int? countin_time { get; set; }

        public string radargraphic { get; set; }

    }
}
