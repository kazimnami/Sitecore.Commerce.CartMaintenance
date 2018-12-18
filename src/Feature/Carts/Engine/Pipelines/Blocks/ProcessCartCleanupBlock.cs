using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Framework.Pipelines;

namespace Feature.Carts.Engine
{
    [PipelineDisplayName(Constants.Pipelines.Blocks.ProcessCartCleanupBlock)]
    public class ProcessCartCleanupBlock : PipelineBlock<CartsCleanupArgument, CartsCleanupArgument, CommercePipelineExecutionContext>
    {
        private CommerceCommander CommerceCommander { get; set; }

        public ProcessCartCleanupBlock(IServiceProvider serviceProvider)
        {
            CommerceCommander = serviceProvider.GetService<CommerceCommander>();
        }

        public async override Task<CartsCleanupArgument> Run(CartsCleanupArgument arg, CommercePipelineExecutionContext context)
        {
            if (arg == null)
            {
                return arg;
            }

            double cartAge = arg.MaintenancePolicy.DaysToRetainCarts;
            if (!arg.Cart.DateUpdated.HasValue)
            {
                context.Logger.LogDebug($"{this.Name} - Cart {arg.Cart.Id} does not have a last update date which isn't expected.");
            }
            else
            {
                cartAge = DateTimeOffset.UtcNow.Subtract(arg.Cart.DateUpdated.Value).TotalDays;
            }

            if (cartAge < arg.MaintenancePolicy.DaysToRetainCarts)
            {
                context.Logger.LogDebug($"{this.Name} - Ignoring cart {arg.Cart.Id} not yet {arg.MaintenancePolicy.DaysToRetainCarts} days old, only {cartAge} days old.");
            }
            else
            {
                context.Logger.LogInformation($"{this.Name} - Deleting cart {arg.Cart.Id} as it's {cartAge} day old and over the retention period of {arg.MaintenancePolicy.DaysToRetainCarts} days");

                await CommerceCommander.DeleteEntity(context.CommerceContext, arg.Cart.Id);
            }

            return arg;
        }
    }
}
