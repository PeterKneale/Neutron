using System;
using Services.Tenant.Models;
using ServiceStack;
using ServiceStack.OrmLite;
using Services.Common;

namespace Services.Tenant
{
    public class GetTenantService : ServiceStack.Service
    {
        public GetTenantResponse Get(GetTenant request)
        {
            using (var transaction = Db.OpenTransaction())
            {
                var data = Db.SingleById<TenantData>(request.Id);
                if (data == null)
                {
                    throw HttpError.NotFound("Tenant does not exist");
                }
                
                var Tenant = data.ConvertTo<TenantModel>();
                return new GetTenantResponse { Tenant = Tenant };
            }
        }
    }
}