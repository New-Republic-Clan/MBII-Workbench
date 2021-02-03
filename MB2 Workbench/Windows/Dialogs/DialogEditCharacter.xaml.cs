using MB2_Workbench.DataTypes.Character;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MB2_Workbench.Windows.Dialogs
{
    /// <summary>
    /// Interaction logic for DialogEditCharacter.xaml
    /// </summary>
    public partial class DialogEditCharacter : Window
    {
        public Character CharacterBeingEdited;

        public DialogEditCharacter(Character characterBeingEdited)
        {

            InitializeComponent();
            CharacterBeingEdited = characterBeingEdited;




        }
    }
}
