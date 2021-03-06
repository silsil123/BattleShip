using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class Player
    {
        public int PlayerId { get; set; }
        
        [MaxLength(32)]
        public string Name { get; set; } = null!;

        [Range(0, int.MaxValue)]
        public int BoardId { get; set; }
        public Board Board { get; set; } = new();
        public List<Ship> ShipTemplate { get; set; } = new ();
        
    }
}