using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EShopProject.DataAccess.Entity
{
    [Flags]
    public enum MyAuthorization
    {
        StandartUser = 1,
        Company = 2,
        Admin = 16
    }
}