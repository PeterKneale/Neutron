using System;
using ServiceStack;

namespace Services.Tenant.Models
{
    [Route("/Tenant", "DELETE", Summary = "Delete Tenants")]
    public class DeleteTenants : IReturn<DeleteTenantResponse>
    {
        
    }

    public class DeleteTenantsResponse
    {
        public string Result { get; set; }
    }

    public class TenantsDeletedEvent
    {
    }
}