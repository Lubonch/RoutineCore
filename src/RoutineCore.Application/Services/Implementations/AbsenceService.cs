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
    public class AbsenceService : IAbsenceService
    {
        private readonly IAbsenceRepository _absenceRepository;
        private readonly IEventsPublisher _eventsPublisher;

        public AbsenceService(IAbsenceRepository absenceRepository, IEventsPublisher eventsPublisher)
        {
            _absenceRepository = absenceRepository;
            _eventsPublisher = eventsPublisher;
        }

        public async Task<AbsenceDto> RequestAbsenceAsync(AbsenceDto dto)
        {
            var entity = new Absence
            {
                EmployeeId = dto.EmployeeId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Reason = dto.Reason,
                Authorized = false,
                ApprovedBy = null
            };

            await _absenceRepository.SaveAsync(entity);

            return new AbsenceDto(
                entity.Id,
                entity.EmployeeId,
                entity.StartDate,
                entity.EndDate,
                entity.Reason,
                entity.Authorized,
                entity.ApprovedBy
            );
        }

        public async Task ApproveAbsenceAsync(Guid id, string approverEmail)
        {
            var entity = await _absenceRepository.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException($"Absence request {id} not found");

            entity.Authorized = true;
            entity.ApprovedBy = approverEmail;

            await _absenceRepository.UpdateAsync(entity);

            // Publish message to MsgQueue / Dispatcher
            await _eventsPublisher.PublishAsync(new AbsenceApprovedEvent(
                entity.Id,
                entity.EmployeeId,
                entity.StartDate,
                entity.EndDate,
                entity.Reason,
                approverEmail
            ));
        }

        public async Task<IEnumerable<AbsenceDto>> GetPendingApprovalsAsync()
        {
            var list = await _absenceRepository.GetPendingApprovalsAsync();
            return list.Select(x => new AbsenceDto(
                x.Id,
                x.EmployeeId,
                x.StartDate,
                x.EndDate,
                x.Reason,
                x.Authorized,
                x.ApprovedBy
            ));
        }

        public async Task<IEnumerable<AbsenceDto>> GetByEmployeeAsync(Guid employeeId)
        {
            var list = await _absenceRepository.GetByEmployeeAsync(employeeId);
            return list.Select(x => new AbsenceDto(
                x.Id,
                x.EmployeeId,
                x.StartDate,
                x.EndDate,
                x.Reason,
                x.Authorized,
                x.ApprovedBy
            ));
        }
    }
}
//
