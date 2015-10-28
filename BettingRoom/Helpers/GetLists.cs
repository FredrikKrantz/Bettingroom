using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BettingRoom.Helpers
{
   
    public class GetLists
    {
        public Calculate Calculate = new Calculate();
        public List<Models.TeamModel> GetTeamStandings(int id)
        {
            var ctx = new DAL.BettingRoomEntities();

            return ctx.Teams
                .Where(t => t.LeagueId == id)
                .OrderByDescending(t => t.PointsInLeague)
                .Select(t => new Models.TeamModel
                {
                    Id = t.Id,
                    Name = t.Name,
                    PointsInLeague = t.PointsInLeague,
                    LeagueName = t.League.LeagueName,
                }).ToList();
        }

        public List<Models.GameModel> GetPlayedGames(int id)
        {
            var ctx = new DAL.BettingRoomEntities();

            return ctx.Games
                .Where(g => g.Team.LeagueId == id)
                .Where(g => g.GameTime <= DateTime.Now)
                .Select(g => new Models.GameModel
                {
                    Id = g.Id,
                    HomeTeam = g.Team.Name,
                    GuestTeam = g.Team1.Name,
                    GameTime = g.GameTime,
                    Odds1 = g.Odds1,
                    OddsX = g.OddsX,
                    Odds2 = g.Odds2,
                    ResultHomeTeam = g.ResultHomeTeam,
                    ResultGuestTeam = g.ResultGuestTeam,
                    Result1X2 = g.Result1X2,
                }).ToList();
        }

        public List<Models.GameModel> GetGamesInSixDaysInLeague(int id)
        {            
            var ctx = new DAL.BettingRoomEntities();

            var lastDate = DateTime.Now.AddDays(6);

            return ctx.Games
                .Where(g => g.Team.LeagueId == id)
                .Where(g => g.GameTime >= DateTime.Now && g.GameTime <= lastDate)
                .Select(g => new Models.GameModel
                {
                    Id = g.Id,
                    HomeTeam = g.Team.Name,
                    GuestTeam = g.Team1.Name,
                    GameTime = g.GameTime,
                    Odds1 = g.Odds1,
                    OddsX = g.OddsX,
                    Odds2 = g.Odds2,
                }).ToList();
        }

        public List<Models.GameModel> GetUpcomingGamesInLeague(int id)
        {
            var ctx = new DAL.BettingRoomEntities();

            return ctx.Games
                .Where(g => g.Team.LeagueId == id)
                .Where(g => g.GameTime >= DateTime.Now)
                .Select(g => new Models.GameModel
                {
                    Id = g.Id,
                    HomeTeam = g.Team.Name,
                    GuestTeam = g.Team1.Name,
                    GameTime = g.GameTime,
                    Odds1 = g.Odds1,
                    OddsX = g.OddsX,
                    Odds2 = g.Odds2,
                }).ToList();
        }

        public List<Models.GameModel> GetTeamsGames(int id)
        {
            var ctx = new DAL.BettingRoomEntities();

            return ctx.Games
                .Where(g => g.HomeTeamId == id || g.GuestTeamId == id)
                .Where(g => g.GameTime <= DateTime.Now)
                .Select(g => new Models.GameModel
                {
                    Id = g.Id,
                    HomeTeam = g.Team.Name,
                    GuestTeam = g.Team1.Name,
                    GameTime = g.GameTime,
                    ResultHomeTeam = g.ResultHomeTeam,
                    ResultGuestTeam = g.ResultGuestTeam,
                    Result1X2 = g.Result1X2,
                }).ToList();
        }

        public List<Models.TransactionModel> GetTransactions(string id)
        {
            var ctx = new DAL.BettingRoomEntities();

            return ctx.Transactions.Where(t => t.AccountBalance.UserId == id).Select(t => new Models.TransactionModel
            {
                Id = t.Id,
                AccountId = t.AccountId,
                Amount = t.Amount,
                TransactionType = t.TransactionType,
                TransactionTime = t.TransactionTime,
            }).OrderByDescending(t => t.TransactionTime).ToList();
        }

        public List<Models.TransactionModel> GetTransactions(string userId, string searchQuery)
        {
            var ctx = new DAL.BettingRoomEntities();

            if (searchQuery == "Transaction")
            {
                return ctx.Transactions.Where(t => t.AccountBalance.UserId == userId)
                    .Select(t => new Models.TransactionModel
                {
                    TransactionTime = t.TransactionTime,
                    TransactionType = t.TransactionType,
                    Amount = t.Amount,
                }).OrderByDescending(t => t.TransactionTime).ToList();
            }
            else
            {
                return ctx.Transactions.Where(t => t.AccountBalance.UserId == userId && t.TransactionType == searchQuery)
                    .Select(t => new Models.TransactionModel
                {
                    TransactionTime = t.TransactionTime,
                    TransactionType = t.TransactionType,
                    Amount = t.Amount,
                }).OrderByDescending(t => t.TransactionTime).ToList();
            }            
        }

        public List<Models.BetOneModel> GetBets(string userId, string searchQuery)
        {
            var ctx = new DAL.BettingRoomEntities();

            if (searchQuery == "Bet")
            {
                return ctx.BetOneGames.Where(b => b.UserId == userId)
                    .Select(b => new Models.BetOneModel 
                {
                    _1X2 = b.C1X2,
                    BetAmount = b.BetAmount,
                    HomeTeam = b.Game.Team.Name,
                    GuestTeam = b.Game.Team1.Name,
                    Odds = b.Odds,
                    WonOrLost = b.WonOrLose,                    
                }).ToList();
            }
            else
            {
                return ctx.BetOneGames.Where(b => b.UserId == userId && b.WonOrLose == searchQuery).Select(b => new Models.BetOneModel
                {
                    _1X2 = b.C1X2,
                    BetAmount = b.BetAmount,
                    HomeTeam = b.Game.Team.Name,
                    GuestTeam = b.Game.Team1.Name,
                    Odds = b.Odds,
                    WonOrLost = b.WonOrLose,
                }).ToList(); 
            }
        }
    }
}