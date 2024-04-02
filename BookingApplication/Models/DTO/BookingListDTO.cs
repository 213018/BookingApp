namespace BookingApplication.Models.DTO
{
    public class BookingListDTO
    {
        public List<BookReservation>? Reservations { get; set; }
        public int TotalPrice { get; set; }
    }
}
