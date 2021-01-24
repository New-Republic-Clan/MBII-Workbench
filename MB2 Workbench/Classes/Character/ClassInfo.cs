using System;
using System.Collections.Generic;
using System.Text;

namespace MB2_Workbench.Classes
{
    class ClassInfo
    {
        public string name { get; set; }
        public string weapons { get; set; }
        public string attributes { get; set; }
        public string saber1 { get; set; }
        public string saber2 { get; set; }
        public string forcepowers { get; set; }
        public int? forcepool { get; set; }
        public double? APMultiplier { get; set; }
        public double? BPMultiplier { get; set; }
        public double? CSMultiplier { get; set; }
        public int? maxHealth { get; set; }
        public int? health { get; set; }
        public int? maxArmor { get; set; }
        public int? armor { get; set; }
        public string model { get; set; }
        public string skin { get; set; }
        public string uishader { get; set; }
        public string characterClass { get; set; }
        public int? classNumberLimit { get; set; }
        public double? modelscale { get; set; }
        public double? damageGiven { get; set; }
        public double? damageTaken { get; set; }

        public double? speed { get; set; }

        public string mbClass { get; set; }

        public int? customRed { get; set; }
        public int? customGreen { get; set; }
        public int? customBlue { get; set; }
    }
}
