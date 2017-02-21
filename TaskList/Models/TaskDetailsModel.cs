using System;
using System.Data;
using System.Web.Mvc;
using System.Configuration;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


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
    }

    
    public class TaskListModel : TaskDetailsModel
    {
        public List<TaskDetailsModel> Tasks { get; set; }
    }   
}