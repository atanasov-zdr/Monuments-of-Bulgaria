namespace MB.Models
{
    using Microsoft.AspNetCore.Identity;

    public class MbUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
