using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;

namespace Domain
{
    public partial class Board
    {
        public int BoardId { get; set; }
        
        [NotMapped]
        private CellState[][] MoveBoard { get; set; } = null!;
        
        [NotMapped]
        private CellState[][] ShipBoard { get; set; } = null!;

        [Range(0, int.MaxValue)]
        public int Width { get; set; } = 10;  //Default width
        
        [Range(0, int.MaxValue)]
        public int Height { get; set; } = 10; //Default height

        public bool ShipsPlacedOnBoard { get; set; }

        public int BoardHealth { get; set; }
        
        [NotMapped]
        public virtual ICollection<Ship> Ships { get; set; } = null!;

        public virtual ICollection<BoardState> BoardStates { get; set; } = new List<BoardState>();

    }
}