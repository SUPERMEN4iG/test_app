using System;
using System.Collections.Generic;
using System.Text;
using test_app.shared.Data;

namespace test_app.shared.ViewModels
{
    public class CaseDrop
    {
        public Int64 Id { get; set; }

        public double Chance { get; set; }

        public Skin Skin { get; set; }
    }
}
