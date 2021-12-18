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
            var leaveRequests = FindAlll()
              .Where(q => q.RequestingEmployeeId == employeeId)
              .ToList();
            return leaveRequests;
        }

        public bool Createe(LeaveRequest entity)
        {
            _context.LeaveRequests.Add(entity);
            return Savee();
        }


        public bool Deletee(LeaveRequest entity)
        {
            var leaveRequest = _context.LeaveRequests.Remove(entity);
            return Savee();
        }

        public ICollection<LeaveRequest> FindAlll()
        {
            var leaveRequest = _context.LeaveRequests
                                       .Include(x=>x.LeaveType)
                                       .Include(x => x.RequestingEmployee)
                                       .ToList();
            return leaveRequest;
        }



        public LeaveRequest FindByIddd(int id)
        {
            var leaveRequest = _context.LeaveRequests.Find(id);
            return leaveRequest;
        }

        public LeaveRequest FindByIdd(int id)
        {
            var leaveRequest = _context.LeaveRequests
                                       .Include(x => x.LeaveType)
                                       .Include(x => x.RequestingEmployee)
                                       .SingleOrDefault(x=>x.Id ==id);
            return leaveRequest;
        }

        public bool Savee()
        {
            var save = _context.SaveChanges();
            if (true)
            {
                return true;
            }
            return false;
        }

        public bool Updatee(LeaveRequest entity)
        {

            _context.LeaveRequests.Update(entity);
            return Savee();
        }

        public bool IsExistss(int id)
        {
            var isExist = _context.LeaveTypes.Any(x => x.Id == id);
            if (isExist)
            {
                return true;
            }
            return false;
        }

        public async Task<ICollection<LeaveRequest>> FindAll()
        {
            var _leaveRequest = await _context.LeaveRequests.ToListAsync();
            return _leaveRequest;
        }

        public async Task<LeaveRequest> FindById(int id)
        {
            var _leaveRequest = await _context.LeaveRequests.FindAsync(id);
            return _leaveRequest;
        }

        public async Task<bool> Create(LeaveRequest entity)
        {
            await _context.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Update(LeaveRequest entity)
        {
            _context.LeaveRequests.Update(entity);
            return await Save();
        }

        public async Task<bool> Delete(LeaveRequest entity)
        {
            _context.LeaveRequests.Remove(entity);
            var _entity = await Save();
            return _entity;
        }

        public async Task<bool> Save()
        {
            var save = await _context.SaveChangesAsync();
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

        public async Task<bool> IsExists(int id)
        {
            var isExist = await _context.LeaveTypes.AnyAsync(x => x.Id == id);
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
