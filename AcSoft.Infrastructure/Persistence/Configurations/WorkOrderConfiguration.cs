using AcSoft.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcSoft.Infrastructure.Persistence.Configurations;

public sealed class WorkOrderConfiguration
    : IEntityTypeConfiguration<WorkOrder>
{
    public void Configure(EntityTypeBuilder<WorkOrder> builder)
    {
        builder.ToTable("WorkOrders");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Number)
            .HasMaxLength(30)
            .IsRequired();

        builder.HasIndex(x => x.Number)
            .IsUnique();

        builder.Property(x => x.CustomerId)
            .IsRequired();

        builder.Property(x => x.EquipmentId)
            .IsRequired();

        builder.Property(x => x.Status)
            .IsRequired();

        // Relación con las actividades de la orden.
        builder.HasMany(x => x.Activities)
            .WithOne()
            .HasForeignKey(x => x.WorkOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(x => x.Activities)
            .HasField("_activities")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        // Relación con las asignaciones de técnicos.
        builder.HasMany(x => x.Assignments)
            .WithOne()
            .HasForeignKey(x => x.WorkOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(x => x.Assignments)
            .HasField("_assignments")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
