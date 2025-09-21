using _1Integ.Server.Entities;

namespace _1Integ.Server.Domain;

public interface IPaymentProvider
{
    public Task<Subscription?> HandleSubscription(Subscription data);
    public Task<Subscription> CreateSubscription(Subscription data);
    public Task<Subscription> GetSubscription(Subscription data);
}