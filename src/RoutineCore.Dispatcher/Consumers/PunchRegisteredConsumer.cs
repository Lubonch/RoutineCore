using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using RoutineCore.Domain.Contracts;

namespace RoutineCore.Dispatcher.Consumers
{
    public class PunchRegisteredConsumer : IConsumer<PunchRegisteredEvent>
    {
        private readonly ILogger<PunchRegisteredConsumer> _logger;

        public PunchRegisteredConsumer(ILogger<PunchRegisteredConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<PunchRegisteredEvent> context)
        {
            var data = context.Message;
            _logger.LogInformation(
                "PulseDispatcher-Plugin: Punch Event Received. Employee: {EmployeeId}, Time: {PunchTime}, Direction: {Direction}, Device: {Device}",
                data.EmployeeId,
                data.PunchTime,
                data.Direction,
                data.DeviceCode
            );

            // Here we can run any integration logic (e.g. sync with outer payroll databases, send to external BI, or notify teams)
            _logger.LogInformation("PulseDispatcher-Plugin: Punch Event processed and published successfully to outer systems.");

            return Task.CompletedTask;
        }
    }
}
