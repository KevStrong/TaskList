using System;
using System.Web;
using System.Linq;
using System.Web.Mvc;
using TaskList.Models;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace TaskList.Controllers
{
    public class TaskListController : Controller
    {
        private List<TaskDetailsModel> ListAllTaskDetails = new List<TaskDetailsModel>();
        private static readonly Random GetRandomNumber = new Random();

        //
        // GET: /TaskList/
        public ActionResult Index()
        {
            TaskListModel modelTaskList = new TaskListModel();

            modelTaskList.Tasks = GetAllTasks();

            return View(modelTaskList);
        }

        //
        // POST: /TaskList/

        [HttpPost]
        public ActionResult Create(TaskListModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Tasks = GetAllTasks();
                return View("Index", model);
            }

            if (DataBaseConnectionStringExists())
            {
                string strEscapedTaskDescription = HttpUtility.HtmlEncode(model.TaskDescription);

                // Assumption is you would hold the ID, Task Description and possibility of the checkbox value in the database
                string strConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                string DBqueryString = "INSERT INTO dbo.[TODO: tablename]([TODO: Colname1],[TODO: Colname2]) VALUES (@Param1,@Param2)";

                using (SqlConnection DBconnection = new SqlConnection(strConnection))
                {
                    SqlCommand DBcommand = new SqlCommand(DBqueryString, DBconnection);
                    DBcommand.Parameters.Add(new SqlParameter("@Param1", strEscapedTaskDescription));
                    DBcommand.Parameters.Add(new SqlParameter("@Param2", false));
                    try
                    {
                        DBconnection.Open();
                        DBcommand.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // Would normally log exception to an error log such as Elmah or Log4net
                        ViewBag.ErrorMessage = "An error has occurred inserting from the database. Details :: " + ex.Message;
                    }
                }   
            }
            else
            {
                List<TaskDetailsModel> TaskListFromSession = (List<TaskDetailsModel>)Session["InMemoryTaskList"];
                if (TaskListFromSession != null)
                    ListAllTaskDetails = TaskListFromSession;

                ListAllTaskDetails.Add(new TaskDetailsModel
                {
                    ID = GenerateNextIdNumber(),
                    TaskDescription = HttpUtility.HtmlEncode(model.TaskDescription),
                    TaskCompleted = false
                });

                Session["InMemoryTaskList"] = ListAllTaskDetails;
            }
            return RedirectToAction("Index");
        }

        //
        // GET: /TaskList/Delete/5

        public ActionResult Delete(int id)
        {
            if (DataBaseConnectionStringExists())
            {
                string strConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                string DBqueryString = "DELETE FROM dbo.[TODO: tablename] WHERE ID = @Param1";

                using (SqlConnection DBconnection = new SqlConnection(strConnection))
                {
                    SqlCommand DBcommand = new SqlCommand(DBqueryString, DBconnection);
                    DBcommand.Parameters.Add(new SqlParameter("@Param1", id));

                    try
                    {
                        DBconnection.Open();
                        DBcommand.ExecuteScalar();
                    }
                    catch (Exception ex)
                    {
                        // Would normally log exception to an error log such as Elmah or Log4net
                        ViewBag.ErrorMessage = "An error has occurred deleting from the database. Details :: " + ex.Message;
                    }
                }   
            }
            else
            {
                List<TaskDetailsModel> TaskListFromSession = (List<TaskDetailsModel>)Session["InMemoryTaskList"];
                if (TaskListFromSession != null)
                    ListAllTaskDetails = TaskListFromSession;

                TaskDetailsModel TaskItemToRemove = ListAllTaskDetails.FirstOrDefault(x => x.ID == id);

                if (TaskItemToRemove != null)
                    ListAllTaskDetails.Remove(TaskItemToRemove);

                Session["InMemoryTaskList"] = ListAllTaskDetails;
            }

            return RedirectToAction("Index");
        }

        # region Methods
        /*
         *  <summary>
         *       Retrieves all tasks
         *      If Connectionstring specified in the web.config it will retrieve tasks from the database
         *      Else tasks will be retrieved from in memory session variable
         *  </summary>
         *  <returns>
         *      List: Tasks
         *  </returns>
        */
        public virtual List<TaskDetailsModel> GetAllTasks()
        {            
            if (DataBaseConnectionStringExists())
            {
                string strConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                string DBqueryString = "SELECT [TODO: Primary Key], [TODO: first column holding description] from dbo.[TODO: tablename]";

                using (SqlConnection DBconnection = new SqlConnection(strConnection))
                {
                    SqlCommand DBcommand = new SqlCommand(DBqueryString, DBconnection);

                    try
                    {
                        DBconnection.Open();
                        SqlDataReader DBreader = DBcommand.ExecuteReader();
                        while (DBreader.Read())
                        {
                            ListAllTaskDetails.Add(new TaskDetailsModel
                            {
                                ID = Convert.ToInt32(DBreader[0]),
                                TaskDescription = HttpUtility.HtmlDecode(DBreader[1].ToString()),
                                TaskCompleted = Convert.ToBoolean(DBreader[2])
                            });
                        }
                        DBreader.Close();
                    }
                    catch (Exception ex)
                    {
                        // Would normally log exception to an error log such as Elmah or Log4net
                        ViewBag.ErrorMessage = "An error has occurred reading the tasklist from the database. Details :: " + ex.Message;
                    }
                }
            }
            else
            {
                ListAllTaskDetails = (List<TaskDetailsModel>)Session["InMemoryTaskList"];
            }
            
            return ListAllTaskDetails;
        }
        
        /*
         *  <summary>
         *      Generates next sequential ID based on the number of items in the list
         *      If something goes wrong it will generate a random ID number between 100 and 999
         *  </summary>
         *  <returns>
         *      int: ID number
         *  </returns>
        */
        public virtual int GenerateNextIdNumber()
        {
            List<TaskDetailsModel> CountTaskDetails = (List<TaskDetailsModel>)Session["InMemoryTaskList"];

            int NextID;

            try
            {
                if (CountTaskDetails == null || CountTaskDetails.Count == 0)
                {
                    NextID = 1;
                }
                else
                {
                    NextID = CountTaskDetails[CountTaskDetails.Count-1].ID + 1;
                }
            }
            catch
            {
                NextID = GetRandomNumber.Next(100, 999); 
            }

            return NextID;
        }

        /*
        *  <summary>
        *       Check if at least one connection string has been specified in web.config and use this to access database 
        *       Checks count is greater than 1 because ConnectionString element (+1) and name (+1) 
        *  </summary>
        *  <returns>
        *      True : if connectionstring exists / False: no connectionstrings exists
        *  </returns>
       */
        public virtual bool DataBaseConnectionStringExists()
        {
            if (ConfigurationManager.ConnectionStrings.Count > 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}