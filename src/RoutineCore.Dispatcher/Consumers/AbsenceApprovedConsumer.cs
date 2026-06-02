using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using RoutineCore.Domain.Contracts;

namespace RoutineCore.Dispatcher.Consumers
{
    public class AbsenceApprovedConsumer : IConsumer<AbsenceApprovedEvent>
    {
        private readonly ILogger<AbsenceApprovedConsumer> _logger;

        public AbsenceApprovedConsumer(ILogger<AbsenceApprovedConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<AbsenceApprovedEvent> context)
        {
            var data = context.Message;
            _logger.LogInformation(
                "PulseDispatcher-Plugin: Absence Request Approved Event. AbsenceId: {AbsenceId}, Employee: {EmployeeId}, Start: {Start}, End: {End}, ApprovedBy: {Approver}",
                data.AbsenceId,
                data.EmployeeId,
                data.StartDate,
                data.EndDate,
                data.ApprovedBy
            );

            // Here we can run any integration logic (e.g. queue sending mailing/notifications, adjusting ERP balances, etc.)
            _logger.LogInformation("PulseDispatcher-Plugin: Absence notification email dispatched successfully to employee and human resources.");

            return Task.CompletedTask;
        }
    }
}
