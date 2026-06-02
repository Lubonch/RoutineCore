using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RoutineCore.Application.Dtos;
using RoutineCore.Application.Services.Interfaces;
using RoutineCore.Domain.Contracts;
using RoutineCore.Domain.Entities;
using RoutineCore.Domain.Repositories;

namespace RoutineCore.Application.Services.Implementations
{
    public class PunchService : IPunchService
    {
        private readonly IPunchRepository _punchRepository;
        private readonly IEventsPublisher _eventsPublisher;

        public PunchService(IPunchRepository punchRepository, IEventsPublisher eventsPublisher)
        {
            _punchRepository = punchRepository;
            _eventsPublisher = eventsPublisher;
        }

        public async Task<PunchDto> RegisterPunchAsync(Guid employeeId, string direction, string deviceCode)
        {
            var entity = new Punch
            {
                EmployeeId = employeeId,
                PunchTime = DateTime.UtcNow,
                Direction = direction,
                DeviceCode = deviceCode,
                Processed = false
            };

            await _punchRepository.SaveAsync(entity);

            var dto = new PunchDto(
                entity.Id,
                entity.EmployeeId,
                entity.PunchTime,
                entity.Direction,
                entity.DeviceCode,
                entity.Processed
            );

            // Publish Integration Event to PulseDispatcher!
            await _eventsPublisher.PublishAsync(new PunchRegisteredEvent(
                dto.Id,
                dto.EmployeeId,
                dto.PunchTime,
                dto.Direction,
                dto.DeviceCode
            ));

            return dto;
        }

        public async Task<IEnumerable<PunchDto>> GetUnprocessedAsync()
        {
            var list = await _punchRepository.GetUnprocessedPunchesAsync();
            return list.Select(x => new PunchDto(
                x.Id,
                x.EmployeeId,
                x.PunchTime,
                x.Direction,
                x.DeviceCode,
                x.Processed
            ));
        }

        public async Task ProcessPunchesAsync()
        {
            var unprocessed = await _punchRepository.GetUnprocessedPunchesAsync();
            foreach (var punch in unprocessed)
            {
                punch.Processed = true;
                await _punchRepository.UpdateAsync(punch);
            }
        }
    }
}
