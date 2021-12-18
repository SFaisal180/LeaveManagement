using AutoMapper;
using LeaveManagement.Contracts;
using LeaveManagement.Data;
using LeaveManagement.DataViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagement.Controllers
{
    public class LeaveTypesController : Controller
    {
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly IMapper _mapper;

        public LeaveTypesController(ILeaveTypeRepository leaveTypeRepository, IMapper mapper)
        {
            _leaveTypeRepository = leaveTypeRepository;
             _mapper = mapper;
        }

        // NOTE WE WANT TO RETURN VIEWMODELS TO VIEW NOT THE CLASS 


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var leaveTypes = await _leaveTypeRepository.FindAll();
            var model = _mapper.Map<List<LeaveType>, List<LeaveTypeViewModel>>(leaveTypes.ToList());
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var isExist = await _leaveTypeRepository.IsExists(id);
            if (!isExist)
            {
                return NotFound();
            }

            var leaveType = _leaveTypeRepository.FindById(id);
            var model = _mapper.Map<LeaveTypeViewModel>(leaveType);
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(LeaveTypeViewModel leaveTypeViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(leaveTypeViewModel);
            }

            var leaveType = _mapper.Map<LeaveType>(leaveTypeViewModel);
            leaveType.DateCreated = DateTime.Now;
            var isSuccess = await _leaveTypeRepository.Create(leaveType);
            if (!isSuccess)
            {
                ModelState.AddModelError("", "Something Went Wrong");
                return View(leaveTypeViewModel);
            }
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> EditLeaveType(int id)
        {
            var leaveType = await _leaveTypeRepository.IsExists(id);
            if (!leaveType)
            {
                return NotFound();
            }

            var model = _mapper.Map<LeaveTypeViewModel>(leaveType);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditLeaveType(LeaveTypeViewModel leaveTypeViewModel)
        {

            if (!ModelState.IsValid)
            {
                return View(leaveTypeViewModel);
            }
           var leaveType = _mapper.Map<LeaveType>(leaveTypeViewModel);
        //   var _leaveType = _leaveTypeRepository.FindById(leaveType.Id);
            
            //_leaveType.Name = leaveTypeViewModel.Name;
            //_leaveType.DefaultDays = leaveTypeViewModel.DefaultDays;


            var isSuccess = await _leaveTypeRepository.Update(leaveType);
            if (!isSuccess)
            {
                ModelState.AddModelError("", "Something Went Wrong");
                return View(leaveTypeViewModel);
            }
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Delete(int id)
        {
            var isExist = await _leaveTypeRepository.IsExists(id);
            if (!isExist)
            {
                return NotFound();
            }
            var leaveType = await _leaveTypeRepository.FindById(id);
            var leaveTypeViewModel = _mapper.Map<LeaveTypeViewModel>(leaveType);
            return View(leaveTypeViewModel);
        }

        // POST: LeaveTypeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(LeaveTypeViewModel model)
        {
            try
            {
                var leaveType = _mapper.Map<LeaveType>(model);
                var isSucess = await _leaveTypeRepository.Delete(leaveType);
                if (!isSucess)
                {
                    return View(model);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
