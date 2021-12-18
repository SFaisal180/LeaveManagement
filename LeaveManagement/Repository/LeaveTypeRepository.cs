using LeaveManagement.Contracts;
using LeaveManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagement.Repository
{
    public class LeaveTypeRepository : ILeaveTypeRepository
    {

        public ApplicationDbContext _context;
        public LeaveTypeRepository(ApplicationDbContext _context)
        {
            this._context = _context;
        }
        public bool Createe(LeaveType entity)
        {
            _context.Add(entity);
            // _context.SaveChanges();
            return Savee();
        }

        public async Task<bool> Create(LeaveType entity)
        {
            await _context.AddAsync(entity);
            return await Save();
        }

        public bool Deletee(LeaveType entity)
        {
            _context.LeaveTypes.Remove(entity);
            var _entity = Savee();
            return _entity;
        }

        public async Task<bool> Delete(LeaveType entity)
        {
            _context.LeaveTypes.Remove(entity);
            var _entity = await Save();
            return _entity;
        }

        public ICollection<LeaveType> FindAlll()
        {
            var leaveTypes = _context.LeaveTypes.ToList();
            return leaveTypes;
        }

        public async Task<ICollection<LeaveType>> FindAll()
        {
            var leaveTypes = await _context.LeaveTypes.ToListAsync();
            return leaveTypes;
        }

        public  LeaveType FindByIdd(int id)
        {
            var leaveType = _context.LeaveTypes.Find(id);
            return leaveType;
        }

        public async Task<LeaveType> FindById(int id)
        {
            var leaveType = await _context.LeaveTypes.FindAsync(id);
            return leaveType;
        }

        public bool Savee()
        {
            var save = _context.SaveChanges();
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

        public bool Updatee(LeaveType entity)
        {
            _context.LeaveTypes.Update(entity);
            return  Savee();
        }

        public async Task<bool> Update(LeaveType entity)
        {
            _context.LeaveTypes.Update(entity);
            return await Save();
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

        public async Task<bool> IsExists(int id)
        {
            var isExist =await _context.LeaveTypes.AnyAsync(x => x.Id == id);
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
