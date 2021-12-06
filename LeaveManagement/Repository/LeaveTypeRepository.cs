using LeaveManagement.Contracts;
using LeaveManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagement.Repository
{
    public class LeaveTypeRepository : ILeaveTypeRepository
    {

        public ApplicationDbContext _context;
        public LeaveTypeRepository(ApplicationDbContext _context)
        {
            this._context = _context;
        }
        public bool Create(LeaveType entity)
        {
            _context.Add(entity);
           // _context.SaveChanges();
           return Save();
        }

        public bool Delete(LeaveType entity)
        {
            _context.LeaveTypes.Remove(entity);
           var _entity = Save();
            return _entity;
        }

        public ICollection<LeaveType> FindAll()
        {
           var leaveTypes = _context.LeaveTypes.ToList();
            return leaveTypes;
        }

        public LeaveType FindById(int id)
        {
            var leaveType = _context.LeaveTypes.Find(id);
            return leaveType;
        }

        public bool Save()
        {
            var save =  _context.SaveChanges();
            if (save > 0)
            {
                return true;
            }
            return false;
            //if (true)
            //{
            //    return true;
            //}
            //return false;
        }

        public bool Update(LeaveType entity)
        {
            _context.LeaveTypes.Update(entity);
            return Save(); 
        }
        public bool IsExists(int id)
        {
            var isExist = _context.LeaveTypes.Any(x => x.Id == id);
            if (isExist)
            {
                return true;
            }
            return false;
        }

        #region MyRegion
        public async Task<int> CreateByIntt(LeaveType entity)
        {
            _context.Add(entity);
            return await CommitChangesAsync();

        }
        private Task<int> CommitChangesAsync()
        {
            try
            {
                return _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                var ex = e.InnerException.ToString();
                return null;
            }
        }

        //int IRepositoryBase<LeaveType>.CreateByInt(LeaveType entity)
        //{
        //    throw new NotImplementedException();
        //} 
        #endregion
    }
}
