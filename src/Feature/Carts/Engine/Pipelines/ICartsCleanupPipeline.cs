using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Framework.Pipelines;

namespace Feature.Carts.Engine
{
    [PipelineDisplayName(Constants.Pipelines.CartsCleanup)]
    public interface ICartsCleanupPipeline : IPipeline<CartsCleanupArgument, CartsCleanupArgument, CommercePipelineExecutionContext>
    {
    }
}