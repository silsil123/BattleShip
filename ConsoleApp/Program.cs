using System;
using System.Linq;
using System.Threading;
using DAL;
using Domain;
using GameBrain;
using GameConsoleUI;
using MenuSystem;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ConsoleApp
{
    class Program
    {
        private static bool _loadCheck; //Check for BattleShip() if a game is created or loaded.
        
        static void Main(string[] args)
        {
            var mainMenu = new Menu(MenuLevel.Level0);
            mainMenu.AddMenuItem(new MenuItem("New game: Human vs Human", "1", BattleShip));
            mainMenu.AddMenuItem(new MenuItem("New game: Person vs AI", "2", DefaultMenuAction));
            mainMenu.AddMenuItem(new MenuItem("New game: AI vs AI", "3", DefaultMenuAction));
            mainMenu.AddMenuItem(new MenuItem("Load Game", "L", LoadMenuAction));
            mainMenu.AddMenuItem(new MenuItem("Tutorial", "T", TutorialMenuAction));

            mainMenu.RunMenu();
        }
        
        static string DefaultMenuAction()
        {
            Menu.ErrorMessage = "Not implemented yet!";
            return "";
        }

        static string LoadMenuAction()
        {
            _loadCheck = true;
            BattleShip();
            return "";
        }
        
        static string TutorialMenuAction()
        {
            BattleShipUI.DrawTutorial();
            return "";
        }

        static string BattleShip()
        {

            var game = new BattleShip();
            var loadSuccess = false;

            if (_loadCheck)
            {
                loadSuccess = LoadGameAction(game);
                if (!loadSuccess)
                {
                    _loadCheck = false;
                    return "";
                }
            }

            if (!loadSuccess)
            {
                game.SetPlayerNames(BattleShipUI.GetPlayerNames());
                game.InitializeBoards(BattleShipUI.GetBoardSize());
                game.SetGameOptions(BattleShipUI.GetGameOptions());
                var customShipData =
                    BattleShipUI.GetCustomShipPermission(game.GetBoardDimensions(), game.ShipsCanTouch);
                if (customShipData.Count != 0)
                {
                    game.AddCustomShipTemplate(customShipData);
                }

                String fPlayerName = game.GetPlayerName(true);
                String sPlayerName = game.GetPlayerName(false);

                var boatMenu = new Menu(MenuLevel.Level0AddOn);
                boatMenu.AddMenuItem(new MenuItem($"{fPlayerName} add boats", "A", () =>
                {

                    var shipData = BattleShipBoardBoatNav.PlaceBoatsOnBoard(game.GetBoardDimensions(),
                        game.GetShipTemplate(true),
                        fPlayerName,
                        game.ShipsCanTouch);
                    game.SetPlayerShips(shipData, true);
                    return "";
                }));

                boatMenu.AddMenuItem(new MenuItem($"{sPlayerName} add boats", "B", () =>
                {
                    var shipData = BattleShipBoardBoatNav.PlaceBoatsOnBoard(game.GetBoardDimensions(),
                        game.GetShipTemplate(false),
                        sPlayerName,
                        game.ShipsCanTouch);
                    game.SetPlayerShips(shipData, false);
                    return "";
                }));

                boatMenu.AddMenuItem(new MenuItem($"{fPlayerName} ships placed randomly.", "C", () =>
                {
                    game.GenerateRandomShips(true);
                    return "";
                }));

                boatMenu.AddMenuItem(new MenuItem($"{sPlayerName} ships placed randomly.", "D", () =>
                {
                    game.GenerateRandomShips(false);
                    return "";
                }));

                boatMenu.RunMenu(); //Add ships to board when starting a new game.
            }

            var menu = new Menu(MenuLevel.Level1);
            menu.AddMenuItem(new MenuItem($"Next move","P", () =>
            {
                
                var (x, y) = BattleShipBoardPlayerNav.GetUserChoiceOnBoard(
                    game.GetCurrentPlayerMoveBoard(),
                        game.GetCurrentPlayerShipBoard(),
                        game.GetCurrentPlayerName());
                BattleShipUI.DrawResult(game.MakeAMove(x, y));
                if (game.CheckWin())
                {
                    BattleShipUI.WinDrawer(game.GetCurrentPlayerName(),
                                            game.GetOpponentPlayerName(),
                                            game.GetCurrentPlayerShipBoard(),
                                            game.GetOtherPlayerShipBoard());
                    return "M";
                }
                return "";
            }));
            menu.AddMenuItem(new MenuItem("Undo a move","U", () =>
            {
                game.LoadLastState();
                return "";
            }));
            menu.AddMenuItem(new MenuItem("Save Game","S",
                () => SaveGameAction(game)));
            
            var userChoice = menu.RunMenu();

            return userChoice;
        }
        
        static string SaveGameAction(BattleShip game)
        {
            game.CreateSaveGame(BattleShipUI.GetSaveFileName());
            return "";
        }

        static bool LoadGameAction(BattleShip game)
        {
            var fileNo = BattleShipUI.GetSaveFileChoice(game.GetSaveFilesList());
            if (fileNo == -1) return false;
            game.LoadStateFromDbGame(fileNo, null);
            return true;

        }
    }
}