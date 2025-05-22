using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using MinesweeperModel;

namespace MinesweeperGraphics.Garden
{
    class RenderGarden
    {
        public Grid Grid { get; set; }
        private Bitmap Image { get; set; }
        private Graphics graphics { get; set; }

        public Point StartingPoint { get; set; }

        public RenderGarden(Grid grid) {
            LoadSettings();

            Grid = grid;
            Size size = GetGridSize(grid);
            Image = new Bitmap(size.Width, size.Height);
            graphics = Graphics.FromImage(Image);

            StartingPoint = new Point(0, 0);
        }
        private Size GetGridSize(Grid grid)
        {
            Size size = new Size
            {
                Width = SettingsGarden.CellSize.Width * grid.Rows[0].Cells.Count,
                Height = SettingsGarden.CellSize.Height * grid.Rows.Count
            };

            return size;
        }

        private void LoadSettings()
        {
            SettingsGarden.CellSize = RenderSettings.CellSize;

            SettingsGarden.NumberedFont = new Font("Arial", (int)Math.Round((double)SettingsGarden.CellSize.Width / 1.76), FontStyle.Bold);
        }

        public Bitmap UpdateGraphics() {
            graphics.Clear(SystemColors.Control);

            DrawGrid();

            return Image;
        }
        public Bitmap UpdateCells(List<Point> cells) {
            for (int i = 0; i < cells.Count; i++) {
                DrawCell(cells[i].X, cells[i].Y);
            }

            return Image;
        }

        //Draw Grid
        private void DrawGrid()
        {
            for (int y = 0; y < Grid.Rows.Count; y++) {
                for (int x = 0; x < Grid.Rows[y].Cells.Count; x++) {
                    DrawCell(x, y);
                }
            }
        }
        private void DrawCell(int x, int y) {
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

        private void DrawUnopenedCell(int x, int y) {
            Color color = GetSecondColor(SettingsGarden.UnopenedCellColor, x, y);

            DrawRectangle(color, x, y);
        }
        private void DrawEmptyCell(int x, int y) {
            Color color = GetSecondColor(SettingsGarden.EmptyCellColor, x, y);

            DrawRectangle(color, x, y);
            DrawOutlines(x, y);
            EraseOutlines(x, y);
        }
        private void DrawNumberedCell(int number, int x, int y) {
            DrawText(number.ToString(), SettingsGarden.NumberColors[number], x, y);
        }

        private void DrawFlag(int x, int y) {
            //Get image
            Bitmap flag = new Bitmap(
                Properties.Resources.Flag,
                new Size((int)(SettingsGarden.CellSize.Width * 0.6), (int)(SettingsGarden.CellSize.Width * 0.6))
            );

            //Calculate location
            PointF locationF = new PointF(
                CellStartX(x) + (SettingsGarden.CellSize.Width - (int)flag.Width) / 2,
                CellStartY(y) + (SettingsGarden.CellSize.Height - (int)flag.Height) / 2
            );

            //Draw image
            graphics.DrawImage(flag, locationF);

            flag.Dispose();
        }
        private void DrawBomb(int x, int y) {
            //Get image
            Bitmap bomb = new Bitmap(
                Properties.Resources.Bomb,
                new Size((int)(SettingsGarden.CellSize.Width * 0.6), (int)(SettingsGarden.CellSize.Width * 0.6))
            );

            //Calculate location
            PointF locationF = new PointF(
                CellStartX(x) + (SettingsGarden.CellSize.Width - (int)bomb.Width) / 2,
                CellStartY(y) + (SettingsGarden.CellSize.Height - (int)bomb.Height) / 2
            );

            //Draw image
            graphics.DrawImage(bomb, locationF);

            bomb.Dispose();
        }
        private void DrawText(string text, Color color, int x, int y) {
            //Get size
            SizeF sizeF = graphics.MeasureString(text, SettingsGarden.NumberedFont);

            //Calculate location
            PointF locationF = new PointF(
                CellStartX(x) + (SettingsGarden.CellSize.Width - (int)sizeF.Width) / 2,
                CellStartY(y) + (SettingsGarden.CellSize.Height - (int)sizeF.Height)
            );

            //Draw
            graphics.DrawString(text, SettingsGarden.NumberedFont, new SolidBrush(color), locationF);
        }
        private void DrawRectangle(Color color, int x, int y) {
            PointF location = new PointF(CellStartX(x), CellStartY(y));
            SizeF size = new SizeF(SettingsGarden.CellSize.Width, SettingsGarden.CellSize.Height);

            graphics.FillRectangle(new SolidBrush(color), new RectangleF(location, size));
        }

        //Outlines
        private void DrawOutlines(int x, int y) {
            List<int[]> unopenedCellsNearby = GetSpecificCellsNearby(x, y, false);

            for (int i = 0; i< unopenedCellsNearby.Count; i++) {
                Point[] points = GetPointsForOutline(x, y, unopenedCellsNearby[i][2]);

                DrawOutline(ColorTranslator.FromHtml("#87AF3A"), points[0], points[1]);
            }
        }
        private void DrawOutline(Color color, Point point1, Point point2) {
            graphics.DrawLine(new Pen(color, SettingsGarden.OutlineThickness), point1, point2);
        }

        private void EraseOutlines(int x, int y)
        {
            List<int[]> emptyCellsNearby = GetSpecificCellsNearby(x, y, true);

            for (int i = 0; i < emptyCellsNearby.Count; i++){
                int direction = InverseDirection(emptyCellsNearby[i][2]);

                Point[] points = GetPointsForOutline(emptyCellsNearby[i][0], emptyCellsNearby[i][1], direction);

                //Erase
                DrawOutline(GetSecondColor(SettingsGarden.EmptyCellColor, emptyCellsNearby[i][0], emptyCellsNearby[i][1]), points[0], points[1]);

                //Draw
                DrawOutlines(emptyCellsNearby[i][0], emptyCellsNearby[i][1]);
            }
        }

        private List<int[]> GetSpecificCellsNearby(int x, int y, bool opened) {
            List<int[]> unopenedCellsNearby = new List<int[]>();

            //Top
            if (y > 0){
                if (Grid.GetCell(x, y - 1).Opened == opened){
                    unopenedCellsNearby.Add(new int[] { x, y - 1, 0 });
                }
            }
            //Right
            if (x < Grid.Rows[0].Cells.Count - 1){
                if (Grid.GetCell(x + 1, y).Opened == opened){
                    unopenedCellsNearby.Add(new int[] { x + 1, y, 1 });
                }
            }
            //Top
            if (y < Grid.Rows.Count - 1){
                if (Grid.GetCell(x, y + 1).Opened == opened){
                    unopenedCellsNearby.Add(new int[] { x, y + 1, 2 });
                }
            }
            //Right
            if (x > 0){
                if (Grid.GetCell(x - 1, y).Opened == opened){
                    unopenedCellsNearby.Add(new int[] { x - 1, y, 3 });
                }
            }

            return unopenedCellsNearby;
        }
        private Point[] GetPointsForOutline(int x, int y, int type) {
            Point point1 = new Point();
            Point point2 = new Point();

            if (type == 0) {//Top
                point1 = new Point(SettingsGarden.CellSize.Width * x, SettingsGarden.CellSize.Width * y + 1);
                point2 = new Point(SettingsGarden.CellSize.Width * (x + 1), point1.Y);
            } else if (type == 1) {//Right
                point1 = new Point(SettingsGarden.CellSize.Width * (x + 1) - 1, SettingsGarden.CellSize.Width * y);
                point2 = new Point(point1.X, SettingsGarden.CellSize.Width * (y + 1));
            } else if (type == 2) {//Bottom
                point1 = new Point(SettingsGarden.CellSize.Width * x, SettingsGarden.CellSize.Width * (y + 1) - 1);
                point2 = new Point(SettingsGarden.CellSize.Width * (x + 1), point1.Y);
            } else if (type == 3) {//Right
                point1 = new Point(SettingsGarden.CellSize.Width * x + 1, SettingsGarden.CellSize.Width * y);
                point2 = new Point(point1.X, SettingsGarden.CellSize.Width * (y + 1));
            }

            return new Point[]{point1, point2};
        }

        //Scripts
        public int CellStartX(int x){
            return (x * SettingsGarden.CellSize.Width);
        }
        public int CellStartY(int y){
            return (y * SettingsGarden.CellSize.Height);
        }

        private int InverseDirection(int value)
        {
            int direction = value;

            if (direction == 0){
                direction = 2;
            }else if (direction == 1){
                direction = 3;
            }else if (direction == 2){
                direction = 0;
            }else if (direction == 3){
                direction = 1;
            }

            return direction;
        }
        private Color GetSecondColor(Color[] colors, int x, int y)
        {
            if (x % 2 == 0){
                if (y % 2 == 0){
                    return colors[0];
                }else{
                    return colors[1];
                }
            }else{
                if (y % 2 == 0){
                    return colors[1];
                }else{
                    return colors[0];
                }
            }
        }
    }
}
