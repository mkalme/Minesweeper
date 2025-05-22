using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MinesweeperGraphics.Classic
{
    public class SettingsClassic
    {
        public static Size CellSize = RenderSettings.CellSize;
        public static int BorderThickness = (int)Math.Round((double)CellSize.Width / 15.0);

        public static Dictionary<int, Color> NumberColors = new Dictionary<int, Color>() {
            {1, Color.Blue },
            {2, Color.Green },
            {3, Color.Red },
            {4, Color.Purple },
            {5, Color.DarkRed},
            {6, Color.DarkCyan},
            {7, Color.Black},
            {8, Color.Black}
        };
        public static Font NumberedFont = new Font("Arial", (int)Math.Round((double)CellSize.Width / 1.76), FontStyle.Bold);
    }
}
