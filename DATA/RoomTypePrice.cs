namespace ReservationSystem.API.Models
{
    public class RoomTypePrice
    {
        public int PriceId { get; set; }

        public int RoomId { get; set; }
        public Room Room { get; set; }  

        public decimal Price { get; set; }
        public DateTime Date { get; set; } 
    }
}
