using System;
using System.Collections.Generic;
using System.Web.Mvc;
using TaskList.Interfaces;
using TaskList.Models;

namespace TaskList.Controllers
{
    public class TaskListController : Controller
    {
        public ITaskList tasklist;
        private List<TaskDetailsModel> ListAllTaskDetails = new List<TaskDetailsModel>();
        private static readonly Random GetRandomNumber = new Random();

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
            ListAllTaskDetails = tasklist.GetAllTasks();
            if (!ModelState.IsValid)
            {
                model.Tasks = ListAllTaskDetails;
                return View("Index", model);
            }

            int NextID = 1;
            if (ListAllTaskDetails != null && ListAllTaskDetails.Count > 0)
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