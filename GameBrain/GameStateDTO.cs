using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Domain;
using Domain.Enums;

namespace GameBrain
{
    public class GameStateDTO
    {
        public Player FirstPlayerDTO { get; set; } = null!;

        public Player SecondPlayerDTO { get; set; } = null!;

        public bool CurrentPlayerFirstDTO { get; set; }

        public ENextMoveAfterHit NextMoveAfterHitDTO { get; set; }
        
    }
}