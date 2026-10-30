using System;
using System.Collections.Generic;
using System.Text;

namespace AcSoft.Application.Abstractions.Services;

public interface IWorkOrderNumberGenerator
{
    Task<string> GenerateAsync(
        CancellationToken cancellationToken = default);
}