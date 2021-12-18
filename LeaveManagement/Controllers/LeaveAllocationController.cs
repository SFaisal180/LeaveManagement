using AutoMapper;
using LeaveManagement.Contracts;
using LeaveManagement.Data;
using LeaveManagement.DataViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagement.Controllers
{

    //NOTE I HAVE SKIPPED THE STEPS OF REPLACING IDENTITY USERS WITH EMPLOYEE folder 7 video no 4  
    //INSTEAD I MAPPED THE IDENTITY USER TO EMPLOYEEVIEWMODEL AS YOU CAN SEE IN 
    //DETAILS AND LISTEMPLOYEES



    public class LeaveAllocationController : Controller
    {

        private readonly ILeaveAllocationRepository _leaveallocationRepository;
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly IMapper _mapper;
    //    private readonly UserManager<Employee> _userManager;
        private readonly UserManager<IdentityUser> _userManager;

        public LeaveAllocationController(ILeaveAllocationRepository leaveallocationRepository, ILeaveTypeRepository leaveTypeRepository, IMapper mapper, 
            UserManager<IdentityUser> userManager)
        {
            _leaveallocationRepository = leaveallocationRepository;
            _leaveTypeRepository = leaveTypeRepository;
            _mapper = mapper;
            _userManager = userManager;
        }

        #region ASYNCHRONUS
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var leaveTypes = await _leaveTypeRepository.FindAll();
            var mappedLeaveTypes = _mapper.Map<List<LeaveType>, List<LeaveTypeViewModel>>(leaveTypes.ToList());
            var model = new CreateLeaveAllocationViewModel()
            {
                LeaveTypes = mappedLeaveTypes,
                NumberUpdated = 0
            };
            return View(model);
        }

        public async Task<IActionResult> SetLeave(int id)
        {
            var period = DateTime.Now.Year;
            var leaveType = await _leaveTypeRepository.FindById(id);
            var employees = _userManager.GetUsersInRoleAsync("Employee").Result;
            foreach (var employee in employees)
            {
                if (_leaveallocationRepository.CheckAllocation(id, employee.Id))
                    continue;
                var leaveAllocationViewModel = new LeaveAllocationViewModel
                {
                    DateCreated = DateTime.Now,
                    EmployeeId = employee.Id,
                    LeaveTypeId = id,
                    NumberOfDays = leaveType.DefaultDays,
                    Period = period
                };

                var leaveAllocation = _mapper.Map<LeaveAllocation>(leaveAllocationViewModel);
                await _leaveallocationRepository.Create(leaveAllocation);

            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ListEmployees()
        {
            var employees = _userManager.GetUsersInRoleAsync("Employee").Result;
            var model = _mapper.Map<List<EmployeeViewModel>>(employees);
            return View(model);
        }

        public async Task<IActionResult>  Details(string id)
        {
            var period = DateTime.Now.Year;
            var employee = _userManager.FindByIdAsync(id).Result;
            var employeeViewModel = _mapper.Map<EmployeeViewModel>(employee);

            #region  You can Use Both the Approch
            var allocation = _leaveallocationRepository.GetLeaveAllocationsByEmployee(id);

            //var leaveAllocations = await _leaveallocationRepository.FindAll().Where
            //    (x => x.EmployeeId == id && x.Period == period).ToList();

            #endregion
            var leaveAllocationsViewModel = _mapper.Map<List<LeaveAllocationViewModel>>(allocation);

            var viewAllocationViewModel = new ViewAllocationViewModel
            {
                Employee = employeeViewModel,
                // EmployeeId = id,
                LeaveAllocations = leaveAllocationsViewModel
            };
            return View(viewAllocationViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var leaveALlocation = await _leaveallocationRepository.FindById(id);
            var leaveALlocationViewModel = _mapper.Map<EditLeaveAllocationViewModel>(leaveALlocation);
            return View(leaveALlocationViewModel);
        }

        public async Task<IActionResult>Edit(EditLeaveAllocationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            //var leaveAllocation = _mapper.Map<LeaveAllocation>(model);
            //var editleaveAllocation = _mapper.Map<LeaveAllocation>(model);
            //var isSuccess =   _leaveAllocation.Update(editleaveAllocation);

            var record = await _leaveallocationRepository.FindById(model.Id);
            record.NumberOfDays = model.NumberOfDays;
            var isSuccess = await _leaveallocationRepository.Update(record);

            if (!isSuccess)
            {
                ModelState.AddModelError("", "Something went wrong");
                return View(model);
            }

            return RedirectToAction("Details", new { id = model.EmployeeId });

        }
        #endregion

        #region SYNCHRONUS
        //[HttpGet]
        //public ActionResult Index()
        //{
        //    var leaveTypes = _leaveTypeRepository.FindAlll().ToList();
        //    var mappedLeaveTypes = _mapper.Map<List<LeaveType>, List<LeaveTypeViewModel>>(leaveTypes);
        //    var model = new CreateLeaveAllocationViewModel()
        //    {
        //        LeaveTypes = mappedLeaveTypes,
        //        NumberUpdated = 0
        //    };
        //    return View(model);
        //}

        //public ActionResult SetLeave(int id)
        //{
        //    var period = DateTime.Now.Year;
        //    var leaveType = _leaveTypeRepository.FindByIdd(id);
        //    var employees = _userManager.GetUsersInRoleAsync("Employee").Result;
        //    foreach (var employee in employees)
        //    {
        //        if (_leaveallocationRepository.CheckAllocation(id, employee.Id))
        //            continue;
        //        var leaveAllocationViewModel = new LeaveAllocationViewModel
        //        {
        //            DateCreated = DateTime.Now,
        //            EmployeeId = employee.Id,
        //            LeaveTypeId = id,
        //            NumberOfDays = leaveType.DefaultDays,
        //            Period = period
        //        };

        //        var leaveAllocation = _mapper.Map<LeaveAllocation>(leaveAllocationViewModel);
        //        _leaveallocationRepository.Create(leaveAllocation);

        //    }
        //    return RedirectToAction("Index");
        //}

        //public ActionResult ListEmployees()
        //{
        //    var employees = _userManager.GetUsersInRoleAsync("Employee").Result;
        //    var model = _mapper.Map<List<EmployeeViewModel>>(employees);
        //    return View(model);
        //}

        //public ActionResult Details(string id)
        //{
        //    var period = DateTime.Now.Year;
        //    var employee = _userManager.FindByIdAsync(id).Result;
        //    var employeeViewModel = _mapper.Map<EmployeeViewModel>(employee);

        //    #region  You can Use Both the Approch
        //    var allocation = _leaveallocationRepository.GetLeaveAllocationsByEmployee(id);

        //    var leaveAllocations = _leaveallocationRepository.FindAlll().Where
        //        (x => x.EmployeeId == id && x.Period == period).ToList();

        //    #endregion
        //    var leaveAllocationsViewModel = _mapper.Map<List<LeaveAllocationViewModel>>(leaveAllocations);

        //    var viewAllocationViewModel = new ViewAllocationViewModel
        //    {
        //        Employee = employeeViewModel,
        //        // EmployeeId = id,
        //        LeaveAllocations = leaveAllocationsViewModel
        //    };
        //    return View(viewAllocationViewModel);
        //}

        //[HttpGet]
        //public ActionResult Edit(int id)
        //{
        //    var leaveALlocation = _leaveallocationRepository.FindById(id);
        //    var leaveALlocationViewModel = _mapper.Map<EditLeaveAllocationViewModel>(leaveALlocation);
        //    return View(leaveALlocationViewModel);
        //}

        //public ActionResult Edit(EditLeaveAllocationViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }
        //    //var leaveAllocation = _mapper.Map<LeaveAllocation>(model);
        //    //var editleaveAllocation = _mapper.Map<LeaveAllocation>(model);
        //    //var isSuccess =   _leaveAllocation.Update(editleaveAllocation);

        //    var record = _leaveallocationRepository.FindByIdd(model.Id);
        //    record.NumberOfDays = model.NumberOfDays;
        //    var isSuccess = _leaveallocationRepository.Updatee(record);

        //    if (!isSuccess)
        //    {
        //        ModelState.AddModelError("", "Something went wrong");
        //        return View(model);
        //    }

        //    return RedirectToAction("Details", new { id = model.EmployeeId });

        //} 
        #endregion
    }
}
