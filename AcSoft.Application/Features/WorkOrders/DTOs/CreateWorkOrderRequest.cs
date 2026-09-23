using System;
using System.Collections.Generic;
using System.Text;

namespace AcSoft.Application.Features.WorkOrders.DTOs;

public sealed record CreateWorkOrderRequest(
    Guid CustomerId,
    Guid EquipmentId
);
