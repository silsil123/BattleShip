using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using Domain;
using Domain.Enums;
using static GameBrain.BoardOperations;
using static GameBrain.ShipOperations;

namespace GameBrain
{
    public class BattleShip
    {
        private Player FirstPlayer { get; set; } = null!;

        private Player SecondPlayer { get; set; } = null!;

        private Player CurrentPlayer { get; set; } = null!;

        private int GameId { get; set; }

        private ENextMoveAfterHit _eNextMoveAfterHit;
        public EShipsCanTouch ShipsCanTouch { get; private set; }

        public void SetPlayerShips(List<Ship> ships, bool firstPlayer) //Add configured ships to player's board.
        {
            Player player = firstPlayer ? FirstPlayer : SecondPlayer;
            
            var health = 0;
            foreach (var ship in ships)
            {
                health += ship.HealthPoints;
            }

            player.Board.Ships = ships;
            player.Board.BoardHealth = health;
            player.Board.SetShipsOnBackBoard();
            player.Board.CreateNewBoardState();
            }

        public void InitializeBoards((int x, int y) size) //Create the boards for the players with given dimensions.
        {
            FirstPlayer.Board.CreateBoard(size.x, size.y);
            SecondPlayer.Board.CreateBoard(size.x, size.y);
        }

        public void SetPlayerNames(String[] names)
        {
            FirstPlayer = new Player()
            {
                Name = names[0],
                ShipTemplate = Ship.GetDefaultShips()
            };
            SecondPlayer = new Player()
            {
                Name = names[1],
                ShipTemplate = Ship.GetDefaultShips()
            };

            CurrentPlayer = FirstPlayer;
        }

        public void SetGameOptions(Enum[] gameOptions)
        {
            ShipsCanTouch = (EShipsCanTouch) gameOptions[0];
            _eNextMoveAfterHit = (ENextMoveAfterHit) gameOptions[1];
        }

        public String GetCurrentPlayerName()
        {
            return CurrentPlayer.Name;
        }

        public String GetPlayerName(bool firstPlayer)
        {
            Player player = firstPlayer ? FirstPlayer : SecondPlayer;
            return player.Name;
        }

        public List<Ship> GetShipTemplate(bool first) //Returns ship list with unfilled coordinates.
        {
            var player = first ? FirstPlayer : SecondPlayer;
            return player.ShipTemplate;
        }

        public CellState[][] GetCurrentPlayerMoveBoard() //Returns copy of current player's move board.
        {
            return CurrentPlayer.Board.GetMoveBoard();
        }

        public CellState[][] GetPlayerMoveBoard(bool first) //Returns copy of chosen player's move board.
        {
            return first ? FirstPlayer.Board.GetMoveBoard() : SecondPlayer.Board.GetMoveBoard();
        }
        
        public CellState[][] GetPlayerShipBoard(bool first) //Returns copy of current player's ship board.
        {
            return first ? FirstPlayer.Board.GetShipBoard() : SecondPlayer.Board.GetShipBoard();
        }
        

        public CellState[][] GetCurrentPlayerShipBoard() //Returns copy of current player's ship board.
        {
            return CurrentPlayer.Board.GetShipBoard();
        }

        public CellState[][] GetOtherPlayerShipBoard()
        {
            return OpponentPlayer().Board.GetShipBoard();
        }

        private Player OpponentPlayer()
        {
            return CurrentPlayer.Name == FirstPlayer.Name ? SecondPlayer : FirstPlayer;
        }

        public String GetOpponentPlayerName()
        {
            return CurrentPlayer == FirstPlayer ? SecondPlayer.Name : FirstPlayer.Name;
        }

        public (int x, int y) GetBoardDimensions()
        {
            return (CurrentPlayer.Board.Width, CurrentPlayer.Board.Height);
        }

        public void CreateSaveGame(string? name)
        {
            Game dbGame = DbOperations.GetDbLoadData(null, GameId);
            var gameName = name ?? FirstPlayer.Name + " vs " + SecondPlayer.Name;

            dbGame.GameName = gameName;
            dbGame.FirstPlayer = FirstPlayer;
            dbGame.SecondPlayer = SecondPlayer;
            dbGame.CurrentPlayerFirst = CurrentPlayer == FirstPlayer;
            dbGame.NextMoveAfterHit = _eNextMoveAfterHit;
            dbGame.EShipsCanTouch = ShipsCanTouch;

            DbOperations.SaveDbData(dbGame);
        }

        public void LoadStateFromDbGame(int fileNo, Game? exDbGame)
        {
            var dbGame = exDbGame ?? DbOperations.GetDbLoadData(fileNo, null);

            GameId = dbGame.GameId;
            
            FirstPlayer = dbGame.FirstPlayer;
            SecondPlayer = dbGame.SecondPlayer;
            CurrentPlayer = dbGame.CurrentPlayerFirst ? FirstPlayer : SecondPlayer;
            ShipsCanTouch = dbGame.EShipsCanTouch;
            _eNextMoveAfterHit = dbGame.NextMoveAfterHit;

            FirstPlayer.Board.SetLoadDataFromJson();
            SecondPlayer.Board.SetLoadDataFromJson();
        }

        public List<string> GetSaveFilesList()
        {
            return DbOperations.GetGameSaveFileNameList();
        }

        public bool MakeAMove(int x, int y)
        {
            var result = OpponentPlayer().Board.CheckMove(x, y);

            if (result == CellState.Hit && ShipsCanTouch == EShipsCanTouch.No)
            {
                AutoFillCorners(x, y, CurrentPlayer, OpponentPlayer());
            }
            
            OpponentPlayer().Board.MakeABoardMove(x, y, false, result);
            CurrentPlayer.Board.MakeABoardMove(x, y, true, result);


            UpdateSunkenShips(CurrentPlayer, OpponentPlayer(), ShipsCanTouch);
            
            
            if (result != CellState.Hit)
            {
                CurrentPlayer = OpponentPlayer();
                SaveState();
                return false;
            }

            if (_eNextMoveAfterHit == ENextMoveAfterHit.OtherPlayer)
            {
                CurrentPlayer = OpponentPlayer();
            }

            OpponentPlayer().Board.BoardHealth--;
            SaveState();
            return true;
        }

        public void GenerateRandomShips(bool firstPlayer)
        {
            Player player = firstPlayer ? FirstPlayer : SecondPlayer;
            var width = player.Board.Width - 1;
            var height = player.Board.Height - 1;
            Random rnd = new Random();
            List<Ship> ships = new();
            var area = player.Board.Height * player.Board.Width;
            var shipAreaCount = 0;

            while (shipAreaCount + 96 < area) //Add ships based on the board size.
            {
                ships.AddRange(Ship.GetDefaultShips());
                shipAreaCount += 96;
            }

            foreach (var ship in ships)
            {
                bool coordCheck = false;
                while (!coordCheck)
                {
                    bool orientation = rnd.NextDouble() > 0.5; //If true, ship is horizontal.
                    int newX = orientation ? rnd.Next(0, width - (ship.ShipSize - 1)) : rnd.Next(0, width);
                    int newY = orientation ? rnd.Next(0, height) : rnd.Next(0, height - (ship.ShipSize - 1));

                    for (int i = 0; i < ship.Coordinates.Length; i++)
                    {
                        ship.Coordinates[i][0] = orientation ? newX + i : newX;
                        ship.Coordinates[i][1] = orientation ? newY : newY + i;
                    }

                    ship.IsPlaced = true;
                    coordCheck = CheckCoordinateAvailability(ships, ship, ShipsCanTouch);
                }
            }

            SetPlayerShips(ships, firstPlayer);
        }

        public void AddCustomShipTemplate(Dictionary<(string name, int size), int> shipData) //Creates custom ships based on user chosen data.
        {
            List<Ship> customShipsPf = new();
            List<Ship> customShipsPs = new();
            foreach (var ship in shipData)
            {
                for (int i = 0; i < ship.Value; i++)
                {
                    customShipsPf.Add(Ship.CreateCustomShip(ship.Key.name, ship.Key.size));
                    customShipsPs.Add(Ship.CreateCustomShip(ship.Key.name, ship.Key.size));
                }
            }

            FirstPlayer.ShipTemplate = customShipsPf;
            SecondPlayer.ShipTemplate = customShipsPs;
        }

        public bool CheckWin()
        {
            return OpponentPlayer().Board.BoardHealth <= 0;
        }

        public void SaveState() //Saves the current board state and adds it to board state list.
        {
            FirstPlayer.Board.CreateNewBoardState();
            SecondPlayer.Board.CreateNewBoardState();
        }

        public void LoadLastState() //Loads the last game state from game state list.
        {
            if (FirstPlayer.Board.BoardStates.Count > 1)
            {
                FirstPlayer.Board.BoardStates.Remove(FirstPlayer.Board.BoardStates.Last());
                SecondPlayer.Board.BoardStates.Remove(SecondPlayer.Board.BoardStates.Last());
                
                FirstPlayer.Board.SetLoadDataFromJson();
                SecondPlayer.Board.SetLoadDataFromJson();
            }
            
        }
        
    }
}