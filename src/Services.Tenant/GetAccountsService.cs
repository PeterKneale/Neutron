using Services.Tenant.Models;
using ServiceStack;
using ServiceStack.OrmLite;
using System.Linq;

namespace Services.Tenant
{
    public class GetTenantsService : Service
    {
        public GetTenantsResponse Get(GetTenants request)
        {
            using (var transaction = Db.OpenTransaction())
            {
                var data = Db.Select<TenantData>();
                var count = Db.Count<TenantData>();
                
                var Tenants = data.Select(x=>x.ConvertTo<TenantModel>()).ToArray();
                return new GetTenantsResponse { Tenants = Tenants, Total = count };
            }
        }

    }
}