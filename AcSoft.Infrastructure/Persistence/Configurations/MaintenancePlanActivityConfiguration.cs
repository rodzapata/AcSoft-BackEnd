using AcSoft.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcSoft.Infrastructure.Persistence.Configurations;

public sealed class MaintenancePlanActivityConfiguration
    : IEntityTypeConfiguration<MaintenancePlanActivity>
{
    public void Configure(EntityTypeBuilder<MaintenancePlanActivity> builder)
    {
        builder.ToTable("MaintenancePlanActivities");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.MaintenancePlanId)
            .IsRequired();

        builder.Property(x => x.ActivityId)
            .IsRequired();

        builder.Property(x => x.Moment)
            .IsRequired();

        builder.Property(x => x.Sequence)
            .IsRequired();

        builder.HasIndex(x => new
        {
            x.MaintenancePlanId,
            x.Moment,
            x.Sequence
        })
        .IsUnique();

        builder.HasOne<MaintenancePlan>()
        .WithMany(x => x.Activities)
        .HasForeignKey(x => x.MaintenancePlanId)
        .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Activity>()
        .WithMany()
        .HasForeignKey(x => x.ActivityId)
        .OnDelete(DeleteBehavior.Restrict);
    }
}
