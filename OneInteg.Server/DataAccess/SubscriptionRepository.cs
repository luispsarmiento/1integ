
using OneInteg.Server.Domain.Repositories;

namespace OneInteg.Server.DataAccess
{
    public partial class SubscriptionRepository : BaseRepository<Subscription>, ISubscriptionRepository
    {
        public SubscriptionRepository(OneIntegDbContext context) : base(context)
        {
        }
    }
}
