using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MinesweeperMechanics;
using MinesweeperGraphics;

namespace Minesweeper
{
    public partial class Options : Form
    {
        private Base BaseForm { get; set; }
        private static Style[] AllStyles = new Style[Enum.GetNames(typeof(Style)).Length];

        public Options(Base baseForm)
        {
            this.BaseForm = baseForm;

            InitializeComponent();
        }

        private void Options_Load(object sender, EventArgs e)
        {
            Load_Options();
        }

        private void Load_Options() {
            //Mechanics
            amountOfBombstextBox.Text = MechanicsSettings.NumberOfBombs.ToString();

            squareAmountXtextBox.Text = Base.Game.Grid.Rows[0].Cells.Count.ToString();
            squareAmountYtextBox.Text = Base.Game.Grid.Rows.Count.ToString();

            //Graphics
            for (int i = 0; i < AllStyles.Length; i++) {
                AllStyles[i] = (Style)i;

                styleComboBox.Items.Add(AllStyles[i]);

                if (RenderSettings.Style == AllStyles[i]) {
                    styleComboBox.SelectedItem = AllStyles[i];
                }
            }

            squareSizeXTextBox.Text = RenderSettings.CellSize.Width.ToString();
            squareSizeYTextBox.Text = RenderSettings.CellSize.Height.ToString();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (checkInput()) {
                saveSettings();

                BaseForm.RestartGame();

                Close();
            }
        }

        private static int[] intValues = new int[5];

        private bool checkInput() {
            bool isSafe = true;

            TextBox[] allTextBoxes = {amountOfBombstextBox, squareAmountXtextBox,
                squareAmountYtextBox, squareSizeXTextBox, squareSizeYTextBox};

            for (int i = 0; i < allTextBoxes.Length; i++) {
                if (!Int32.TryParse(allTextBoxes[i].Text, out intValues[i])) {
                    return false;
                }
            }

            if (string.IsNullOrEmpty(styleComboBox.Text)) {
                return false;
            }

            return isSafe;
        }
        private void saveSettings() {
            //Mechanics
            MechanicsSettings.NumberOfBombs = intValues[0];

            MechanicsSettings.GridSize.Width = intValues[1];
            MechanicsSettings.GridSize.Height = intValues[2];

            //Graphics
            RenderSettings.Style = AllStyles[styleComboBox.SelectedIndex];

            RenderSettings.CellSize.Width = intValues[3];
            RenderSettings.CellSize.Height = intValues[4];
        }
    }
}
