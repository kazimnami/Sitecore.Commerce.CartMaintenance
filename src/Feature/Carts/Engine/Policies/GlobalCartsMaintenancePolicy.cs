using Sitecore.Commerce.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Carts.Engine
{
    public class GlobalCartsMaintenancePolicy : Policy
    {
        public GlobalCartsMaintenancePolicy()
        {
            this.DaysToRetainCarts = 30;
        }

        public int DaysToRetainCarts { get; set; }
    }
}
