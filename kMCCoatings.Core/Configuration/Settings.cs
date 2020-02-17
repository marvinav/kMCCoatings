using System;
using System.Collections.Generic;
using System.Text;
using kMCCoatings.Core.Entities.AtomRoot;

namespace kMCCoatings.Core.Configuration
{
    public class Settings
    {
        public Element[] Elements { get; set; }

        public DepositionSettings DepositionSettings { get; set; }
    }
}
