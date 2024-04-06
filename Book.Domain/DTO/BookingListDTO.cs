using Book.Domain.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Domain.DTO
{
    public class BookingListDTO
    {
        public List<BookReservation>? Reservations { get; set; }
        public int TotalPrice { get; set; }
    }
}
