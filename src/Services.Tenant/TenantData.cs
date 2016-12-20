using System;
using ServiceStack.DataAnnotations;

namespace Services.Tenant
{
    public class TenantData
    {
        [AutoIncrement]
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
