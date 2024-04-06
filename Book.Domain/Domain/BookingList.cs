using Book.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Domain.Domain
{
    public class BookingList : BaseEntity
    {

        public string UserId { get; set; }

        public BookingApplicationUser? User { get; set; }

        public virtual ICollection<BookReservation>? BookReservations { get; set; }

    }
}
