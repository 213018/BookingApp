using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Book.Domain.Domain;
using Book.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace BookingApplication.Controllers
{
    public class ApartmentsController : Controller
    {
        private readonly IApartmentService _apartmentSerice;

        public ApartmentsController(IApartmentService apartmentSerice)
        {
            this._apartmentSerice = apartmentSerice;
        }


        // GET: Apartments
        public IActionResult Index()
        {
            return View(_apartmentSerice.GetAllApartments());
        }

        // GET: Apartments/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apartment = _apartmentSerice.GetDetailsForApartment(id);
            if (apartment == null)
            {
                return NotFound();
            }

            return View(apartment);
        }

        // GET: Apartments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Apartments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,ApartmentName,City,Description,Price_per_night,Rating")] Apartment apartment)
        {
            if (ModelState.IsValid)
            {
                apartment.Id = Guid.NewGuid();
                _apartmentSerice.CreateNewApartment(apartment);
                return RedirectToAction(nameof(Index));
            }
            return View(apartment);
        }

        // GET: Apartments/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apartment = _apartmentSerice.GetDetailsForApartment(id);
            if (apartment == null)
            {
                return NotFound();
            }
            return View(apartment);
        }

        // POST: Apartments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Id,ApartmentName,City,Description,Price_per_night,Rating")] Apartment apartment)
        {
            if (id != apartment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _apartmentSerice.UpdateExistingApartment(apartment);
                }
                catch (DbUpdateConcurrencyException)
                {
                   
                    throw;
                    
                }
                return RedirectToAction(nameof(Index));
            }
            return View(apartment);
        }

        // GET: Apartments/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apartment = _apartmentSerice.GetDetailsForApartment(id);
            if (apartment == null)
            {
                return NotFound();
            }

            return View(apartment);
        }

        // POST: Apartments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            _apartmentSerice.DeleteApartment(id);
            return RedirectToAction(nameof(Index));
        }

       
    }
}
