using Services.Common;
using Services.User.Models;
using ServiceStack;
using ServiceStack.Messaging;
using ServiceStack.OrmLite;

namespace Services.User
{
    public class CreateUserService : ServiceStack.Service
    {
        private readonly IBus _bus;
        
        public CreateUserService(IBus bus)
        {
            _bus = bus;
        }

        public CreateUserResponse Post(CreateUser request)
        {
            var id = Db.Insert(new UserData { FirstName = request.FirstName, LastName = request.LastName }, selectIdentity: true);

            _bus.Publish<UserCreatedEvent>(new UserCreatedEvent { Id = id });
            
            var model = Db.SingleById<UserData>(id).ConvertTo<UserModel>();
            return new CreateUserResponse { User = model };
        }
        
        public void Post(UserCreatedEvent request)
        {
            
        }
    }
}