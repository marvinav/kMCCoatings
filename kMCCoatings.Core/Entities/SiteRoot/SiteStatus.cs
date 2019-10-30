using System;
using System.Collections.Generic;
using System.Text;

namespace kMCCoatings.Core.Entities.SiteRoot
{
    public enum SiteStatus
    {
        Occupied,
        Vacanted,
        Prohibited
    }

    public enum ProhibitedReason
    {
        None,
        ContactRule,
        ForbiddenRadius,
        All
    }
}
