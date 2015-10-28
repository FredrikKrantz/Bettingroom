using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace BettingRoom.Controllers
{
    public class HomeController : Controller
    {
        public Helpers.GetLists helperLists = new Helpers.GetLists();
        public Helpers.Calculate Calculate = new Helpers.Calculate();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GamesInSixDays()
        {
            var games = Calculate.GenerateOdds();

            return PartialView("_GamesInSixDays", games);
        }

        public ActionResult Balance()
        {
            var ctx = new DAL.BettingRoomEntities();
            var userId = User.Identity.GetUserId();

            var account = ctx.AccountBalances.Where(a => a.UserId == userId).Select(a => new Models.AccountModel
            {
                Id = a.Id,
                AmountInEuro = a.AmountInEuro,
            }).FirstOrDefault();

            return PartialView("_DisplayBalance", account);
        }

        public ActionResult GetTransactions()
        {
            var transactions = helperLists.GetTransactions(User.Identity.GetUserId());

            return PartialView("_DisplayTransactions", transactions);
        }

        public ActionResult WithDrawAttempt(Models.TransactionModel model)
        {
            var ctx = new DAL.BettingRoomEntities();
            var userId = User.Identity.GetUserId();
            var account = ctx.AccountBalances.Where(a => a.UserId == userId).FirstOrDefault();

            if (ModelState.IsValid)
            {
                if (model.Amount <= account.AmountInEuro)
                {
                    account.AmountInEuro = account.AmountInEuro - model.Amount;
                    ctx.SaveChanges();

                    ctx.Transactions.Add(new DAL.Transaction
                    {
                        Amount = model.Amount,
                        TransactionTime = DateTime.Now,
                        TransactionType = "Withdraw",
                        AccountId = model.AccountId,
                    });
                    ctx.SaveChanges();
                    
                    return View("Index");
                }             
                return PartialView("_CreateWithdraw", model);
            }
            return PartialView("_CreateWithdraw", model);
        }

        public ActionResult DepositAttempt(Models.TransactionModel model)
        {
            var ctx = new DAL.BettingRoomEntities();
            if (ModelState.IsValid)
            {               
                ctx.Transactions.Add(new DAL.Transaction
                {
                    Amount = model.Amount,
                    TransactionTime = DateTime.Now,
                    TransactionType = "Deposit",
                    AccountId = model.AccountId,
                });                
                ctx.SaveChanges();  
              
                var userId = User.Identity.GetUserId();
                var account = ctx.AccountBalances.Where(a => a.UserId == userId).FirstOrDefault();

                account.AmountInEuro = account.AmountInEuro + model.Amount;
                ctx.SaveChanges();
                return View("Index");
            }
            return PartialView("_CreateDeposit", model);
        }

        public ActionResult CreateWithdraw()
        {
            var ctx = new DAL.BettingRoomEntities();
            var userId = User.Identity.GetUserId();

            var account = ctx.AccountBalances.Where(a => a.UserId == userId).Select(a => new Models.AccountModel
            {
                Id = a.Id,
                AmountInEuro = a.AmountInEuro,
            }).FirstOrDefault();

            return PartialView("_CreateWithdraw", account);
        }

        public ActionResult CreateDeposit()
        {
            var ctx = new DAL.BettingRoomEntities();
            var userId = User.Identity.GetUserId();

            var account = ctx.AccountBalances.Where(a => a.UserId == userId).Select(a => new Models.AccountModel
            {
                Id = a.Id,
                AmountInEuro = a.AmountInEuro,
            }).FirstOrDefault();

            return PartialView("_CreateDeposit", account);
        }

        public ActionResult ViewTransactions(string searchQuery)
        {
            List<Models.TransactionModel> itemsToView = helperLists.GetTransactions(User.Identity.GetUserId(), searchQuery);

            return PartialView("_DisplayTransactions", itemsToView);
        }

        public ActionResult ViewBets(string searchQuery)
        {
            List<Models.BetOneModel> itemsToView = helperLists.GetBets(User.Identity.GetUserId(), searchQuery);

            return PartialView("_DisplayBets", itemsToView);
        }
    }
}
