using System;
using System.Threading;
using Domain.Enums;
using GameBrain;

namespace GameConsoleUI
{
    public static class BattleShipBoardPlayerNav //Keyboard navigation for game board.
    {
        public static (int x, int y) GetUserChoiceOnBoard(CellState[][] board, CellState[][] boatBoard, string playerName)
        {
            var currentX = 0;
            var currentY = 0;

            var width = board.Length;
            var height = board.Length;

            var savedPreviousX = 0;
            var savedPreviousY = 0;

            var previousState = board[0][0];

            ConsoleKey key;

            bool emptyCheck = false;

            Console.CursorVisible = false;

            do
            {
                Console.Clear();
                board[savedPreviousX][savedPreviousY] = previousState;
                previousState = board[currentX][currentY];

                board[currentX][currentY] = CellState.Player;

                savedPreviousX = currentX;
                savedPreviousY = currentY;

                var sampleTxt = $"{playerName}'s turn.";
                Console.WriteLine(sampleTxt);
                BattleShipUI.DrawBoards(board, boatBoard);

                key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.LeftArrow:
                    {
                        if (currentX > 0)
                            currentX--;
                        break;
                    }
                    case ConsoleKey.RightArrow:
                    {
                        if (currentX < width - 1)
                            currentX++;
                        break;
                    }
                    case ConsoleKey.UpArrow:
                    {
                        if (currentY > 0)
                            currentY--;
                        break;
                    }
                    case ConsoleKey.DownArrow:
                    {
                        if (currentY < height - 1)
                            currentY++;
                        break;
                    }
                }
                if (key == ConsoleKey.Enter)
                {
                    if (previousState == CellState.Empty)
                    {
                        emptyCheck = true;
                    }
                    else
                    {
                        Console.WriteLine("Choose an empty spot!");
                        Thread.Sleep(1000);
                    }
                }
                
            } while (key != ConsoleKey.Enter || !emptyCheck);

            board[currentX][currentY] = CellState.Empty;
            Console.CursorVisible = true;

            return (currentX, currentY);
        }
    }
}