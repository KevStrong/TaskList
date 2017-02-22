using System;
using System.Collections.Generic;
using System.Linq;
using TaskList.Models;

namespace TaskList.Interfaces
{
    public class TaskList : ITaskList
    {
        public List<TaskDetailsModel> ListAllTaskDetails = new List<TaskDetailsModel>();
        private static readonly Random GetRandomNumber = new Random();

        public List<TaskDetailsModel> GetAllTasks()
        {
            if (System.Web.HttpContext.Current.Session["InMemoryTaskList"] != null)
                ListAllTaskDetails = (List<TaskDetailsModel>)System.Web.HttpContext.Current.Session["InMemoryTaskList"];

            return ListAllTaskDetails;
        }

        public int GenerateNextIdNumber()
        {
            ListAllTaskDetails = GetAllTasks();
            int NextID;

            try
            {
                if (ListAllTaskDetails == null || ListAllTaskDetails.Count == 0)
                {
                    NextID = 1;
                }
                else
                {
                    NextID = ListAllTaskDetails[ListAllTaskDetails.Count - 1].ID + 1;
                }
            }
            catch
            {
                NextID = GetRandomNumber.Next(100, 999);
            }
            return NextID;
        }

        public void Inserttask(TaskDetailsModel taskDetails)
        {
            ListAllTaskDetails = GetAllTasks();
    
            ListAllTaskDetails.Add(taskDetails);
            System.Web.HttpContext.Current.Session["InMemoryTaskList"] = ListAllTaskDetails;
        }

        public void DeleteTask(int taskID)
        {
            ListAllTaskDetails = GetAllTasks();
            TaskDetailsModel TaskItemToRemove = ListAllTaskDetails.FirstOrDefault(x => x.ID == taskID);

            if (TaskItemToRemove != null)
                ListAllTaskDetails.Remove(TaskItemToRemove);

            System.Web.HttpContext.Current.Session["InMemoryTaskList"] = ListAllTaskDetails;
        }
    }
}