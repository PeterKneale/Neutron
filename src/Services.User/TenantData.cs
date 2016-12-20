using System;
using ServiceStack.DataAnnotations;

namespace Services.User
{
    public class UserData
    {
        [AutoIncrement]
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
