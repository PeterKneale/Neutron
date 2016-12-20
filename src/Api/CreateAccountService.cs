using ServiceStack;
using ServiceStack.OrmLite;

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
            var id = Db.Insert(new AccountData { Name = request.Name }, selectIdentity: true);

            _bus.Publish<AccountCreatedEvent>(new AccountCreatedEvent { Id = id });
            
            var model = Db.SingleById<AccountData>(id).ConvertTo<AccountModel>();
            return new CreateAccountResponse { Account = model };
        }
    }
}