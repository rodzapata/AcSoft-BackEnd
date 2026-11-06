using AcSoft.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcSoft.Infrastructure.Persistence;

public class AcDbContext : DbContext
{
    public AcDbContext(DbContextOptions<AcDbContext> options)
        : base(options)
    {
    }

    public DbSet<WorkOrder> WorkOrders => Set<WorkOrder>();

    public DbSet<MaintenancePlan> MaintenancePlans =>
        Set<MaintenancePlan>();

    public DbSet<MaintenancePlanActivity> MaintenancePlanActivities =>
        Set<MaintenancePlanActivity>();

    public DbSet<Activity> Activities => Set<Activity>();

    public DbSet<ActivityQuestion> ActivityQuestions =>
        Set<ActivityQuestion>();

    public DbSet<Question> Questions => Set<Question>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AcDbContext).Assembly);
    }
}
