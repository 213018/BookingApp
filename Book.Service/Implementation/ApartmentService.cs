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
    public class ApartmentService : IApartmentService
    {
        private readonly IRepository<Reservation> _reservationRepository;
        private readonly IRepository<Apartment> _apartmentRepository;
        private readonly IRepository<BookingList> _bookingListRepository;
        private readonly IUserRepository _userRepository;

        public ApartmentService(IRepository<Reservation> reservationRepository, IRepository<Apartment> apartmentRepository, IRepository<BookingList> bookingListRepository, IUserRepository userRepository)
        {
            _reservationRepository = reservationRepository;
            _apartmentRepository = apartmentRepository;
            _bookingListRepository = bookingListRepository;
            _userRepository = userRepository;
        }

        public void CreateNewApartment(Apartment p)
        {
            _apartmentRepository.Insert(p);
        }

        public void DeleteApartment(Guid id)
        {
            var apartment = _apartmentRepository.Get(id);
            _apartmentRepository.Delete(apartment);
        }

        public List<Apartment> GetAllApartments()
        {
            return _apartmentRepository.GetAll().ToList();
        }

        public Apartment GetDetailsForApartment(Guid? id)
        {
            return _apartmentRepository.Get(id);
        }

        public void UpdateExistingApartment(Apartment p)
        {
            _apartmentRepository.Update(p);
        }
    }
}
