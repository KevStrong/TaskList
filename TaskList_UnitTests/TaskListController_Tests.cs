using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TaskList.Controllers;
using TaskList.Interfaces;
using TaskList.Models;


namespace TaskList_UnitTests
{
    [TestClass]
    public class TaskListController_Tests
    {
        private Mock<ITaskList> mockTasklist;
        private Mock<HttpRequestBase> mockRequest;
        private Mock<HttpResponseBase> mockResponse;
        private Mock<HttpContextBase> mockContext;

        [TestInitialize]
        public void Setup()
        {
            this.mockTasklist = new Mock<ITaskList>();
            this.mockRequest = new Mock<HttpRequestBase>(MockBehavior.Strict);
            this.mockResponse = new Mock<HttpResponseBase>(MockBehavior.Strict);
            this.mockContext = new Mock<HttpContextBase>(MockBehavior.Strict);
        }

        [TestMethod]
        public void Index_with_1_Task_in_List()
        {
            #region Arrange
            List<TaskDetailsModel> mockTaskList = new List<TaskDetailsModel>();
            TaskDetailsModel mockTaskDetails = new TaskDetailsModel
            {
                ID = 1,
                TaskCompleted = false,
                TaskDescription = HttpUtility.HtmlEncode("Mocked Task")
            };
            TaskListModel mockTaskListModel = new TaskListModel();
            mockTaskList.Add(mockTaskDetails);

            this.mockContext.SetupGet(x => x.Request).Returns(this.mockRequest.Object);
            this.mockContext.SetupGet(x => x.Response).Returns(this.mockResponse.Object);
            mockContext.Setup(x => x.Session["InMemoryTaskList"]).Returns(mockTaskListModel);
            var mockControllerContext = new RequestContext(this.mockContext.Object, new RouteData());

            TaskListController controller = new TaskListController();
            controller.tasklist = mockTasklist.Object;
            controller.ControllerContext = new ControllerContext(mockControllerContext, controller);
            this.mockTasklist.Setup(c => c.GetAllTasks()).Returns(mockTaskList);
            #endregion

            #region Act
            var result = (ActionResult)controller.Index();
            var viewModel = controller.ViewData.Model as TaskListModel;
            #endregion

            #region Assert
            this.mockTasklist.Verify(c => c.GetAllTasks(), Times.Once());
            Assert.IsNotNull(controller.HttpContext.Session["InMemoryTaskList"]);
            Assert.AreEqual(1, mockTaskList.Count);
            #endregion
        }

        [TestMethod]
        public void Create_Task_Model_isInvalid()
        {
            #region Arrange
            TaskListModel mockTaskList = new TaskListModel
            {
                ID = 1,
                TaskCompleted = false,
                TaskDescription = null
            };
            
            this.mockContext.SetupGet(x => x.Request).Returns(this.mockRequest.Object);
            this.mockContext.SetupGet(x => x.Response).Returns(this.mockResponse.Object);
            mockContext.Setup(x => x.Session["InMemoryTaskList"]).Returns(null);
            var mockControllerContext = new RequestContext(this.mockContext.Object, new RouteData());

            TaskListController controller = new TaskListController();
            controller.tasklist = mockTasklist.Object;
            controller.ControllerContext = new ControllerContext(mockControllerContext, controller);
            controller.ViewData.ModelState.AddModelError("TaskDescription", "Please specify a task description before submitting again");
            #endregion

            #region Act
            var result = (ViewResult)controller.Create(mockTaskList);
            var viewModel = controller.ViewData.Model as TaskListModel;
            #endregion

            #region Assert
            Assert.IsFalse(result.ViewData.ModelState.IsValid);
            Assert.IsNull(controller.HttpContext.Session["InMemoryTaskList"]);
            #endregion
        }

        [TestMethod]
        public void Create_1st_Task_Success()
        {
            #region Arrange
            List<TaskDetailsModel> mockTaskList = new List<TaskDetailsModel>();
            TaskDetailsModel mockTaskDetails = new TaskDetailsModel
            {
                ID = 1,
                TaskCompleted = false,
                TaskDescription = HttpUtility.HtmlEncode("Mocked Task")
            };
            TaskListModel mockTaskListModel = new TaskListModel();
            mockTaskListModel.ID = mockTaskDetails.ID;
            mockTaskListModel.TaskCompleted = mockTaskDetails.TaskCompleted;
            mockTaskListModel.TaskDescription = mockTaskDetails.TaskDescription;
            mockTaskListModel.Tasks = mockTaskList;

            this.mockContext.SetupGet(x => x.Request).Returns(this.mockRequest.Object);
            this.mockContext.SetupGet(x => x.Response).Returns(this.mockResponse.Object);
            mockContext.Setup(x => x.Session["InMemoryTaskList"]).Returns("");
            var mockControllerContext = new RequestContext(this.mockContext.Object, new RouteData());

            TaskListController controller = new TaskListController();
            controller.tasklist = mockTasklist.Object;
            controller.ControllerContext = new ControllerContext(mockControllerContext, controller);
            this.mockTasklist.Setup(c => c.GetAllTasks());
            this.mockTasklist.Setup(c => c.Inserttask(mockTaskDetails.ID, mockTaskDetails.TaskDescription, mockTaskDetails.TaskCompleted));
            #endregion

            #region Act
            var result = (RedirectToRouteResult)controller.Create(mockTaskListModel);
            var viewModel = controller.ViewData.Model as TaskListModel;
            #endregion

            #region Assert
            this.mockTasklist.Verify(c => c.GetAllTasks(), Times.Once());
            this.mockTasklist.Verify(c => c.GenerateNextIdNumber(), Times.Never());
            this.mockTasklist.Verify(c => c.Inserttask(mockTaskDetails.ID, mockTaskDetails.TaskDescription, mockTaskDetails.TaskCompleted), Times.Once());
            Assert.IsNotNull(controller.HttpContext.Session["InMemoryTaskList"]);
            #endregion
        }

        [TestMethod]
        public void Create_2nd_Task_Success()
        {
            #region Arrange
            List<TaskDetailsModel> mockTaskList = new List<TaskDetailsModel>();
            TaskDetailsModel mock1stTaskDetails = new TaskDetailsModel
            {
                ID = 1,
                TaskCompleted = false,
                TaskDescription = HttpUtility.HtmlEncode("Mocked Task")
            };

            TaskDetailsModel mock2ndTaskDetails = new TaskDetailsModel
            {
                ID = 2,
                TaskCompleted = false,
                TaskDescription = HttpUtility.HtmlEncode("2nd Mocked Task")
            };

            TaskListModel mockTaskListModel = new TaskListModel();
            mockTaskList.Add(mock1stTaskDetails);
            mockTaskListModel.ID = mock2ndTaskDetails.ID;
            mockTaskListModel.TaskCompleted = mock2ndTaskDetails.TaskCompleted;
            mockTaskListModel.TaskDescription = mock2ndTaskDetails.TaskDescription;
            mockTaskListModel.Tasks = mockTaskList;

            this.mockContext.SetupGet(x => x.Request).Returns(this.mockRequest.Object);
            this.mockContext.SetupGet(x => x.Response).Returns(this.mockResponse.Object);
            mockContext.Setup(x => x.Session["InMemoryTaskList"]).Returns(mockTaskListModel);
            var mockControllerContext = new RequestContext(this.mockContext.Object, new RouteData());

            TaskListController controller = new TaskListController();
            controller.tasklist = mockTasklist.Object;
            controller.ControllerContext = new ControllerContext(mockControllerContext, controller);
            this.mockTasklist.Setup(c => c.GetAllTasks()).Returns(mockTaskList);
            this.mockTasklist.Setup(c => c.GenerateNextIdNumber()).Returns(2);
            this.mockTasklist.Setup(c => c.Inserttask(mock2ndTaskDetails.ID, mock2ndTaskDetails.TaskDescription, mock2ndTaskDetails.TaskCompleted));
            #endregion

            #region Act
            var result = (RedirectToRouteResult)controller.Create(mockTaskListModel);
            var viewModel = controller.ViewData.Model as TaskListModel;
            #endregion

            #region Assert
            this.mockTasklist.Verify(c => c.GetAllTasks(), Times.Once());
            this.mockTasklist.Verify(c => c.GenerateNextIdNumber(), Times.Once());
            this.mockTasklist.Verify(c => c.Inserttask(mock2ndTaskDetails.ID, mock2ndTaskDetails.TaskDescription, mock2ndTaskDetails.TaskCompleted), Times.Once());
            Assert.AreEqual(2, mock2ndTaskDetails.ID);
            Assert.IsNotNull(controller.HttpContext.Session["InMemoryTaskList"]);
            #endregion
        }

        [TestMethod]
        public void Delete_Task_Success()
        {
            #region Arrange
            List<TaskDetailsModel> mockTaskList = new List<TaskDetailsModel>();
            TaskDetailsModel mockTaskDetails = new TaskDetailsModel
            {
                ID = 1,
                TaskCompleted = false,
                TaskDescription = HttpUtility.HtmlEncode("Mocked Task")
            };
            TaskDetailsModel mock2ndTaskDetails = new TaskDetailsModel
            {
                ID = 2,
                TaskCompleted = false,
                TaskDescription = HttpUtility.HtmlEncode("2nd Mocked Task")
            };
            TaskListModel mockTaskListModel = new TaskListModel();
            mockTaskList.Add(mockTaskDetails);
            mockTaskList.Add(mock2ndTaskDetails);
            mockTaskListModel.Tasks = mockTaskList;

            this.mockContext.SetupGet(x => x.Request).Returns(this.mockRequest.Object);
            this.mockContext.SetupGet(x => x.Response).Returns(this.mockResponse.Object);
            mockContext.Setup(x => x.Session["InMemoryTaskList"]).Returns(mockTaskListModel);
            var mockControllerContext = new RequestContext(this.mockContext.Object, new RouteData());

            TaskListController controller = new TaskListController();
            controller.tasklist = mockTasklist.Object;
            controller.ControllerContext = new ControllerContext(mockControllerContext, controller);
            this.mockTasklist.Setup(c => c.DeleteTask(mockTaskDetails.ID));
            #endregion

            #region Act
            var result = (RedirectToRouteResult)controller.Delete(mockTaskDetails.ID);
            #endregion

            #region Assert
            this.mockTasklist.Verify(c => c.DeleteTask(mockTaskDetails.ID), Times.Once());
            #endregion
        }

        [TestMethod]
        public void Update_Task_Success()
        {
            #region Arrange
            List<TaskDetailsModel> mockTaskList = new List<TaskDetailsModel>();
            TaskDetailsModel mockTaskDetails = new TaskDetailsModel
            {
                ID = 1,
                TaskCompleted = false,
                TaskDescription = HttpUtility.HtmlEncode("Mocked Task")
            };
            TaskDetailsModel mock2ndTaskDetails = new TaskDetailsModel
            {
                ID = 2,
                TaskCompleted = false,
                TaskDescription = HttpUtility.HtmlEncode("2nd Mocked Task")
            };
            TaskListModel mockTaskListModel = new TaskListModel();
            mockTaskList.Add(mockTaskDetails);
            mockTaskList.Add(mock2ndTaskDetails);
            mockTaskListModel.Tasks = mockTaskList;

            this.mockContext.SetupGet(x => x.Request).Returns(this.mockRequest.Object);
            this.mockContext.SetupGet(x => x.Response).Returns(this.mockResponse.Object);
            mockContext.Setup(x => x.Session["InMemoryTaskList"]).Returns(mockTaskListModel);
            var mockControllerContext = new RequestContext(this.mockContext.Object, new RouteData());

            TaskListController controller = new TaskListController();
            controller.tasklist = mockTasklist.Object;
            controller.ControllerContext = new ControllerContext(mockControllerContext, controller);
            this.mockTasklist.Setup(c => c.UpdateTask(mockTaskDetails.ID, true));
            #endregion

            #region Act
            var result = (JsonResult)controller.Update(mockTaskDetails.ID, true);
            #endregion

            #region Assert
            this.mockTasklist.Verify(c => c.UpdateTask(mockTaskDetails.ID, true), Times.Once());
            Assert.IsNotNull(result.Data);
            #endregion
        }

    }
}
