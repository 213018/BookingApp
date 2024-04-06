using Book.Domain.Domain;
using Book.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Service.Interface
{
    public interface IBookingListService
    {
        BookingListDTO getBookingListInfo(string userId);
        bool deleteReservationFromBookingList(string userId, Guid reservationId);
        bool bookNow(string userId);
        bool AddToBookingListConfirmed(BookReservation model, string userId);
    }
}
