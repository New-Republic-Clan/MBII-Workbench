using System;
using System.Collections.Generic;
using System.Text;

namespace MB2_Workbench.DataTypes.Siege
{
    public class TeamDetails
    {
        public string teamName { get; set; }
        public string useTeam { get; set; }

        public string teamIcon { get; set; }

        public string teamColorOn { get; set; }

        public string teamColorOff { get; set; }

        public string wonround { get; set; }
        public string lostround { get; set; }
        public string roundover_sound_wewon { get; set; }
        public string roundover_sound_welost { get; set; }
        public string roundover_target { get; set; }
        public int? attackers { get; set; }
        public string briefing { get; set; }

        public int? requiredObjectives { get; set; }

        public Objective objective1 { get; set; }
        public Objective objective2 { get; set; }
        public Objective objective3 { get; set; }
        public Objective objective4 { get; set; }
        public Objective objective5 { get; set; }
        public Objective objective6 { get; set; }
        public Objective objective7 { get; set; }
        public Objective objective8 { get; set; }
        public Objective objective9 { get; set; }

        public int? timed { get; set; }

    }
}
