using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Threading;
using MinesweeperModel;
using System.Drawing;

namespace Minesweeper
{
    class RunGraphics
    {
        private static BackgroundWorker backgroundWorker = new BackgroundWorker();
        private static Base BaseForm;

        private static Grid tempGrid = new Grid();
        private static Grid prevGrid = new Grid();

        public static void InitializeBackgroundWorker() {
            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;

            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true; //Allow for the process to be cancelled
        }
        public static void StartRunningGraphics(Base baseForm) {
            BaseForm = baseForm;

            tempGrid = new Grid();
            prevGrid = new Grid();

            backgroundWorker.RunWorkerAsync();
        }

        //Work
        private static void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            BaseForm.pictureBox1.Image = Base.Game.Render.Image;
        }
        private static void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            bool canRun = true;

            while (canRun){
                Thread.Sleep(5);

                tempGrid = Base.Game.Grid.Clone();
                List<Point> cellsToUpdate = GetCellsToUpdate(tempGrid, prevGrid);

                if (cellsToUpdate.Count > 0){
                    Base.Game.Render.UpdateCells(cellsToUpdate);

                    prevGrid = tempGrid.Clone();

                    backgroundWorker.ReportProgress(1);
                }

                if (backgroundWorker.CancellationPending) {
                    e.Cancel = true;

                    canRun = false;
                }
            }
        }

        private static List<Point> GetCellsToUpdate(Grid grid, Grid prevGrid) {
            List<Point> cells = new List<Point>();

            //Compare row count
            if (grid.Rows.Count != prevGrid.Rows.Count)
            {
                cells = GetAllCells(grid);
            }else {
                for (int y = 0; y < grid.Rows.Count; y++) {
                    if (grid.Rows[y].Cells.Count != prevGrid.Rows[y].Cells.Count){
                        cells.Clear();
                        cells = GetAllCells(grid);

                        y = grid.Rows.Count;

                        goto after_loop;
                    }
                    else {
                        for (int x = 0; x < grid.Rows[y].Cells.Count; x++) {
                            Cell cell1 = grid.GetCell(x, y);
                            Cell cell2 = prevGrid.GetCell(x, y);

                            bool cellsAreEqual = CellsAreEqual(cell1, cell2);

                            if (!cellsAreEqual) {
                                cells.Add(new Point(x, y));
                            }
                        }
                    }
                }
            }
            after_loop:

            return cells;
        }

        private static List<Point> GetAllCells(Grid grid) {
            List<Point> cells = new List<Point>();

            for (int y = 0; y < grid.Rows.Count; y++) {
                for (int x = 0; x < grid.Rows[y].Cells.Count; x++) {
                    cells.Add(new Point(x, y));
                }
            }

            return cells;
        }
        private static bool CellsAreEqual(Cell cell1, Cell cell2) {
            if (cell1.Flagged != cell2.Flagged){
                return false;
            }
            if (cell1.Number != cell2.Number){
                return false;
            }
            if (cell1.Opened != cell2.Opened){
                return false;
            }
            if (cell1.Type != cell2.Type){
                return false;
            }

            return true;
        }
    }
}
