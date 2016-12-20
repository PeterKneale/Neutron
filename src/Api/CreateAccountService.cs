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
            var tenantResponse = _bus.Send<CreateTenant, CreateTenantResponse>(new CreateTenant{ Name = request.Name });
            var userResponse = _bus.Send<CreateUser, CreateUserResponse>(new CreateUser{ FirstName = request.FirstName, LastName = request.LastName });
            var accountId = tenantResponse.Tenant.Id;
            var userId = userResponse.User.Id;
            
            return new CreateAccountResponse {AccountId = accountId, UserId = userId };
        }
    }
}