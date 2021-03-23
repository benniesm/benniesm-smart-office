using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartOffice.Models
{
    public class TaskReview
    {
        public int Id { get; set; }
        [Display(Name = "Reviewed by")]
        public string OwnerId { get; set; }
        public int TaskReportId { get; set; }
        [Required]
        [Display(Name = "Review")]
        public string Details { get; set; }
        [Display(Name = "Reviewed at")]
        public DateTime TimeIn { get; set; } = DateTime.Now;
        [ForeignKey("TaskReportId")]
        public TaskReport TaskReport { get; set; }
        
        public string Activity;
        public string Achievement;
        public string Comments;
        public string OwnerInfo;
        public string Author;
    }
}