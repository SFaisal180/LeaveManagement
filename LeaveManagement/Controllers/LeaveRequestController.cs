using AutoMapper;
using LeaveManagement.Contracts;
using LeaveManagement.Data;
using LeaveManagement.DataViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagement.Controllers
{
    public class LeaveRequestController : Controller
    {
        private readonly ILeaveRequest _leaveRequest;
        private readonly ILeaveAllocationRepository _leaveallocationRepository;
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;

        public LeaveRequestController(ILeaveRequest leaveRequest, ILeaveAllocationRepository leaveallocationRepository, ILeaveTypeRepository leaveTypeRepository, IMapper mapper
            , UserManager<IdentityUser> userManager)
        {
            _leaveRequest = leaveRequest;
            _leaveallocationRepository = leaveallocationRepository;
            _leaveTypeRepository = leaveTypeRepository;
            _mapper = mapper;
            _userManager = userManager;
        }

        #region ASYNCHRONUS
        public async Task<ActionResult> Index()
        {
            var leaveRequest = await _leaveRequest.FindAll();
            var leaveRequestViewModel = _mapper.Map<List<LeaveRequestViewModel>>(leaveRequest);
            var adminLeaveRequestViewModel = new AdminLeaveRequestViewModel()
            {
                TotalRequest = leaveRequestViewModel.Count,
                ApprovedRequest = leaveRequestViewModel.Where(x => x.Approved == true).Count(),
                RejectedRequest = leaveRequestViewModel.Where(x => x.Approved == false).Count(),
                PendingRequest = leaveRequestViewModel.Where(x => x.Approved == null).Count(),
                LeaveRequests = leaveRequestViewModel
            };
            return View(adminLeaveRequestViewModel);
        }

        [HttpGet]
        public async Task<ActionResult> Create()
        {
            var leaveType = await _leaveTypeRepository.FindAll();
            var leaveTypeItems = leaveType.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            var model = new CreateLeaveRequestViewModel()
            {
                LeaveTypes = leaveTypeItems
            };
            return View(model);
        }

        public async Task<ActionResult> Create(CreateLeaveRequestViewModel model)
        {
            try
            {
                var period = DateTime.Now.Year;
                var startDate = Convert.ToDateTime(model.StartDate);
                var endDate = Convert.ToDateTime(model.EndDate);

                #region It didn't tracking the DropdownList ThereFore populating model with DropDown
                var leaveType = await _leaveTypeRepository.FindAll();
                var leaveTypeItems = leaveType.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                });
                model.LeaveTypes = leaveTypeItems;
                #endregion

                //Determining weather Model State is Valid or Not
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                if (DateTime.Compare(startDate, endDate) > 1)
                {
                    ModelState.AddModelError("", "Start Date cannot be futher in the future than the End Date");
                    return View(model);
                }

                //Fetching Employee
                var employee = _userManager.GetUserAsync(User).Result;

                //Fetching Allocations For A LeaveType ie SickLeave Or VacationLeave Not the complete list of all allocations for that employee it can be done with below two methods
                var allocation = _leaveallocationRepository.GetLeaveAllocationsByEmployeeAndType(employee.Id, model.LeaveTypeId);
                //var allocation = await _leaveallocationRepository.FindAll().Where(x => x.EmployeeId == employee.Id && x.LeaveTypeId == model.LeaveTypeId && x.Period == period).FirstOrDefault();


                int daysRequested = (int)(endDate.Month - startDate.Month);

                if (daysRequested > allocation.NumberOfDays)
                {
                    ModelState.AddModelError("", "You do not have sufficent days for this request");
                    return View(model);
                }

                var leaveRequestViewModel = new LeaveRequestViewModel()
                {
                    LeaveTypeId = model.LeaveTypeId,
                    RequestingEmployeeId = employee.Id,
                    DateRequested = DateTime.Now,
                    DateActioned = DateTime.Now,
                    Approved = null,
                    RequestComments = model.RequestComments,
                    StartDate = startDate,
                    EndDate = endDate
                };
                var leaveRequest = _mapper.Map<LeaveRequest>(leaveRequestViewModel);
                var isSuccess = _leaveRequest.Createe(leaveRequest);
                if (!isSuccess)
                {
                    ModelState.AddModelError("", "Something Went Wrong");
                    return View(model);

                }
                return RedirectToAction(nameof(Index), "Home");
                //return RedirectToAction("MyLeave");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Something Went Wrong");
                return View(model);
            }
        }

        public ActionResult CheckNumberOfDaysPerLeave()
        {
            var leaveTypeId = 2;
            var employee = _userManager.GetUserAsync(User).Result;
            var model = _leaveallocationRepository.GetLeaveAllocationsByEmployeeAndType(employee.Id, leaveTypeId);
            return View(model);
        }

        public async Task<ActionResult> Details(int id)
        {
            var leaveRequest = await _leaveRequest.FindById(id);
            var leaveRequestViewModel = _mapper.Map<LeaveRequestViewModel>(leaveRequest);
            return View(leaveRequestViewModel);
        }

        public async Task<ActionResult> ApproveRequest(int id)
        {
            try
            {
                var user = _userManager.GetUserAsync(User).Result;
                var leaveRequest = await _leaveRequest.FindById(id);
                var leaveTypeId = leaveRequest.LeaveTypeId;
                var employeeId = leaveRequest.RequestingEmployeeId;

                var allocation = _leaveallocationRepository.GetLeaveAllocationsByEmployeeAndType(employeeId, leaveTypeId);

                var startDate = Convert.ToDateTime(leaveRequest.StartDate);
                var endDate = Convert.ToDateTime(leaveRequest.EndDate);

                int daysRequested = (int)(endDate.Month - startDate.Month);
                //int daysRequested = (int)(leaveRequest.EndDate - leaveRequest.StartDate).TotalDays;
                //allocation.NumberOfDays -= daysRequested;
                allocation.NumberOfDays = allocation.NumberOfDays - daysRequested;

                leaveRequest.Approved = true;
                leaveRequest.ApprovedById = user.Id;
                leaveRequest.DateActioned = DateTime.Now;

                await _leaveRequest.Update(leaveRequest);
                await _leaveallocationRepository.Update(allocation);

                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }

        public async Task<ActionResult> RejectRequest(int id)
        {
            try
            {
                var user = _userManager.GetUserAsync(User).Result;
                var leaveRequest = await _leaveRequest.FindById(id);
                leaveRequest.Approved = false;
                leaveRequest.ApprovedById = user.Id;
                leaveRequest.DateActioned = DateTime.Now;

                var isSuccess = await _leaveRequest.Update(leaveRequest);
                if (!isSuccess)
                {
                    return RedirectToAction("Index");
                }
                return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");

            }
        }

        public ActionResult MyLeave()
        {
            var employee = _userManager.GetUserAsync(User).Result;
            var employeeid = employee.Id;
            var employeeAllocations = _leaveallocationRepository.GetLeaveAllocationsByEmployee(employeeid);
            var employeeRequests = _leaveRequest.GetLeaveRequestsByEmployee(employeeid);

            var employeeAllocationsModel = _mapper.Map<List<LeaveAllocationViewModel>>(employeeAllocations);
            var employeeRequestsModel = _mapper.Map<List<LeaveRequestViewModel>>(employeeRequests);

            var model = new EmployeeLeaveRequestViewModel
            {
                LeaveAllocations = employeeAllocationsModel,
                LeaveRequests = employeeRequestsModel
            };

            return View(model);
        }
        public async Task<ActionResult> CancelRequest(int id)
        {
            var leaveRequest = await _leaveRequest.FindById(id);
            leaveRequest.Cancelled = true;
            await _leaveRequest.Update(leaveRequest);
            return RedirectToAction("MyLeave");
        }
        #endregion

        #region SYNCHRONUS
        //public ActionResult Index()
        //{
        //    var leaveRequest = _leaveRequest.FindAlll();
        //    var leaveRequestViewModel = _mapper.Map<List<LeaveRequestViewModel>>(leaveRequest);
        //    var adminLeaveRequestViewModel = new AdminLeaveRequestViewModel()
        //    {
        //        TotalRequest = leaveRequestViewModel.Count,
        //        ApprovedRequest = leaveRequestViewModel.Where(x => x.Approved == true).Count(),
        //        RejectedRequest = leaveRequestViewModel.Where(x => x.Approved == false).Count(),
        //        PendingRequest = leaveRequestViewModel.Where(x => x.Approved == null).Count(),
        //        LeaveRequests = leaveRequestViewModel
        //    };
        //    return View(adminLeaveRequestViewModel);
        //}

        //[HttpGet]
        //public ActionResult Create()
        //{
        //    var leaveType = _leaveTypeRepository.FindAlll();
        //    var leaveTypeItems = leaveType.Select(x => new SelectListItem
        //    {
        //        Text = x.Name,
        //        Value = x.Id.ToString()
        //    });
        //    var model = new CreateLeaveRequestViewModel()
        //    {
        //        LeaveTypes = leaveTypeItems
        //    };
        //    return View(model);
        //}

        //public ActionResult Create(CreateLeaveRequestViewModel model)
        //{
        //    try
        //    {
        //        var period = DateTime.Now.Year;
        //        var startDate = Convert.ToDateTime(model.StartDate);
        //        var endDate = Convert.ToDateTime(model.EndDate);

        //        #region It didn't tracking the DropdownList ThereFore populating model with DropDown
        //        var leaveType = _leaveTypeRepository.FindAlll();
        //        var leaveTypeItems = leaveType.Select(x => new SelectListItem
        //        {
        //            Text = x.Name,
        //            Value = x.Id.ToString()
        //        });
        //        model.LeaveTypes = leaveTypeItems;
        //        #endregion

        //        //Determining weather Model State is Valid or Not
        //        if (!ModelState.IsValid)
        //        {
        //            return View(model);
        //        }

        //        if (DateTime.Compare(startDate, endDate) > 1)
        //        {
        //            ModelState.AddModelError("", "Start Date cannot be futher in the future than the End Date");
        //            return View(model);
        //        }

        //        //Fetching Employee
        //        var employee = _userManager.GetUserAsync(User).Result;

        //        //Fetching Allocations For A LeaveType ie SickLeave Or VacationLeave Not the complete list of all allocations for that employee it can be done with below two methods
        //        var allocationn = _leaveallocationRepository.GetLeaveAllocationsByEmployeeAndType(employee.Id, model.LeaveTypeId);
        //        var allocation = _leaveallocationRepository.FindAlll().Where(x => x.EmployeeId == employee.Id && x.LeaveTypeId == model.LeaveTypeId && x.Period == period).FirstOrDefault();


        //        int daysRequested = (int)(endDate.Month - startDate.Month);

        //        if (daysRequested > allocation.NumberOfDays)
        //        {
        //            ModelState.AddModelError("", "You do not have sufficent days for this request");
        //            return View(model);
        //        }

        //        var leaveRequestViewModel = new LeaveRequestViewModel()
        //        {
        //            LeaveTypeId = model.LeaveTypeId,
        //            RequestingEmployeeId = employee.Id,
        //            DateRequested = DateTime.Now,
        //            DateActioned = DateTime.Now,
        //            Approved = null,
        //            RequestComments = model.RequestComments,
        //            StartDate = startDate,
        //            EndDate = endDate
        //        };
        //        var leaveRequest = _mapper.Map<LeaveRequest>(leaveRequestViewModel);
        //        var isSuccess = _leaveRequest.Createe(leaveRequest);
        //        if (!isSuccess)
        //        {
        //            ModelState.AddModelError("", "Something Went Wrong");
        //            return View(model);

        //        }
        //        return RedirectToAction(nameof(Index), "Home");
        //        //return RedirectToAction("MyLeave");
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError("", "Something Went Wrong");
        //        return View(model);
        //    }
        //}

        //public ActionResult CheckNumberOfDaysPerLeave()
        //{
        //    var leaveTypeId = 2;
        //    var employee = _userManager.GetUserAsync(User).Result;
        //    var model = _leaveallocationRepository.GetLeaveAllocationsByEmployeeAndType(employee.Id, leaveTypeId);
        //    return View(model);
        //}

        //public ActionResult Details(int id)
        //{
        //    var leaveRequest = _leaveRequest.FindByIdd(id);
        //    var leaveRequestViewModel = _mapper.Map<LeaveRequestViewModel>(leaveRequest);
        //    return View(leaveRequestViewModel);
        //}

        //public ActionResult ApproveRequest(int id)
        //{
        //    try
        //    {
        //        var user = _userManager.GetUserAsync(User).Result;
        //        var leaveRequest = _leaveRequest.FindByIdd(id);
        //        var leaveTypeId = leaveRequest.LeaveTypeId;
        //        var employeeId = leaveRequest.RequestingEmployeeId;

        //        var allocation = _leaveallocationRepository.GetLeaveAllocationsByEmployeeAndType(employeeId, leaveTypeId);

        //        var startDate = Convert.ToDateTime(leaveRequest.StartDate);
        //        var endDate = Convert.ToDateTime(leaveRequest.EndDate);

        //        int daysRequested = (int)(endDate.Month - startDate.Month);
        //        //int daysRequested = (int)(leaveRequest.EndDate - leaveRequest.StartDate).TotalDays;
        //        //allocation.NumberOfDays -= daysRequested;
        //        allocation.NumberOfDays = allocation.NumberOfDays - daysRequested;

        //        leaveRequest.Approved = true;
        //        leaveRequest.ApprovedById = user.Id;
        //        leaveRequest.DateActioned = DateTime.Now;

        //        _leaveRequest.Update(leaveRequest);
        //        _leaveallocationRepository.Update(allocation);

        //        return RedirectToAction("Index");

        //    }
        //    catch (Exception ex)
        //    {
        //        return RedirectToAction("Index");
        //    }
        //}

        //public ActionResult RejectRequest(int id)
        //{
        //    try
        //    {
        //        var user = _userManager.GetUserAsync(User).Result;
        //        var leaveRequest = _leaveRequest.FindByIdd(id);
        //        leaveRequest.Approved = false;
        //        leaveRequest.ApprovedById = user.Id;
        //        leaveRequest.DateActioned = DateTime.Now;

        //        var isSuccess = _leaveRequest.Updatee(leaveRequest);
        //        if (!isSuccess)
        //        {
        //            return RedirectToAction("Index");
        //        }
        //        return View();
        //    }
        //    catch (Exception ex)
        //    {
        //        return RedirectToAction("Index");

        //    }
        //}

        //public ActionResult MyLeave()
        //{
        //    var employee = _userManager.GetUserAsync(User).Result;
        //    var employeeid = employee.Id;
        //    var employeeAllocations = _leaveallocationRepository.GetLeaveAllocationsByEmployee(employeeid);
        //    var employeeRequests = _leaveRequest.GetLeaveRequestsByEmployee(employeeid);

        //    var employeeAllocationsModel = _mapper.Map<List<LeaveAllocationViewModel>>(employeeAllocations);
        //    var employeeRequestsModel = _mapper.Map<List<LeaveRequestViewModel>>(employeeRequests);

        //    var model = new EmployeeLeaveRequestViewModel
        //    {
        //        LeaveAllocations = employeeAllocationsModel,
        //        LeaveRequests = employeeRequestsModel
        //    };

        //    return View(model);
        //}
        //public ActionResult CancelRequest(int id)
        //{
        //    var leaveRequest = _leaveRequest.FindByIdd(id);
        //    leaveRequest.Cancelled = true;
        //    _leaveRequest.Update(leaveRequest);
        //    return RedirectToAction("MyLeave");
        //} 
        #endregion
    }
}
