using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookingApplication.Data;
using BookingApplication.Models;
using System.Security.Claims;
using BookingApplication.Models.DTO;
using System.Diagnostics;

namespace BookingApplication.Controllers
{
    public class BookingListsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingListsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BookingLists
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var loggedInUser = _context.Users
            //    .Include(x => x.BookingList)
            //    .Include("BookingList.BookReservations")
            //    .Include("BookingList.BookReservations.Reservation")
            //    .FirstOrDefault(z => z.Id == userId);

            var loggedInUser = await _context.Users
                .Include(x => x.BookingList)
                    .ThenInclude(bl => bl.BookReservations)
                        .ThenInclude(br => br.Reservation)
                            .ThenInclude(r => r.Apartment) // Include the Apartment for each Reservation
            .FirstOrDefaultAsync(z => z.Id == userId);

            var userList = loggedInUser?.BookingList;
            var allReservations = userList?.BookReservations?.ToList();


            //var totalPrice = allReservations.Select(x => (x.Reservation.Apartment.Price_per_night * x.Number_of_nights)).Sum();

            var totalPrice = allReservations
            .Select(x =>
            {
                var apartment = x.Reservation.Apartment;
                return apartment != null ? apartment.Price_per_night * x.Number_of_nights : 0;
             })
             .Sum();



            BookingListDTO dto = new BookingListDTO
            {
                Reservations = allReservations,
                TotalPrice = totalPrice
            };

            return View(dto);


        }

        public IActionResult bookNow()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var loggedInUser = _context.Users
                .Include(x => x.BookingList)
                .Include("BookingList.BookReservations")
                .Include("BookingList.BookReservations.Reservation")
                .FirstOrDefault(z => z.Id == userId);


            var userList = loggedInUser?.BookingList;
            var allReservations = userList?.BookReservations?.ToList();

            foreach (var resr in allReservations)
            {
                userList.BookReservations.Remove(resr);
            }

            _context.BookingLists.Update(userList);
            _context.SaveChanges();

            return RedirectToAction("Index");

        }

        public IActionResult DeleteFromListSingle(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var loggedInUser = _context.Users
               .Include(x => x.BookingList)
               .Include("BookingList.BookReservations")
               .Include("BookingList.BookReservations.Reservation")
               .FirstOrDefault(z => z.Id == userId);


            var userList = loggedInUser?.BookingList;

            var reservation = userList.BookReservations.Where(x => x.ReservationId == id).FirstOrDefault();

           

            userList.BookReservations.Remove(reservation);
            _context.BookingLists.Update(userList);
            _context.SaveChanges();

            return RedirectToAction("Index");

        }



        // GET: BookingLists/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookingList = await _context.BookingLists
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bookingList == null)
            {
                return NotFound();
            }

            return View(bookingList);
        }

        // GET: BookingLists/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: BookingLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId")] BookingList bookingList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bookingList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", bookingList.UserId);
            return View(bookingList);
        }

        // GET: BookingLists/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookingList = await _context.BookingLists.FindAsync(id);
            if (bookingList == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", bookingList.UserId);
            return View(bookingList);
        }

        // POST: BookingLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid? id, [Bind("Id,UserId")] BookingList bookingList)
        {
            if (id != bookingList.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookingList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingListExists(bookingList.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", bookingList.UserId);
            return View(bookingList);
        }

        // GET: BookingLists/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookingList = await _context.BookingLists
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bookingList == null)
            {
                return NotFound();
            }

            return View(bookingList);
        }

        // POST: BookingLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid? id)
        {
            var bookingList = await _context.BookingLists.FindAsync(id);
            if (bookingList != null)
            {
                _context.BookingLists.Remove(bookingList);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingListExists(Guid? id)
        {
            return _context.BookingLists.Any(e => e.Id == id);
        }
    }
}
