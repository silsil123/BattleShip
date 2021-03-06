using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using Domain;
using GameWebAppUI;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace WebApp.Pages.ShipPlacement
{
    public class Index : PageModel
    {
        private readonly ILogger<NewGameModel> _logger;
        private readonly ApplicationDbContext _context;

        public Index(ApplicationDbContext context, ILogger<NewGameModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public List<Ship> Ships { get; set; } = null!;

        public int CurShipNo { get; set; }
        
        public bool AllShipsPlaced { get; set; }

        public Game Game { get; set; } = null!;

        public string Name { get; set; } = null!;

        public int PlayerTurn { get; set; }

        public async Task OnGetAsync(int c, int id, string? dir, int p) //c = ship count, p = player, g = placement
        {
            Game = await _context.Games.Where(x => x.GameId == id)
                .Include(x => x.FirstPlayer)
                .ThenInclude(x => x.ShipTemplate)
                .Include(x => x.FirstPlayer.Board)
                .ThenInclude(x => x.BoardStates)
                .Include(x => x.SecondPlayer)
                .ThenInclude(x => x.ShipTemplate)
                .Include(x => x.SecondPlayer.Board)
                .ThenInclude(x => x.BoardStates)
                .FirstOrDefaultAsync();
            
            
            PlayerTurn = p;
            var player = p == 0 ? Game.FirstPlayer : Game.SecondPlayer;
            Name = player.Name;
            Ships = player.ShipTemplate;

            if (c != 0)
            {
                CurShipNo = c;
            }

            foreach (var ship in Ships)
            {
                ship.SetShipCoordinatesFromJson();
            }

            if (dir != null)
            {
                var boatPlaced = WebAppBoatNav.BoatNavValidation(dir, Ships, CurShipNo, (player.Board.Width, player.Board.Height), Game.EShipsCanTouch);
                if (boatPlaced)
                {
                    CurShipNo++;
                }
            }
            
            AllShipsPlaced = true;
            foreach (var ship in Ships)
            {
                ship.SetJsonShipCoordinates();
                if (!ship.IsPlaced)
                {
                    AllShipsPlaced = false;
                }
            }

            if (AllShipsPlaced)
            {
                player.Board.Ships = Ships;
                player.Board.SetShipsOnBackBoard();
                player.Board.ShipsPlacedOnBoard = true;
                player.Board.CreateNewBoardState();
            }

            _context.Update(Game);
            await _context.SaveChangesAsync();
        }
    }
}
