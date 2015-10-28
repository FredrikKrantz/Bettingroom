using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace BettingRoom.Models
{
    public class TeamModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [DisplayName("SPORT")]
        public string SportName { get; set; }
        [DisplayName("PLAYS IN LEAGUE")]
        public string LeagueName { get; set; }
        [DisplayName("TOTAL POINTS")]
        public int PointsInLeague { get; set; }
        [DisplayName("TEAMS ODDS")]
        public string TeamOdds { get; set; }
    }
}