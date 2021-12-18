using LeaveManagement.Contracts;
using LeaveManagement.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagement.Repository
{
    public class LeaveAllocationRepository : ILeaveAllocationRepository
    {

        public ApplicationDbContext _context;
        public LeaveAllocationRepository(ApplicationDbContext _context)
        {
            this._context = _context;
        }


        #region For Checking ALlocation
        //public bool CheckAllocation(int leaveTypeId, string EmployeeId)
        //{
        //    var period = DateTime.Now.Year;
        //    return FindAll().Where(x => x.LeaveTypeId == leaveTypeId && x.EmployeeId == EmployeeId && x.Period == period).Any();
        //}

        public bool CheckAllocation(int leaveTypeId, string EmployeeId)
        {
            var period = DateTime.Now.Year;
            var isExist =  FindAll().Where(x => x.LeaveTypeId == leaveTypeId && x.EmployeeId == EmployeeId && x.Period == period).Any();
            if (isExist)
            {
                return true;
            }
            return false;
        }

        public ICollection<LeaveAllocation> GetLeaveAllocationsByEmployee(string employeeid)
        {
            var period = DateTime.Now.Year;
            var allocation = FindAll()
                 .Where(x => x.EmployeeId == employeeid && x.Period == period).ToList();
            return allocation;
        }

        //public LeaveAllocation GetLeaveAllocationsByEmployeeAndType(string id, int leaveTypeId)
        //{
        //    var period = DateTime.Now.Year;
        //    var allocation = FindAll()
        //         .FirstOrDefault(x => x.EmployeeId == id && x.Period == period && x.LeaveTypeId == leaveTypeId)
        //         ;
        //    return allocation;
        //}

        public LeaveAllocation GetLeaveAllocationsByEmployeeAndType(string employeeid, int leavetypeid)
        {
            var period = DateTime.Now.Year;
            var allocation = FindAll()
                   .FirstOrDefault(q => q.EmployeeId == employeeid && q.Period == period && q.LeaveTypeId == leavetypeid);
            return allocation;
        }
        #endregion

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
            var leaveAllocations = _context.LeaveAllocations
                                           .Include(x => x.LeaveType)
                                           .Include(x => x.Employee).ToList();
            return leaveAllocations;
        }

        public Data.LeaveAllocation FindByIdd(int id)
        {
            var leaveAllocation = _context.LeaveAllocations.Find(id);
            return leaveAllocation;
        }     
        public Data.LeaveAllocation FindById(int id)
        {
            var leaveAllocation = _context.LeaveAllocations
                                          .Include(x=>x.LeaveType)
                                          .Include(x=>x.Employee)
                                          .SingleOrDefault(x=>x.Id == id);
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
