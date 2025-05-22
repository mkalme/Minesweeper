using System;
using System.Collections.Generic;
using System.Text;

namespace MinesweeperModel
{
    public class Row
    {
        public List<Cell> Cells { get; set; }

        public Row() {
            Cells = new List<Cell>();
        }

        public Row(List<Cell> cells) {
            this.Cells = cells;
        }
    }
}
