using System.Collections.Generic;
using TaskList.Models;

namespace TaskList.Interfaces
{
    public interface ITaskList
    {
        List<TaskDetailsModel> GetAllTasks();
        int GenerateNextIdNumber();
        void Inserttask(int ID, string TaskDescription, bool TaskCompleted);
        void DeleteTask(int taskID);
    }
}
