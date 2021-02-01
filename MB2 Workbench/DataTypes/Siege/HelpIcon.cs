using System;
using System.Collections.Generic;
using System.Text;

namespace MB2_Workbench.DataTypes.Siege
{
    public class HelpIcon
    {
        public int? radius { get; set; }


        public string origin { get; set; }

        public string blueicon { get; set; }
        public string redicon { get; set; }
        public string specicon { get; set; }

        public int? sideobjective { get; set; }

        public int? dynamic { get; set; }

        public string end0 { get; set; }
        public string end1 { get; set; }
        public string end2 { get; set; }

        public string start0 { get; set; }
        public string start1 { get; set; }
        public string start2 { get; set; }

        public int? needvisible { get; set; }

        public int? childradius { get; set; }

        public int? parent { get; set; }

    }
}
