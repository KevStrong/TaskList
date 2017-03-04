using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;


namespace TaskList.Models
{
    public class TaskDetailsModel
    {
        public int ID { get; set;}
        
        [AllowHtml]
        [Required(ErrorMessage="Please specify a task description before submitting again")]
        [DisplayName("Task")]
        public string TaskDescription { get; set; }

        [DisplayName("Completed")]
        public bool TaskCompleted { get; set; }

        public static List<TaskDetailsModel> Tasks = new List<TaskDetailsModel>();
    }   
}