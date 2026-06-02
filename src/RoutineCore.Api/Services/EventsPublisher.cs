using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RoutineCore.Application.Services.Interfaces;

namespace RoutineCore.Api.Services
{
    public class EventsPublisher : IEventsPublisher
    {
        private readonly ILogger<EventsPublisher> _logger;
        private readonly MassTransit.IPublishEndpoint? _publishEndpoint;

        public EventsPublisher(ILogger<EventsPublisher> logger, MassTransit.IPublishEndpoint? publishEndpoint = null)
        {
            _logger = logger;
            _publishEndpoint = publishEndpoint;
        }

        public async Task PublishAsync<T>(T message) where T : class
        {
            if (_publishEndpoint != null)
            {
                _logger.LogInformation("Publishing integration event of type {Type} via MassTransit", typeof(T).Name);
                await _publishEndpoint.Publish(message);
            }
            else
            {
                _logger.LogWarning("MassTransit publish endpoint not configured. Integration event {Type} logged locally: {@Message}", typeof(T).Name, message);
            }
        }
    }
}
