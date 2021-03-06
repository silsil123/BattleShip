namespace Domain
{
    public class BoardState
    {
        public int BoardStateId { get; set; }

        public int BoardId { get; set; }

        public int BoardHealth { get; set; }
        public string MoveBoardJsonString { get; set; } = null!;

        public string ShipBoardJsonString { get; set; } = null!;

        public string ShipListJsonString { get; set; } = null!;
    }
}