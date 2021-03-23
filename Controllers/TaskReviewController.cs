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
using System.Security.Claims;

namespace SmartOffice.Controllers
{
    [Authorize(Roles ="superuser,admin,manager")]
    public class TaskReviewController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TaskReviewController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TaskReview
        public async Task<IActionResult> Index()
        {

            var reviews = await (from s in _context.TaskReview
                join u in _context.Users on s.OwnerId equals u.Id into User
                from m in User.DefaultIfEmpty()
                join p in _context.TaskReport on s.TaskReportId equals p.Id into Report
                from n in Report.DefaultIfEmpty()
                select new TaskReview
                {
                    Id = s.Id,
                    OwnerId = s.OwnerId,
                    Activity = n.Activity,
                    TimeIn = s.TimeIn,
                    OwnerInfo = m.FirstName + " " + m.LastName
                })
                .ToListAsync();

            return View(reviews);
        }

        // GET: TaskReview/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskReview = (from s in _context.TaskReview
                join u in _context.Users on s.OwnerId equals u.Id into User
                from m in User.DefaultIfEmpty()
                join p in _context.TaskReport on s.TaskReportId equals p.Id into Report
                from n in Report.DefaultIfEmpty()
                join w in _context.Users on n.OwnerId equals w.Id into Author
                from r in Author.DefaultIfEmpty()
                select new TaskReview
                {
                    Id = s.Id,
                    OwnerId = s.OwnerId,
                    TimeIn = s.TimeIn,
                    Details = s.Details,
                    Activity = n.Activity,
                    Achievement = n.Achievement,
                    Comments = n.Comments,
                    OwnerInfo = m.FirstName + " " + m.LastName,
                    Author = r.FirstName + " " + r.LastName
                })
                .FirstOrDefault(m => m.Id == id);

            if (taskReview == null)
            {
                return NotFound();
            }

            return View(taskReview);
        }

        // GET: TaskReview/Create
        public IActionResult Create()
        {
            ClaimsPrincipal currentUser = this.User;
            ViewData["currentUserId"] = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            return View();
        }

        // POST: TaskReview/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OwnerId,TaskReportId,Details,TimeIn")] TaskReview taskReview)
        {
            if (ModelState.IsValid)
            {
                _context.Add(taskReview);
                await _context.SaveChangesAsync();
                //return RedirectToAction(actionName: "Details", controllerName: "TaskReport", routeValues: new { id = "TaskReportId" });
                return RedirectToAction(nameof(Index));
            }
            return View(taskReview);
        }

        // GET: TaskReview/Edit/5
        /*
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskReview = await _context.TaskReview.FindAsync(id);
            if (taskReview == null)
            {
                return NotFound();
            }
            return View(taskReview);
        }

        // POST: TaskReview/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OwnerId,TaskReportId,Details,TimeIn")] TaskReview taskReview)
        {
            if (id != taskReview.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taskReview);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskReviewExists(taskReview.Id))
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
            return View(taskReview);
        }

        // GET: TaskReview/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskReview = await _context.TaskReview
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taskReview == null)
            {
                return NotFound();
            }

            return View(taskReview);
        }

        // POST: TaskReview/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var taskReview = await _context.TaskReview.FindAsync(id);
            _context.TaskReview.Remove(taskReview);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        */

        private bool TaskReviewExists(int id)
        {
            return _context.TaskReview.Any(e => e.Id == id);
        }
    }
}
