﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class BettingRoomEntities : DbContext
    {
        public BettingRoomEntities()
            : base("name=BettingRoomEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AccountBalance> AccountBalances { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<BetOneGame> BetOneGames { get; set; }
        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<League> Leagues { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
    }
}
