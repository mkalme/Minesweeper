using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using MinesweeperModel;

namespace MinesweeperGraphics.Classic
{
    class RenderClassic
    {
        public Grid Grid { get; set; }
        private Bitmap Image { get; set; }
        private Graphics graphics { get; set; }

        public Point StartingPoint { get; set; }
        private int FrameThickness { get; set; }

        public RenderClassic(Grid grid) {
            LoadSettings();
            FrameThickness = GetFrameThickness();

            Grid = grid;
            Size size = GetGridSize(grid);
            Image = new Bitmap(size.Width, size.Height);
            graphics = Graphics.FromImage(Image);

            StartingPoint = new Point(FrameThickness, FrameThickness);
        }
        private Size GetGridSize(Grid grid)
        {
            Size size = new Size
            {
                Width = SettingsClassic.CellSize.Width * grid.Rows[0].Cells.Count + (FrameThickness * 2),
                Height = SettingsClassic.CellSize.Height * grid.Rows.Count + (FrameThickness * 2)
            };

            return size;
        }

        private void LoadSettings() {
            SettingsClassic.CellSize = RenderSettings.CellSize;
            SettingsClassic.BorderThickness = (int)Math.Round((double)SettingsClassic.CellSize.Width / 15.0);

            SettingsClassic.NumberedFont = new Font("Arial", (int)Math.Round((double)SettingsClassic.CellSize.Width / 1.76), FontStyle.Bold);
        }

        public Bitmap UpdateGraphics() {
            graphics.Clear(SystemColors.Control);

            DrawFrame();
            DrawGrid();

            return Image;
        }
        public Bitmap UpdateCells(List<Point> cells) {
            for (int i = 0; i < cells.Count; i++) {
                DrawCell(cells[i].X, cells[i].Y);
            }

            return Image;
        }

        //Draw Frame
        private void DrawFrame()
        {
            DrawSides();
            DrawCorners();
        }

        private void DrawSides()
        {
            DrawVerticalSide(0);
            DrawVerticalSide(1);

            DrawHorizontalSide(0);
            DrawHorizontalSide(1);
        }
        private void DrawVerticalSide(int side)
        {
            //Get image
            Bitmap verticalSide = new Bitmap(Properties.Resources.VerticalSide, new Size(FrameThickness, CellStartY(Grid.Rows.Count)));

            //Get location
            PointF locationF = new PointF();

            if (side == 0)
            {
                locationF.X = 0;
            }
            else if (side == 1)
            {
                locationF.X = CellStartX(Grid.Rows[0].Cells.Count);
            }

            locationF.Y = FrameThickness;

            //Draw image
            graphics.DrawImage(verticalSide, locationF);

            verticalSide.Dispose();
        }
        private void DrawHorizontalSide(int side)
        {
            //Get image
            Bitmap horizontalSide = new Bitmap(Properties.Resources.HorizontalSide, new Size(CellStartX(Grid.Rows[0].Cells.Count), FrameThickness));

            //Get location
            PointF locationF = new PointF();

            if (side == 0){
                locationF.Y = 0;
            }else if (side == 1){
                locationF.Y = CellStartY(Grid.Rows.Count);
            }

            locationF.X = CellStartX(0);

            //Draw image
            graphics.DrawImage(horizontalSide, locationF);

            horizontalSide.Dispose();
        }

        private void DrawCorners()
        {
            DrawCorner(0);
            DrawCorner(1);
            DrawCorner(2);
            DrawCorner(3);
        }
        private void DrawCorner(int type)
        {
            //Get image && location
            Image image = null;
            PointF locationF = new PointF();

            if (type == 0)
            {
                image = Properties.Resources.TopRightCorner;
                locationF = new PointF(CellStartX(Grid.Rows[0].Cells.Count), 0);
            }
            else if (type == 1)
            {
                image = Properties.Resources.BottomRightCorner;
                locationF = new PointF(CellStartX(Grid.Rows[0].Cells.Count), CellStartY(Grid.Rows.Count));
            }
            else if (type == 2)
            {
                image = Properties.Resources.BottomLeftCorner;
                locationF = new PointF(0, CellStartY(Grid.Rows.Count));
            }
            else if (type == 3)
            {
                image = Properties.Resources.TopLeftCorner;
                locationF = new PointF(0, 0);
            }

            Bitmap imageBitmap = new Bitmap(image, new Size(FrameThickness, FrameThickness));

            //Draw image
            graphics.DrawImage(imageBitmap, locationF);

            imageBitmap.Dispose();
            image.Dispose();
        }

        //Draw Grid
        private void DrawGrid()
        {
            for (int y = 0; y < Grid.Rows.Count; y++){
                for (int x = 0; x < Grid.Rows[y].Cells.Count; x++){
                    DrawCell(x, y);
                }
            }
        }
        private void DrawCell(int x, int y){
            Cell cell = Grid.GetCell(x, y);

            if (!cell.Opened){
                DrawUnopenedCell(x, y);

                if (cell.Flagged){
                    DrawFlag(x, y);
                }
            }else{
                DrawEmptyCell(x, y);

                if (cell.Type == CellType.Bomb){
                    DrawBomb(x, y);
                }else if (cell.Type == CellType.Numbered){
                    DrawNumberedCell(cell.Number, x, y);
                }
            }
        }

        private void DrawUnopenedCell(int x, int y)
        {
            //Get image
            Bitmap square = new Bitmap(Properties.Resources.ClassicSquareSmall, SettingsClassic.CellSize);

            //Location
            PointF locationF = new PointF(CellStartX(x), CellStartY(y));

            //Draw image
            graphics.DrawImage(square, locationF);

            square.Dispose();
        }
        private void DrawEmptyCell(int x, int y)
        {
            //DrawLine Line 1 (Vertical)
            Point point1 = new Point(CellStartX(x), CellStartY(y));
            Point point2 = new Point(point1.X, CellStartY(y) + SettingsClassic.CellSize.Height);

            int splicer = GetSplicer();

            graphics.DrawLine(
                new Pen(ColorTranslator.FromHtml("#7B7B7B"), SettingsClassic.BorderThickness),
                new Point(point1.X + splicer, point1.Y + 1), new Point(point2.X + splicer, point2.Y)
            );

            //DrawLine Line 2 (Horizontal)
            point2 = new Point(CellStartX(x) + SettingsClassic.CellSize.Width, point1.Y);

            graphics.DrawLine(
                new Pen(ColorTranslator.FromHtml("#7B7B7B"), SettingsClassic.BorderThickness),
                new Point(point1.X, point1.Y + splicer), new Point(point2.X, point2.Y + splicer)
            );

            //Draw Rectangle
            PointF locationF = new PointF(point1.X + SettingsClassic.BorderThickness, point1.Y + SettingsClassic.BorderThickness);

            graphics.FillRectangle(
                new SolidBrush(ColorTranslator.FromHtml("#BDBDBD")),
                new RectangleF(locationF, new SizeF(SettingsClassic.CellSize.Width - SettingsClassic.BorderThickness, SettingsClassic.CellSize.Height - SettingsClassic.BorderThickness))
            );
        }
        private void DrawNumberedCell(int number, int x, int y)
        {
            DrawText(number.ToString(), SettingsClassic.NumberColors[number], x, y);
        }

        private void DrawFlag(int x, int y)
        {
            //Get image
            Bitmap flag = new Bitmap(
                Properties.Resources.Flag,
                new Size((int)(SettingsClassic.CellSize.Width * 0.6), (int)(SettingsClassic.CellSize.Width * 0.6))
            );

            //Calculate location
            PointF locationF = new PointF(
                CellStartX(x) + (SettingsClassic.CellSize.Width - (int)flag.Width) / 2,
                CellStartY(y) + (SettingsClassic.CellSize.Height - (int)flag.Height) / 2
            );

            //Draw image
            graphics.DrawImage(flag, locationF);

            flag.Dispose();
        }
        private void DrawBomb(int x, int y)
        {
            //Get image
            Bitmap bomb = new Bitmap(
                Properties.Resources.Bomb,
                new Size((int)(SettingsClassic.CellSize.Width * 0.6), (int)(SettingsClassic.CellSize.Width * 0.6))
            );

            //Calculate location
            PointF locationF = new PointF(
                CellStartX(x) + (SettingsClassic.CellSize.Width - (int)bomb.Width) / 2 + (SettingsClassic.BorderThickness / 2),
                CellStartY(y) + (SettingsClassic.CellSize.Height - (int)bomb.Height) / 2 + (SettingsClassic.BorderThickness / 2)
            );

            //Draw image
            graphics.DrawImage(bomb, locationF);

            bomb.Dispose();
        }
        private void DrawText(string text, Color color, int x, int y)
        {
            //Get size
            SizeF sizeF = graphics.MeasureString(text, SettingsClassic.NumberedFont);

            //Calculate location
            PointF locationF = new PointF(
                CellStartX(x) + (SettingsClassic.CellSize.Width - (int)sizeF.Width) / 2,
                CellStartY(y) + (SettingsClassic.CellSize.Height - (int)sizeF.Height) + (SettingsClassic.BorderThickness / 2)
            );

            //Draw
            graphics.DrawString(text, SettingsClassic.NumberedFont, new SolidBrush(color), locationF);
        }

        //Scripts
        public int CellStartX(int x) {
            return (x * SettingsClassic.CellSize.Width) + FrameThickness;
        }
        public int CellStartY(int y){
            return (y * SettingsClassic.CellSize.Height) + FrameThickness;
        }

        private int GetFrameThickness() {
            return (int)((double)Properties.Resources.VerticalSide.Width * ((double)SettingsClassic.CellSize.Width / (double)Properties.Resources.ClassicSquareSmall.Width));
        }

        public int GetSplicer() {
            if (SettingsClassic.BorderThickness % 2 == 0)
            {
                return SettingsClassic.BorderThickness % 2 == 0 ? SettingsClassic.BorderThickness / 2 : SettingsClassic.BorderThickness / 2 + 1;
            }else {
                return SettingsClassic.BorderThickness % 2 == 0 ? SettingsClassic.BorderThickness / 2 + 1 : SettingsClassic.BorderThickness / 2;
            }
        }
    }
}
