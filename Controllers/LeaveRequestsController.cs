using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagementSystem.Controllers
{
    public class LeaveRequestsController : Controller
    {
        private readonly LeaveManagementDbContext _context;

        public LeaveRequestsController(LeaveManagementDbContext context)
        {
            _context = context;
        }

        // GET: LeaveRequests
        public async Task<IActionResult> Index(string searchString)
        {
            // Hämta alla ledighetsansökningar inklusive anställda och ledighetstyper
            var leaveRequests = from leaveRequest in _context.LeaveRequests.Include(l => l.Employee).Include(l => l.LeaveType)
                                select leaveRequest;

            // Filtrera om söksträngen inte är tom
            if (!String.IsNullOrEmpty(searchString))
            {
                leaveRequests = leaveRequests.Where(lr => lr.Employee.Name.Contains(searchString));
            }

            // Skicka söksträngen till vyn
            ViewData["CurrentFilter"] = searchString;

            return View(await leaveRequests.ToListAsync());
        }

        // GET: LeaveRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveRequest = await _context.LeaveRequests
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .FirstOrDefaultAsync(m => m.RequestId == id);
            if (leaveRequest == null)
            {
                return NotFound();
            }

            return View(leaveRequest);
        }

        // GET: LeaveRequests/Create
        public IActionResult Create()
        {
            ViewBag.Employees = new SelectList(_context.Employees, "Id", "Name");
            ViewBag.LeaveTypes = new SelectList(_context.LeaveTypes, "Id", "TypeName");
            return View(new LeaveRequest());
        }

        // POST: LeaveRequests/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RequestId,StartDate,EndDate,LeaveTypeId,EmployeeId")] LeaveRequest leaveRequest)
        {
            if (ModelState.IsValid)
            {
                leaveRequest.ApplicationDate = DateTime.Now;
                _context.Add(leaveRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Återskapa dropdown-listor om formuläret har fel
            ViewBag.Employees = new SelectList(_context.Employees, "Id", "Name", leaveRequest.EmployeeId);
            ViewBag.LeaveTypes = new SelectList(_context.LeaveTypes, "Id", "TypeName", leaveRequest.LeaveTypeId);
            return View(leaveRequest);
        }

        // GET: LeaveRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveRequest = await _context.LeaveRequests.FindAsync(id);
            if (leaveRequest == null)
            {
                return NotFound();
            }

            ViewBag.Employees = new SelectList(_context.Employees, "Id", "Name", leaveRequest.EmployeeId);
            ViewBag.LeaveTypes = new SelectList(_context.LeaveTypes, "Id", "TypeName", leaveRequest.LeaveTypeId);
            return View(leaveRequest);
        }

        // POST: LeaveRequests/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RequestId,StartDate,EndDate,LeaveTypeId,EmployeeId")] LeaveRequest leaveRequest)
        {
            if (id != leaveRequest.RequestId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(leaveRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaveRequestExists(leaveRequest.RequestId))
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

            ViewBag.Employees = new SelectList(_context.Employees, "Id", "Name", leaveRequest.EmployeeId);
            ViewBag.LeaveTypes = new SelectList(_context.LeaveTypes, "Id", "TypeName", leaveRequest.LeaveTypeId);
            return View(leaveRequest);
        }

        // GET: LeaveRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveRequest = await _context.LeaveRequests
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .FirstOrDefaultAsync(m => m.RequestId == id);
            if (leaveRequest == null)
            {
                return NotFound();
            }

            return View(leaveRequest);
        }

        // POST: LeaveRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var leaveRequest = await _context.LeaveRequests.FindAsync(id);
            if (leaveRequest != null)
            {
                _context.LeaveRequests.Remove(leaveRequest);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeaveRequestExists(int id)
        {
            return _context.LeaveRequests.Any(e => e.RequestId == id);
        }
    }
}
