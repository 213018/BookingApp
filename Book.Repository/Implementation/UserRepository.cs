using Book.Domain.Identity;
using Book.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Repository.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<BookingApplicationUser> entities;
        string ErrorMessage = string.Empty;

        public UserRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<BookingApplicationUser>();
        }

        public void Delete(BookingApplicationUser entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");

            context.Remove(entity);
            context.SaveChanges();
        }

        public BookingApplicationUser Get(string? id)
        {
            return entities
                .Include(z => z.BookingList)
                .Include("BookingList.BookReservations")
                .Include("BookingList.BookReservations.Reservation")
                .SingleOrDefault(z => z.Id == id);
        }

        public IEnumerable<BookingApplicationUser> GetAll()
        {
            return entities.AsEnumerable();
        }

        public void Insert(BookingApplicationUser entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");

            context.Add(entity);
            context.SaveChanges();
        }

        public void Update(BookingApplicationUser entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");

            context.Update(entity);
            context.SaveChanges();
        }
    }
}
