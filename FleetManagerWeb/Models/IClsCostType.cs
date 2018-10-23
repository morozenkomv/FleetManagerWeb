using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FleetManagerWeb.Models
{
    public interface IClsCostType
    {
        long SaveCostType(ClsCostType costType);
    }
}