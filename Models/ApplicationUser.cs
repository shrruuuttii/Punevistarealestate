using Microsoft.AspNetCore.Identity;

namespace PersonalFinanceTracker.Models
{
    /// <summary>
    /// Extended IdentityUser class to include additional user profile information
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// User's contact details (phone number or contact information)
        /// </summary>
        public string ContactDetails { get; set; }

        /// <summary>
        /// User's current location
        /// </summary>
        public string CurrentLocation { get; set; }

        /// <summary>
        /// Message or bio from the user
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Account creation timestamp
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
