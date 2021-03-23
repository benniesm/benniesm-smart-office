using System;
using System.ComponentModel.DataAnnotations;

namespace SmartOffice.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }
        [Required]
        public string Owner { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public string GroupName { get; set; }
        [Display(Name = "Time submitted")]
        public DateTime TimeIn { get; set; } = DateTime.Now;
    }
}