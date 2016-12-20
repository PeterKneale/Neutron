using System;
using ServiceStack;

namespace Api.Models
{
    [Route("/account", "POST", Summary = "Create an Account")]
    public class CreateAccount : IReturn<CreateAccountResponse>
    {
        [ApiMember(Name = "Name", Description = "Account Name", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Name { get; set; }
        
        [ApiMember(Name = "FirstName", Description = "First Name", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string FirstName { get; set; }
        
        [ApiMember(Name = "LastName", Description = "Last Name", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string LastName { get; set; }
    }

    public class CreateAccountResponse
    {
        public string Result { get; set; }
    }
}
