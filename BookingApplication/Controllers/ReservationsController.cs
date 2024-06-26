﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Book.Service.Interface;
using Book.Domain.Domain;
using System.Diagnostics;

namespace BookingApplication.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly IReservationService _reservationService;
        private readonly IBookingListService _bookingListService;
        private readonly IApartmentService _apartmentService;

        public ReservationsController(IReservationService reservationService, IBookingListService bookingListService, IApartmentService apartmentService)
        {
            _reservationService = reservationService;
            _bookingListService = bookingListService;
            _apartmentService = apartmentService;
        }




        // GET: Reservations
        public IActionResult Index()
        {
            return View(_reservationService.GetAllReservations());
        }

        public IActionResult AddToList(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = _reservationService.GetDetailsForReservation(id);


            BookReservation bookReservation = new BookReservation();

            if (reservation != null)
            {
                bookReservation.ReservationId = reservation.Id;
                bookReservation.Reservation = reservation;
              
            }

            return View(bookReservation);
        }

        [HttpPost]
        public IActionResult AddToListConfirmed(BookReservation model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

           _bookingListService.AddToBookingListConfirmed(model, userId);
            return View("Index", _reservationService.GetAllReservations());
        }

        // GET: Reservations/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

           var reservation = _reservationService.GetDetailsForReservation(id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservations/Create
        public IActionResult Create()
        {
            ViewData["ApartmentId"] = new SelectList(_apartmentService.GetAllApartments(), "Id", "ApartmentName");
            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Check_in_date,ApartmentId")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                var apartment = _apartmentService.GetDetailsForApartment(reservation.ApartmentId);
                
                if (apartment != null)
                    reservation.Apartment = apartment;


                reservation.Id = Guid.NewGuid();

                _reservationService.CreateNewReservation(reservation);
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApartmentId"] = new SelectList(_apartmentService.GetAllApartments(), "Id", "ApartmentName", reservation.ApartmentId);
            return View(reservation);
        }

        // GET: Reservations/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = _reservationService.GetDetailsForReservation(id);
            if (reservation == null)
            {
                return NotFound();
            }
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Id,Check_in_date,ApartmentId")] Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _reservationService.UpdateExistingReservation(reservation);
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = _reservationService.GetDetailsForReservation(id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            _reservationService.DeleteReservation(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
