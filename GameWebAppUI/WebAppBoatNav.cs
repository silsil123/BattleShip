using System;
using System.Collections.Generic;
using Domain;
using Domain.Enums;
using GameBrain;

namespace GameWebAppUI
{
    public static class WebAppBoatNav
    {

        public static bool BoatNavValidation(string dir, List<Ship> ships, int shipNo,
            (int x, int y) boardDimensions, EShipsCanTouch option)
        {
            Ship curShip = ships[shipNo];

            switch (dir)
            {
                case "left":
                {
                    ShipOperations.BoatNavigation("left", curShip, boardDimensions);
                    break;
                }
                case "right":
                {
                    ShipOperations.BoatNavigation("right", curShip, boardDimensions);
                    break;
                }
                case "up":
                {
                    ShipOperations.BoatNavigation("up", curShip, boardDimensions);
                    break;
                }
                case "down":
                {
                    ShipOperations.BoatNavigation("down", curShip, boardDimensions);
                    break;
                }
                case "rotate":
                {
                    ShipOperations.BoatNavigation("rotate", curShip, boardDimensions);
                    break;
                }
                case "enter":
                {
                    ShipOperations.CheckCoordinateAvailability(ships, curShip, option);
                    if (curShip.IsPlaced)
                    {
                        return true;
                    }
                    break;
                }
            }

            return false;
        }
    }
}
