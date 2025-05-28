using System.ComponentModel.DataAnnotations;

namespace ReservationSystem.API.Models
{
    public class Hotel
    {
        public int HotelId { get; set; }

        [Required(ErrorMessage = "Emri i hotelit eshte i detyrueshem.")]
        [MaxLength(100, ErrorMessage = "Emri nuk mund te jete me i gjate se 100 karaktere.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Adresa eshte e detyrueshme.")]
        [MaxLength(200, ErrorMessage = "Adresa nuk mund te jete me e gjate se 200 karaktere.")]
        public string Address { get; set; }

        public List<Room>? Rooms { get; set; }
    }
}
