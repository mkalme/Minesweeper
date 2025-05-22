using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using MinesweeperGraphics;
using MinesweeperModel;
using MinesweeperMechanics;

namespace Minesweeper
{
    class UserInput
    {
        public static void UserClick(PictureBox pictureBox, MouseEventArgs e) {
            Point relativeToImage = GetRelativeToImage(pictureBox, e);
            Point cellCoordinates = GetCellCoordinates(relativeToImage);

            if (e.Button == MouseButtons.Left) {
                CellClick.UserOpenCell(Base.Game.Grid, cellCoordinates.X, cellCoordinates.Y);
            } else if (e.Button == MouseButtons.Right) {
                CellClick.UserFlagCell(Base.Game.Grid, cellCoordinates.X, cellCoordinates.Y);
            }
        }

        public static Point GetRelativeToImage(PictureBox pictureBox, MouseEventArgs e) {
            Point relativeToImage = new Point(
                e.X - ((pictureBox.Width - pictureBox.Image.Width) / 2) - Base.Game.Render.StartingPoint.X,
                e.Y - ((pictureBox.Height - pictureBox.Image.Height) / 2) - Base.Game.Render.StartingPoint.Y
            );

            return relativeToImage;
        }
        public static Point GetCellCoordinates(Point relativeToImage) {
            Point cellCoordinates = new Point(-1, -1);

            if (relativeToImage.X > -1 && relativeToImage.Y > -1) {
                Size cellSize = new Size();
                if (RenderSettings.Style == Style.Garden) {
                    cellSize = MinesweeperGraphics.Garden.SettingsGarden.CellSize;
                } else if (RenderSettings.Style == Style.Classic) {
                    cellSize = MinesweeperGraphics.Classic.SettingsClassic.CellSize;
                }

                cellCoordinates.X = relativeToImage.X / cellSize.Width;
                cellCoordinates.Y = relativeToImage.Y / cellSize.Height;
            }

            return cellCoordinates;
        }
    }
}
