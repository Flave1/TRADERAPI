using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace TradeChat.Services.Ecrypt
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AllowUncryptedStreamAttribute : Attribute, IAllowAnonymous
    {
    }
}
