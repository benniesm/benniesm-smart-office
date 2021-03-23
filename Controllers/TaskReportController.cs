using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SmartOffice.Data;
using SmartOffice.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using X.PagedList;
using X.PagedList.Mvc.Core;
using X.PagedList.Mvc;

namespace SmartOffice.Controllers
{
    public class TaskReportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TaskReportController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TaskReport
        public ViewResult Index(string searchString, string currentFilter, string sortOrder, int? page)
        {
            //ViewData["currentUserId"] = HttpContext.User.Identity.Name;
            ClaimsPrincipal currentUser = this.User;
            string idOfCurrentUser = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            string roleOfCurrentUser = currentUser.FindFirst(ClaimTypes.Role).Value;

            ViewBag.CurrentSort = sortOrder;
            ViewBag.OwnerSortParm = String.IsNullOrEmpty(sortOrder) ? "owner" : "";
            ViewBag.TimeSortParm = sortOrder == "Time" ? "time_desc" : "Time";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var filterReports = from s in _context.TaskReport
                join u in _context.Users on s.OwnerId equals u.Id into User
                from m in User.DefaultIfEmpty()
                select new TaskReport
                {
                    Id = s.Id,
                    OwnerId = s.OwnerId,
                    Activity = s.Activity,
                    TimeIn = s.TimeIn,
                    OwnerInfo = m.FirstName + " " + m.LastName
                };

            var allUsers = _context.Users
                .Select(s => new {
                    Names = s.FirstName + " " + s.LastName,
                    Id = s.Id
                })
                .ToList();
            // Filter reports by creator if current user does not have special access
            if (roleOfCurrentUser != "manager"
                && roleOfCurrentUser != "admin"
                && roleOfCurrentUser != "superuser")
            {
                filterReports = filterReports.Where(s => s.OwnerId.Equals(idOfCurrentUser));
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                string searchId = "";
                foreach (var item in allUsers)
                {
                    if (item.Names == searchString)
                    {
                        searchId = item.Id;
                    }
                }

                filterReports = filterReports.Where(s => s.OwnerId.Contains(searchId));
            }

            switch (sortOrder)
            {
                case "owner":
                    filterReports = filterReports.OrderByDescending(s => s.OwnerId);
                    break;
                case "Time":
                    filterReports = filterReports.OrderBy(s => s.TimeIn);
                    break;
                case "time_desc":
                    filterReports = filterReports.OrderByDescending(s => s.TimeIn);
                    break;
                default:
                    filterReports = filterReports.OrderByDescending(s => s.TimeIn);
                    break;
            }

            ViewData["idOfCurrentUser"] = idOfCurrentUser;

            // Boolean parameter to determine if edit/delete button will display.
            Dictionary<int, string> reviewInfo = new Dictionary<int, string>();

            foreach (var item in filterReports)
            {
                var taskReview = _context.TaskReview
                    .Where(s => s.TaskReportId.Equals(item.Id))
                    .ToList();

                if (taskReview.Count > 0)
                {
                    reviewInfo.Add(item.Id, "true");
                }
                else
                {
                    reviewInfo.Add(item.Id, "false");
                }
            }

            ViewData["reviewInfo"] = reviewInfo;

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(filterReports.ToPagedList(pageNumber, pageSize));

            //return View(await filterReports.ToListAsync());
        }



        // GET: TaskReport/Details/5
        //[Authorize(Roles ="admin,manager")]
        public async Task<IActionResult> Details(int? id)
        {
            ClaimsPrincipal currentUser = this.User;
            string idOfCurrentUser = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            string roleOfCurrentUser = currentUser.FindFirst(ClaimTypes.Role).Value;

            if (id == null)
            {
                return NotFound();
            }

            var taskReport = (from s in _context.TaskReport
                join u in _context.Users on s.OwnerId equals u.Id into User
                from m in User.DefaultIfEmpty()
                select new TaskReport
                {
                    Id = s.Id,
                    OwnerId = s.OwnerId,
                    Activity = s.Activity,
                    Achievement = s.Achievement,
                    Comments = s.Comments,
                    TimeIn = s.TimeIn,
                    OwnerInfo = m.FirstName + " " + m.LastName
                })
                .FirstOrDefault(m => m.Id == id);

            if (taskReport == null)
            {
                return NotFound();
            }

            //Deny access if no special access and current user is not the creator; or report has been reviewed.
            var taskReview = await _context.TaskReview
                .Where(s => s.TaskReportId.Equals(taskReport.Id))
                .ToListAsync();

            string editable = "true";
            if (taskReport.OwnerId != idOfCurrentUser
                && roleOfCurrentUser != "manager"
                && roleOfCurrentUser != "admin"
                && roleOfCurrentUser != "superuser")
            {
                return Unauthorized();
            }

            if (taskReport.OwnerId != idOfCurrentUser || taskReview.Count > 0) {
                editable = "false";                
            }

            ViewData["editable"] = editable;
            ViewData["currentUserId"] = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            ViewData["currentUserRole"] = currentUser.FindFirst(ClaimTypes.Role).Value;
            
            return View(taskReport);
        }

        // GET: TaskReport/Create
        public IActionResult Create()
        {
            ClaimsPrincipal currentUser = this.User;
            ViewData["currentUserId"] = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            return View();
        }

        // POST: TaskReport/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OwnerId,Activity,Achievement,Comments,TimeIn")] TaskReport taskReport)
        {
            if (ModelState.IsValid)
            {
                _context.Add(taskReport);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(taskReport);
        }

        // GET: TaskReport/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ClaimsPrincipal currentUser = this.User;
            string idOfCurrentUser = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (id == null)
            {
                return NotFound();
            }

            var taskReport = await _context.TaskReport.FindAsync(id);
            if (taskReport == null)
            {
                return NotFound();
            }

            // Deny Edit access unless current user is the creator or if reviewed
            var taskReview = await _context.TaskReview
                .Where(s => s.TaskReportId.Equals(taskReport.Id))
                .ToListAsync();

            if (taskReport.OwnerId != idOfCurrentUser || taskReview.Count > 0)
            {
                return Unauthorized();
            }

            return View(taskReport);
        }

        // POST: TaskReport/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OwnerId,Activity,Achievement,Comments,TimeIn")] TaskReport taskReport)
        {
            ClaimsPrincipal currentUser = this.User;
            string idOfCurrentUser = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            string roleOfCurrentUser = currentUser.FindFirst(ClaimTypes.Role).Value;

            if (id != taskReport.Id)
            {
                return NotFound();
            }

            // Deny Edit access unless current user is the creator or if reviewed
            var taskReview = await _context.TaskReview
                .Where(s => s.TaskReportId.Equals(taskReport.Id))
                .ToListAsync();
                
            if (taskReport.OwnerId != idOfCurrentUser || taskReview.Count > 0)
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taskReport);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskReportExists(taskReport.Id))
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
            return View(taskReport);
        }

        // GET: TaskReport/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ClaimsPrincipal currentUser = this.User;
            string idOfCurrentUser = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            string roleOfCurrentUser = currentUser.FindFirst(ClaimTypes.Role).Value;

            if (id == null)
            {
                return NotFound();
            }

            var taskReport = (from s in _context.TaskReport
                join u in _context.Users on s.OwnerId equals u.Id into User
                from m in User.DefaultIfEmpty()
                select new TaskReport
                {
                    Id = s.Id,
                    OwnerId = s.OwnerId,
                    Activity = s.Activity,
                    Achievement = s.Achievement,
                    Comments = s.Comments,
                    TimeIn = s.TimeIn,
                    OwnerInfo = m.FirstName + " " + m.LastName
                })
                .FirstOrDefault(m => m.Id == id);

            if (taskReport == null)
            {
                return NotFound();
            }

            // Deny Delete access unless current user is the creator or if reviewed
            var taskReview = await _context.TaskReview
                .Where(s => s.TaskReportId.Equals(taskReport.Id))
                .ToListAsync();
                
            if (taskReport.OwnerId != idOfCurrentUser || taskReview.Count > 0)
            {
                return Unauthorized();
            }

            return View(taskReport);
        }

        // POST: TaskReport/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ClaimsPrincipal currentUser = this.User;
            string idOfCurrentUser = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            string roleOfCurrentUser = currentUser.FindFirst(ClaimTypes.Role).Value;

            var taskReport = await _context.TaskReport.FindAsync(id);
            
            // Deny Edit access unless current user is the creator or if reviewed
            var taskReview = await _context.TaskReview
                .Where(s => s.TaskReportId.Equals(taskReport.Id))
                .ToListAsync();
                
            if (taskReport.OwnerId != idOfCurrentUser || taskReview.Count > 0)
            {
                return Unauthorized();
            }
            
            _context.TaskReport.Remove(taskReport);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaskReportExists(int id)
        {
            return _context.TaskReport.Any(e => e.Id == id);
        }
    }
}
