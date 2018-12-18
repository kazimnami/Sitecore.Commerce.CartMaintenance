using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Framework.Pipelines;
using Microsoft.Extensions.Logging;

namespace Feature.Carts.Engine
{
    [PipelineDisplayName(Constants.Pipelines.CartsCleanup)]
    public class CartsCleanupPipeline : CommercePipeline<CartsCleanupArgument, CartsCleanupArgument>, ICartsCleanupPipeline
    {
        public CartsCleanupPipeline(IPipelineConfiguration<ICartsCleanupPipeline> configuration, ILoggerFactory loggerFactory)
            : base(configuration, loggerFactory)
        {
        }
    }
}