using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace BettingRoom.Helpers
{
    public class Calculate
    {
        public Random rnd = new Random();
        public void GenerateScores()
        {
            var ctx = new DAL.BettingRoomEntities();

            var games = ctx.Games.Where(g => g.GameTime <= DateTime.Now).Where(g => g.Result1X2 == null).ToList();

            GenerateHomeTeamScore(games);
            GenerateGuestTeamScore(games);
            SetResult1X2();
        }

        public void SetResult1X2()
        {
            var ctx = new DAL.BettingRoomEntities();

            var games = ctx.Games.Where(g => g.GameTime <= DateTime.Now).Where(g => g.Result1X2 == null).ToList();

            foreach (var game in games)
            {
                var home = game.ResultHomeTeam;
                var guest = game.ResultGuestTeam;

                if (home == guest)
                {
                    game.Result1X2 = "X";
                }

                else if (home < guest)
                {
                    game.Result1X2 = "2";
                }

                else if (home > guest)
                {
                    game.Result1X2 = "1";
                }
                ctx.SaveChanges();
                SetPoints(game.Id);
                CheckWinners(game.Id);
            }
        }

        public void GenerateHomeTeamScore(List<DAL.Game> games)
        {
            var ctx = new DAL.BettingRoomEntities();

            foreach (var item in games)
            {
                var game = ctx.Games.Where(g => g.Id == item.Id).FirstOrDefault();

                if (game.Team.SportName == "SOCCER")
                {
                    int scoreSoccer = rnd.Next(0, 4);
                    game.ResultHomeTeam = scoreSoccer;
                }
                else if (game.Team.SportName == "ICE HOCKEY")
                {
                    int scoreHockey = rnd.Next(0, 10);
                    game.ResultHomeTeam = scoreHockey;
                }
                else if (game.Team.SportName == "FLOORBALL")
                {
                    int scoreFloorball = rnd.Next(0, 10);
                    game.ResultHomeTeam = scoreFloorball;
                }
                ctx.SaveChanges();
            }
        }

        public void GenerateGuestTeamScore(List<DAL.Game> games)
        {
            var ctx = new DAL.BettingRoomEntities();

            foreach (var item in games)
            {
                var game = ctx.Games.Where(g => g.Id == item.Id).FirstOrDefault();

                if (game.Team.SportName == "SOCCER")
                {
                    int scoreSoccer = rnd.Next(0, 4);
                    game.ResultGuestTeam = scoreSoccer;
                }
                else if (game.Team.SportName == "ICE HOCKEY")
                {
                    int scoreHockey = rnd.Next(0, 10);
                    game.ResultGuestTeam = scoreHockey;
                }
                else if (game.Team.SportName == "FLOORBALL")
                {
                    int scoreFloorball = rnd.Next(0, 10);
                    game.ResultGuestTeam = scoreFloorball;
                }
                ctx.SaveChanges();
            }
        }

        public void CheckWinners(int gameId)
        {
            var ctx = new DAL.BettingRoomEntities();

            var game = ctx.Games.Where(g => g.Id == gameId).FirstOrDefault();

            CheckWinningBets(gameId, ctx, game);

            CheckLosingBets(gameId, ctx, game);
        }

        private static void CheckWinningBets(int gameId, DAL.BettingRoomEntities ctx, DAL.Game game)
        {
            var winningBets = ctx.BetOneGames.Where(b => b.GameId == gameId && b.C1X2 == game.Result1X2).ToList();

            if (winningBets.Count > 0)
            {
                foreach (var bet in winningBets)
                {
                    bet.WonOrLose = "WON";
                    var account = ctx.AccountBalances.Where(a => a.UserId == bet.UserId).FirstOrDefault();
                    account.AmountInEuro = account.AmountInEuro + (bet.BetAmount * bet.Odds);

                    var transaction = new DAL.Transaction
                    {
                        AccountId = account.Id,
                        TransactionType = "Winning",
                        TransactionTime = game.GameTime.AddHours(2),
                        Amount = bet.BetAmount * bet.Odds,
                    };
                    ctx.Transactions.Add(transaction);
                    ctx.SaveChanges();
                }
            }
        }

        private static void CheckLosingBets(int gameId, DAL.BettingRoomEntities ctx, DAL.Game game)
        {
            var loosingBets = ctx.BetOneGames.Where(b => b.GameId == gameId && b.C1X2 != game.Result1X2).ToList();

            if (loosingBets.Count > 0)
            {
                foreach (var bet in loosingBets)
                {
                    bet.WonOrLose = "LOST";
                    ctx.SaveChanges();
                }
            }
        }

        public void SetPoints(int gameId)
        {
            var ctx = new DAL.BettingRoomEntities();

            var game = ctx.Games.Where(g => g.Id == gameId).FirstOrDefault();

            if (game.Result1X2 == "1")
            {
                game.Team.PointsInLeague += 3;
                game.Team.TeamOdds *= 0.9;
            }
            else if (game.Result1X2 == "X")
            {
                game.Team.PointsInLeague += 1;
                game.Team1.PointsInLeague += 1;
            }
            else if (game.Result1X2 == "2")
            {
                game.Team1.PointsInLeague += 3;
                game.Team1.TeamOdds *= 1.1;
            }
            ctx.SaveChanges();
        }

        public DAL.AccountBalance CheckAccount(string user)
        {
            var ctx = new DAL.BettingRoomEntities();

            if (ctx.AccountBalances.Where(a => a.UserId == user).FirstOrDefault() == null)
            {
                var account = new DAL.AccountBalance
                {
                    AmountInEuro = 0,
                    UserId = user,
                };

                account.AmountInEuro = 2000;
                ctx.AccountBalances.Add(account);
                ctx.SaveChanges();
                return account;
            }
            else
            {
                return ctx.AccountBalances.Where(a => a.UserId == user).FirstOrDefault();
            }
        }

        public List<Models.GameModel> GenerateOdds()
        {
            var ctx = new DAL.BettingRoomEntities();

            var lastDate = DateTime.Now.AddDays(6);

            var fixedGames = new List<Models.GameModel>();

            foreach (var game in ctx.Games.Where(g => g.GameTime >= DateTime.Now && g.GameTime <= lastDate).ToList())
            {
                var home = double.Parse(game.Team.TeamOdds.ToString());
                var guest = double.Parse(game.Team1.TeamOdds.ToString());
                game.Odds1 = home * 1.1;
                game.OddsX = (home * 1.5 + guest * 1.5) / 2;
                game.Odds2 = guest;
                ctx.SaveChanges();

                fixedGames.Add(new Models.GameModel
                {
                    Odds1 = game.Odds1,
                    Odds2 = game.Odds2,
                    OddsX = game.OddsX,
                    GameTime = game.GameTime,
                    HomeTeam = game.Team.Name,
                    GuestTeam = game.Team1.Name,
                });
            }
            return fixedGames;
        }
    }
}