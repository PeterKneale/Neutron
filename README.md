# Cosmos
ServiceStack API + Rabbit

```
docker run -d --hostname rabbit --name rabbit -p 8080:15672 -p 5672:5672 rabbitmq:3-management
```

# Solution
  Service.Api
  Service.Api.Models
    CreateAccount
    CreateAccountResponse

  Service.Tenant
    TenantService
    UserService

  Service.Tenant.Models
    CreateTenant
    CreateTenantResponse
    TenantCreatedEvent
    AddUserToTenant
    AddUserToTenantResponse
    TenantCreatedEvent
    TenantUpdatedEvent

  Service.User
  Service.User.Models
    CreateUser
    CreateUserResponse
    UserCreatedEvent

mkdir src
mkdir src/Api
mkdir src/Api.Host
mkdir src/Api.Models
mkdir src/Services.Host
mkdir src/Services.Tenant
mkdir src/Services.Tenant.Models
mkdir src/Services.User
mkdir src/Services.User.Models

cd src/Api
dotnet new --type lib
cd ../../

cd src/Api.Models
dotnet new --type lib
cd ../../

cd src/Api.Host
dotnet new --type console
cd ../../

cd src/Services.Host
dotnet new --type console
cd ../../

cd src/Services.Tenant
dotnet new --type lib
cd ../../

cd src/Services.Tenant.Models
dotnet new --type lib
cd ../../

cd src/Services.User
dotnet new --type lib
cd ../../

cd src/Services.User.Models
dotnet new --type lib
cd ../../
