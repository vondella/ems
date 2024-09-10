﻿namespace Evently.Modules.Events.Application.Abstractions.Data;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancelletionToken = default);
}