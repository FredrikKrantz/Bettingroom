using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace BettingRoom.Models
{
    public class AccountModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        [DisplayName("BALANCE IN €")]
        public double AmountInEuro { get; set; }
    }
}