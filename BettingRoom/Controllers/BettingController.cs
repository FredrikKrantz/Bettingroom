using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace BettingRoom.Controllers
{
    public class BettingController : Controller
    {
        public Helpers.GetLists HelperList = new Helpers.GetLists();
        public Helpers.Calculate Calculate = new Helpers.Calculate();        

        public ActionResult Index()
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        public ActionResult GetLeagueStandings(int id)
        {           
            var teams = HelperList.GetTeamStandings(id);

            return PartialView("_LeagueStandings", teams);
        }

        public ActionResult GetPlayedGames(int id)
        {
            var games = HelperList.GetPlayedGames(id);           

            return PartialView("_DisplayPlayedGames", games);
        }

        public ActionResult GetBettableGames(int id) 
        {            
            var games = HelperList.GetGamesInSixDaysInLeague(id);

            return PartialView("_PlayableGames", games);
        }

        public ActionResult GetNextGames(int id)
        {
            var games = HelperList.GetUpcomingGamesInLeague(id);

            return PartialView("_DisplayUpcomingGames", games);
        }

        public ActionResult GetSpecificTeam(int id)
        {
            var games = HelperList.GetTeamsGames(id);

            return PartialView("_DisplayPlayedGames", games);
        }

        public ActionResult SaveTheBet(Models.BetOneModel model) 
        {
            if (ModelState.IsValid)
            {
                var ctx = new DAL.BettingRoomEntities();
                var account = ctx.AccountBalances.Where(a => a.UserId == model.UserId).FirstOrDefault();

                if (account.AmountInEuro >= model.BetAmount)
                {
                    account.AmountInEuro = account.AmountInEuro - model.BetAmount;
                    ctx.SaveChanges();

                    var newBet = new DAL.BetOneGame
                    {
                        GameId = model.GameId,
                        UserId = model.UserId,
                        BetAmount = model.BetAmount,
                        C1X2 = model._1X2,
                        Odds = model.Odds,
                        WonOrLose = "NOT FINISH",
                        BetTime = DateTime.Now,
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
                            TransactionTime = DateTime.Now,
                        });
                        ctx.SaveChanges();
                    }
                    return PartialView("_Success");
                }
                return PartialView("_FinishBet", model);
            }
            return PartialView("_FinishBet", model);
        }

        private static void CreateTheTransaction(Models.BetOneModel model, DAL.BettingRoomEntities ctx, DAL.AccountBalance account)
        {
            ctx.Transactions.Add(new DAL.Transaction
            {
                AccountId = account.Id,
                Amount = model.BetAmount,
                TransactionTime = DateTime.Now,
                TransactionType = "Bet",
            });
            ctx.SaveChanges();
        }

        private static void CreateTheBet(Models.BetOneModel model, DAL.BettingRoomEntities ctx)
        {
            ctx.BetOneGames.Add(new DAL.BetOneGame
            {
                BetAmount = model.BetAmount,
                C1X2 = model._1X2,
                Odds = model.Odds,
                GameId = model.GameId,
                WonOrLose = "NOT FINISH",
                BetTime = DateTime.Now,
                UserId = model.UserId,
            });
            ctx.SaveChanges();
        }

        public ActionResult CreateBet(int id, string result) 
        {
            var ctx = new DAL.BettingRoomEntities();

            var tempOdds = double.Parse(CheckOdds(id, result).ToString());
            
            var user = User.Identity.GetUserId();

            var bet = new Models.BetOneModel() 
            {
                GameId = id,
                _1X2 = result,
                Odds = tempOdds,
                UserId = user,
                BetAmount = 1,                
            };           
            return PartialView("_FinishBet", bet);
        }

        private object CheckOdds(int id, string result)
        {
            var ctx = new DAL.BettingRoomEntities();

            var game = ctx.Games.Where(g => g.Id == id).FirstOrDefault();

            if (result == "1")
            {
                return game.Odds1;
            }
            else if (result == "X")
            {
                return game.OddsX;
            }
            else if (result == "2")
            {
                return game.Odds2;
            }

            return null;
        }

        public void CreateTransactions() 
        {
            var fill = new Helpers.FillAndChangeDB();
            fill.CreateTransactions(User.Identity.GetUserId());
        }

        public void ResetResult() 
        {
            var fill = new Helpers.FillAndChangeDB();
            fill.ResetResult(User.Identity.GetUserId());           
        }
    }
}
