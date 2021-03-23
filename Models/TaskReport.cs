using System;
using System.ComponentModel.DataAnnotations;

namespace SmartOffice.Models
{
    public class TaskReport
    {
        public int Id { get; set; }
        public string OwnerId { get; set; }
        [Required]
        public string Activity { get; set; }
        [Required]
        public string Achievement { get; set; }
        public string Comments { get; set; }
        [Display(Name = "Time submitted")]
        public DateTime TimeIn { get; set; } = DateTime.Now;

        public TaskReview TaskReview { get; set; }
        
        public string OwnerInfo;
    }
}