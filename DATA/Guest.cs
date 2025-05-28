namespace ReservationSystem.API.Models
{
    public class Guest
    {
        public int GuestId { get; set; }            
        public string FullName { get; set; } = string.Empty; 
        public string Email { get; set; } = string.Empty;     
        public string PhoneNumber { get; set; } = string.Empty; 
    }
}
