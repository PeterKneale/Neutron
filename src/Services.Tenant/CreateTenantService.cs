using Services.Tenant.Models;
using ServiceStack;
using ServiceStack.Messaging;
using ServiceStack.OrmLite;
using Services.Common;

namespace Services.Tenant
{
    public class CreateTenantService : ServiceStack.Service
    {
        private readonly IBus _bus;
        
        public CreateTenantService(IBus bus)
        {
            _bus = bus;
        }

        public CreateTenantResponse Post(CreateTenant request)
        {
            var id = Db.Insert(new TenantData { Name = request.Name }, selectIdentity: true);

            _bus.Publish<TenantCreatedEvent>(new TenantCreatedEvent { Id = id });
            
            var model = Db.SingleById<TenantData>(id).ConvertTo<TenantModel>();
            return new CreateTenantResponse { Tenant = model };
        }
    }
}