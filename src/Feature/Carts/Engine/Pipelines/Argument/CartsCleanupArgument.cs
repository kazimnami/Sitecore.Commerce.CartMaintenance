using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Framework.Conditions;

namespace Feature.Carts.Engine
{
    public class CartsCleanupArgument : PipelineArgument
    {
        public CartsCleanupArgument(Cart cart, GlobalCartsMaintenancePolicy maintenancePolicy)           
        {
            Condition.Requires(cart).IsNotNull("The cart can not be null ");
            Condition.Requires(maintenancePolicy).IsNotNull("The maintenance policy can not be null ");
            this.Cart = cart;
            this.MaintenancePolicy = maintenancePolicy;
        }

        public Cart Cart { get; set; }
        public GlobalCartsMaintenancePolicy MaintenancePolicy { get; set; }
    }
}
