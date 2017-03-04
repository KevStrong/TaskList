using System.Linq;
using System.Web;
using TaskList.Models;

namespace TaskList.Interfaces
{
    public class TaskList : ITaskList
    {
        public int GenerateNextIdNumber()
        {
            int NextID;

            NextID = TaskDetailsModel.Tasks[TaskDetailsModel.Tasks.Count - 1].ID + 1;

            return NextID;
        }

        public void Inserttask(int taskID, string taskDescription, bool taskCompleted)
        {
            
            TaskDetailsModel taskDetails = new TaskDetailsModel
            {
                ID = taskID,
                TaskDescription = HttpUtility.HtmlEncode(taskDescription),
                TaskCompleted = taskCompleted
            };

            TaskDetailsModel.Tasks.Add(taskDetails);
        }

        public void DeleteTask(int taskID)
        {
            TaskDetailsModel TaskItemToRemove = TaskDetailsModel.Tasks.FirstOrDefault(x => x.ID == taskID);

            if (TaskItemToRemove != null)
                TaskDetailsModel.Tasks.Remove(TaskItemToRemove);
        }
        public void UpdateTask(int taskID, bool taskComplete)
        {
            TaskDetailsModel TaskItemToUpdate = TaskDetailsModel.Tasks.FirstOrDefault(x => x.ID == taskID);

            if (TaskItemToUpdate != null)
                TaskItemToUpdate.TaskCompleted = taskComplete;
        }
    }
}