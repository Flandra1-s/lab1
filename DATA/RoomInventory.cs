namespace ReservationSystem.API.Models
{
    public class RoomInventory
    {
        public int RoomInventoryId { get; set; }
        public int RoomId { get; set; }
        public DateTime Date { get; set; }
        public bool IsAvailable { get; set; }

       
        public Room Room { get; set; }
    }
}
