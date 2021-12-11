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
        public IActionResult Index()
        {
            var leaveTypes = _leaveTypeRepository.FindAll().ToList();
            var model = _mapper.Map<List<LeaveType>, List<LeaveTypeViewModel>>(leaveTypes);
            return View(model);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var isExist = _leaveTypeRepository.IsExists(id);
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
        public IActionResult Create(LeaveTypeViewModel leaveTypeViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(leaveTypeViewModel);
            }

            var leaveType = _mapper.Map<LeaveType>(leaveTypeViewModel);
            leaveType.DateCreated = DateTime.Now;
            var isSuccess = _leaveTypeRepository.Create(leaveType);
            if (!isSuccess)
            {
                ModelState.AddModelError("", "Something Went Wrong");
                return View(leaveTypeViewModel);
            }
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult EditLeaveType(int id)
        {
            if (!_leaveTypeRepository.IsExists(id))
            {
                return NotFound();
            }
            var leaveType = _leaveTypeRepository.FindById(id);
            var model = _mapper.Map<LeaveTypeViewModel>(leaveType);
            return View(model);
        }

        [HttpPost]
        public IActionResult EditLeaveType(LeaveTypeViewModel leaveTypeViewModel)
        {

            if (!ModelState.IsValid)
            {
                return View(leaveTypeViewModel);
            }
           var leaveType = _mapper.Map<LeaveType>(leaveTypeViewModel);
        //   var _leaveType = _leaveTypeRepository.FindById(leaveType.Id);
            
            //_leaveType.Name = leaveTypeViewModel.Name;
            //_leaveType.DefaultDays = leaveTypeViewModel.DefaultDays;


            var isSuccess = _leaveTypeRepository.Update(leaveType);
            if (!isSuccess)
            {
                ModelState.AddModelError("", "Something Went Wrong");
                return View(leaveTypeViewModel);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            if (!_leaveTypeRepository.IsExists(id))
            {
                return NotFound();
            }
            var leaveType = _leaveTypeRepository.FindById(id);
            var leaveTypeViewModel = _mapper.Map<LeaveTypeViewModel>(leaveType);
            return View(leaveTypeViewModel);
        }

        // POST: LeaveTypeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(LeaveTypeViewModel model)
        {
            try
            {
                var leaveType = _mapper.Map<LeaveType>(model);
                var isSucess = _leaveTypeRepository.Delete(leaveType);
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
