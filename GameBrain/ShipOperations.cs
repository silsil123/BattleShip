using System.Collections.Generic;
using Domain;
using Domain.Enums;

namespace GameBrain
{
    public static class ShipOperations
    {
        public static void BoatNavigation(string dir, Ship ship, (int x, int y) dimensions)
        {
            switch (dir)
            {
                case "left":
                {
                    if (ship.Coordinates[0][0] > 0)
                        for (int i = 0; i < ship.Coordinates.Length; i++)
                        {
                            ship.Coordinates[i][0] -= 1;
                        }
                    break;
                }
                case "right":
                {
                    if (!ship.Horizontal && ship.Coordinates[0][0] < dimensions.x - 1 ||
                        ship.Horizontal && (ship.Coordinates[0][0] + ship.ShipSize - 1) < dimensions.x - 1)
                        for (int i = 0; i < ship.Coordinates.Length; i++)
                        {
                            ship.Coordinates[i][0] += 1;
                        }
                    break;
                }
                case "up":
                {
                    if (ship.Coordinates[0][1] > 0)
                        for (int i = 0; i < ship.Coordinates.Length; i++)
                        {
                            ship.Coordinates[i][1] -= 1;
                        }

                    break;
                }
                case "down":
                {
                    if (!ship.Horizontal && (ship.Coordinates[0][1] + ship.ShipSize - 1) < dimensions.y - 1 ||
                        ship.Horizontal && ship.Coordinates[0][1] < dimensions.y - 1)
                        for (int i = 0; i < ship.Coordinates.Length; i++)
                        {
                            ship.Coordinates[i][1] += 1;
                        }
                    break; 
                }
                case "rotate":
                {
                    switch (ship.Horizontal)
                    {
                        case false when (ship.Coordinates[0][0] + ship.ShipSize - 1) < dimensions.x:
                        case true when (ship.Coordinates[0][1] + ship.ShipSize - 1) < dimensions.y:
                            RotateShip(ship);
                            break;
                    }

                    break;
                }
            }
        }

        private static void RotateShip(Ship ship)
        {
            int newX = ship.Coordinates[0][0];
            int newY = ship.Coordinates[0][1];
            for (int i = 0; i < ship.Coordinates.Length; i++)
            {
                if (!ship.Horizontal)
                {
                    ship.Coordinates[i][0] = newX + i;
                    ship.Coordinates[i][1] = newY;
                }
                else
                {
                    ship.Coordinates[i][0] = newX;
                    ship.Coordinates[i][1] = newY + i;
                }
            }

            ship.Horizontal = !ship.Horizontal;
        }
        
        public static bool CheckCoordinateAvailability(List<Ship> ships, Ship nShip, EShipsCanTouch option)
        {
            nShip.IsPlaced = true;
            foreach (var eShip in ships)
            {
                if (!eShip.Equals(nShip) && eShip.IsPlaced)
                {
                    foreach (var eCoords in eShip.Coordinates)
                    {
                        foreach (var nCoords in nShip.Coordinates)
                        {
                            if (option == EShipsCanTouch.Yes)
                            {
                                if (nCoords[0] == eCoords[0] && nCoords[1] == eCoords[1]) //check overlapping
                                {
                                    nShip.IsPlaced = false;
                                    return false;
                                }
                            }
                            else if (option == EShipsCanTouch.Corner)
                            {
                                if (CoordCheckSides(nCoords, eCoords))
                                {
                                    nShip.IsPlaced = false;
                                    return false;
                                }
                            }
                            else if (option == EShipsCanTouch.No)
                            {
                                if (CoordCheckSides(nCoords, eCoords) ||  //check overlapping and sides 
                                    nCoords[0] + 1 == eCoords[0] && nCoords[1] + 1== eCoords[1] ||  //check below right
                                    nCoords[0] + 1 == eCoords[0] && nCoords[1] - 1== eCoords[1] ||  //check above right
                                    nCoords[0] - 1 == eCoords[0] && nCoords[1] + 1 == eCoords[1] || //check below left
                                    nCoords[0] - 1 == eCoords[0] && nCoords[1] - 1 == eCoords[1]    //check above left 
                                )
                                {
                                    nShip.IsPlaced = false;
                                    return false;
                                }
                            }
                        }
                    }
                }
            }

            return true;
        }
        
        private static bool CoordCheckSides(int[] nCoords, int[] eCoords)
        {
            return nCoords[0] == eCoords[0] && nCoords[1] == eCoords[1] ||  //check overlapping
                   nCoords[0] + 1 == eCoords[0] && nCoords[1] == eCoords[1] || //check right
                   nCoords[0] - 1 == eCoords[0] && nCoords[1] == eCoords[1] || //check left
                   nCoords[0] == eCoords[0] && nCoords[1] + 1 == eCoords[1] || //check below
                   nCoords[0] == eCoords[0] && nCoords[1] - 1 == eCoords[1];   //check above
        }
    }
}