using OneInteg.Server.Domain.Entities;

namespace OneInteg.Server.Domain.Services
{
    public interface ISubscriptionService : IBaseService<Subscription>
    {
        Task<string> GetCheckoutUrl(Tenant tenant, Customer customer, string planReference);
    }
}
