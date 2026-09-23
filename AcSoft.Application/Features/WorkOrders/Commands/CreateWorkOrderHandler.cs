using AcSoft.Application.Abstractions.Persistence;
using AcSoft.Application.Features.WorkOrders.DTOs;
using AcSoft.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;


namespace AcSoft.Application.Features.WorkOrders.Commands;

public sealed class CreateWorkOrderHandler
{
    private readonly IWorkOrderRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateWorkOrderHandler(
        IWorkOrderRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> HandleAsync(
        CreateWorkOrderRequest request,
        CancellationToken cancellationToken)
    {
        var workOrder = WorkOrder.Create(
            number: $"OT-{DateTime.UtcNow:yyyyMMddHHmmss}",
            customerId: request.CustomerId,
            equipmentId: request.EquipmentId);

        await _repository.AddAsync(
            workOrder,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return workOrder.Id;
    }
}