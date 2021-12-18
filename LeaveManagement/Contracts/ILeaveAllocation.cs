using LeaveManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagement.Contracts
{
    public interface ILeaveAllocationRepository : IRepositoryBase<LeaveAllocation>
    {

        bool CheckAllocation(int leaveTypeId, string EmployeeId);

        ICollection<LeaveAllocation> GetLeaveAllocationsByEmployee(string id);

        //LeaveAllocation GetLeaveAllocationsByEmployeeAndType(string id,int leaveTypeId);
        LeaveAllocation GetLeaveAllocationsByEmployeeAndType(string employeeid, int leavetypeid);
    }
}
