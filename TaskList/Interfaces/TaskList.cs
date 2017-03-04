using System.Collections.Generic;
using System.Linq;
using System.Web;
using TaskList.Models;

namespace TaskList.Interfaces
{
    public class TaskList : ITaskList
    {
        public List<TaskDetailsModel> ListAllTaskDetails = new List<TaskDetailsModel>();

        public List<TaskDetailsModel> GetAllTasks()
        {
            if (HttpContext.Current.Session["InMemoryTaskList"] != null)
                ListAllTaskDetails = (List<TaskDetailsModel>)HttpContext.Current.Session["InMemoryTaskList"];

            return ListAllTaskDetails;
        }

        public int GenerateNextIdNumber()
        {
            ListAllTaskDetails = GetAllTasks();
            int NextID;

            NextID = ListAllTaskDetails[ListAllTaskDetails.Count - 1].ID + 1;

            return NextID;
        }

        public void Inserttask(int taskID, string taskDescription, bool taskCompleted)
        {
            ListAllTaskDetails = GetAllTasks();

            TaskDetailsModel taskDetails = new TaskDetailsModel
            {
                ID = taskID,
                TaskDescription = HttpUtility.HtmlEncode(taskDescription),
                TaskCompleted = taskCompleted
            };

            ListAllTaskDetails.Add(taskDetails);
            HttpContext.Current.Session["InMemoryTaskList"] = ListAllTaskDetails;
        }

        public void DeleteTask(int taskID)
        {
            ListAllTaskDetails = GetAllTasks();
            TaskDetailsModel TaskItemToRemove = ListAllTaskDetails.FirstOrDefault(x => x.ID == taskID);

            if (TaskItemToRemove != null)
                ListAllTaskDetails.Remove(TaskItemToRemove);

            HttpContext.Current.Session["InMemoryTaskList"] = ListAllTaskDetails;
        }

        public void UpdateTask(int taskID, bool taskComplete)
        {
            ListAllTaskDetails = GetAllTasks();
            TaskDetailsModel TaskItemToUpdate = ListAllTaskDetails.FirstOrDefault(x => x.ID == taskID);

            if (TaskItemToUpdate != null)
                TaskItemToUpdate.TaskCompleted = taskComplete;

            HttpContext.Current.Session["InMemoryTaskList"] = ListAllTaskDetails;
        }
    }
}