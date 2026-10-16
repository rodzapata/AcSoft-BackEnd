using AcSoft.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcSoft.Infrastructure.Persistence;

public class AcDbContext: DbContext
{
    public AcDbContext(DbContextOptions<AcDbContext> options) : base(options)
    {
        
    }

    protected AcDbContext()
    {

    }

    public DbSet<WorkOrder> WorkOrders { get; set; }


}
