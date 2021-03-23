using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SmartOffice.Models;

namespace SmartOffice.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<SmartOffice.Models.TaskReport> TaskReport { get; set; }
        public DbSet<SmartOffice.Models.TaskReview> TaskReview { get; set; }
        public DbSet<SmartOffice.Models.TaskAssignment> TaskAssignment { get; set; }
        public DbSet<SmartOffice.Models.ChatMessage> ChatMessage { get; set; }
    }
}