﻿namespace NafanyaVPN.Entities.Subscription;

public interface ISubscriptionService
{
    Task<SubscriptionPlan> GetAsync(string name);
    Task UpdateAsync(SubscriptionPlan model);
}