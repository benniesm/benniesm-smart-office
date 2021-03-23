using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ChatMessageController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ChatMessageController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChatMessage>>> Get()
        {
            var chatMessages = await _context.ChatMessage.ToListAsync();

            return chatMessages;
        }
    }
}
