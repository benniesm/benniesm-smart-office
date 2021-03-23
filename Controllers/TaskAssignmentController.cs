using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SmartOffice.Data;
using SmartOffice.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using X.PagedList;
using X.PagedList.Mvc.Core;
using X.PagedList.Mvc;

namespace SmartOffice.Controllers
{
    public class TaskAssignmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TaskAssignmentController(ApplicationDbContext context)
        {
            _context = context;
        }
/*
        class UsersModel
        { 
            public IEnumerable<User> Users { get; set; }
            public string User { get; set;}
        }
*/
        // GET: TaskAssignment
        public ViewResult Index(string searchString, string currentFilter, string sortOrder, int? page)
        {
            ClaimsPrincipal currentUser = this.User;
            string idOfCurrentUser = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            string roleOfCurrentUser = currentUser.FindFirst(ClaimTypes.Role).Value;

            ViewBag.CurrentSort = sortOrder;
            ViewBag.RunnerSortParm = String.IsNullOrEmpty(sortOrder) ? "runner" : "";
            ViewBag.DeadlineSortParm = sortOrder == "Deadline" ? "deadline_desc" : "Deadline";
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

            var filterAssignments = from s in _context.TaskAssignment
                join u in _context.Users on s.RunnerId equals u.Id into User
                from m in User.DefaultIfEmpty()
                select new TaskAssignment
                {
                    Id = s.Id,
                    OwnerId = s.OwnerId,
                    RunnerId = s.RunnerId,
                    Title = s.Title,
                    Deadline = s.Deadline,
                    TimeIn = s.TimeIn,
                    status = s.status,
                    Runner = m.FirstName + " " + m.LastName
                };

            // Filter assignments by creator if current user does not have special access
            if (roleOfCurrentUser != "manager"
                && roleOfCurrentUser != "admin"
                && roleOfCurrentUser != "superuser")
            {
                filterAssignments = filterAssignments
                    .Where(s => s.OwnerId.Equals(idOfCurrentUser)
                        || s.RunnerId.Equals(idOfCurrentUser));
            }

            var allUsers = _context.Users
                .Select(s => new {
                    Names = s.FirstName + " " + s.LastName,
                    Id = s.Id
                })
                .ToList();

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

                filterAssignments = filterAssignments.Where(s => s.RunnerId.Contains(searchId));
            }

            switch (sortOrder)
            {
                case "runner":
                    filterAssignments = filterAssignments.OrderByDescending(s => s.RunnerId);
                    break;
                case "Deadline":
                    filterAssignments = filterAssignments.OrderBy(s => s.Deadline);
                    break;
                case "deadline_desc":
                    filterAssignments = filterAssignments.OrderByDescending(s => s.Deadline);
                    break;
                case "Time":
                    filterAssignments = filterAssignments.OrderBy(s => s.TimeIn);
                    break;
                case "time_desc":
                    filterAssignments = filterAssignments.OrderByDescending(s => s.TimeIn);
                    break;
                default:
                    filterAssignments = filterAssignments.OrderByDescending(s => s.TimeIn);
                    break;
            }

            ViewData["idOfCurrentUser"] = idOfCurrentUser;

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(filterAssignments.ToPagedList(pageNumber, pageSize));
        }

        // GET: TaskAssignment/Details/5
        public IActionResult Details(int? id)
        {
            ClaimsPrincipal currentUser = this.User;
            string idOfCurrentUser = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            string roleOfCurrentUser = currentUser.FindFirst(ClaimTypes.Role).Value;

            if (id == null)
            {
                return NotFound();
            }

            var taskAssignment = (from s in _context.TaskAssignment
                join u in _context.Users on s.OwnerId equals u.Id into User
                from m in User.DefaultIfEmpty()
                join x in _context.Users on s.RunnerId equals x.Id into Runner
                from r in Runner.DefaultIfEmpty()
                select new TaskAssignment
                {
                    Id = s.Id,
                    OwnerId = s.OwnerId,
                    RunnerId = s.RunnerId,
                    Title = s.Title,
                    Instructions = s.Instructions,
                    TimeIn = s.TimeIn,
                    Deadline = s.Deadline,
                    Started = s.Started,
                    Executed = s.Executed,
                    ExecutedNotes = s.ExecutedNotes,
                    Completed = s.Completed,
                    CompletionNotes = s.CompletionNotes,
                    status = s.status,
                    Owner = m.FirstName + " " + m.LastName,
                    Runner = r.FirstName + " " + r.LastName
                })
                .FirstOrDefault(m => m.Id == id);

            if (taskAssignment == null)
            {
                return NotFound();
            }

            if (taskAssignment.OwnerId != idOfCurrentUser
                && taskAssignment.RunnerId != idOfCurrentUser
                && roleOfCurrentUser != "manager"
                && roleOfCurrentUser != "admin"
                && roleOfCurrentUser != "superuser")
            {
                return Unauthorized();
            }

            ViewBag.Editable = true;
            ViewBag.Role = "ordinary";
            if (taskAssignment.OwnerId != idOfCurrentUser
                || taskAssignment.status == "executed"
                || taskAssignment.status == "completed")
            {
                ViewBag.Editable = false;
            }
            if (roleOfCurrentUser == "superuser"
                || roleOfCurrentUser == "admin"
                || roleOfCurrentUser == "manager") 
            {
                ViewBag.Role = "special";
            }

            ViewBag.CurrentUserId = idOfCurrentUser;

            return View(taskAssignment);
        }

        // GET: TaskAssignment/Create
        public IActionResult Create()
        {
/*            var UsersModel = new UsersModel()
            {
                
            };
*/
            var allUsers = _context.Users
            .Select(s => new ApplicationUser {
                FullName = s.FirstName + " " + s.LastName,
                Id = s.Id
            })
            .OrderBy(x => x.FullName)
            .ToList();
            ViewBag.AllUsers = allUsers;

            ClaimsPrincipal currentUser = this.User;
            ViewData["currentUserId"] = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            return View();
        }

        // POST: TaskAssignment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OwnerId,RunnerId,Title,Instructions,TimeIn,Deadline,Started,Executed,ExecutedNotes,Completed,CompletionNotes,status")] TaskAssignment taskAssignment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(taskAssignment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(taskAssignment);
        }

        // GET: TaskAssignment/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ClaimsPrincipal currentUser = this.User;
            string idOfCurrentUser = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (id == null)
            {
                return NotFound();
            }

            var taskAssignment = await _context.TaskAssignment.FindAsync(id);
            if (taskAssignment == null)
            {
                return NotFound();
            }

            if (taskAssignment.OwnerId != idOfCurrentUser
                || taskAssignment.status == "executed"
                || taskAssignment.status == "completed")
            {
                return Unauthorized();
            }

            var allUsers = _context.Users
            .Select(s => new ApplicationUser {
                FullName = s.FirstName + " " + s.LastName,
                Id = s.Id
            })
            .OrderBy(x => x.FullName)
            .ToList();
            ViewBag.AllUsers = allUsers;

            return View(taskAssignment);
        }

        // POST: TaskAssignment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OwnerId,RunnerId,Title,Instructions,TimeIn,Deadline,Started,Executed,ExecutedNotes,Completed,CompletionNotes,status")] TaskAssignment taskAssignment)
        {
            ClaimsPrincipal currentUser = this.User;
            string idOfCurrentUser = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (id != taskAssignment.Id)
            {
                return NotFound();
            }

            if (taskAssignment.OwnerId != idOfCurrentUser
                || taskAssignment.status == "executed"
                || taskAssignment.status == "completed")
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taskAssignment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskAssignmentExists(taskAssignment.Id))
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
            return View(taskAssignment);
        }

        // GET: TaskAssignment/Delete/5
        public IActionResult Delete(int? id)
        {
            ClaimsPrincipal currentUser = this.User;
            string idOfCurrentUser = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            
            if (id == null)
            {
                return NotFound();
            }

            var taskAssignment = (from s in _context.TaskAssignment
                join u in _context.Users on s.OwnerId equals u.Id into User
                from m in User.DefaultIfEmpty()
                join x in _context.Users on s.RunnerId equals x.Id into Runner
                from r in Runner.DefaultIfEmpty()
                select new TaskAssignment
                {
                    Id = s.Id,
                    OwnerId = s.OwnerId,
                    RunnerId = s.RunnerId,
                    Title = s.Title,
                    Instructions = s.Instructions,
                    TimeIn = s.TimeIn,
                    Deadline = s.Deadline,
                    Started = s.Started,
                    Executed = s.Executed,
                    ExecutedNotes = s.ExecutedNotes,
                    Completed = s.Completed,
                    CompletionNotes = s.CompletionNotes,
                    status = s.status,
                    Owner = m.FirstName + " " + m.LastName,
                    Runner = r.FirstName + " " + r.LastName
                })
                .FirstOrDefault(m => m.Id == id);

            if (taskAssignment == null)
            {
                return NotFound();
            }
            
            if (taskAssignment.OwnerId != idOfCurrentUser
            || taskAssignment.status == "executed"
            || taskAssignment.status == "completed")
            {
                return Unauthorized();
            }

            return View(taskAssignment);
        }

        // POST: TaskAssignment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ClaimsPrincipal currentUser = this.User;
            string idOfCurrentUser = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            var taskAssignment = await _context.TaskAssignment.FindAsync(id);
            
            if (taskAssignment.OwnerId != idOfCurrentUser
                || taskAssignment.status == "executed"
                || taskAssignment.status == "completed")
            {
                return Unauthorized();
            }

            _context.TaskAssignment.Remove(taskAssignment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaskAssignmentExists(int id)
        {
            return _context.TaskAssignment.Any(e => e.Id == id);
        }
    }
}
