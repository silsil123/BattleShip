using System.Linq;
using System.Threading.Tasks;
using DAL;
using Domain;
using Domain.Enums;
using GameBrain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace WebApp.Pages.GamePlay
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

        public Game Game { get; set; } = null!;
        public BattleShip BsGame { get; set; } = new BattleShip();

        public Player CurrentPlayer { get; set; } = null!;

        public int ShipBoardActive { get; set; }
        public bool GameWon { get; set; }
        public int Undo { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int MoveDone { get; set; }
        
        public async Task<IActionResult> OnGetAsync(int id, int? posX, int? posY, int? g, int? s, int? t, int? m, int? u)//g=generate random ships,s=ship visibility,t=turn swap,m=move done, u=undo
        {
            Game = await _context.Games.Where(x => x.GameId == id)
                .Include(x => x.FirstPlayer)
                .ThenInclude(x => x.Board)
                .ThenInclude(x => x.BoardStates)
                .Include(x => x.SecondPlayer)
                .ThenInclude(x => x.Board)
                .ThenInclude(x => x.BoardStates)
                .FirstOrDefaultAsync();
            if (u == 1)
            {
                
                if (Game.FirstPlayer.Board.BoardStates.Count > 1)
                {
                    Game.FirstPlayer.Board.BoardStates.Remove(Game.FirstPlayer.Board.BoardStates.Last());
                    Game.SecondPlayer.Board.BoardStates.Remove(Game.SecondPlayer.Board.BoardStates.Last());
                }
                _context.Update(Game);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index", new { id = Game.GameId, s, m});
            }
            BsGame.LoadStateFromDbGame(id, Game);
            var dimensions = BsGame.GetBoardDimensions();
            Width = dimensions.x;
            Height = dimensions.y;
            CurrentPlayer = Game.CurrentPlayerFirst ? Game.FirstPlayer : Game.SecondPlayer;

            if (g == 1)
            {
                BsGame.GenerateRandomShips(true);
                BsGame.GenerateRandomShips(false);
            }

            if (m == 1)
            {
                MoveDone = 1;
            }

            if (s != null)
            {
                ShipBoardActive = (int) s;
            }

            

            if (posX != null && posY != null)
            {
                var result = CurrentPlayer.Board.GetMoveBoard()[(int) posX][(int) posY];
                if (result == CellState.Empty)
                {
                    MoveDone = BsGame.MakeAMove(posX.Value, posY.Value) ? 1 : 0;
                }
            }

            GameWon = BsGame.CheckWin();

            if (t == 1)
            {
                Game.CurrentPlayerFirst = !Game.CurrentPlayerFirst;
                CurrentPlayer = Game.CurrentPlayerFirst ? Game.FirstPlayer : Game.SecondPlayer;
            }

            
            
            _context.Update(Game);
            await _context.SaveChangesAsync();
            return Page();
        }
        
    }
}