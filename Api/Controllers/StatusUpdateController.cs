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
    public class StatusUpdateController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StatusUpdateController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPut("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<TaskAssignment>> Edit(int id, [Bind("Id,OwnerId,RunnerId,Title,Instructions,TimeIn,Deadline,Started,Executed,ExecutedNotes,Completed,CompletionNotes,status")] TaskAssignment taskAssignment)
        {
            if (id != taskAssignment.Id)
            {
                return NotFound();
            }

            try
            {
                _context.Update(taskAssignment);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return Ok(new {updated = id});
        }
    }
}
