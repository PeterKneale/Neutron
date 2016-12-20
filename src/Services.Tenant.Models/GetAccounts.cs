using System;
using ServiceStack;

namespace Services.Tenant.Models
{
    [Route("/Tenant", "GET", Summary = "Get all Tenants")]
    public class GetTenants : IReturn<GetTenantsResponse>
    {

    }

    public class GetTenantsResponse
    {
        public TenantModel[] Tenants { get; set; }
        public long Total { get; set; }
        public string Result { get; set; }
    }
}