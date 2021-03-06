using System.Collections.Generic;

namespace Domain
{
    public partial class Ship
    {
        public static List<Ship> GetDefaultShips()
        {
            List<Ship> defaultBoats = new List<Ship>()
            {
                new Ship { Name = "Carrier", ShipSize = 5, HealthPoints = 5, Coordinates = new []
                {
                    new [] {0, 0},
                    new [] {0, 1},
                    new [] {0, 2},
                    new [] {0, 3},
                    new [] {0, 4},
                }},
                new Ship { Name = "Battleship", ShipSize = 4, HealthPoints = 4, Coordinates = new []
                {
                    new [] {0, 0},
                    new [] {0, 1},
                    new [] {0, 2},
                    new [] {0, 3},
                }},
                new Ship { Name = "Submarine", ShipSize = 3, HealthPoints = 3, Coordinates = new []
                {
                    new [] {0, 0},
                    new [] {0, 1},
                    new [] {0, 2}
                }},
                new Ship { Name = "Cruiser", ShipSize = 2, HealthPoints = 2, Coordinates = new []
                {
                    new [] {0, 0},
                    new [] {0, 1}
                }},
                new Ship { Name = "Cruiser", ShipSize = 2, HealthPoints = 2, Coordinates = new []
                {
                    new [] {0, 0},
                    new [] {0, 1}
                }},
                new Ship { Name = "Patrol", ShipSize = 1, HealthPoints = 1, Coordinates = new []
                {
                    new [] {0, 0}
                }},
                new Ship { Name = "Patrol", ShipSize = 1, HealthPoints = 1, Coordinates = new []
                {
                    new [] {0, 0}
                }},
                new Ship { Name = "Patrol", ShipSize = 1, HealthPoints = 1, Coordinates = new []
                {
                    new [] {0, 0}
                }}
            };
            
            return defaultBoats;
        }
        
        public static Ship CreateCustomShip(string name, int shipSize)
        {
            Ship ship = new Ship { Name = name,
                ShipSize = shipSize,
                HealthPoints = shipSize,
                Coordinates = new int[shipSize][]
            };
            for (int i = 0; i < ship.Coordinates.Length; i++)
            {
                ship.Coordinates[i] = new[] {0, i};
            }

            return ship;
        }

        public void SetJsonShipCoordinates()
        {
            CoordinatesJsonString = System.Text.Json.JsonSerializer.Serialize(Coordinates);
        }

        public void SetShipCoordinatesFromJson()
        {
            var res = System.Text.Json.JsonSerializer.Deserialize<int[][]>(CoordinatesJsonString);
            if (res != null)
            {
                Coordinates = res;
            }
        }
    }
}

