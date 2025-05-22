using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinesweeperGraphics;
using MinesweeperModel;

namespace Minesweeper
{
    public class Game
    {
        public Grid Grid { get; set; }
        public Render Render { get; set; }
        public bool Generated { get; set; }
        public bool GameInSession { get; set; }

        public Game(Grid grid, Render render, bool generated, bool gameInSession) {
            this.Grid = grid;
            this.Render = render;
            this.Generated = generated;
            this.GameInSession = gameInSession;
        }
    }
}
