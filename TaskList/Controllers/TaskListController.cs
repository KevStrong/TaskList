using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using TaskList.Interfaces;
using TaskList.Models;

namespace TaskList.Controllers
{
    public class TaskListController : Controller
    {
        public ITaskList tasklist;
        private List<TaskDetailsModel> ListAllTaskDetails = new List<TaskDetailsModel>();

        public TaskListController()
        {
            this.tasklist = new Interfaces.TaskList();
        }

        public ActionResult Index()
        {
            TaskListModel modelTaskList = new TaskListModel();
            modelTaskList.Tasks = tasklist.GetAllTasks();

            return View(modelTaskList);
        }

        [HttpPost]
        public ActionResult Create(TaskListModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Tasks = tasklist.GetAllTasks();
                return View("Index", model);
            }

            TaskDetailsModel taskDetails = new TaskDetailsModel
            {
                ID = tasklist.GenerateNextIdNumber(),
                TaskDescription = HttpUtility.HtmlEncode(model.TaskDescription),
                TaskCompleted = false
            };
            tasklist.Inserttask(taskDetails);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            tasklist.DeleteTask(id);
            return RedirectToAction("Index");
        }
    }
}