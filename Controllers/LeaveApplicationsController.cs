using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LeaveApplicationApp.Data;
using LeaveApplicationApp.Models;
using Microsoft.AspNetCore.Authorization;
using LeaveApplicationApp.ViewModels;

namespace LeaveApplicationApp.Controllers
{
    public class LeaveApplicationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LeaveApplicationsController(ApplicationDbContext context)
        {
            _context = context;
        }
        private List<SelectListItem> GetLeaveTypeList()
        {
            return Enum.GetValues(typeof(LeaveType))
                .Cast<LeaveType>()
                .Select(e => new SelectListItem
                {
                    Value = e.ToString(),
                    Text = Enum.GetName(typeof(LeaveType), e)
                })
                .ToList();
        }

        // GET: LeaveApplications
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.LeaveApplications.Include(l => l.Employee);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: LeaveApplications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveApplication = await _context.LeaveApplications
                .Include(l => l.Employee)
                .FirstOrDefaultAsync(m => m.LeaveApplicationId == id);
            if (leaveApplication == null)
            {
                return NotFound();
            }

            return View(leaveApplication);
        }

        // GET: LeaveApplications/LeaveApplicationsByMonth
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> LeaveApplicationsByMonth(int? year, int? month)
        {
            if (year == null || month == null)
            {
                // If year or month parameters are not provided, redirect to a default view or handle the error
                return RedirectToAction(nameof(Index));
            }

            // Get the first day and the last day of the selected month
            DateTime startDate = new DateTime(year.Value, month.Value, 1);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);

            // Retrieve leave applications within the selected month
            var leaveApplications = await _context.LeaveApplications
                .Include(l => l.Employee)
                .Where(l => l.ApplicationTime >= startDate && l.ApplicationTime <= endDate)
                .ToListAsync();

            // Prepare a list of view models to pass to the view
            List<LeaveApplicationViewModel> viewModelList = new List<LeaveApplicationViewModel>();
            foreach (var leaveApp in leaveApplications)
            {
                int totalDaysApplied = (leaveApp.EndDate - leaveApp.StartDate).Days + 1;
                var viewModel = new LeaveApplicationViewModel
                {
                    EmployeeName = $"{leaveApp.Employee.FirstName} {leaveApp.Employee.LastName}",
                    ApplicationTime = leaveApp.ApplicationTime,
                    TotalDaysApplied = totalDaysApplied,
                    StartDate = leaveApp.StartDate,
                    WorkLeaveType = leaveApp.WorkLeaveType // Include the LeaveType
                };
                viewModelList.Add(viewModel);
            }

            return View(viewModelList);
        }

        // GET: LeaveApplications/Create
        public IActionResult Create()
        {
            ViewData["LeaveType"] = GetLeaveTypeList();
            ViewData["FkEmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "FirstName");
            return View();
        }

        // POST: LeaveApplications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LeaveApplicationId,WorkLeaveType,ApplicationTime,StartDate,EndDate,FkEmployeeId")] LeaveApplication leaveApplication)
        {
            if (ModelState.IsValid)
            {
                _context.Add(leaveApplication);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LeaveType"] = GetLeaveTypeList();
            ViewData["FkEmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "FirstName", leaveApplication.FkEmployeeId);
            return View(leaveApplication);
        }

        // GET: LeaveApplications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveApplication = await _context.LeaveApplications.FindAsync(id);
            if (leaveApplication == null)
            {
                return NotFound();
            }
            ViewData["LeaveType"] = GetLeaveTypeList();
            ViewData["FkEmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "FirstName", leaveApplication.FkEmployeeId);
            return View(leaveApplication);
        }

        // POST: LeaveApplications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LeaveApplicationId,WorkLeaveType,ApplicationTime,StartDate,EndDate,FkEmployeeId")] LeaveApplication leaveApplication)
        {
            if (id != leaveApplication.LeaveApplicationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(leaveApplication);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaveApplicationExists(leaveApplication.LeaveApplicationId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["LeaveType"] = GetLeaveTypeList();
            ViewData["FkEmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "FirstName", leaveApplication.FkEmployeeId);
            return View(leaveApplication);
        }

        // GET: LeaveApplications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveApplication = await _context.LeaveApplications
                .Include(l => l.Employee)
                .FirstOrDefaultAsync(m => m.LeaveApplicationId == id);
            if (leaveApplication == null)
            {
                return NotFound();
            }

            return View(leaveApplication);
        }

        // POST: LeaveApplications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var leaveApplication = await _context.LeaveApplications.FindAsync(id);
            if (leaveApplication != null)
            {
                _context.LeaveApplications.Remove(leaveApplication);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeaveApplicationExists(int id)
        {
            return _context.LeaveApplications.Any(e => e.LeaveApplicationId == id);
        }
    }
}
