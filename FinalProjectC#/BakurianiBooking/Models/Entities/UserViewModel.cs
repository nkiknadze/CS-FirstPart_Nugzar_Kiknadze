using Microsoft.AspNetCore.Identity;

namespace BakurianiBooking.Models.Entities
{
    public class UserViewModel
    {
        public IdentityUser User { get; set; }
        public IList<string> Roles { get; set; }
    }
}