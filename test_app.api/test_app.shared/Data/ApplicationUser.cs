using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace test_app.shared.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string SteamId { get; set; }
        public int SteamProfileState { get; set; }
        public string SteamAvatar { get; set; }
        public string SteamUsername { get; set; }
        public Decimal Balance { get; set; }
        public String TradeofferUrl { get; set; }

        [NotMapped]
        public IList<string> Roles { get; set; }
    }
}
