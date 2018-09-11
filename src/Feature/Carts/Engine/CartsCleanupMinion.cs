using System;
using Sitecore.Commerce.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sitecore.Framework.Pipelines;
using System.Threading.Tasks;
using System.Threading;
using Sitecore.Commerce.Plugin.Carts;
using System.Collections.Generic;
using System.Linq;

namespace Feature.Carts.Engine
{
    public class CartsCleanupMinion : Minion
    {
        //protected ICartsCleanupMinionPipeline MinionPipeline { get; set; }
        protected IRemoveListEntitiesPipeline RemoveListEntitiesPipeline { get; set; }
        protected IDeleteEntityPipeline DeleteEntityPipeline { get; set; }

        public override void Initialize(IServiceProvider serviceProvider, ILogger logger, MinionPolicy policy, CommerceEnvironment environment, CommerceContext globalContext)
        {
            base.Initialize(serviceProvider, logger, policy, environment, globalContext);
            //this.MinionPipeline = serviceProvider.GetService<ICartsCleanupMinionPipeline>();
            this.RemoveListEntitiesPipeline = serviceProvider.GetService<IRemoveListEntitiesPipeline>();
            this.DeleteEntityPipeline = serviceProvider.GetService<IDeleteEntityPipeline>();
        }

        public override async Task<MinionRunResultsModel> Run()
        {
            CartsCleanupMinion minion = this;
            minion.Logger.LogInformation($"{minion.Name} - Review List {minion.Policy.ListToWatch}");

            var maintenancePolicy = minion.MinionContext.GetPolicy<GlobalCartsMaintenancePolicy>();

            // Potentially introduce a wait for an hour of the day.

            long listCount = await minion.GetListCount(minion.Policy.ListToWatch);
            minion.Logger.LogInformation($"{minion.Name} - Review List {minion.Policy.ListToWatch} - Has Count {listCount}");

            for (int listIndex = 0; listCount >= listIndex; listIndex += minion.Policy.ItemsPerBatch)
            {
                // TEST when happens when batch and fetch numbers are out of bounds.
                var cartList = await minion.GetListItems<Cart>(minion.Policy.ListToWatch, minion.Policy.ItemsPerBatch, listIndex);
                if (cartList == null || cartList.Count().Equals(0))
                {
                    break;
                }

                foreach (var cart in (cartList))
                {
                    minion.Logger.LogDebug($"{minion.Name} - Checking: CartId={cart.Id}");

                    var cartAge = cart.DateUpdated.HasValue ? cart.DateUpdated.Value.Subtract(DateTimeOffset.UtcNow).TotalDays : maintenancePolicy.DaysToRetainCarts;

                    if (cartAge < maintenancePolicy.DaysToRetainCarts)
                    {
                        minion.Logger.LogDebug($"{minion.Name} - Ignoring cart {cart.Id} not yet {maintenancePolicy.DaysToRetainCarts} days old, only {cartAge} days old.");
                    }
                    else
                    {
                        minion.Logger.LogInformation($"{minion.Name} - Deleting cart {cart.Id} as it's {cartAge} day old and over the retention period of {maintenancePolicy.DaysToRetainCarts} days");
                        // TEST if this list removal is required.
                        await RemoveListEntitiesPipeline.Run(new ListEntitiesArgument(new List<string> { cart.Id }, minion.Policy.ListToWatch), GetPipelineExecutionContextOptions(minion));
                        await DeleteEntityPipeline.Run(new DeleteEntityArgument(cart.Id), GetPipelineExecutionContextOptions(minion));
                    }
                }
            }

            return new MinionRunResultsModel();
        }

        private static IPipelineExecutionContextOptions GetPipelineExecutionContextOptions(CartsCleanupMinion minion)
        {
            return new CommercePipelineExecutionContextOptions(new CommerceContext(minion.Logger, minion.MinionContext.TelemetryClient, null)
            {
                Environment = minion.Environment
            }, null, null, null, null, null);
        }
    }
}
