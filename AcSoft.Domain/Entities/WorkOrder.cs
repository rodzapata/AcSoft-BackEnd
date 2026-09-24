using AcSoft.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcSoft.Domain.Entities;

public sealed class WorkOrder
{
    private readonly List<WorkOrderAssignment> _assignments = [];

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

    public IReadOnlyCollection<WorkOrderAssignment> Assignments =>
        _assignments.AsReadOnly();

    public Guid? CurrentTechnicianId =>
        _assignments
            .SingleOrDefault(x => x.IsActive)
            ?.TechnicianId;

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

    public void AssignTechnician(
        Guid technicianId,
        DateTimeOffset assignedAt)
    {
        EnsureOrderCanBeAssigned();

        var currentAssignment = _assignments
            .SingleOrDefault(x => x.IsActive);

        if (currentAssignment is not null)
            throw new InvalidOperationException(
                "La orden ya tiene un técnico asignado.");

        var assignment = WorkOrderAssignment.Create(
            technicianId,
            assignedAt);

        _assignments.Add(assignment);

        Status = WorkOrderStatus.Assigned;
    }

    public void UnassignTechnician(
        DateTimeOffset unassignedAt,
        AssignmentEndReason reason)
    {
        var currentAssignment = _assignments
            .SingleOrDefault(x => x.IsActive);

        if (currentAssignment is null)
            throw new InvalidOperationException(
                "La orden no tiene un técnico actualmente asignado.");

        currentAssignment.End(
            unassignedAt,
            reason);

        if (Status == WorkOrderStatus.Assigned)
        {
            Status = WorkOrderStatus.Pending;
        }
    }

    public void ReassignTechnician(
        Guid newTechnicianId,
        DateTimeOffset changedAt)
    {
        EnsureOrderCanBeAssigned();

        var currentAssignment = _assignments
            .SingleOrDefault(x => x.IsActive);

        if (currentAssignment is null)
            throw new InvalidOperationException(
                "La orden no tiene un técnico actualmente asignado.");

        if (currentAssignment.TechnicianId == newTechnicianId)
            throw new InvalidOperationException(
                "El nuevo técnico es el mismo técnico actual.");

        currentAssignment.End(
            changedAt,
            AssignmentEndReason.Reassigned);

        var newAssignment = WorkOrderAssignment.Create(
            newTechnicianId,
            changedAt);

        _assignments.Add(newAssignment);

        Status = WorkOrderStatus.Assigned;
    }

    public void Start()
    {
        if (Status != WorkOrderStatus.Assigned)
            throw new InvalidOperationException(
                "Solo una orden asignada puede iniciar.");

        if (CurrentTechnicianId is null)
            throw new InvalidOperationException(
                "La orden debe tener un técnico asignado.");

        Status = WorkOrderStatus.InProgress;
    }

    public void SetWaiting()
    {
        if (Status != WorkOrderStatus.InProgress)
            throw new InvalidOperationException(
                "Solo una orden en ejecución puede pasar a espera.");

        Status = WorkOrderStatus.Waiting;
    }

    public void Resume()
    {
        if (Status != WorkOrderStatus.Waiting)
            throw new InvalidOperationException(
                "Solo una orden en espera puede reanudarse.");

        if (CurrentTechnicianId is null)
            throw new InvalidOperationException(
                "La orden debe tener un técnico asignado.");

        Status = WorkOrderStatus.InProgress;
    }

    public void Complete()
    {
        if (Status != WorkOrderStatus.InProgress)
            throw new InvalidOperationException(
                "Solo una orden en ejecución puede completarse.");

        Status = WorkOrderStatus.Completed;
    }

    public void Cancel()
    {
        if (Status == WorkOrderStatus.Completed)
            throw new InvalidOperationException(
                "Una orden completada no puede cancelarse.");

        if (Status == WorkOrderStatus.Cancelled)
            throw new InvalidOperationException(
                "La orden ya está cancelada.");

        Status = WorkOrderStatus.Cancelled;
    }

    private void EnsureOrderCanBeAssigned()
    {
        if (Status == WorkOrderStatus.Completed)
            throw new InvalidOperationException(
                "No se puede asignar un técnico a una orden completada.");

        if (Status == WorkOrderStatus.Cancelled)
            throw new InvalidOperationException(
                "No se puede asignar un técnico a una orden cancelada.");
    }
}