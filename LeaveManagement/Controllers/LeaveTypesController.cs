using LeaveManagement.Contracts;
using LeaveManagement.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagement.Controllers
{
    public class LeaveTypesController : Controller
    {
        private ILeaveTypeRepository _leaveTypeRepository { get; }

        public LeaveTypesController(ILeaveTypeRepository leaveTypeRepository)
        {
            _leaveTypeRepository = leaveTypeRepository;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var leaveType = _leaveTypeRepository.FindAll();
            return View(leaveType);
        }
        [HttpGet]
        public IActionResult Details()
        {
            var leaveType = _leaveTypeRepository.FindAll();
            return View(leaveType);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(LeaveType leaveType)
        {
            var data = _leaveTypeRepository.Create(leaveType);
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult EditLeaveType()
        {
            return View();
        }

        [HttpPost]
        public IActionResult EditLeaveType(LeaveType leaveType)
        {
            var data = _leaveTypeRepository.Create(leaveType);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult DeleteLeaveType()
        {
            return View();
        }

        [HttpPost]
        public IActionResult DeleteLeaveType(LeaveType leaveType)
        {
            var data = _leaveTypeRepository.Create(leaveType);
            return RedirectToAction("Index");
        }


    }
}
