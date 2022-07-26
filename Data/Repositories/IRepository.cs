using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public interface IRepository <T> where T : class
    {
        public long Add(T entity);
        public IEnumerable<T> GetAll();
        public T GetById(long id);
        public bool Update(T entity);
        public bool Delete(long id);
    }
}
