using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Carts.Engine
{
    public class CartsCleanupMinion : Minion
    {
        private CommerceCommander CommerceCommander { get; set; }
        private GlobalCartsMaintenancePolicy maintenancePolicy { get; set; }

        public override void Initialize(IServiceProvider serviceProvider, ILogger logger, MinionPolicy policy, CommerceEnvironment environment, CommerceContext globalContext)
        {
            base.Initialize(serviceProvider, logger, policy, environment, globalContext);
            CommerceCommander = serviceProvider.GetService<CommerceCommander>();
            maintenancePolicy = environment.GetPolicy<GlobalCartsMaintenancePolicy>();
            LogInitialization(logger, policy);
        }

        private void LogInitialization(ILogger logger, MinionPolicy policy)
        {
            var initializationLogMessage = new StringBuilder();
            initializationLogMessage.Append($"CartsCleanupMinion settings:{System.Environment.NewLine}");
            initializationLogMessage.AppendLine($"\t WakeupInterval = {policy.WakeupInterval}");
            initializationLogMessage.AppendLine($"\t ListToWatch = {policy.ListToWatch}");
            initializationLogMessage.AppendLine($"\t ItemsPerBatch {policy.ItemsPerBatch}");
            initializationLogMessage.AppendLine($"\t SleepBetweenBatches {policy.SleepBetweenBatches}");
            initializationLogMessage.AppendLine($"\t DaysToRetainCarts = {maintenancePolicy.DaysToRetainCarts}");
            initializationLogMessage.AppendLine($"\t StopOverrun = {maintenancePolicy.StopOverrun}");
            initializationLogMessage.AppendLine($"\t AllowedSchedules:");

            for (int i = 0; i < maintenancePolicy.AllowedSchedules.Count(); i++)
            {
                initializationLogMessage.AppendLine($"\t\t StartTime = {maintenancePolicy.AllowedSchedules[i].StartTime}, EndTime = {maintenancePolicy.AllowedSchedules[i].StartTime}");
            }

            logger.LogInformation(initializationLogMessage.ToString());
        }

        private bool AllowExecution(DateTimeOffset executionDateTime)
        {
            CartsCleanupMinion minion = this;

            var executionTime = executionDateTime.TimeOfDay;

            foreach (var schedule in maintenancePolicy.AllowedSchedules)
            {
                var allowedStartTime = schedule.GetStartTime();
                var allowedEndTime = schedule.GetEndTime();

                if (allowedEndTime <= allowedStartTime)
                {
                    minion.Logger.LogError($"{minion.Name} - Invalid allowed execution times, "
                        + $"{nameof(schedule.EndTime)} '{schedule.EndTime}' must be greater than"
                        + $"{nameof(schedule.StartTime)} '{schedule.StartTime}'.");
                    return false;
                }

                if (allowedStartTime <= executionTime && executionTime <= allowedEndTime)
                {
                    return true;
                }
            }

            return false;
        }

        public override async Task<MinionRunResultsModel> Run()
        {
            // Setting this up for unit testing
            var start = DateTimeOffset.Now;
            var now = DateTimeOffset.MinValue;

            return await Run(start, now);
        }

        public async Task<MinionRunResultsModel> Run(DateTimeOffset start, DateTimeOffset now)
        {
            CartsCleanupMinion minion = this;
            minion.Logger.LogInformation($"{minion.Name} - Review List {minion.Policy.ListToWatch}");
            
            if (!AllowExecution(start))
            {
                minion.Logger.LogInformation($"{minion.Name} - Skipping execution due to current system time '{start}' being outside of allowed execution windows.");
                return new MinionRunResultsModel();
            }

            long listCount = await minion.GetListCount(minion.Policy.ListToWatch);
            minion.Logger.LogInformation($"{minion.Name} - Review List {minion.Policy.ListToWatch} - Has Count {listCount}");

            if (listCount <= 0)
            {
                return new MinionRunResultsModel();
            }

            for (int listIndex = 0; listCount >= listIndex; listIndex += minion.Policy.ItemsPerBatch)
            {
                var cartList = await minion.GetListItems<Cart>(minion.Policy.ListToWatch, minion.Policy.ItemsPerBatch, listIndex);
                if (cartList == null || cartList.Count().Equals(0))
                {
                    break;
                }

                foreach (var cart in (cartList))
                {
                    if (maintenancePolicy.StopOverrun && !AllowExecution(GetNowDateTime(now)))
                    {
                        minion.Logger.LogInformation($"{minion.Name} - Skipping execution due to current system time '{now.TimeOfDay}' being outside of allowed execution window.");
                        return new MinionRunResultsModel();
                    }

                    minion.Logger.LogDebug($"{minion.Name} - Checking: CartId={cart.Id}");

                    var arg = new CartsCleanupArgument(cart, maintenancePolicy);
                    await CommerceCommander.Pipeline<ICartsCleanupPipeline>().Run(arg, minion.MinionContext.GetPipelineContext());
                }
            }

            return new MinionRunResultsModel();
        }

        private DateTimeOffset GetNowDateTime(DateTimeOffset now)
        {
            return now == DateTimeOffset.MinValue ? DateTime.Now : now;
        }
    }
}
