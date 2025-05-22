using System;
using System.Collections.Generic;
using System.Text;

namespace MinesweeperModel
{
    public class Cell
    {
        public bool Opened { get; set; }
        public bool Flagged { get; set; }
        public CellType Type { get; set; }

        public int Number { get; set; }

        public Cell() {
            Opened = false;
            Flagged = false;
            Type = CellType.Empty;
            Number = -1;
        }

        public Cell(bool opened, bool flagged, CellType type, int number) {
            this.Opened = opened;
            this.Flagged = flagged;
            this.Type = type;
            this.Number = number;
        }
    }
}
