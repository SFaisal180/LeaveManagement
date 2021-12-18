using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagement.DataViewModels
{
    public class LeaveRequestViewModel
    {
        public int Id { get; set; }


        public EmployeeViewModel RequestingEmployee { get; set; }

        [Display(Name = "Employee Name")]
        public string RequestingEmployeeId { get; set; }

        [Display(Name = "Start Date")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public LeaveTypeViewModel LeaveType { get; set; }
        public int LeaveTypeId { get; set; }

        public IEnumerable<SelectListItem> LeaveTypes { get; set; }

        [Display(Name = "Date Requested")]
        public DateTime DateRequested { get; set; }

        [Display(Name = "Date Actioned")]
        public DateTime DateActioned { get; set; }

        [Display(Name = "Approval State")]
        public bool? Approved { get; set; }

        public EmployeeViewModel ApprovedBy { get; set; }

        [Display(Name = "Approver Name")]
        public string ApprovedById { get; set; }


        public bool Cancelled { get; set; }

        [Display(Name = "Employee Comments")]
        [MaxLength(300)]
        public string RequestComments { get; set; }
    }

    public class AdminLeaveRequestViewModel
    {
        [Display(Name = "Total Number Of Request")]
        public int TotalRequest { get; set; }

        [Display(Name = "Pending Request")]
        public int PendingRequest { get; set; }

        [Display(Name = "Approved Request")]
        public int ApprovedRequest { get; set; }

        [Display(Name = "Rejected Request")]
        public int RejectedRequest { get; set; }

        public List<LeaveRequestViewModel> LeaveRequests { get; set; }
    }

    public class CreateLeaveRequestViewModel
    {

        [Display(Name = "Start Date")]
        [Required]
        //public DateTime StartDate { get; set; }
        public string StartDate { get; set; }

        [Display(Name = "End Date")]
        [Required]
        //public DateTime EndDate { get; set; }
        public string EndDate { get; set; }

        public IEnumerable<SelectListItem> LeaveTypes { get; set; }
        //public IEnumerable<LeaveType> LeaveTypes { get; set; }

        [Display(Name = "Leave Type")]
        public int LeaveTypeId { get; set; }

        [Display(Name = "Comments")]
        [MaxLength(300)]
        public string RequestComments { get; set; }

    }

    public class EmployeeLeaveRequestViewModel
    {
        public List<LeaveAllocationViewModel> LeaveAllocations { get; set; }
        public List<LeaveRequestViewModel> LeaveRequests { get; set; }
    }
}
