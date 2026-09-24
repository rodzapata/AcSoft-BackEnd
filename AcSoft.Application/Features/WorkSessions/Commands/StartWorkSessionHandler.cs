using AcSoft.Application.Abstractions.Persistence;
using AcSoft.Domain.Entities;
using AcSoft.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcSoft.Application.Features.WorkSessions.Commands;

public sealed class StartWorkSessionHandler
{
    private readonly IWorkOrderRepository _workOrderRepository;
    private readonly IWorkSessionRepository _workSessionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public StartWorkSessionHandler(
        IWorkOrderRepository workOrderRepository,
        IWorkSessionRepository workSessionRepository,
        IUnitOfWork unitOfWork)
    {
        _workOrderRepository = workOrderRepository;
        _workSessionRepository = workSessionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(
        Guid workOrderId,
        Guid technicianId,
        CancellationToken cancellationToken)
    {
        var workOrder = await _workOrderRepository
            .GetByIdAsync(
                workOrderId,
                cancellationToken);

        if (workOrder is null)
            throw new InvalidOperationException(
                "La orden de trabajo no existe.");

        if (workOrder.CurrentTechnicianId != technicianId)
            throw new InvalidOperationException(
                "El técnico no está asignado a esta orden.");

        var activeSession = await _workSessionRepository
            .GetActiveByTechnicianAsync(
                technicianId,
                cancellationToken);

        if (activeSession is not null)
            throw new InvalidOperationException(
                "El técnico ya tiene una sesión de trabajo activa.");

        workOrder.Start();

        var session = WorkSession.Start(
            technicianId,
            workOrderId,
            WorkSessionType.Work,
            DateTimeOffset.UtcNow);

        await _workSessionRepository.AddAsync(
            session,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);
    }
}
