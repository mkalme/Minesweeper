using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinesweeperModel;
using System.Drawing;

namespace MinesweeperMechanics
{
    public class ActionGrid
    {
        private static List<Point> EmptyOrNumeredCells = new List<Point>();

        //Expand
        public static void ExpandGrid(Grid grid, int x, int y) {
            EmptyOrNumeredCells.Clear();

            AddEmptyOrNumeredCells(grid, x, y);

            for (int i = 0; i < EmptyOrNumeredCells.Count; i++) {
                grid.GetCell(EmptyOrNumeredCells[i].X, EmptyOrNumeredCells[i].Y).Opened = true;
            }
        }

        private static void AddEmptyOrNumeredCells(Grid grid, int x, int y) {
            if (!IfAlreadyFound(x, y)) {
                Cell cell = grid.GetCell(x, y);

                if (cell.Type == CellType.Empty) {
                    EmptyOrNumeredCells.Add(new Point(x, y));

                    SearchCells(grid, x, y);
                } else if (cell.Type == CellType.Numbered) {
                    EmptyOrNumeredCells.Add(new Point(x, y));
                }
            }
        }
        private static bool IfAlreadyFound(int x, int y) {
            for (int i = 0; i < EmptyOrNumeredCells.Count; i++) {
                if (EmptyOrNumeredCells[i].X == x && EmptyOrNumeredCells[i].Y == y) {
                    return true;
                }
            }

            return false;
        }
        private static void SearchCells(Grid grid, int x, int y)
        {
            //Top
            if (y > 0){
                AddEmptyOrNumeredCells(grid, x, y - 1);
            }

            //Right
            if (x < grid.Rows[0].Cells.Count - 1){
                AddEmptyOrNumeredCells(grid, x + 1, y);
            }

            //Bottom
            if (y < grid.Rows.Count - 1){
                AddEmptyOrNumeredCells(grid, x, y + 1);
            }

            //Left
            if (x > 0){
                AddEmptyOrNumeredCells(grid, x - 1, y);
            }
        }

        //UserInput
        public static void ClickOnBomb(Grid grid) {
            for (int y = 0; y < grid.Rows.Count; y++) {
                for (int x = 0; x < grid.Rows[y].Cells.Count; x++) {
                    grid.GetCell(x, y).Opened = true;
                }
            }
        }
    }
}
