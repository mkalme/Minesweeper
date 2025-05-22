using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinesweeperModel;
using System.Drawing;

namespace MinesweeperMechanics
{
    public class GenerateGrid
    {
        public static Grid Generate(Grid grid, Point coordinates) {
            grid = GenerateBombs(grid, coordinates);
            grid = GenerateNumbers(grid);

            return grid;
        }
        public static Grid GenerateEmptyGrid(Size size){
            Grid grid = new Grid();

            for (int y = 0; y < size.Height; y++){
                Row row = new Row();

                for (int x = 0; x < size.Width; x++){
                    row.Cells.Add(new Cell());
                }

                grid.Rows.Add(row);
            }

            return grid;
        }

        private static Grid GenerateBombs(Grid grid, Point coordinates) {
            Random rand = new Random();

            for (int i = 0; i < MechanicsSettings.NumberOfBombs;) {
                int x = rand.Next(grid.Rows[0].Cells.Count);
                int y = rand.Next(grid.Rows.Count);

                if (BombEligible(grid, coordinates, x, y)) {
                    grid.GetCell(x, y).Type = CellType.Bomb;

                    i++;
                }
            }

            return grid;
        }
        private static Grid GenerateNumbers(Grid grid) {
            for (int y = 0; y < grid.Rows.Count; y++) {
                for (int x = 0; x < grid.Rows[y].Cells.Count; x++) {
                    Cell cell = grid.GetCell(x, y);

                    if (cell.Type == CellType.Empty) {
                        int numberOfBombs = GetNumberOfBombsNearby(grid, x, y);

                        if (numberOfBombs > 0) {
                            cell.Type = CellType.Numbered;
                            cell.Number = numberOfBombs;
                        }
                    }
                }
            }

            return grid;
        }

        //Bomb
        private static bool BombEligible(Grid grid, Point coordinates, int x, int y) {
            Cell cell = grid.GetCell(x, y);

            if (cell.Type != CellType.Empty) {
                return false;
            }
            if (x == coordinates.X && y == coordinates.Y) {
                return false;
            }
            if (SpawnPointNearby(grid, coordinates, x, y)) {
                return false;
            }

            return true;
        }

        private static bool SpawnPointNearby(Grid grid, Point coordinates, int x, int y) {
            int spawnPointDetections = 0;

            //Top
            if (y > 0){
                spawnPointDetections += IfSpawnPoint(coordinates, x, y - 1);
            }

            //Top-Right
            if (x < grid.Rows[y].Cells.Count - 1 && y > 0){
                spawnPointDetections += IfSpawnPoint(coordinates, x + 1, y - 1);
            }

            //Right
            if (x < grid.Rows[y].Cells.Count - 1){
                spawnPointDetections += IfSpawnPoint(coordinates, x + 1, y);
            }

            //Bottom-Right
            if (x < grid.Rows[y].Cells.Count - 1 && y < grid.Rows.Count - 1){
                spawnPointDetections += IfSpawnPoint(coordinates, x + 1, y + 1);
            }

            //Bottom
            if (y < grid.Rows.Count - 1){
                spawnPointDetections += IfSpawnPoint(coordinates, x, y + 1);
            }

            //Bottom-Left
            if (x > 0 && y < grid.Rows.Count - 1){
                spawnPointDetections += IfSpawnPoint(coordinates, x - 1, y + 1);
            }

            //Left
            if (x > 0){
                spawnPointDetections += IfSpawnPoint(coordinates, x - 1, y);
            }

            //Top-Left
            if (x > 0 && y > 0){
                spawnPointDetections += IfSpawnPoint(coordinates, x - 1, y - 1);
            }

            return spawnPointDetections > 0 ? true : false;
        }
        private static int IfSpawnPoint(Point coordinates, int x, int y) {
            if (coordinates.X == x && coordinates.Y == y){
                return 1;
            }

            return 0;
        }

        //Numbers
        private static int GetNumberOfBombsNearby(Grid grid, int x, int y) {
            int numberOfBombs = 0;

            //Top
            if (y > 0) {
                numberOfBombs += AddIfBomb(grid, x, y - 1);
            }

            //Top-Right
            if (x < grid.Rows[y].Cells.Count - 1 && y > 0) {
                numberOfBombs += AddIfBomb(grid, x + 1, y - 1);
            }

            //Right
            if (x < grid.Rows[y].Cells.Count - 1) {
                numberOfBombs += AddIfBomb(grid, x + 1, y);
            }

            //Bottom-Right
            if (x < grid.Rows[y].Cells.Count - 1 && y < grid.Rows.Count - 1){
                numberOfBombs += AddIfBomb(grid, x + 1, y + 1);
            }

            //Bottom
            if (y < grid.Rows.Count - 1){
                numberOfBombs += AddIfBomb(grid, x, y + 1);
            }

            //Bottom-Left
            if (x > 0 && y < grid.Rows.Count - 1){
                numberOfBombs += AddIfBomb(grid, x - 1, y + 1);
            }

            //Left
            if (x > 0){
                numberOfBombs += AddIfBomb(grid, x - 1, y);
            }

            //Top-Left
            if (x > 0 && y > 0){
                numberOfBombs += AddIfBomb(grid, x - 1, y - 1);
            }

            return numberOfBombs;
        }
        private static int AddIfBomb(Grid grid, int x, int y) {
            int numberOfBombs = 0;

            if (grid.GetCell(x, y).Type == CellType.Bomb){
                numberOfBombs++;
            }

            return numberOfBombs;
        }
    }
}
