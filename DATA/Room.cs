using System.ComponentModel.DataAnnotations;

namespace ReservationSystem.API.Models
{
    public class Room
    {
        public int RoomId { get; set; }

        [Required(ErrorMessage = "Numri i dhomes eshte i detyrueshem.")]
        [Range(1, 9999, ErrorMessage = "Numri i dhomes duhet te jete mes 1 dhe 9999.")]
        public int RoomNumber { get; set; }

        [Required(ErrorMessage = "Lloji i dhomes eshte i detyrueshem.")]
        [MaxLength(50, ErrorMessage = "Lloji nuk mund te jete me i gjate se 50 karaktere.")]
        public string Type { get; set; }

        [Required(ErrorMessage = "Çmimi eshte i detyrueshem.")]
        [Range(1, 10000, ErrorMessage = "Çmimi duhet te jete pozitiv.")]
        public decimal Price { get; set; }

        public int HotelId { get; set; }
        public Hotel? Hotel { get; set; }

        public List<Reservation>? Reservations { get; set; }
    }
}
