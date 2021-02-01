using System;
using System.Collections.Generic;
using System.Text;

namespace MB2_Workbench.DataTypes.Siege
{
    public class Objective
    {
        public int? final { get; set; }
        public string goalname { get; set; }

        public string message_team1 { get; set; }

        public string message_team2 { get; set; }
        public string objdesc { get; set; }
        public string objgfx { get; set; }

        public string target { get; set; }
        public string mapIcon { get; set; }

        public string doneMapIcon { get; set; }

        public string mappos { get; set; }

        public string sound_team1 { get; set; }
        public string sound_team2 { get; set; }

        public string roundover_sound_wewon { get; set; }

        public string roundover_sound_welost { get; set; }

        public string litmapicon { get; set; }
    }
}
