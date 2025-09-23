using OneInteg.Server.Domain.Entities;

namespace OneInteg.Server.Services;

public interface IPaymentProvider
{
    public Task<Subscription?> HandleSubscription(Subscription data);
    public Task<Subscription> CreateSubscription(Customer customer, Subscription data);
    public Task<Subscription> GetSubscription(Subscription data);
}