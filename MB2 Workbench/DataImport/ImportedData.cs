using MB2_Workbench.DataTypes.Character;
using MB2_Workbench.DataTypes.Map;
using MB2_Workbench.DataTypes.Model;
using MB2_Workbench.DataTypes.Siege;
using MB2_Workbench.DataTypes.Skin;
using MB2_Workbench.DataTypes.Team;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MB2_Workbench.DataImport
{
    public static class ImportedData
    {

        /* All Imported Characters */
        public static ObservableCollection<Character> importedCharacters = new ObservableCollection<Character>();

        /* All Imported Teams */
        public static ObservableCollection<Team> importedTeams = new ObservableCollection<Team>();

        /* All Imported Map Sieges */
        public static ObservableCollection<Siege> importedSieges = new ObservableCollection<Siege>();

        /* All Imported Maps */
        public static ObservableCollection<Map> importedMaps = new ObservableCollection<Map>();

        /* All Imported Skins */
        public static ObservableCollection<Skin> importedSkins = new ObservableCollection<Skin>();

        /* All Imported Player Models */
        public static ObservableCollection<PlayerModel> importedPlayerModels = new ObservableCollection<PlayerModel>();

        /* All Imported Player Models */
        public static ObservableCollection<WeaponModel> importedWeaponModels = new ObservableCollection<WeaponModel>();

    }
}
