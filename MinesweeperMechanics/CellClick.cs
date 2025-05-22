using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinesweeperModel;

namespace MinesweeperMechanics
{
    public class CellClick
    {
        public static void UserOpenCell(Grid grid, int x, int y){
            if (CoordinatesValid(grid, x, y)) {
                ActionGrid.ExpandGrid(grid, x, y);

                grid.GetCell(x, y).Opened = true;

                //If bomb
                if (grid.GetCell(x, y).Type == CellType.Bomb) {
                    ActionGrid.ClickOnBomb(grid);
                }
            }
        }
        public static void UserFlagCell(Grid grid, int x, int y){
            if (CoordinatesValid(grid, x, y)) {
                Cell cell = grid.GetCell(x, y);

                cell.Flagged = cell.Flagged ? false : true;
            }
        }

        public static bool CoordinatesValid(Grid grid, int x, int y){
            if ((x > -1 && y > -1) &&
                (x < grid.Rows[0].Cells.Count && y < grid.Rows.Count))
            {
                return true;
            }else{
                return false;
            }
        }
    }
}
