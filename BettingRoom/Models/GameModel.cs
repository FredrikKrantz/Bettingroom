using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BettingRoom.Models
{
    public class GameModel
    {
        public int Id { get; set; }
        [DisplayName("HOME TEAM")]
        public string HomeTeam { get; set; }
        [DisplayName("GUEST TEAM")]
        public string GuestTeam { get; set; }
        [DisplayName("1")]
        public Nullable<double> Odds1 { get; set; }
        [DisplayName("X")]
        public Nullable<double> OddsX { get; set; }
        [DisplayName("2")]
        public Nullable<double> Odds2 { get; set; }
        [DisplayName("GAME TIME")]
        public DateTime GameTime { get; set; }
        public string Result1X2 { get; set; }

        public string Result { get { return ResultHomeTeam.ToString() + " - " + ResultGuestTeam.ToString(); } }

        public string LeagueName { get; set; }
        
       
        public Nullable<int> ResultHomeTeam { get; set; }
      
        public Nullable<int> ResultGuestTeam { get; set; }        
    }
}


        
    
