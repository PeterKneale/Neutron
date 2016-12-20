using ServiceStack;
using Api.Models;
using Services.Common;
using Services.Tenant.Models;
using Services.User.Models;

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
            var createTenantResponse = _bus.Send<CreateTenant, CreateTenantResponse>(new CreateTenant{ Name = request.Name });
            var createUserResponse = _bus.Send<CreateUser, CreateUserResponse>(new CreateUser{ FirstName = request.FirstName, LastName = request.LastName });
            
            return new CreateAccountResponse { };
        }
    }
}