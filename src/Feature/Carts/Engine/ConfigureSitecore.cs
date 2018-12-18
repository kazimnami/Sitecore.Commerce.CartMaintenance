namespace Feature.Carts.Engine
{
    using System.Reflection;   
    using Microsoft.Extensions.DependencyInjection;
    using Sitecore.Commerce.Core;
    using Sitecore.Framework.Configuration;
    using Sitecore.Framework.Pipelines.Definitions.Extensions;

    public class ConfigureSitecore : IConfigureSitecore
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            services.RegisterAllPipelineBlocks(assembly);

            services.Sitecore().Pipelines(config => config
                .AddPipeline<ICartsCleanupPipeline, CartsCleanupPipeline>(
                 c =>
                 {
                     c.Add<ProcessCartCleanupBlock>();
                 }));

            services.RegisterAllCommands(assembly);
        }
    }
}