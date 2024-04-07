using Book.Domain.Domain;
using Book.Domain.DTO;
using Book.Repository.Interface;
using Book.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Microsoft.EntityFrameworkCore;

namespace Book.Service.Implementation
{
    public class BookingListService : IBookingListService
    {
        private readonly IRepository<Reservation> _reservationRepository;
        private readonly IRepository<Apartment> _apartmentRepository;
        private readonly IRepository<BookingList> _bookingListRepository;
        private readonly IUserRepository _userRepository;

        public BookingListService(IRepository<Reservation> reservationRepository, IRepository<Apartment> apartmentRepository, IRepository<BookingList> bookingListRepository, IUserRepository userRepository)
        {
            _reservationRepository = reservationRepository;
            _apartmentRepository = apartmentRepository;
            _bookingListRepository = bookingListRepository;
            _userRepository = userRepository;
        }

        public bool AddToBookingListConfirmed(BookReservation model, string userId)
        {
            var loggedInUser = _userRepository.Get(userId);

            var bookingList = loggedInUser.BookingList;

            if (bookingList.BookReservations == null)
            {
                bookingList.BookReservations = new List<BookReservation>();
            }
            bookingList.BookReservations.Add(model);
            _bookingListRepository.Update(bookingList);
            return true;
        }

        public bool bookNow(string userId)
        {
            var loggedInUser = _userRepository.Get(userId);

            var bookingList = loggedInUser.BookingList;

            foreach(var item in bookingList.BookReservations)
            {
                bookingList.BookReservations.Remove(item);
            }
            _bookingListRepository.Update(bookingList);
            return true;
        }

        public bool deleteReservationFromBookingList(string userId, Guid reservationId)
        {
            if (reservationId != null)
            {
                var loggedInUser = _userRepository.Get(userId);

                var bookingList = loggedInUser.BookingList;

                var reservation = bookingList.BookReservations.Where(x => x.ReservationId == reservationId).FirstOrDefault();

                bookingList.BookReservations.Remove(reservation);
                _bookingListRepository.Update(bookingList);
                return true;
            }
            return false;
        }

        public BookingListDTO getBookingListInfo(string userId)
        {
            var loggedInUser = _userRepository.Get(userId);

            var bookingList = loggedInUser.BookingList;

            var allReservations = bookingList?.BookReservations?.ToList();

            var totalPrice = 0;
            foreach (var resr in allReservations)
            {
                var apartment = _apartmentRepository.Get(resr.Reservation.ApartmentId);
                resr.Reservation.Apartment = apartment;
                totalPrice += apartment.Price_per_night * resr.Number_of_nights;
            }

            BookingListDTO dto = new BookingListDTO();

            dto.TotalPrice = totalPrice;
            dto.Reservations = allReservations;
           
            return dto;
        }
    }
}
