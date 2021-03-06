using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Domain;
using Domain.Enums;

namespace GameConsoleUI
{
    public static class BattleShipUI 
    {
        public static void DrawBoards(CellState[][] moveBoard, CellState[][] shipBoard) //Prints the current player's moveboard and shipboard to console.
        {
            var width = moveBoard.Length; //x
            var height = moveBoard.Length; //y
            BoardDrawer(width, height, moveBoard);
            Console.WriteLine("Positions struck by the enemy.");
            BoardDrawer(width, height, shipBoard);
        }

        public static void WinDrawer(String wPlayerName, String lPlayerName, CellState[][] wShipBoard, CellState[][] lShipBoard)
        {
            Console.Clear();
            var width = wShipBoard.Length;
            var height = wShipBoard.Length;
            Console.WriteLine($"Player {wPlayerName}'s board");
            BoardDrawer(width, height, wShipBoard);
            Console.WriteLine($"Player {lPlayerName}'s board");
            BoardDrawer(width, height, lShipBoard);
            Console.WriteLine("=====================================");
            Console.WriteLine($"The winner is {wPlayerName}!");
            Console.WriteLine("=====================================");
            Console.WriteLine("Press any key to return to Main Menu");

            Console.ReadKey();

        }

        public static void BoardDrawer(int width, int height, CellState[][] board) //Draws the given board to console.
        {
            string colHeader = "";
            char colChar = 'A';
            for (int colIndex = 0; colIndex < width + 1; colIndex++)
            {
                if (width > 26)
                {
                    if (colIndex < 10)
                    {
                        colHeader = "0" + colIndex;
                    }
                    else
                    {
                        colHeader = colIndex.ToString();
                    }
                }
                else
                {
                    colHeader = "=" + colChar;
                }
                
                if (colIndex == 0)
                {
                    Console.Write("====");
                }
                else if (colIndex == width)
                {
                    Console.Write($"{colHeader}===");
                    colChar++;
                }
                else
                {
                    Console.Write($"{colHeader}==");
                    colChar++;
                }
            }
            Console.WriteLine();
            for (int rowIndex = 0; rowIndex < height; rowIndex++)
            {
                string rowInt = "" + (rowIndex + 1);
                if (rowIndex < 9)
                {
                    rowInt = "0" + (rowIndex + 1);
                }
                for (int colIndex = 0; colIndex < width; colIndex++)
                {
                    if (colIndex == 0)
                    {
                        Console.Write($"{rowInt}|| {CellString(board[colIndex][rowIndex])} ");
                    } 
                    else if (colIndex == width - 1)
                    {
                        Console.Write($"| {CellString(board[colIndex][rowIndex])} ||");
                    }
                    else
                    {
                        Console.Write($"| {CellString(board[colIndex][rowIndex])} ");    
                    }
                    
                }

                Console.WriteLine();
                for (int colIndex = 0; colIndex < width; colIndex++)
                {
                    if (rowIndex == height - 1)
                    {
                        if (colIndex == 0)
                        {
                            Console.Write("=====");
                        }
                        else if (colIndex == width - 1)
                        {
                            Console.Write("========");
                        }
                        else
                        {
                            Console.Write("====");
                        }
                    }
                    else if(colIndex == 0)
                    {
                        Console.Write("-----");
                    }
                    else if (colIndex == width - 1)
                    {
                        Console.Write("--------");
                    }
                    else
                    {
                        Console.Write("----");
                    }
                }

                Console.WriteLine();
            }
        }

        public static void DrawResult(bool result)
        {
            Console.WriteLine(result ? "IT'S A HIT!" : "IT'S A MISS!");
            Thread.Sleep(2000);
        }
        
        public static string[] GetPlayerNames() //Asks the user for the player names.
        {
            var names = new string[2];
            var nameValue1 = "";
            var nameValue2 = "";
            
            Console.Write("=====> Enter player 1 name:");
            while (String.IsNullOrWhiteSpace(nameValue1) || nameValue1!.Length > 24)
            {
                try
                {
                    nameValue1 = Console.ReadLine();
                    if (String.IsNullOrWhiteSpace(nameValue1)) 
                    {
                        Console.Write("     Incorrect input! Try again:");
                    }

                    if (nameValue1!.Length > 24)
                    {
                        Console.Write("     Maximum name lenght is 24! Try again:");
                    }

                    if (String.IsNullOrWhiteSpace(nameValue1) || nameValue1.Length > 24) continue;
                    nameValue2 = nameValue1;
                    Console.Write("=====> Enter player 2 name:");
                    while (String.IsNullOrWhiteSpace(nameValue2) || nameValue2.Equals(nameValue1) || nameValue2.Length > 24)
                    {
                        nameValue2 = Console.ReadLine();
                        if (String.IsNullOrWhiteSpace(nameValue2))
                        {
                            Console.Write("     Incorrect input! Try again:");
                        }
                        if (nameValue2!.Equals(nameValue1))
                        {
                            Console.Write("     Use a different name!:");
                        }

                        if (nameValue2.Length > 24)
                        {
                            Console.Write("     Maximum name lenght is 24! Try again:");
                        }
                        
                    }
                }
                catch (Exception)
                {
                    Console.Write("     Incorrect input! Try again:");
                } 
            }
            names[0] = nameValue1;
            names[1] = nameValue2!;
            return names;
        }
        
        public static (int x, int y) GetBoardSize() //Asks the user for the board size.
        {
            var x = 10;
            var y = 10;
            var correctFormat = false;
            Console.WriteLine("Warning! Bigger boards might not display correctly on smaller screens.");
            Console.Write("=====> Give game board width(10-40), height(10-40):");
            while (!correctFormat)
            {
                try
                {
                    var userValue = Console.ReadLine()!.Split(',');
                    x = int.Parse(userValue[0].Trim());
                    y = int.Parse(userValue[1].Trim());
                    if (x > 40 || y > 40 || x < 10|| y < 10 || userValue.Length > 2)
                    {
                        throw new FormatException();
                    }
                    correctFormat = true;
                }
                catch (Exception)
                {
                    Console.Write("     Incorrect input! Try again:");
                }
            }

            return (x, y);
        }

        public static string CellString(CellState cellState) //Cellstate symbols on console board.
        {
            switch (cellState)
            {
                case CellState.Empty: return " ";
                case CellState.Miss: return "*";
                case CellState.Hit: return "X";
                case CellState.Ship: return "S";
                case CellState.Sunk: return "#";
            }

            return "P";
        }
        
        public static Enum[] GetGameOptions() //When creating a new game, asks the user to enter the preferred game options.
        {
            var options = new Enum[2];
            var userchoice1 = 0;
            var userchoice2 = 0;
            Console.WriteLine("Placement options:");
            Console.WriteLine("     1) Boats can't touch each other.");
            Console.WriteLine("     2) Boats can touch by corners.");
            Console.WriteLine("     3) Boats can touch each other.");
            Console.Write("=====> Choose option by number (1-3):");
            while (userchoice1 < 1 || userchoice1 > 3)
            {
                try
                {
                    userchoice1 = int.Parse(Console.ReadLine()!);
                    if (userchoice1 > 3 || userchoice1 < 1)
                    {
                        Console.Write("     Incorrect input! Try again:");
                    }
                } catch (FormatException) { Console.Write("     Incorrect input! Try again:"); } 
            }
            Console.WriteLine("Move turn options:");
            Console.WriteLine("     1) Same player moves again after a hit.");
            Console.WriteLine("     2) Next player moves after a hit.");
            Console.Write("=====> Choose option by number (1-2):");
            while (userchoice2 < 1 || userchoice2 > 2)
            {
                try
                {
                    userchoice2 = int.Parse(Console.ReadLine()!);
                    if (userchoice2 > 2 || userchoice2 < 1)
                    {
                        Console.Write("     Incorrect input! Try again:");
                    }
                } catch (FormatException) { Console.Write("     Incorrect input! Try again:"); } 
            }
            
            options[0] = userchoice1 switch
            {
                1 => EShipsCanTouch.No,
                2 => EShipsCanTouch.Corner,
                3 => EShipsCanTouch.Yes,
                _ => options[0]
            };
            options[1] = userchoice2 switch
            {
                1 => ENextMoveAfterHit.SamePlayer,
                2 => ENextMoveAfterHit.OtherPlayer,
                _ => options[1]
            };
            return options;
        }

        public static Dictionary<(string name, int size), int> GetCustomShipPermission((int x, int y) boardDimensions, EShipsCanTouch option)
        {
            var shipData = new List<(string name, int size)>();
            string userChoice = GetSingleQuestionChoice("=====> Would you like to use custom ships? (y/n):");
            if (userChoice.ToLower() == "y")
            {
                shipData.Add(GetCustomShipData(boardDimensions));
                string userChoice2 = "";
                while (userChoice2.ToLower() != "n")
                {
                    userChoice2 = GetSingleQuestionChoice("=====> Would you like to add more custom ships? (y/n):");
                    if (userChoice2.ToLower() == "y")
                    {
                        shipData.Add(GetCustomShipData(boardDimensions));
                    }
                }
                
                return GetShipAmount(shipData, boardDimensions, option);
            }
            

            return new Dictionary<(string name, int size), int>();
        }

        private static (string name, int size) GetCustomShipData((int x, int y) boardDimensions)
        {
            (string name, int size) shipUnits = ("", 0);

            Console.Write("     Enter ship name:");
            while (String.IsNullOrWhiteSpace(shipUnits.name!))
            {
                try
                {
                    shipUnits.name = Console.ReadLine()!;
                    if (String.IsNullOrWhiteSpace(shipUnits.name!)) 
                    {
                        Console.Write("         Incorrect input! Try again:");
                    }

                    if (String.IsNullOrWhiteSpace(shipUnits.name!)) continue;
                    
                    Console.Write("     Enter ship size:");
                    while (shipUnits.size < 1 || shipUnits.size > boardDimensions.x || shipUnits.size > boardDimensions.y)
                    {
                        try
                        {
                            shipUnits.size = int.Parse(Console.ReadLine()!);
                            if (shipUnits.size > boardDimensions.x || shipUnits.size > boardDimensions.y || shipUnits.size < 1)
                            {
                                Console.Write("         Incorrect input! Try again:");
                            }
                        } catch (FormatException) { Console.Write("         Incorrect input! Try again:"); } 
                    }
                }
                catch (Exception)
                {
                    Console.Write("         Incorrect input! Try again:");
                } 
            }
            
            return shipUnits;
        }

        private static Dictionary<(string name, int size), int> GetShipAmount(List<(string name, int size)> shipList,
                                                                    (int x, int y) boardDimensions,
                                                                    EShipsCanTouch option)
        {
            Dictionary<(string name, int size), int> shipData = new ();
            var correctFormat = false;
            foreach (var ship in shipList)
            {
                shipData.Add(ship, 1);
            }
            
            
            string userChoice = GetSingleQuestionChoice("=====> Would you like to change the amount of each ship? (y/n):");
            if (userChoice == "y")
            {
                Console.WriteLine("     Warning! Choosing above the recommended amount might cause");
                Console.WriteLine("     the ships to not fit on the board.");
                int shipAmount = 1;
                foreach (var ship in shipList)
                {
                    double boardSize = (boardDimensions.x * boardDimensions.y) * 0.25;
                    int recAmount = option switch
                    {
                        EShipsCanTouch.Yes => (int) boardSize / ship.size,
                        EShipsCanTouch.Corner => (int) boardSize / (ship.size * 2),
                        EShipsCanTouch.No => (int) boardSize / (ship.size * 3),
                        _ => 0
                    };
                    recAmount = recAmount >= 1 ? recAmount : 1;
                    Console.Write($"    Enter the amount of {ship.name} ships(Recommended <= {recAmount}):");
                    while (!correctFormat)
                    {
                        try
                        {
                            shipAmount = int.Parse(Console.ReadLine()!);
                            correctFormat = true;
                        }
                        catch (Exception)
                        {
                            Console.Write("         Incorrect input! Try again:");

                        }
                    }
                    
                    shipData[ship] = shipAmount;
                    correctFormat = false;
                }
                
            }

            return shipData;
        }

        private static string GetSingleQuestionChoice(string message)
        {
            string userChoice = "";
            
            Console.Write(message);
            while (userChoice != "y" && userChoice != "n")
            {
                userChoice = Console.ReadLine()!;
                if (userChoice != "y" && userChoice != "n")
                {
                    Console.Write("     Incorrect input! Try again(y/n):");
                }
            }

            return userChoice;
        }

        public static string GetSaveFileName()
        {
            var fileName = "";

            Console.Write("=====> Enter save file name:");
            while (String.IsNullOrWhiteSpace(fileName) || fileName.Length > 24)
            {
                try
                {
                    fileName = Console.ReadLine();
                    if (String.IsNullOrWhiteSpace(fileName))
                    {
                        Console.Write("     Incorrect input! Try again:");
                    }
                    if (fileName!.Length > 24)
                    {
                        Console.Write("     Maximum name lenght is 24! Try again:");
                    }
                }
                catch (Exception)
                {
                    Console.Write("     Incorrect input! Try again:");
                }
                        
            }
            Console.WriteLine("Saving the game...");
            return fileName;
        }

        public static int GetSaveFileChoice(List<string> savedGames)
        {
            var fileNo = 0;
            if (savedGames.Count > 0)
            {
                for (int i = 0; i < savedGames.Count; i++)
                {
                    Console.WriteLine($"{i + 1} - {savedGames[i]}");
                }
                
                Console.Write("=====> Choose a save file by the number:");
                while (fileNo < 1 || fileNo > savedGames.Count)
                {
                    try
                    {
                        var userValue = Console.ReadLine()!;
                        fileNo = int.Parse(userValue);
                        if (fileNo > savedGames.Count || fileNo <= 0)
                        {
                            throw new FormatException();
                        }
                    }
                    catch (Exception)
                    {
                        Console.Write("     Incorrect input! Try again:");
                    }
                }
            }
            else
            {
                Console.WriteLine("No saved Game Files found.");
                Thread.Sleep(2000);
                return -1;
            }
            Console.Write("Loading...");

            return fileNo - 1; //-1 because 0 index.
           
        }

        public static void DrawTutorial()
        {
            Console.Clear();
            Console.WriteLine("=============> BATTLESHIP <======================================================");
            Console.Write("");
            string[] lines = File.ReadAllLines(Path.Combine(Environment.CurrentDirectory, "Tutorial.txt"));
            foreach (var line in lines)
            {
                Console.WriteLine(line);
            }
            Console.WriteLine("=================================================================================");
            Console.WriteLine("Press any key to return to Main Menu");
            Console.ReadKey();
        }

    }
}