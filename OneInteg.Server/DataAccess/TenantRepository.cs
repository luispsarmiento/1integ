
using OneInteg.Server.Domain.Repositories;

namespace OneInteg.Server.DataAccess
{
    public partial class TenantRepository : BaseRepository<Tenant>, ITenantRepository
    {
        public TenantRepository(OneIntegDbContext context) : base(context)
        {
        }
    }
}
