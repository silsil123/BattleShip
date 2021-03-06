using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Domain
{
    public partial class Ship
    {
        public int ShipId { get; set; }

        public int BoardId { get; set; }

        [MaxLength(32)]
        public string Name { get; set; } = null!;
        
        [Range(0, int.MaxValue)]
        public int ShipSize { get; set; }

        public bool IsPlaced { get; set; }

        public bool Horizontal { get; set; }

        [Range(0, int.MaxValue)]
        public int HealthPoints { get; set; }

        [NotMapped]
        public int[][] Coordinates { get; set; } = null!;

        public string CoordinatesJsonString { get; set; } = "";
    }
}