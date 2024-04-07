using Book.Domain.Domain;
using Book.Repository.Interface;
using Book.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Service.Implementation
{
    public class ReservationService : IReservationService
    {
        private readonly IRepository<Reservation> _reservationRepository;
        private readonly IRepository<Apartment> _apartmentRepository;
        private readonly IRepository<BookingList> _bookingListRepository;
        private readonly IUserRepository _userRepository;

        public ReservationService(IRepository<Reservation> reservationRepository, IRepository<Apartment> apartmentRepository, IRepository<BookingList> bookingListRepository, IUserRepository userRepository)
        {
            _reservationRepository = reservationRepository;
            _apartmentRepository = apartmentRepository;
            _bookingListRepository = bookingListRepository;
            _userRepository = userRepository;
        }

        public void CreateNewReservation(Reservation p)
        {
            _reservationRepository.Insert(p);
        }

        public void DeleteReservation(Guid id)
        {
            var reservation = _reservationRepository.Get(id);
           
            _reservationRepository.Delete(reservation);
           
        }

        public List<Reservation> GetAllReservations()
        {
            return _reservationRepository.GetAll("Apartment").ToList();
        }

        public Reservation GetDetailsForReservation(Guid? id)
        {
            return _reservationRepository.Get(id);
        }

        public void UpdateExistingReservation(Reservation p)
        {
            _reservationRepository.Update(p);
        }
    }
}
