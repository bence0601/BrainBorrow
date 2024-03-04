using System.ComponentModel.DataAnnotations;

namespace BrainBorrowAPI.Contracts
{
    public record RegistrationRequest(
        [Required] string Email,
        [Required] string Username,
        [Required] string Password);
}
