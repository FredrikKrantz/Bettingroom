using BettingRoom.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BettingRoom.Models
{
    public class BetOneModel
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }
        [Range(1, 1000)]
        [DisplayName("BET AMOUNT ")]
        public int BetAmount { get; set; }
        [DisplayName("GAME: ")]
        public int GameId { get; set; }
        [DisplayName("1 X 2 ON THE GAME ")]
        public string _1X2 { get; set; }
        [DisplayName("ODDS ON THE BET ")]
        public double Odds { get; set; }
        [Required]
        public string UserId { get; set; }
        [DisplayName("WON OR LOST")]
        public string WonOrLost { get; set; }
        [DisplayName("HOME TEAM")]
        public string HomeTeam { get; set; }
        [DisplayName("GUEST TEAM")]
        public string GuestTeam { get; set; }
        [DisplayName("TIME YOU LAID THE BET")]
        public System.DateTime BetTime { get; set; }


        [ScaffoldColumn(false)]
        public virtual Game Game { get; set; }
        [ScaffoldColumn(false)]
        public virtual AspNetUser AspNetUser { get; set; }
    }
}