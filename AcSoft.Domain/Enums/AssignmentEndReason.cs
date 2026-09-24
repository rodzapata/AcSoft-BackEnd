using System;
using System.Collections.Generic;
using System.Text;

namespace AcSoft.Domain.Enums;

public enum AssignmentEndReason
{
    Reassigned = 1,
    TechnicianUnavailable = 2,
    TechnicianChanged = 3,
    WorkOrderCancelled = 4,
    Other = 99
}
