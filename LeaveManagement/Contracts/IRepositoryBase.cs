using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagement.Contracts
{
    public interface IRepositoryBase <T> where T : class 
    {
        ICollection<T> FindAll();

        T FindById(int id);
        //int CreateByInt(T entity);
        bool Create(T entity);
        bool Update(T entity);
        bool Delete(T entity);
        bool Save();

        bool IsExists(int id);
    }
}
