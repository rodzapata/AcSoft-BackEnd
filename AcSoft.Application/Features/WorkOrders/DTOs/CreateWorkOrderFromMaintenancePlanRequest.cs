using System;
using System.Collections.Generic;
using System.Text;

namespace AcSoft.Application.Features.WorkOrders.DTOs;

public sealed record CreateWorkOrderFromMaintenancePlanRequest(
    Guid CustomerId,
    Guid EquipmentId,
    Guid MaintenancePlanId);