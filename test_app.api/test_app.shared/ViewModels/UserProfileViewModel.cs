using System;
using System.Collections.Generic;
using System.Text;

namespace test_app.shared.ViewModels
{
    public class UserProfileViewModel
    {
        public string Id { get; set; }

        public string SteamAvatar { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string TradeofferUrl { get; set; }

        public List<UserWinnerViewModel> WonItems { get; set; }
    }
}
