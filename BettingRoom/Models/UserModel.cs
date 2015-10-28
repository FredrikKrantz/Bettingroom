using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BettingRoom.Models
{
    public class UserModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public double Balance { get; set; }
    }
}