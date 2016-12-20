using ServiceStack;
using Api.Models;
using Services.Tenant.Models;

namespace Api
{
    public class CreateAccountService : Service
    {
        private readonly IBus _bus;
        
        public CreateAccountService(IBus bus)
        {
            _bus = bus;
        }

        public CreateAccountResponse Post(CreateAccount request)
        {
            var createAccountResponse = _bus.Send<CreateTenant, CreateTenantResponse>(new CreateTenant{ Name = request.Name });

            return new CreateAccountResponse { };
        }
    }
}