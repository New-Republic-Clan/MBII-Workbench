using MB2_Workbench.DataImport;
using MB2_Workbench.DataTypes.Character;
using MB2_Workbench.DataTypes.Team;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MB2_Workbench.Classes.Extensions
{
    public static class TeamExtensions
    {
        /// <summary>Fetch a List of characters used by this team</summary>
        public static List<Character> TeamToCharacterList(this Team team)
        {

            List<Character> foundCharacters = ImportedData.importedCharacters.Where(x =>
                x.classinfo.name == team.classes.class1 ||
                x.classinfo.name == team.classes.class2 ||
                x.classinfo.name == team.classes.class3 ||
                x.classinfo.name == team.classes.class4 ||
                x.classinfo.name == team.classes.class5 ||
                x.classinfo.name == team.classes.class6 ||
                x.classinfo.name == team.classes.class7 ||
                x.classinfo.name == team.classes.class8 ||
                x.classinfo.name == team.classes.class9 ||

                team.SubClassesForClass1 != null && (
                x.classinfo.name == team.SubClassesForClass1.Subclass1 ||
                x.classinfo.name == team.SubClassesForClass1.Subclass2 ||
                x.classinfo.name == team.SubClassesForClass1.Subclass3 ||
                x.classinfo.name == team.SubClassesForClass1.Subclass4 ||
                x.classinfo.name == team.SubClassesForClass1.Subclass5 ||
                x.classinfo.name == team.SubClassesForClass1.Subclass6 ||
                x.classinfo.name == team.SubClassesForClass1.Subclass7 ||
                x.classinfo.name == team.SubClassesForClass1.Subclass8 ||
                x.classinfo.name == team.SubClassesForClass1.Subclass9 ||
                x.classinfo.name == team.SubClassesForClass1.Subclass10
                ) ||

                team.SubClassesForClass2 != null && (
                x.classinfo.name == team.SubClassesForClass2.Subclass1 ||
                x.classinfo.name == team.SubClassesForClass2.Subclass2 ||
                x.classinfo.name == team.SubClassesForClass2.Subclass3 ||
                x.classinfo.name == team.SubClassesForClass2.Subclass4 ||
                x.classinfo.name == team.SubClassesForClass2.Subclass5 ||
                x.classinfo.name == team.SubClassesForClass2.Subclass6 ||
                x.classinfo.name == team.SubClassesForClass2.Subclass7 ||
                x.classinfo.name == team.SubClassesForClass2.Subclass8 ||
                x.classinfo.name == team.SubClassesForClass2.Subclass9 ||
                x.classinfo.name == team.SubClassesForClass2.Subclass10
                ) ||

                 team.SubClassesForClass3 != null && (
                x.classinfo.name == team.SubClassesForClass3.Subclass1 ||
                x.classinfo.name == team.SubClassesForClass3.Subclass2 ||
                x.classinfo.name == team.SubClassesForClass3.Subclass3 ||
                x.classinfo.name == team.SubClassesForClass3.Subclass4 ||
                x.classinfo.name == team.SubClassesForClass3.Subclass5 ||
                x.classinfo.name == team.SubClassesForClass3.Subclass6 ||
                x.classinfo.name == team.SubClassesForClass3.Subclass7 ||
                x.classinfo.name == team.SubClassesForClass3.Subclass8 ||
                x.classinfo.name == team.SubClassesForClass3.Subclass9 ||
                x.classinfo.name == team.SubClassesForClass3.Subclass10
                ) ||


                 team.SubClassesForClass4 != null && (
                x.classinfo.name == team.SubClassesForClass4.Subclass1 ||
                x.classinfo.name == team.SubClassesForClass4.Subclass2 ||
                x.classinfo.name == team.SubClassesForClass4.Subclass3 ||
                x.classinfo.name == team.SubClassesForClass4.Subclass4 ||
                x.classinfo.name == team.SubClassesForClass4.Subclass5 ||
                x.classinfo.name == team.SubClassesForClass4.Subclass6 ||
                x.classinfo.name == team.SubClassesForClass4.Subclass7 ||
                x.classinfo.name == team.SubClassesForClass4.Subclass8 ||
                x.classinfo.name == team.SubClassesForClass4.Subclass9 ||
                x.classinfo.name == team.SubClassesForClass4.Subclass10
                ) ||


                 team.SubClassesForClass5 != null && (
                x.classinfo.name == team.SubClassesForClass5.Subclass1 ||
                x.classinfo.name == team.SubClassesForClass5.Subclass2 ||
                x.classinfo.name == team.SubClassesForClass5.Subclass3 ||
                x.classinfo.name == team.SubClassesForClass5.Subclass4 ||
                x.classinfo.name == team.SubClassesForClass5.Subclass5 ||
                x.classinfo.name == team.SubClassesForClass5.Subclass6 ||
                x.classinfo.name == team.SubClassesForClass5.Subclass7 ||
                x.classinfo.name == team.SubClassesForClass5.Subclass8 ||
                x.classinfo.name == team.SubClassesForClass5.Subclass9 ||
                x.classinfo.name == team.SubClassesForClass5.Subclass10
                ) ||


                 team.SubClassesForClass6 != null && (
                x.classinfo.name == team.SubClassesForClass6.Subclass1 ||
                x.classinfo.name == team.SubClassesForClass6.Subclass2 ||
                x.classinfo.name == team.SubClassesForClass6.Subclass3 ||
                x.classinfo.name == team.SubClassesForClass6.Subclass4 ||
                x.classinfo.name == team.SubClassesForClass6.Subclass5 ||
                x.classinfo.name == team.SubClassesForClass6.Subclass6 ||
                x.classinfo.name == team.SubClassesForClass6.Subclass7 ||
                x.classinfo.name == team.SubClassesForClass6.Subclass8 ||
                x.classinfo.name == team.SubClassesForClass6.Subclass9 ||
                x.classinfo.name == team.SubClassesForClass6.Subclass10
                ) ||


                 team.SubClassesForClass7 != null && (
                x.classinfo.name == team.SubClassesForClass7.Subclass1 ||
                x.classinfo.name == team.SubClassesForClass7.Subclass2 ||
                x.classinfo.name == team.SubClassesForClass7.Subclass3 ||
                x.classinfo.name == team.SubClassesForClass7.Subclass4 ||
                x.classinfo.name == team.SubClassesForClass7.Subclass5 ||
                x.classinfo.name == team.SubClassesForClass7.Subclass6 ||
                x.classinfo.name == team.SubClassesForClass7.Subclass7 ||
                x.classinfo.name == team.SubClassesForClass7.Subclass8 ||
                x.classinfo.name == team.SubClassesForClass7.Subclass9 ||
                x.classinfo.name == team.SubClassesForClass7.Subclass10
                ) ||


                 team.SubClassesForClass8 != null && (
                x.classinfo.name == team.SubClassesForClass8.Subclass1 ||
                x.classinfo.name == team.SubClassesForClass8.Subclass2 ||
                x.classinfo.name == team.SubClassesForClass8.Subclass3 ||
                x.classinfo.name == team.SubClassesForClass8.Subclass4 ||
                x.classinfo.name == team.SubClassesForClass8.Subclass5 ||
                x.classinfo.name == team.SubClassesForClass8.Subclass6 ||
                x.classinfo.name == team.SubClassesForClass8.Subclass7 ||
                x.classinfo.name == team.SubClassesForClass8.Subclass8 ||
                x.classinfo.name == team.SubClassesForClass8.Subclass9 ||
                x.classinfo.name == team.SubClassesForClass8.Subclass10
                ) ||


                 team.SubClassesForClass9 != null && (
                x.classinfo.name == team.SubClassesForClass9.Subclass1 ||
                x.classinfo.name == team.SubClassesForClass9.Subclass2 ||
                x.classinfo.name == team.SubClassesForClass9.Subclass3 ||
                x.classinfo.name == team.SubClassesForClass9.Subclass4 ||
                x.classinfo.name == team.SubClassesForClass9.Subclass5 ||
                x.classinfo.name == team.SubClassesForClass9.Subclass6 ||
                x.classinfo.name == team.SubClassesForClass9.Subclass7 ||
                x.classinfo.name == team.SubClassesForClass9.Subclass8 ||
                x.classinfo.name == team.SubClassesForClass9.Subclass9 ||
                x.classinfo.name == team.SubClassesForClass9.Subclass10
                ) ||

                 team.SubClassesForClass10 != null && (
                x.classinfo.name == team.SubClassesForClass10.Subclass1 ||
                x.classinfo.name == team.SubClassesForClass10.Subclass2 ||
                x.classinfo.name == team.SubClassesForClass10.Subclass3 ||
                x.classinfo.name == team.SubClassesForClass10.Subclass4 ||
                x.classinfo.name == team.SubClassesForClass10.Subclass5 ||
                x.classinfo.name == team.SubClassesForClass10.Subclass6 ||
                x.classinfo.name == team.SubClassesForClass10.Subclass7 ||
                x.classinfo.name == team.SubClassesForClass10.Subclass8 ||
                x.classinfo.name == team.SubClassesForClass10.Subclass9 ||
                x.classinfo.name == team.SubClassesForClass10.Subclass10
                )

            ).ToList();

            return foundCharacters;

        }

    }
}
