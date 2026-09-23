using AcSoft.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcSoft.Domain.Entities;

public sealed class WorkOrder
{
    private WorkOrder()
    {
    }

    private WorkOrder(
        Guid id,
        string number,
        Guid customerId,
        Guid equipmentId)
    {
        Id = id;
        Number = number;
        CustomerId = customerId;
        EquipmentId = equipmentId;
        Status = WorkOrderStatus.Pending;
    }

    public Guid Id { get; private set; }

    public string Number { get; private set; } = null!;

    public Guid CustomerId { get; private set; }

    public Guid EquipmentId { get; private set; }

    public WorkOrderStatus Status { get; private set; }

    public static WorkOrder Create(
        string number,
        Guid customerId,
        Guid equipmentId)
    {
        if (string.IsNullOrWhiteSpace(number))
            throw new ArgumentException(
                "El número de la orden es obligatorio.",
                nameof(number));

        if (customerId == Guid.Empty)
            throw new ArgumentException(
                "El cliente es obligatorio.",
                nameof(customerId));

        if (equipmentId == Guid.Empty)
            throw new ArgumentException(
                "El equipo es obligatorio.",
                nameof(equipmentId));

        return new WorkOrder(
            Guid.NewGuid(),
            number,
            customerId,
            equipmentId);
    }

    public void Start()
    {
        if (Status != WorkOrderStatus.Pending)
            throw new InvalidOperationException(
                "Solo una orden pendiente puede iniciarse.");

        Status = WorkOrderStatus.InProgress;
    }

    public void Complete()
    {
        if (Status != WorkOrderStatus.InProgress)
            throw new InvalidOperationException(
                "Solo una orden en progreso puede completarse.");

        Status = WorkOrderStatus.Completed;
    }
}
