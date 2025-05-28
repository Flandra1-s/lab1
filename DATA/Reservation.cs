using System.ComponentModel.DataAnnotations;

namespace ReservationSystem.API.Models
{
    public class Reservation
    {
        public int ReservationId { get; set; }

        [Required(ErrorMessage = "Data e nisjes eshte e detyrueshme.")]
        public DateTime DateFrom { get; set; }

        [Required(ErrorMessage = "Data e perfundimit eshte e detyrueshme.")]
        public DateTime DateTo { get; set; }

        [Required(ErrorMessage = "Perdoruesi eshte i detyrueshem.")]
        public int UserId { get; set; }
        public User? User { get; set; }

        [Required(ErrorMessage = "Dhoma eshte e detyrueshme.")]
        public int RoomId { get; set; }
        public Room? Room { get; set; }

        
        [MaxLength(500, ErrorMessage = "Komenti nuk mund te jete me i gjate se 500 karaktere.")]
        public string? Notes { get; set; }
    }
}
