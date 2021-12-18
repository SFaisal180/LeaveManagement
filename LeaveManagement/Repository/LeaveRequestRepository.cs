using LeaveManagement.Contracts;
using LeaveManagement.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagement.Repository
{
    public class LeaveRequestRepository : ILeaveRequest
    {

        public ApplicationDbContext _context;
        public LeaveRequestRepository(ApplicationDbContext _context)
        {
            this._context = _context;
        }


        public ICollection<LeaveRequest> GetLeaveRequestsByEmployee(string employeeId)
        {
            var leaveRequests = FindAll()
              .Where(q => q.RequestingEmployeeId == employeeId)
              .ToList();
            return leaveRequests;
        }

        public bool Create(LeaveRequest entity)
        {
            _context.LeaveRequests.Add(entity);
            return Save();
        }


        public bool Delete(LeaveRequest entity)
        {
            var leaveRequest = _context.LeaveRequests.Remove(entity);
            return Save();
        }

        public ICollection<LeaveRequest> FindAll()
        {
            var leaveRequest = _context.LeaveRequests
                                       .Include(x=>x.LeaveType)
                                       .Include(x => x.RequestingEmployee)
                                       .ToList();
            return leaveRequest;
        }



        public LeaveRequest FindByIdd(int id)
        {
            var leaveRequest = _context.LeaveRequests.Find(id);
            return leaveRequest;
        }

        public LeaveRequest FindById(int id)
        {
            var leaveRequest = _context.LeaveRequests
                                       .Include(x => x.LeaveType)
                                       .Include(x => x.RequestingEmployee)
                                       .SingleOrDefault(x=>x.Id ==id);
            return leaveRequest;
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

        public bool Update(LeaveRequest entity)
        {

            _context.LeaveRequests.Update(entity);
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

        #region
        //public int CreateByInt(LeaveRequest entity)
        //{
        //    _context.LeaveRequests.Add(entity);
        //    return await CommitChangesAsync();
        //}

        //private Task<int> CommitChangesAsync()
        //{
        //    try
        //    {
        //        return _context.SaveChangesAsync();
        //    }
        //    catch (Exception e)
        //    {
        //        var ex = e.InnerException.ToString();
        //        return null;
        //    }
        //}
        #endregion

    }
}
