//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BettingRoom.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Game
    {
        public Game()
        {
            this.BetOneGames = new HashSet<BetOneGame>();
        }
    
        public int Id { get; set; }
        public int HomeTeamId { get; set; }
        public int GuestTeamId { get; set; }
        public double Odds1 { get; set; }
        public double OddsX { get; set; }
        public double Odds2 { get; set; }
        public System.DateTime GameTime { get; set; }
        public Nullable<int> ResultHomeTeam { get; set; }
        public Nullable<int> ResultGuestTeam { get; set; }
        public string Result1X2 { get; set; }
    
        public virtual ICollection<BetOneGame> BetOneGames { get; set; }
        public virtual Team Team { get; set; }
        public virtual Team Team1 { get; set; }
    }
}