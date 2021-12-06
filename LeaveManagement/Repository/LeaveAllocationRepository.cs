using LeaveManagement.Contracts;
using LeaveManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagement.Repository
{
    public class LeaveAllocationRepository : ILeaveAllocation
    {

        public ApplicationDbContext _context;
        public LeaveAllocationRepository(ApplicationDbContext _context)
        {
            this._context = _context;
        }
        public bool Create(Data.LeaveAllocation entity)
        {
            _context.LeaveAllocations.Add(entity);
            return Save();
        }

        public bool Delete(Data.LeaveAllocation entity)
        {
          _context.LeaveAllocations.Remove(entity);
            var _entity = Save();
            return _entity;
        }

        public ICollection<Data.LeaveAllocation> FindAll()
        {
            var leaveAllocations = _context.LeaveAllocations.ToList();
            return leaveAllocations;
        }

        public Data.LeaveAllocation FindById(int id)
        {
            var leaveAllocation = _context.LeaveAllocations.Find(id);
            return leaveAllocation;
        }

        public bool Save()
        {
            var save = _context.SaveChanges();
            if (true)
            {
                return true;
            }
            return false;
        }

        public bool Update(Data.LeaveAllocation entity)
        {
            _context.LeaveAllocations.Update(entity);
            return Save();
        }

        public int CreateByIntAsync(Data.LeaveAllocation entity)
        {
            throw new NotImplementedException();
        }

        #region MyRegion

        public async Task<int> CreateByIntt(Data.LeaveAllocation entity)
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

        public bool IsExists(int id)
        {
            var isExist = _context.LeaveTypes.Any(x => x.Id == id);
            if (isExist)
            {
                return true;
            }
            return false;

           //var isExist =  FindById(id);
            // if (isExist)
            // {
            //     return true;
            // }
            // return false;
        }

        #endregion
    }
}
