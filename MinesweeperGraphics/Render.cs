using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using MinesweeperModel;

namespace MinesweeperGraphics
{
    public class Render
    {
        public Grid Grid { get; set; }
        public Bitmap Image { get; set; }
        public Point StartingPoint { get; set; }

        private Garden.RenderGarden GardenRender {get; set;}
        private Classic.RenderClassic ClassicRender { get; set; }

        public Render(Grid grid)
        {
            Grid = grid;
            Image = null;

            if (RenderSettings.Style == Style.Garden) {
                GardenRender = new Garden.RenderGarden(Grid);
                StartingPoint = GardenRender.StartingPoint;
            } else if (RenderSettings.Style == Style.Classic) {
                ClassicRender = new Classic.RenderClassic(Grid);
                StartingPoint = ClassicRender.StartingPoint;
            }
        }

        public void UpdateGraphics()
        {
            if (RenderSettings.Style == Style.Garden) {
                Image = GardenRender.UpdateGraphics();
            } else if (RenderSettings.Style == Style.Classic) {
                Image = ClassicRender.UpdateGraphics();
            }
        }
        public void UpdateCells(List<Point> cells) {
            if (RenderSettings.Style == Style.Garden){
                Image = GardenRender.UpdateCells(cells);
            }else if (RenderSettings.Style == Style.Classic){
                Image = ClassicRender.UpdateCells(cells);
            }
        }
    }
}
