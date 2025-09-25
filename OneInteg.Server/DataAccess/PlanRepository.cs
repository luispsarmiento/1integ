using OneInteg.Server.Domain.Repositories;

namespace OneInteg.Server.DataAccess
{
    public partial class PlanRepository : BaseRepository<Plan>, IPlanRepository
    {
        public PlanRepository(OneIntegDbContext context) : base(context)
        {
        }
    }
}
