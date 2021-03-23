using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartOffice.Data;
using SmartOffice.Models;
using Microsoft.AspNetCore.Authorization;

namespace SmartOffice.ApiControllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles ="superuser,admin,manager")]
    public class ReportReviewController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ReportReviewController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskReview>> Get(int? id)
        {
            if (id == null)
            {
                NotFound();
            }
            
            var review = await _context.TaskReview
                .FirstOrDefaultAsync(m => m.TaskReportId == id);
            if (review == null)
            {
                return NotFound();
            }

            return review;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<TaskReview>> Create([Bind("Id,OwnerId,TaskReportId,Details,TimeIn")] TaskReview taskReview)
        {
            _context.Add(taskReview);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = taskReview.Id }, taskReview);
        }
    }
}
