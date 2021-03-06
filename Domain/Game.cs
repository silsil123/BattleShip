using System;
using System.Collections;
using System.Collections.Generic;
using Domain.Enums;


namespace Domain
{
    public class Game
    {
        public int GameId { get; set; }

        public string Description { get; set; } = DateTime.Now.ToLongDateString();
        public string GameName { get; set; } = null!;
        public int FirstPlayerId { get; set; }
        public Player FirstPlayer { get; set; } = null!;

        public int SecondPlayerId { get; set; }
        public Player SecondPlayer { get; set; } = null!;
        
        public bool CurrentPlayerFirst { get; set; } 
        
        public ENextMoveAfterHit NextMoveAfterHit { get; set; }

        public EShipsCanTouch EShipsCanTouch { get; set; }
    }
}