using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Domain.Enums;

namespace Domain
{
    public partial class Board
    {
        public void CreateBoard(int x, int y)
        {
            Width = x;
            Height = y;
            MoveBoard = new CellState[x][];
            for (int i = 0; i < x; i++)
            {
                MoveBoard[i] = new CellState[y];
            }
            ShipBoard = new CellState[x][];
            for (int i = 0; i < x; i++)
            {
                ShipBoard[i] = new CellState[y];
            }
            
        }
        
        public CellState CheckMove(int x, int y)
        {
            if (ShipBoard[x][y] == CellState.Ship)
            {
                foreach (var ship in Ships)
                {
                    foreach (var coord in ship.Coordinates)
                    {
                        if (x != coord[0] || y != coord[1]) continue;
                        ship.HealthPoints -= 1;
                        if (ship.HealthPoints != 0) return CellState.Hit;
                        foreach (var _ in ship.Coordinates)
                        {
                            ShipBoard[coord[0]][coord[1]] = CellState.Sunk;
                        }
                        return CellState.Hit;
                    }
                }
            }

            return CellState.Miss;
        }

        public void MakeABoardMove(int x, int y, bool moveBoard, CellState result)
        {
            if (moveBoard)
            {
                MoveBoard[x][y] = result;
            }
            else
            {
                ShipBoard[x][y] = result;
            }
        }

        public CellState GetBoardStatus(int x, int y, bool moveBoard)
        {
            if (moveBoard)
            {
                return MoveBoard[x][y];
            }
            else
            {
                return ShipBoard[x][y];
            }
        }

        public CellState[][] GetMoveBoard()
        {
            var res = InitializeJaggedArray(Width, Height);
            Array.Copy(MoveBoard,
                res,
                MoveBoard.Length);
            return res;
        }
        
        public CellState[][] GetShipBoard() //Returns copy of current player's ship board.
        {
            var res = InitializeJaggedArray(Width, Height);
            Array.Copy(ShipBoard,
                res,
                ShipBoard.Length);
            return res;
        }
        
        private CellState[][] InitializeJaggedArray(int x, int y) //Initializes empty CellState jagged array.
        {
            var jaggedArray = new CellState[x][];
            for (int i = 0; i < x; i++)
            {
                jaggedArray[i] = new CellState[y];
            }

            return jaggedArray;
        }
        
        public void SetShipsOnBackBoard()
        {
            CreateBoard(Width, Height); //Reset board to avoid double placement.
            foreach (var ship in Ships)
            {
                foreach (var coord in ship.Coordinates)
                {
                    ShipBoard[coord[0]][coord[1]] = CellState.Ship;
                }
            }
        }

        public void CreateNewBoardState()
        {
            var boardState = new BoardState()
            {
                BoardHealth = BoardHealth,
                MoveBoardJsonString = System.Text.Json.JsonSerializer.Serialize(MoveBoard),
                ShipBoardJsonString = System.Text.Json.JsonSerializer.Serialize(ShipBoard),
                ShipListJsonString = System.Text.Json.JsonSerializer.Serialize(Ships)
            };
            BoardStates.Add(boardState);
        }

        public void SetLoadDataFromJson()
        {
            if (BoardStates.Count == 0)
            {
                CreateBoard(Width, Height);
            }
            else
            {
                var boardState = BoardStates.Last();
                var resFront =
                    System.Text.Json.JsonSerializer.Deserialize<CellState[][]>(boardState.MoveBoardJsonString);
                var resBack =
                    System.Text.Json.JsonSerializer.Deserialize<CellState[][]>(boardState.ShipBoardJsonString);
                var resShip =
                    System.Text.Json.JsonSerializer.Deserialize<ICollection<Ship>>(boardState.ShipListJsonString);
                if (resFront != null)
                {
                    MoveBoard = resFront;
                }

                if (resBack != null)
                {
                    ShipBoard = resBack;
                }

                if (resShip != null)
                {
                    Ships = resShip;
                }
            }
        }
    }
    
}