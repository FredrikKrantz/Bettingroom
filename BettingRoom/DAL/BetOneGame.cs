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
    
    public partial class BetOneGame
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int BetAmount { get; set; }
        public int GameId { get; set; }
        public string C1X2 { get; set; }
        public double Odds { get; set; }
        public string WonOrLose { get; set; }
        public Nullable<System.DateTime> BetTime { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual Game Game { get; set; }
    }
}
