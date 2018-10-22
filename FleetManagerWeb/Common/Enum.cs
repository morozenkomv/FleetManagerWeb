namespace FleetManagerWeb.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public static class Enum
    {
        public enum USER_ADMIN_ROLES
        {
            //  SYSADMIN and ADMIN roleid fix, Other roleid get from Role Table.
            SYSADMIN = 14,
            ADMIN = 22
        }
    }
}