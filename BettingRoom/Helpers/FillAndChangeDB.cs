using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BettingRoom.Helpers
{
    public class FillAndChangeDB
    {
        public Random rnd = new Random();
        public Calculate cal = new Calculate();

        public void AtFirstSignIn(string userId)
        {
            CreateTransactions(userId);
            GenerateScoresWithBet(userId);
        }

        public void CreateTransactions(string user)
        {
            var ctx = new DAL.BettingRoomEntities();

            var account = ctx.AccountBalances.Where(a => a.UserId == user).FirstOrDefault();

            for (int i = 1; i < 30; i++)
            {
                var transaction = new DAL.Transaction
                {
                    AccountId = account.Id,
                    Amount = GetAmount(),
                    TransactionType = GetTransactionType(),
                    TransactionTime = new DateTime(2015, 04, i)
                };

                if (transaction.TransactionType == "Withdraw" || transaction.TransactionType == "Bet")
                {
                    if (account.AmountInEuro >= transaction.Amount)
                    {
                        ctx.Transactions.Add(transaction);
                        account.AmountInEuro = account.AmountInEuro - transaction.Amount;
                        ctx.SaveChanges();
                    }
                }

                else if (transaction.TransactionType == "Winning" || transaction.TransactionType == "Deposit")
                {
                    ctx.Transactions.Add(transaction);
                    account.AmountInEuro = account.AmountInEuro + transaction.Amount;
                    ctx.SaveChanges();
                }
            }
         
        }

        public void CreateBet(int gameId, string userId)
        {
            var ctx = new DAL.BettingRoomEntities();

            DAL.AccountBalance account;

            if (ctx.AccountBalances.Where(a => a.UserId == userId).FirstOrDefault() == null)
            {
                ctx.AccountBalances.Add(new DAL.AccountBalance
                {
                    UserId = userId,
                    AmountInEuro = 100,
                });
                ctx.SaveChanges();

                account = ctx.AccountBalances.Where(a => a.UserId == userId).FirstOrDefault();
            }
            else
            {
                account = ctx.AccountBalances.Where(a => a.UserId == userId).FirstOrDefault();
            }

            var result = Get1X2();
            var game = ctx.Games.Find(gameId);

            var newBet = new DAL.BetOneGame
            {
                GameId = gameId,
                UserId = userId,
                BetAmount = GetBetAmount(),
                C1X2 = result,
                Odds = GetOdds(gameId, result),
                WonOrLose = "NOT FINISH",
            };

            if (newBet.BetAmount <= account.AmountInEuro)
            {
                account.AmountInEuro = account.AmountInEuro - newBet.BetAmount;
                ctx.BetOneGames.Add(newBet);

                ctx.Transactions.Add(new DAL.Transaction()
                {
                    AccountId = account.Id,
                    Amount = newBet.BetAmount,
                    TransactionType = "Bet",
                    TransactionTime = game.GameTime.AddHours(2),
                });
                ctx.SaveChanges();
            }
        }

        public void GenerateScoresWithBet(string userId)
        {
            var ctx = new DAL.BettingRoomEntities();

            var games = ctx.Games.Where(g => g.GameTime <= DateTime.Now).Where(g => g.Result1X2 == null).ToList();

            foreach (var game in games)
            {
                var home = double.Parse(game.Team.TeamOdds.ToString());
                var guest = double.Parse(game.Team1.TeamOdds.ToString());
                game.Odds1 = home * 1.1;
                game.OddsX = (home * 1.5 + guest * 1.5) / 2;
                game.Odds2 = guest;
                ctx.SaveChanges();

                CreateBet(game.Id, userId);
            }
            cal.GenerateHomeTeamScore(games);
            cal.GenerateGuestTeamScore(games);
            cal.SetResult1X2();
        }

        public void ResetResult(string userId)
        {
            var ctx = new DAL.BettingRoomEntities();

            foreach (var game in ctx.Games.Where(g => g.GameTime <= DateTime.Now).ToList())
            {
                game.ResultGuestTeam = null;
                game.ResultHomeTeam = null;
                game.Result1X2 = null;
                game.Odds1 = 0.0;
                game.Odds2 = 0.0;
                game.OddsX = 0.0;
                ctx.SaveChanges();
            }

            foreach (var team in ctx.Teams.ToList())
            {
                team.PointsInLeague = 0;
                team.TeamOdds = 1.35;
                ctx.SaveChanges();
            }

            GenerateScoresWithBet(userId);

        }

        private string GetTransactionType()
        {
            var number = rnd.Next(0, 4);

            if (number == 0)
            {
                return "Withdraw";
            }
            else if (number == 1)
            {
                return "Bet";
            }
            else if (number == 2)
            {
                return "Deposit";
            }
            else if (number == 3)
            {
                return "Winning";
            }
            return null;
        }

        private double GetAmount()
        {
            return rnd.Next(1, 1000);
        }

        private double GetOdds(int gameId, string result)
        {
            var ctx = new DAL.BettingRoomEntities();

            var game = ctx.Games.Where(g => g.Id == gameId).FirstOrDefault();

            if (result == "1")
            {
                return game.Odds1;
            }
            else if (result == "X")
            {
                return game.OddsX;
            }
            else
            {
                return game.Odds2;
            }
        }

        private string Get1X2()
        {
            var result = rnd.Next(0, 3);

            if (result == 0)
            {
                return "1";
            }
            else if (result == 1)
            {
                return "X";
            }
            else if (result == 2)
            {
                return "2";
            }
            return null;
        }

        private int GetBetAmount()
        {
            return rnd.Next(1, 1000);
        }
    }
}