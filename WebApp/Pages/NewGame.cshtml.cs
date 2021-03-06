using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using Domain;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace WebApp.Pages
{
    public class NewGameModel : PageModel
    {
        private readonly ILogger<NewGameModel> _logger;
        private readonly ApplicationDbContext _context;

        public NewGameModel(ApplicationDbContext context, ILogger<NewGameModel> logger)
        {
            _context = context;
            _logger = logger;
        }
        
        public List<int> BoardSizeList = new ();
        [BindProperty] 
        public string GameName { get; set; } = "";
        [BindProperty]
        public string FirstPlayer { get; set; } = "";
        [BindProperty]
        public string SecondPLayer { get; set; } = "";
        [BindProperty]
        public int BoardWidth { get; set; } = 10;
        [BindProperty]
        public int BoardHeight { get; set; } = 10;

        [BindProperty]
        public int ShipPlacement { get; set; }

        [BindProperty]
        public EShipsCanTouch EShipsCanTouch { get; set; } = EShipsCanTouch.No;

        [BindProperty]
        public bool ENextMoveAfterHit { get; set; } = true;

        public void OnGet()
        {
            for (int x = 10; x < 41; x++)
            {
                BoardSizeList.Add(x);
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var fBoard = new Board()
            {
                Width = BoardWidth,
                Height = BoardHeight,
                BoardHealth = 15,
                Ships = Ship.GetDefaultShips(),
                BoardStates = new List<BoardState>()
            };
            fBoard.CreateBoard(BoardWidth, BoardHeight);

            var sBoard = new Board()
            {
                Width = BoardWidth,
                Height = BoardHeight,
                BoardHealth = 15,
                Ships = Ship.GetDefaultShips(),
                BoardStates = new List<BoardState>()
            };
            sBoard.CreateBoard(BoardWidth, BoardHeight);

            var fPlayer = new Player()
            {
                Name = string.IsNullOrWhiteSpace(FirstPlayer) ? "Player 1" : FirstPlayer,
                Board = fBoard,
                ShipTemplate = Ship.GetDefaultShips()
            };
            
            foreach (var ship in fPlayer.ShipTemplate)
            {
                ship.SetJsonShipCoordinates();
            }

            var sPlayer = new Player()
            {
                Name = string.IsNullOrWhiteSpace(SecondPLayer) ? "Player 2" : SecondPLayer,
                Board = sBoard,
                ShipTemplate = Ship.GetDefaultShips()
            };
            
            foreach (var ship in sPlayer.ShipTemplate)
            {
                ship.SetJsonShipCoordinates();
            }

            var game = new Game()
            {
                GameName = GameName != "" ? GameName : fPlayer.Name + " vs " + sPlayer.Name,
                FirstPlayer = fPlayer,
                SecondPlayer = sPlayer,
                CurrentPlayerFirst = true,
                EShipsCanTouch = EShipsCanTouch,
                NextMoveAfterHit = ENextMoveAfterHit ? Domain.Enums.ENextMoveAfterHit.SamePlayer : Domain.Enums.ENextMoveAfterHit.OtherPlayer,
            };
            
            await _context.Games.AddAsync(game);
            await _context.SaveChangesAsync();

            return ShipPlacement == 1 ? RedirectToPage("./GamePlay/Index", new { id = game.GameId, g = 1, m = 1}) //Random ship placement or by user.
                : RedirectToPage("./ShipPlacement/Index", new { id = game.GameId, p = 0});
        }
    }
}