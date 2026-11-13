using AcSoft.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcSoft.Infrastructure.Persistence.Configurations;

public sealed class WorkOrderAssignmentConfiguration
    : IEntityTypeConfiguration<WorkOrderAssignment>
{
    public void Configure(
        EntityTypeBuilder<WorkOrderAssignment> builder)
    {
        builder.ToTable("WorkOrderAssignments");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.WorkOrderId)
            .IsRequired();

        builder.Property(x => x.TechnicianId)
            .IsRequired();

        builder.Property(x => x.AssignedAt)
            .IsRequired();

        builder.Property(x => x.UnassignedAt);

        builder.Property(x => x.EndReason);

        // Una orden no puede tener dos asignaciones activas.
        builder.HasIndex(x => x.WorkOrderId)
            .IsUnique()
            .HasFilter("\"UnassignedAt\" IS NULL");
    }
}

