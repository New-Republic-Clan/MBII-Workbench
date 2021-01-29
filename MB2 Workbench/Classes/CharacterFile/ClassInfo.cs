using MB2_Workbench.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MB2_Workbench.Classes
{
    public class ClassInfo
    {
        public string name { get; set; }
        public List<Weapon> weapons { get; set; }
        public List<Tuple<Attributes, int?>> attributes { get; set; }
        public string saber1 { get; set; }
        public string saber2 { get; set; }

        public int? saberColor { get; set; }
        public int? saber2Color { get; set; }
        public List<SaberStyles> saberStyle { get; set; }
        public List<Tuple<ForcePowers, int?>> forcepowers { get; set; }
        public int? forcepool { get; set; }
        public double? APMultiplier { get; set; }

        public double? ASmultiplier { get; set; }
        public double? BPMultiplier { get; set; }

        public double? CSMultiplier { get; set; }

        public int? APBonus { get; set; }

        public double? damageAmplify { get; set; }
        public int? maxHealth { get; set; }
        public int? health { get; set; }
        public int? maxArmor { get; set; }

        public int? mmaxhealth { get; set; }

        public double? forceRegen { get; set; }
        public int? armor { get; set; }
        public string model { get; set; }
        public string skin { get; set; }
        public string uishader { get; set; }
        public string characterClass { get; set; }
        public int? classNumberLimit { get; set; }
        public double? modelscale { get; set; }
        public double? damageGiven { get; set; }
        public double? damageTaken { get; set; }

        public double? damageProtection { get; set; }

        public int? extraLives { get; set; }

        public double? speed { get; set; }

        public CharacterClass mbClass { get; set; }

        public double? customRed { get; set; }
        public double? customGreen { get; set; }
        public double? customBlue { get; set; }

        public List<Items> holdables { get; set; }
        
        public List<ClassFlags> classFlags { get; set; }
    
        public double? rateOfFire { get; set; }

        public string WP_DetPackFlags { get; set; }

        public string WP_SaberFlags { get; set; }

        public string WP_BryarOldFlags { get; set; }

        public string WP_BlasterFlags { get; set; }

        public string WP_BlasterPistolFlags { get; set; }

        public string WP_StunBatonFlags { get; set; }

        public string WP_Demp2Flags { get; set; }

        public string WP_RocketLauncherFlags { get; set; }

        public string WP_ThermalFlags { get; set; }

        public string WP_FlechetteFlags { get; set; }

        public string WP_MeleeFlags { get; set; }

        public string WP_DisruptorFlags { get; set; }

        public string WP_RepeaterFlags { get; set; }

        public string WP_TripMineFlags { get; set; }

        public string WP_BowcasterFlags { get; set; }

        public double? meleeknockback { get; set; }

        public string jetpackJetTag { get; set; }

        public string jetpackJet2Tag { get; set; }

        public string jetpackJetOffset { get; set; }

        public string jetpackJet2Offset { get; set; }

        public string jetpackJetAngles { get; set; }
        public string jetpackJet2Angles { get; set; }

        public string jetpackThrustSound { get; set; }

        public string jetpackIdleSound { get; set; }

        public string jetpackThrustEffect { get; set; }

        public string jetpackIdleEffect { get; set; }

        public int? jetpackFuelCooldown { get; set; }

        public int? jetpackFuelAmount { get; set; }

        public string customveh { get; set; }

        public string jetpackStartSound { get; set; }

        public string forceallignment { get; set; }

        public int? respawnWait { get; set; }

        public string rageSoundOverride { get; set; }

        public string bargeSoundOverride { get; set; }

        public string meleeMoves { get; set; }

        public string headSwapModel { get; set; }
        public string headSwapSkin { get; set; }


    }
}
