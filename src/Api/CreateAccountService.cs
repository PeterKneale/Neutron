using ServiceStack;
using ServiceStack.OrmLite;
using Api.Models;

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
            return new CreateAccountResponse { };
        }
    }
}