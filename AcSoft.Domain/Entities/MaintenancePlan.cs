using AcSoft.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcSoft.Domain.Entities;

public sealed class MaintenancePlan
{
    private readonly List<MaintenancePlanActivity> _activities = [];

    private MaintenancePlan()
    {
    }

    private MaintenancePlan(
        Guid id,
        string code,
        string name,
        string? description)
    {
        Id = id;
        Code = code;
        Name = name;
        Description = description;
        Status = MaintenancePlanStatus.Draft;
    }

    public Guid Id { get; private set; }

    public string Code { get; private set; } = null!;

    public string Name { get; private set; } = null!;

    public string? Description { get; private set; }

    public MaintenancePlanStatus Status { get; private set; }

    public IReadOnlyCollection<MaintenancePlanActivity> Activities =>
        _activities.AsReadOnly();

    public static MaintenancePlan Create(
        string code,
        string name,
        string? description = null)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentException(
                "El código del plan es obligatorio.",
                nameof(code));
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(
                "El nombre del plan es obligatorio.",
                nameof(name));
        }

        return new MaintenancePlan(
            Guid.NewGuid(),
            code.Trim(),
            name.Trim(),
            description?.Trim());
    }

    public void AddActivity(
        Guid activityId,
        ExecutionMoment moment,
        int sequence)
    {
        EnsurePlanCanBeModified();

        var sequenceAlreadyExists = _activities.Any(
            x => x.Moment == moment && x.Sequence == sequence);

        if (sequenceAlreadyExists)
        {
            throw new InvalidOperationException(
                "Ya existe una actividad con esa secuencia para el momento indicado.");
        }

        var planActivity = MaintenancePlanActivity.Create(
            maintenancePlanId: Id,
            activityId: activityId,
            moment: moment,
            sequence: sequence);

        _activities.Add(planActivity);
    }

    public void Activate()
    {
        if (Status != MaintenancePlanStatus.Draft)
        {
            throw new InvalidOperationException(
                "Solo un plan en borrador puede activarse.");
        }

        if (_activities.Count == 0)
        {
            throw new InvalidOperationException(
                "No se puede activar un plan sin actividades.");
        }

        Status = MaintenancePlanStatus.Active;
    }

    public void Deactivate()
    {
        if (Status != MaintenancePlanStatus.Active)
        {
            throw new InvalidOperationException(
                "Solo un plan activo puede desactivarse.");
        }

        Status = MaintenancePlanStatus.Inactive;
    }

    private void EnsurePlanCanBeModified()
    {
        if (Status != MaintenancePlanStatus.Draft)
        {
            throw new InvalidOperationException(
                "Solo se pueden modificar las actividades de un plan en borrador.");
        }
    }
}
