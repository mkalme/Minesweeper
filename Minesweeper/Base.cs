using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MinesweeperGraphics;
using MinesweeperModel;
using MinesweeperMechanics;

namespace Minesweeper
{
    public partial class Base : Form
    {
        public static Game Game;

        public Base()
        {
            InitializeComponent();
        }
        private void Base_Load(object sender, EventArgs e)
        {
            RunGraphics.InitializeBackgroundWorker();
            StartGame();
        }

        //Start the game
        public void StartGame() {
            Grid grid = SetupGrid();
            Render render = SetupRender(grid);

            Game = new Game(
                grid,
                render,                
                false,                
                false
            );

            ChangeFormSize();

            RunGraphics.StartRunningGraphics(this);

            Game.GameInSession = true;
        }

        private Grid SetupGrid() {
            return GenerateGrid.GenerateEmptyGrid(MechanicsSettings.GridSize);
        }
        private Render SetupRender(Grid grid) {
            Render render = new Render(grid);
            render.UpdateGraphics();

            return render;
        }

        private void ChangeFormSize() {
            Size imageSize = Game.Render.Image.Size;

            Size = new Size(imageSize.Width + 16, imageSize.Height + 39);

            CenterToScreen();
        }

        //Restart the game
        public void RestartGame() {
            Grid grid = SetupGrid();
            Render render = SetupRender(grid);

            Game = new Game(
                grid,
                render,
                false,
                false
            );

            ChangeFormSize();

            Game.GameInSession = true;
        }

        //MouseClick event
        private void PictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (Game.GameInSession) {
                if (Game.Generated) {
                    UserInput.UserClick(pictureBox1, e);
                }
                else {
                    Point relativeToImage = UserInput.GetRelativeToImage(pictureBox1, e);
                    Point cellCoordinates = UserInput.GetCellCoordinates(relativeToImage);

                    if (CellClick.CoordinatesValid(Game.Grid, cellCoordinates.X, cellCoordinates.Y)) {
                        Game.Grid = GenerateGrid.Generate(Game.Grid, cellCoordinates);

                        Game.Generated = true;
                        UserInput.UserClick(pictureBox1, e);
                    }
                }
            }
        }

        //Key event
        private void Base_KeyUp(object sender, KeyEventArgs e)
        {
            if ((ModifierKeys & Keys.Control) == Keys.Control && e.KeyCode == Keys.O) {
                Options options = new Options(this);
                options.ShowDialog();
            }
        }
    }
}
