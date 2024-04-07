using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Diagnostics;
using Book.Domain;
using Book.Service.Interface;

namespace BookingApplication.Controllers
{
    public class BookingListsController : Controller
    {
        private readonly IBookingListService _bookingListService;

        public BookingListsController(IBookingListService bookingListService)
        {
            _bookingListService = bookingListService;
        }


        // GET: BookingLists
        public IActionResult Index()
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var dto = _bookingListService.getBookingListInfo(userId);
            return View(dto);


        }

        public IActionResult bookNow()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            _bookingListService.bookNow(userId);

            return RedirectToAction("Index");

        }

        public IActionResult DeleteFromListSingle(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            _bookingListService.deleteReservationFromBookingList(userId, id);

            return RedirectToAction("Index");

        }
    }
}
