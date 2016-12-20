using System;
using ServiceStack;

namespace Services.Tenant.Models
{
    [Route("/Tenant/{id}", "DELETE", Summary = "Delete an Tenant")]
    public class DeleteTenant : IReturn<DeleteTenantResponse>
    {
        [ApiMember(Name = "Id", Description = "Identifier", ParameterType = "path", DataType = "int", IsRequired = true)]
        public int Id { get; set; }
    }

    public class DeleteTenantResponse
    {
        public string Result { get; set; }
    }

    public class TenantDeletedEvent
    {
        public int Id { get; set; }
    }
}