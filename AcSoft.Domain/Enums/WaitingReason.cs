using System;
using System.Collections.Generic;
using System.Text;

namespace AcSoft.Domain.Enums;

public enum WaitingReason
{
    Customer = 1,
    SparePart = 2,
    Authorization = 3,
    EquipmentShutdown = 4,
    Access = 5,
    Other = 99
}
