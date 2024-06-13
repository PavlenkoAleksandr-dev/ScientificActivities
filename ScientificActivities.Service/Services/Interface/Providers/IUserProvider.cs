﻿using ScientificActivities.Data.Models.UserActivity;

namespace ScientificActivities.Service.Services.Interface.Providers;

public interface IUserProvider : IProvider<User>
{
    Task<User?> FindAsync(string name, CancellationToken cancellationToken);
}