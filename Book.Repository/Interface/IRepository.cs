using Book.Domain.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Repository.Interface
{
    public interface IRepository<T> where T : BaseEntity
    {
        public IEnumerable<T> GetAll(string includeProperties = "");
        T Get(Guid? id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
