using System.Collections.Generic;
using TaskList.Models;

namespace TaskList.Interfaces
{
    public interface ITaskList
    {
        int GenerateNextIdNumber();
        void Inserttask(int ID, string TaskDescription, bool TaskCompleted);
        void DeleteTask(int taskID);
        void UpdateTask(int id, bool markedComplete);
    }
}
