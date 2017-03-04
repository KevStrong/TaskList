using System;
using System.Web.Mvc;
using TaskList.Interfaces;
using TaskList.Models;

namespace TaskList.Controllers
{
    public class TaskListController : Controller
    {
        public ITaskList tasklist;
        private TaskDetailsModel ListAllTaskDetails = new TaskDetailsModel();

        private static readonly Random GetRandomNumber = new Random();

        public TaskListController()
        {
            this.tasklist = new Interfaces.TaskList();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(TaskDetailsModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            int NextID = 1;
            if (TaskDetailsModel.Tasks != null && TaskDetailsModel.Tasks.Count > 0)
                NextID = tasklist.GenerateNextIdNumber();

            tasklist.Inserttask(NextID, model.TaskDescription, false);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            tasklist.DeleteTask(id);
            return RedirectToAction("Index");
        }

        public JsonResult Update(int id, bool markedComplete)
        {
            tasklist.UpdateTask(id, markedComplete);
            return Json(new { success = "true" });
        }
    }
}