using System;
using ServiceStack;

namespace Services.Tenant.Models
{
    [Route("/Tenant", "POST", Summary = "Create an Tenant")]
    public class CreateTenant : IReturn<CreateTenantResponse>
    {
        [ApiMember(Name = "Name", Description = "Name", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Name { get; set; }
    }

    public class CreateTenantResponse
    {
        public TenantModel Tenant { get; set; }
        public string Result { get; set; }
    }

    public class TenantCreatedEvent
    {
        public long Id { get; set; }
    }
}
