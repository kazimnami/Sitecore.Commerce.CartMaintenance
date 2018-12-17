using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Framework.Pipelines;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Feature.Carts.Engine
{
    public class CartsCleanupMinion : Minion
    {
        private CommerceCommander CommerceCommander { get; set; }
        private DateTimeOffset StartDateTime { get; set; }

        public override void Initialize(IServiceProvider serviceProvider, ILogger logger, MinionPolicy policy, CommerceEnvironment environment, CommerceContext globalContext)
        {
            base.Initialize(serviceProvider, logger, policy, environment, globalContext);
            CommerceCommander = serviceProvider.GetService<CommerceCommander>();
            StartDateTime = DateTime.Now;
        }

        public override async Task<MinionRunResultsModel> Run()
        {
            CartsCleanupMinion minion = this;
            minion.Logger.LogInformation($"{minion.Name} - Review List {minion.Policy.ListToWatch}");

            var maintenancePolicy = minion.MinionContext.GetPolicy<GlobalCartsMaintenancePolicy>();

            //if (maintenancePolicy.GetAllowedDailyEndTime() <= maintenancePolicy.GetAllowedDailyStartTime())
            //{
            //    minion.Logger.LogError($"{minion.Name} - Invalid allowed execution times, "
            //        + $"{nameof(maintenancePolicy.AllowedDailyEndTime)} '{maintenancePolicy.AllowedDailyEndTime}' must be greater than"
            //        + $"{nameof(maintenancePolicy.AllowedDailyStartTime)} '{maintenancePolicy.AllowedDailyStartTime}'.");
            //    return new MinionRunResultsModel();
            //}

            //var allowedStart = StartDateTime.Date + maintenancePolicy.GetAllowedDailyStartTime();
            //var allowedEnd = StartDateTime.Date + maintenancePolicy.GetAllowedDailyEndTime();
            //if (DateTime.Now < allowedStart || DateTime.Now < allowedEnd)
            //{
            //    minion.Logger.LogInformation($"{minion.Name} - Skipping execution due to current system time '{DateTime.Now}' being outside of allowed execution window between '{maintenancePolicy.AllowedDailyStartTime}' and '{maintenancePolicy.AllowedDailyEndTime}'.");
            //    return new MinionRunResultsModel();
            //}

            //long listCount = await minion.GetListCount(minion.Policy.ListToWatch);
            //minion.Logger.LogInformation($"{minion.Name} - Review List {minion.Policy.ListToWatch} - Has Count {listCount}");

            //for (int listIndex = 0; listCount >= listIndex; listIndex += minion.Policy.ItemsPerBatch)
            //{
            //    var cartList = await minion.GetListItems<Cart>(minion.Policy.ListToWatch, minion.Policy.ItemsPerBatch, listIndex);
            //    if (cartList == null || cartList.Count().Equals(0))
            //    {
            //        break;
            //    }

            //    foreach (var cart in (cartList))
            //    {
            //        if (DateTime.Now < allowedStart || DateTime.Now < allowedEnd)
            //        {
            //            minion.Logger.LogInformation($"{minion.Name} - Skipping execution due to current system time '{DateTime.Now.TimeOfDay}' being outside of allowed execution window between '{maintenancePolicy.AllowedDailyStartTime}' and '{maintenancePolicy.AllowedDailyEndTime}'.");
            //            return new MinionRunResultsModel();
            //        }

            //        minion.Logger.LogDebug($"{minion.Name} - Checking: CartId={cart.Id}");

            //        double cartAge = maintenancePolicy.DaysToRetainCarts;
            //        if (!cart.DateUpdated.HasValue)
            //        {
            //            minion.Logger.LogDebug($"{minion.Name} - Cart {cart.Id} does not have a last update date which isn't expected.");
            //        }
            //        else
            //        {
            //            cartAge = DateTimeOffset.UtcNow.Subtract(cart.DateUpdated.Value).TotalDays;
            //        }

            //        if (cartAge < maintenancePolicy.DaysToRetainCarts)
            //        {
            //            minion.Logger.LogDebug($"{minion.Name} - Ignoring cart {cart.Id} not yet {maintenancePolicy.DaysToRetainCarts} days old, only {cartAge} days old.");
            //        }
            //        else
            //        {
            //            minion.Logger.LogInformation($"{minion.Name} - Deleting cart {cart.Id} as it's {cartAge} day old and over the retention period of {maintenancePolicy.DaysToRetainCarts} days");

            //            await minion.CommerceCommander.DeleteEntity(minion.GlobalContext, cart.Id);
            //        }
            //    }
            //}

            return new MinionRunResultsModel();
        }
    }
}
