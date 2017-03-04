using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Web;
using TaskList;
using TaskList.Models;
using TaskList.Interfaces;
using TaskList.Controllers;
using System.Web.Mvc;
using System.Web.Routing;
using System.Collections.Generic;

namespace TaskList_UnitTests
{
    [TestClass]
    public class TaskListController_Tests
    {
        private Mock<ITaskList> mockTasklist;
 
        [TestInitialize]
        public void Setup()
        {
            this.mockTasklist = new Mock<ITaskList>();
        }

        [TestMethod]
        public void Index_with_1_Task_in_List()
        {
            #region Arrange
            TaskDetailsModel mockTaskDetails = new TaskDetailsModel
            {
                ID = 1,
                TaskCompleted = false,
                TaskDescription = HttpUtility.HtmlEncode("Mocked Task"),

            };      
            TaskDetailsModel.Tasks.Add(mockTaskDetails);

            TaskListController controller = new TaskListController();
            controller.tasklist = mockTasklist.Object;
            #endregion

            #region Act
            var result = (ActionResult)controller.Index();
            var viewModel = controller.ViewData.Model as TaskDetailsModel;
            #endregion

            #region Assert
            Assert.AreEqual(1, TaskDetailsModel.Tasks.Count);
            #endregion
        }

        [TestMethod]
        public void Create_Task_Model_isInvalid()
        {
            #region Arrange
            TaskDetailsModel mockTaskList = new TaskDetailsModel
            {
                ID = 1,
                TaskCompleted = false,
                TaskDescription = null
            };

            TaskListController controller = new TaskListController();
            controller.tasklist = mockTasklist.Object;
            controller.ViewData.ModelState.AddModelError("TaskDescription", "Please specify a task description before submitting again");
            #endregion

            #region Act
            var result = (ViewResult)controller.Create(mockTaskList);
            var viewModel = controller.ViewData.Model as TaskDetailsModel;
            #endregion

            #region Assert
            Assert.IsFalse(result.ViewData.ModelState.IsValid);
            Assert.AreEqual(0, TaskDetailsModel.Tasks.Count);
            #endregion
        }

        //[TestMethod]
        //public void Create_1st_Task_Success()
        //{
        //    #region Arrange
        //    TaskDetailsModel mockTaskDetails = new TaskDetailsModel
        //    {
        //        ID = 1,
        //        TaskCompleted = false,
        //        TaskDescription = HttpUtility.HtmlEncode("Mocked Task")
        //    };

        //    TaskListController controller = new TaskListController();
        //    controller.tasklist = mockTasklist.Object;
        //    this.mockTasklist.Setup(c => c.Inserttask(mockTaskDetails.ID, mockTaskDetails.TaskDescription, mockTaskDetails.TaskCompleted));
        //    #endregion

        //    #region Act
        //    var result = (RedirectToRouteResult)controller.Create(mockTaskDetails);
        //    var viewModel = controller.ViewData.Model as TaskDetailsModel;
        //    #endregion

        //    #region Assert
        //    this.mockTasklist.Verify(c => c.GenerateNextIdNumber(), Times.Never());
        //    this.mockTasklist.Verify(c => c.Inserttask(mockTaskDetails.ID, mockTaskDetails.TaskDescription, mockTaskDetails.TaskCompleted), Times.Once());
        //    Assert.AreEqual(1, TaskDetailsModel.Tasks.Count);
        //    #endregion
        //}

        //[TestMethod]
        //public void Create_2nd_Task_Success()
        //{
        //    #region Arrange
        //    List<TaskDetailsModel> mockTaskList = new List<TaskDetailsModel>();
        //    TaskDetailsModel mock1stTaskDetails = new TaskDetailsModel
        //    {
        //        ID = 1,
        //        TaskCompleted = false,
        //        TaskDescription = HttpUtility.HtmlEncode("Mocked Task")
        //    };

        //    TaskDetailsModel mock2ndTaskDetails = new TaskDetailsModel
        //    {
        //        ID = 2,
        //        TaskCompleted = false,
        //        TaskDescription = HttpUtility.HtmlEncode("2nd Mocked Task")
        //    };

        //    TaskDetailsModel mockTaskDetailsModel = new TaskDetailsModel();
        //    mockTaskList.Add(mock1stTaskDetails);
        //    mockTaskDetailsModel.ID = mock2ndTaskDetails.ID;
        //    mockTaskDetailsModel.TaskCompleted = mock2ndTaskDetails.TaskCompleted;
        //    mockTaskDetailsModel.TaskDescription = mock2ndTaskDetails.TaskDescription;
        //    mockTaskDetailsModel.Tasks = mockTaskList;

        //    TaskListController controller = new TaskListController();
        //    controller.tasklist = mockTasklist.Object;
        //    this.mockTasklist.Setup(c => c.GenerateNextIdNumber()).Returns(2);
        //    this.mockTasklist.Setup(c => c.Inserttask(mock2ndTaskDetails.ID, mock2ndTaskDetails.TaskDescription, mock2ndTaskDetails.TaskCompleted));
        //    #endregion

        //    #region Act
        //    var result = (RedirectToRouteResult)controller.Create(mockTaskDetailsModel);
        //    var viewModel = controller.ViewData.Model as TaskDetailsModel;
        //    #endregion

        //    #region Assert
        //    this.mockTasklist.Verify(c => c.GenerateNextIdNumber(), Times.Once());
        //    this.mockTasklist.Verify(c => c.Inserttask(mock2ndTaskDetails.ID, mock2ndTaskDetails.TaskDescription, mock2ndTaskDetails.TaskCompleted), Times.Once());
        //    Assert.AreEqual(2, mock2ndTaskDetails.ID);
        //    Assert.IsNotNull(controller.HttpContext.Session["InMemoryTaskList"]);
        //    #endregion
        //}

        //[TestMethod]
        //public void Delete_Task_Success()
        //{
        //    #region Arrange
        //    List<TaskDetailsModel> mockTaskList = new List<TaskDetailsModel>();
        //    TaskDetailsModel mockTaskDetails = new TaskDetailsModel
        //    {
        //        ID = 1,
        //        TaskCompleted = false,
        //        TaskDescription = HttpUtility.HtmlEncode("Mocked Task")
        //    };
        //    TaskDetailsModel mock2ndTaskDetails = new TaskDetailsModel
        //    {
        //        ID = 2,
        //        TaskCompleted = false,
        //        TaskDescription = HttpUtility.HtmlEncode("2nd Mocked Task")
        //    };
        //    TaskDetailsModel mockTaskDetailsModel = new TaskDetailsModel();
        //    mockTaskList.Add(mockTaskDetails);
        //    mockTaskList.Add(mock2ndTaskDetails);
        //    mockTaskDetailsModel.Tasks = mockTaskList;

        //    TaskListController controller = new TaskListController();
        //    controller.tasklist = mockTasklist.Object;
        //    controller.ControllerContext = new ControllerContext(mockControllerContext, controller);
        //    this.mockTasklist.Setup(c => c.DeleteTask(mockTaskDetails.ID));
        //    #endregion

        //    #region Act
        //    var result = (RedirectToRouteResult)controller.Delete(mockTaskDetails.ID);
        //    #endregion

        //    #region Assert
        //    this.mockTasklist.Verify(c => c.DeleteTask(mockTaskDetails.ID), Times.Once());
        //    #endregion
        //}
    }
    }
