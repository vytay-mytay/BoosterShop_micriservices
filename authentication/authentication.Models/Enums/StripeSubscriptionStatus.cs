using System;
using System.Collections.Generic;
using System.Text;

namespace authentication.Models.Enums
{
    public enum StripeSubscriptionStatus
    {
        Incomplete,
        IncompleteExpired,
        Trialing,
        Active,
        PastDue,
        Canceled,
        Unpaid
    }
}
