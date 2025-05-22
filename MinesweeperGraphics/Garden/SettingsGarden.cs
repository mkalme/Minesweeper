using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MinesweeperGraphics.Garden
{
    public class SettingsGarden
    {
        public static Size CellSize = RenderSettings.CellSize;
        public static int OutlineThickness = 2;

        public static Color[] UnopenedCellColor = { ColorTranslator.FromHtml("#AAD751"), ColorTranslator.FromHtml("#A2D149") };
        public static Color[] EmptyCellColor = { ColorTranslator.FromHtml("#E5C29F"), ColorTranslator.FromHtml("#D7B899") };

        public static Dictionary<int, Color> NumberColors = new Dictionary<int, Color>() {
            {1, ColorTranslator.FromHtml("#1976D2") },
            {2, ColorTranslator.FromHtml("#3C8F3E") },
            {3, ColorTranslator.FromHtml("#D32F2F") },
            {4, ColorTranslator.FromHtml("#7B1FA2") },
            {5, Color.DarkRed},
            {6, Color.DarkCyan},
            {7, Color.Black},
            {8, Color.Black}
        };
        public static Font NumberedFont = new Font("Arial", (int)Math.Round((double)CellSize.Width / 1.76), FontStyle.Bold);
    }
}
