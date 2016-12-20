using Services.Tenant.Models;
using ServiceStack;
using ServiceStack.OrmLite;
using Services.Common;

namespace Services.Tenant
{
    public class DeleteTenantsService : Service
    {
        private readonly IBus _bus;
        
        public DeleteTenantsService(IBus bus)
        {
            _bus = bus;
        }

        public DeleteTenantsResponse Delete(DeleteTenants request)
        {
            Db.DeleteAll<TenantData>();

            _bus.Publish<TenantsDeletedEvent>(new TenantsDeletedEvent());
            
            return new DeleteTenantsResponse();
        }
    }
}