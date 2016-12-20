using System;
using ServiceStack;

namespace Services.Tenant.Models
{
    [Route("/Tenant/{id}", "GET", Summary = "Get an Tenant")]
    public class GetTenant : IReturn<GetTenantResponse>
    {
        [ApiMember(Name="Id", Description = "Identifier", ParameterType = "path", DataType = "int", IsRequired = true)]
        public int Id { get; set; }
    }

    public class GetTenantResponse
    {
        public TenantModel Tenant { get; set; }
        public string Result { get; set; }
    }
}