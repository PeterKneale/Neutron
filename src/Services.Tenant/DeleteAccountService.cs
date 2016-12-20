using Services.Tenant;
using Services.Tenant.Models;
using ServiceStack;
using ServiceStack.Messaging;
using ServiceStack.OrmLite;
using Services.Common;

namespace Services.Tenant
{
    public class DeleteTenantService : Service
    {
        private readonly IBus _bus;
        
        public DeleteTenantService(IBus bus)
        {
            _bus = bus;
        }

        public DeleteTenantResponse Delete(DeleteTenant request)
        {
            Db.DeleteById<TenantData>(request.Id);

            _bus.Publish<TenantDeletedEvent>(new TenantDeletedEvent { Id = request.Id });
            
            return new DeleteTenantResponse();
        }
    }
}