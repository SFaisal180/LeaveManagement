using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagement.Contracts
{
    public interface IRepositoryBase <T> where T : class 
    {

        //Synchronus
        ICollection<T> FindAlll();

        T FindByIdd(int id);
        //int CreateByInt(T entity);
        bool Createe(T entity);
        bool Updatee(T entity);
        bool Deletee(T entity);
        bool Savee();

        bool IsExistss(int id);


        //Asynchronus

        Task<ICollection<T>> FindAll();

        Task<T> FindById(int id);
        //int CreateByInt(T entity);
        Task<bool> Create(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(T entity);
        Task<bool> Save();

        Task<bool> IsExists(int id);
    }
}
