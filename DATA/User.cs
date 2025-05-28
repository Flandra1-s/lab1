using System.ComponentModel.DataAnnotations;

namespace ReservationSystem.API.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Emri i plote eshte i detyrueshem.")]
        [MaxLength(100, ErrorMessage = "Emri nuk mund te jete me i gjate se 100 karaktere.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email-i eshte i detyrueshem.")]
        [EmailAddress(ErrorMessage = "Email-i nuk eshte ne format te sakte.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Fjalekalimi eshte i detyrueshem.")]
        [MinLength(6, ErrorMessage = "Fjalekalimi duhet te kete te pakten 6 karaktere.")]
        public string PasswordHash { get; set; }

        [Required(ErrorMessage = "Roli eshte i detyrueshem.")]
        [RegularExpression("^(User|Admin)$", ErrorMessage = "Roli duhet te jete 'User' ose 'Admin'.")]
        public string Role { get; set; } = "User";

        public List<Reservation>? Reservations { get; set; }
    }
}
