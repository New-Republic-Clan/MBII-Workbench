using MB2_Workbench.DataImport;
using MB2_Workbench.Classes.Helpers;
using MB2_Workbench.Classes;
using MB2_Workbench.Classes.Extensions;
using MB2_Workbench.DataTypes.Siege;

using System;
using System.IO;
using System.IO.Compression;
using System.Windows;
using System.Linq;
using Path = System.IO.Path;
using System.Collections.ObjectModel;
using MB2_Workbench.DataTypes.Team;
using MB2_Workbench.DataTypes.Character;

namespace MB2_Workbench.Windows.Dialogs
{
    /// <summary>
    /// Interaction logic for DialogCloneCharacter.xaml
    /// </summary>
    public partial class DialogCloneCharacter : Window
    {

        public Character CharacterBeingCloned;

        public DialogCloneCharacter(Character characterBeingCloned)
        {
            
            InitializeComponent();
            Prefix.Text = ActiveProject.Project.prefix;
            CharacterBeingCloned = characterBeingCloned;


        }

        /* User clicks "clone character" button */
        private void CloneCharacterButton_Click(object sender, RoutedEventArgs e)
        {

            if (CharacterID.Text.Length == 0 || !CharacterID.Text.ToLower().Equals(CharacterID.Text) || !CharacterID.Text.Replace(" ", "").Equals(CharacterID.Text))
            {
                MessageBox.Show("Your character ID must contain no spaces, no capitals or match an existing character ID");
                return;
            }

            if (CharacterName.Text.Length > 50 )
            {
                MessageBox.Show("Your character name should be less than 50 characters in length");
                return;
            }

            /* Logic to clone the character */
            Character clonedCharacter = CharacterBeingCloned;
            clonedCharacter.description = clonedCharacter.description.Replace(clonedCharacter.name, CharacterName.Text);
            clonedCharacter.classinfo.name = ActiveProject.Project.prefix + CharacterID.Text;

            ActiveProject.Project.characters.Add(clonedCharacter);

            MainWindow mainWindow = WindowFinder.FindOpenWindowByType<MainWindow>();

            mainWindow.ProjectDataTree.Items.Refresh();
            mainWindow.ProjectDataTree.UpdateLayout();

            this.Close();
        }

    }
}
