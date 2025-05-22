using System;
using System.Collections.Generic;
using System.Text;

namespace MinesweeperModel
{
    public class Grid
    {
        public List<Row> Rows { get; set; }

        public Grid() {
            Rows = new List<Row>();
        }

        public Grid(List<Row> rows) {
            this.Rows = rows;
        }

        public Cell GetCell(int x, int y) {
            return Rows[y].Cells[x];
        }
        public Grid Clone() {
            Grid grid = new Grid();

            for (int y = 0; y < Rows.Count; y++) {
                Row row = new Row();

                for (int x = 0; x < Rows[y].Cells.Count; x++) {
                    Cell cell = new Cell();

                    cell.Flagged = Rows[y].Cells[x].Flagged;
                    cell.Number = Rows[y].Cells[x].Number;
                    cell.Opened = Rows[y].Cells[x].Opened;
                    cell.Type = Rows[y].Cells[x].Type;

                    row.Cells.Add(cell);
                }

                grid.Rows.Add(row);
            }

            return grid;
        }
    }
}
