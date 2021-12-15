using LeaveManagement.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagement.DataViewModels
{
    public class LeaveAllocationViewModel
    {
        public int Id { get; set; }
        [Required]
        public int NumberOfDays { get; set; }
        public DateTime DateCreated { get; set; }

        #region This is One Entity called Employee and this is one Entity called LeaveTypeViewModel with their corressponding Id's they are not lists
        
        public EmployeeViewModel Employee { get; set; }
        public string EmployeeId { get; set; }

        public LeaveTypeViewModel LeaveType { get; set; }
        public int LeaveTypeId { get; set; } 
        #endregion

        public int Period { get; set; }

        #region We Dont want this we removed it in Leave Allocation Module
        //public IEnumerable<SelectListItem> Employees { get; set; }
        //public IEnumerable<SelectListItem> LeaveTypes { get; set; } 
        #endregion
    }

    public class CreateLeaveAllocationViewModel
    {
        public int NumberUpdated { get; set; }
        public List<LeaveTypeViewModel> LeaveTypes { get; set; }
    }

    public class ViewAllocationViewModel
    {
        public EmployeeViewModel Employee { get; set; }
       // public Employee Employee { get; set; }
        public string EmployeeId { get; set; }
        public List<LeaveAllocationViewModel> LeaveAllocations { get; set; }
    }

    public class EditLeaveAllocationViewModel
    {
        public int Id { get; set; }
        public EmployeeViewModel Employee { get; set; }
        public string EmployeeId { get; set; }
        public LeaveTypeViewModel LeaveType { get; set; }
        public int LeaveTypeId { get; set; }

        public int NumberOfDays { get; set; }
    }
}
