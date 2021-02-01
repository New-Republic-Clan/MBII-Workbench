using System;
using System.Collections.Generic;
using System.Text;

namespace MB2_Workbench.DataTypes.Team
{
    public class Team
    {

        public string name { get; set; }
        public int? timePeriod { get; set; }
        public int? euAllowed { get; set; }
        public Classes classes { get; set; }
        public int? classesAllowed { get; set; }
        public SubClassesForClass SubClassesForClass1 { get; set; }
        public SubClassesForClass SubClassesForClass2 { get; set; }
        public SubClassesForClass SubClassesForClass3 { get; set; }
        public SubClassesForClass SubClassesForClass4 { get; set; }
        public SubClassesForClass SubClassesForClass5 { get; set; }
        public SubClassesForClass SubClassesForClass6 { get; set; }
        public SubClassesForClass SubClassesForClass7 { get; set; }
        public SubClassesForClass SubClassesForClass8 { get; set; }
        public SubClassesForClass SubClassesForClass9 { get; set; }

        public SubClassesForClass SubClassesForClass10 { get; set; }
    }
}
