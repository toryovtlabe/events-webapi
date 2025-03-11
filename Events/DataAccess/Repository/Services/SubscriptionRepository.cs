using DataAccess.Entity;

namespace DataAccess.Repository.Service
{
    public class SubscriptionRepository(ApplicationContext context) : Repository<Subscription>(context)
    {
    }
}
