using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using Domain;
using Domain.Enums;
using GameBrain;

namespace GameConsoleUI
{
    public static class BattleShipBoardBoatNav
    {
        public static List<Ship> PlaceBoatsOnBoard((int x, int y) dimensions, List<Ship> ships,
            String playerName, EShipsCanTouch option)
        {
            int shipCount = 0;
            foreach (Ship ship in ships)
            {
                shipCount++;
                bool roomCheck = false;

                ConsoleKey key;

                Console.CursorVisible = false;

                do
                {
                    Console.Clear();

                    var board = new CellState[dimensions.x][];
                    for (int i = 0; i < dimensions.x; i++)
                    {
                        board[i] = new CellState[dimensions.y];
                    }

                    for (int i = 0; i < shipCount; i++)
                    {
                        Ship b = ships[i];
                        for (int j = 0; j < b.Coordinates.Length; j++)
                        {
                            board[b.Coordinates[j][0]][b.Coordinates[j][1]] = CellState.Ship;
                        }
                    }

                    Console.WriteLine($"Player: {playerName}");
                    Console.WriteLine(
                        $"Use 'arrow' keys to navigate, 'space' to rotate and 'enter' to place the ships.");
                    Console.WriteLine($"Currently placing: {ship.Name}");
                    BattleShipUI.BoardDrawer(dimensions.x, dimensions.y, board);

                    key = Console.ReadKey(true).Key;
                    switch (key)
                    {
                        case ConsoleKey.LeftArrow:
                        {
                            ShipOperations.BoatNavigation("left", ship, dimensions);
                            break;
                        }
                        case ConsoleKey.RightArrow:
                        {
                            ShipOperations.BoatNavigation("right", ship, dimensions);
                            break;
                        }
                        case ConsoleKey.UpArrow:
                        {
                            ShipOperations.BoatNavigation("up", ship, dimensions);
                            break;
                        }
                        case ConsoleKey.DownArrow:
                        {
                            ShipOperations.BoatNavigation("down", ship, dimensions);
                            break;
                        }
                        case ConsoleKey.Spacebar:
                        {
                            ShipOperations.BoatNavigation("rotate", ship, dimensions);
                            break;
                        }
                        case ConsoleKey.Enter:
                        {
                            roomCheck = ShipOperations.CheckCoordinateAvailability(ships, ship, option);
                            if (!roomCheck)
                            {
                                Console.WriteLine($"This spot is occupied by another ship!");
                                Thread.Sleep(1000);
                            }

                            break;
                        }
                    }
                } while (key != ConsoleKey.Enter || !roomCheck);

                Console.CursorVisible = true;
            }

            return ships;
        }
    }
}