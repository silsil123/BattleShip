using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public static class DbOperations
    {
        private static ApplicationDbContext GetDbContext()
        {
            ApplicationDbContext dbContext = new (new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlServer(
                @"
                    Server=barrel.itcollege.ee,1533;
                    User id=student;
                    Password=Student.Bad.password.0;
                    MultipleActiveResultSets=true;
                    Database=silsil_battleshipDB"
            ).Options);

            return dbContext;
        }

        public static Game GetDbLoadData(int? fileNo, int? id) //Return a game based on it's id or file number.
        {
            using var dbCtx = GetDbContext();
            dbCtx.Database.Migrate();
            
            var savedGames = dbCtx.Games.ToList();
            var data = 0;

            if (fileNo == null && id != null) //Return existing game based on given id or user chosen file number.
            {
                data = (int) id;
            }
            else if (fileNo != null && id == null)
            {
                data = savedGames[(int)fileNo].GameId;
            }

            var dbGame = dbCtx.Games.Where(x => x.GameId == data)
                .Include(x => x.FirstPlayer)
                .ThenInclude(x => x.Board)
                .ThenInclude(x => x.BoardStates)
                .Include(x => x.SecondPlayer)
                .ThenInclude(x => x.Board)
                .ThenInclude(x => x.BoardStates)
                .FirstOrDefaultAsync();
            if (dbGame.Result == null) //If game doesn't exist in db, return a new one.
            {
                return new Game();
            }
            return dbGame.Result;
        }

        public static void SaveDbData(Game dbGame) //Saves game to db.
        {
            using var dbCtx = GetDbContext();
            dbCtx.Database.Migrate();

            if (dbGame.GameId == 0)
            {
                dbCtx.Games.Add(dbGame);
            }
            else
            {
                dbCtx.Games.Update(dbGame);
            }

            dbCtx.SaveChanges();
        }

        public static List<string> GetGameSaveFileNameList() //Returns a list with game names.
        {
            using var dbCtx = GetDbContext();
            dbCtx.Database.Migrate();
            
            var savedGames = dbCtx.Games.ToList();

            return savedGames.Select(game => game.GameName).ToList();
        }
    }
}