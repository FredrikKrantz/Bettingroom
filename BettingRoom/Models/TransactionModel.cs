using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace BettingRoom.Models
{
    public class TransactionModel
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        [DisplayName("TRANSACTION TYPE")]
        public string TransactionType { get; set; }
        [DisplayName("AMOUNT")]
        public double Amount { get; set; }
        [DisplayName("DATE OF THE TRANSACTION")]
        public System.DateTime TransactionTime { get; set; }
    }
}