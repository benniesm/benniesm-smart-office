using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartOffice.Models
{
    public class TaskAssignment
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Created by")]
        public string OwnerId { get; set; }

        [Required]
        [Display(Name = "Execution by")]
        public string RunnerId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Task details")]
        public string Instructions { get; set; }

        [Display(Name = "Time submitted")]
        public DateTime TimeIn { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Completion deadline")]
        public DateTime Deadline { get; set; }

        [Display(Name = "Start date")]
        public DateTime? Started { get; set; }

        [Display(Name = "Execution date")]
        public DateTime? Executed { get; set; }

        [Display(Name = "Execution note")]
        public string ExecutedNotes { get; set; }

        [Display(Name = "Completion date")]
        public DateTime? Completed { get; set; }

        [Display(Name = "Completion note")]
        public string CompletionNotes { get; set; }
        
        public string status { get; set; }

        public string Owner;
        public string Runner;
    }
}