using System.Collections.Generic;
using Domain;
using Domain.Enums;

namespace GameBrain
{
    public static class BoardOperations
    {
        public static void AutoFillCorners(int x, int y, Player current, Player opponent) //Fills the corners that can't be a hit.
        {
            var width = opponent.Board.Width;
            var height = opponent.Board.Height;
            if (x - 1 >= 0 && y - 1 >= 0 && x - 1 < width && y - 1 < height)
            {
                opponent.Board.MakeABoardMove(x - 1, y - 1, false, CellState.Miss);
                current.Board.MakeABoardMove(x - 1, y - 1, true, CellState.Miss);
            }

            if (x - 1 >= 0 && y + 1 >= 0 && x - 1 < width && y + 1 < height)
            {
                opponent.Board.MakeABoardMove(x - 1, y + 1, false, CellState.Miss);
                current.Board.MakeABoardMove(x - 1, y + 1, true, CellState.Miss);
            }

            if (x + 1 >= 0 && y - 1 >= 0 && x + 1 < width && y - 1 < height)
            {
                opponent.Board.MakeABoardMove(x + 1, y - 1, false, CellState.Miss);
                current.Board.MakeABoardMove(x + 1, y - 1, true, CellState.Miss);
            }

            if (x + 1 >= 0 && y + 1 >= 0 && x + 1 < width && y + 1 < height)
            {
                opponent.Board.MakeABoardMove(x + 1, y + 1, false, CellState.Miss);
                current.Board.MakeABoardMove(x + 1, y + 1, true, CellState.Miss);
            }
        }


        public static void UpdateSunkenShips(Player current, Player opponent, EShipsCanTouch shipsCanTouch) //Update current player's front board and enemy player's
        {                                                                                                   //backboard if the ship is sunken.
            var width = current.Board.Width;
            var height = current.Board.Height;

            foreach (var ship in opponent.Board.Ships)
            {
                if (ship.HealthPoints == 0)
                {
                    foreach (var c in ship.Coordinates)
                    {
                        current.Board.MakeABoardMove(c[0], c[1], true, CellState.Sunk);
                        opponent.Board.MakeABoardMove(c[0], c[1], false, CellState.Sunk);
                        if (shipsCanTouch == EShipsCanTouch.No) //Declare surrounding positions a miss.
                        {
                            for (int x = -1; x < 2; x++)
                            {
                                for (int y = -1; y < 2; y++)
                                {
                                    if (c[0] + x < width && c[1] + y < height && c[0] + x >= 0 && c[1] + y >= 0 &&
                                        opponent.Board.GetBoardStatus(c[0] + x, c[1] + y, false) == CellState.Empty)
                                    {
                                        opponent.Board.MakeABoardMove(c[0] + x, c[1] + y, false, CellState.Miss);
                                        current.Board.MakeABoardMove(c[0] + x, c[1] + y, true, CellState.Miss);
                                    }
                                }
                            }
                        }

                        if (shipsCanTouch == EShipsCanTouch.Corner) //Leave corners out.
                        {
                            if (c[0] - 1 >= 0 &&
                                opponent.Board.GetBoardStatus(c[0] - 1, c[1], false) == CellState.Empty) //left
                            {
                                opponent.Board.MakeABoardMove(c[0] - 1, c[1], false, CellState.Miss);
                                current.Board.MakeABoardMove(c[0] - 1, c[1], true, CellState.Miss);
                            }

                            if (c[0] + 1 < width &&
                                opponent.Board.GetBoardStatus(c[0] + 1, c[1], false) == CellState.Empty) //right
                            {
                                opponent.Board.MakeABoardMove(c[0] + 1, c[1], false, CellState.Miss);
                                current.Board.MakeABoardMove(c[0] + 1, c[1], true, CellState.Miss);
                                
                            }

                            if (c[1] - 1 >= 0 &&
                                opponent.Board.GetBoardStatus(c[0], c[1] - 1, false) == CellState.Empty) //above
                            {
                                opponent.Board.MakeABoardMove(c[0], c[1] - 1, false, CellState.Miss);
                                current.Board.MakeABoardMove(c[0], c[1] - 1, true, CellState.Miss);
                            }

                            if (c[1] + 1 < height &&
                                opponent.Board.GetBoardStatus(c[0], c[1] + 1, false) == CellState.Empty) //below
                            {
                                opponent.Board.MakeABoardMove(c[0], c[1] + 1, false, CellState.Miss);
                                current.Board.MakeABoardMove(c[0], c[1] + 1, true, CellState.Miss);
                            }
                        }
                    }
                }
            }
        }
        
    }
}