using AcSoft.Application.Abstractions.Persistence;
using AcSoft.Application.Abstractions.Services;
using AcSoft.Application.Features.WorkOrders.DTOs;
using AcSoft.Domain.Entities;
using AcSoft.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcSoft.Application.Features.WorkOrders.Commands;

public sealed class CreateWorkOrderFromMaintenancePlanHandler
{
    private readonly IMaintenancePlanRepository _maintenancePlanRepository;
    private readonly IActivityRepository _activityRepository;
    private readonly IWorkOrderRepository _workOrderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateWorkOrderFromMaintenancePlanHandler(
        IMaintenancePlanRepository maintenancePlanRepository,
        IActivityRepository activityRepository,
        IWorkOrderRepository workOrderRepository,
        IUnitOfWork unitOfWork)
    {
        _maintenancePlanRepository = maintenancePlanRepository;
        _activityRepository = activityRepository;
        _workOrderRepository = workOrderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> HandleAsync(
        CreateWorkOrderFromMaintenancePlanRequest request,
        CancellationToken cancellationToken)
    {
        // 1. Consultar el plan preventivo.
        var maintenancePlan =
            await _maintenancePlanRepository.GetByIdWithActivitiesAsync(
                request.MaintenancePlanId,
                cancellationToken);

        if (maintenancePlan is null)
        {
            throw new InvalidOperationException(
                "El plan de mantenimiento no existe.");
        }

        // 2. Validar que el plan esté activo.
        if (maintenancePlan.Status != MaintenancePlanStatus.Active)
        {
            throw new InvalidOperationException(
                "Solo se pueden generar órdenes desde un plan activo.");
        }

        // 3. Generar el número de la orden.
        var workOrderNumber = $"OT-{DateTime.UtcNow:yyyyMMddHHmmss}";

        // 4. Crear la orden.
        var workOrder = WorkOrder.Create(
            number: workOrderNumber,
            customerId: request.CustomerId,
            equipmentId: request.EquipmentId);

        // 5. Obtener las actividades del plan.
        foreach (var planActivity in maintenancePlan.Activities
                     .OrderBy(x => x.Moment)
                     .ThenBy(x => x.Sequence))
        {
            var activity =
                await _activityRepository.GetByIdWithQuestionsAsync(
                    planActivity.ActivityId,
                    cancellationToken);

            if (activity is null)
            {
                throw new InvalidOperationException(
                    $"No se encontró la actividad del catálogo " +
                    $"con identificador {planActivity.ActivityId}.");
            }

            // 6. Copiar la actividad y sus preguntas a la orden.
            workOrder.AddActivity(
                activity: activity,
                moment: planActivity.Moment,
                sequence: planActivity.Sequence,
                isUnplanned: false);
        }

        // 7. Agregar la orden al repositorio.
        await _workOrderRepository.AddAsync(
            workOrder,
            cancellationToken);

        // 8. Guardar los cambios.
        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return workOrder.Id;
    }
}