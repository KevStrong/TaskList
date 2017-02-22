using System.Collections.Generic;
using TaskList.Models;

namespace TaskList.Interfaces
{
    public interface ITaskList
    {
        List<TaskDetailsModel> GetAllTasks();
        int GenerateNextIdNumber();
        void Inserttask(TaskDetailsModel taskDetails);
        void DeleteTask(int taskID);
    }
}
