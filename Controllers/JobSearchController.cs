using JobSearch.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace JobSearch.Controllers
{
    public class JobSearchController : Controller
    {
        IConfiguration _configuration;
        SqlConnection _Connection;
        public JobSearchController(IConfiguration configuration)
        {
            _configuration = configuration;
            _Connection = new SqlConnection(_configuration.GetConnectionString("Job_Details"));
        }

        public List<JobSearchModel> GetJobSearch()
        {
            List<JobSearchModel> Job_Search = new();
            _Connection.Open();
            SqlCommand cmd = new SqlCommand("FETCH_JOB", _Connection);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                JobSearchModel job = new();
                job.Id = (int)dr[0];
                job.Role = (string)dr[1];
                job.Salary =""+(dr[2]);
                job.Cmpy_name = dr.GetString(3);
                job.Cmpy_Location = dr.GetString(4);
                job.Req_exp = dr.GetInt32(5);
                Job_Search.Add(job);
            }
            dr.Close();
            _Connection.Close();
            return Job_Search;
        }

        // GET: JobSearchController
        public ActionResult Users()
        {
            return View(GetJobSearch());
        }

        [HttpPost]

        public ActionResult Users(int id, string searchQuery)
        {
            List<JobSearchModel> jobList = GetJobSearch();
            var students = from s in jobList
                           select s;
            if (!String.IsNullOrEmpty(searchQuery))
            {
                List<JobSearchModel> search = new();
                _Connection.Open();
                SqlCommand cmd = new SqlCommand($"SELECT * FROM JOB_DETAILS WHERE CMPY_NAME LIKE '%{searchQuery}%' OR CMPY_LOCATION LIKE '%{searchQuery}%' OR JOB_ROLE LIKE '%{searchQuery}%'", _Connection);

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    JobSearchModel user = new();
                    user.Id = (int)dr[0];
                    user.Role = (string)dr[1];
                    user.Salary = "" + (dr[2]);
                    user.Cmpy_name = (string)(dr[3]);
                    user.Cmpy_Location = dr.GetString(4);
                    user.Req_exp = (int)(dr[5]);
                    user.Placed_emp = (int)dr[6];
                    search.Add(user);
                }
                dr.Close();
                _Connection.Close();
                return View(search);

            }
            return View(jobList);
        }





        // GET: JobSearchController
        public ActionResult Index()
        {
            return View(GetJobSearch());
        }

        [HttpPost]

        public ActionResult Index(int id,string searchQuery)
        {
            List<JobSearchModel> jobList = GetJobSearch();
            var students = from s in jobList
                           select s;
            if (!String.IsNullOrEmpty(searchQuery))
            {
                List<JobSearchModel> search = new();
                _Connection.Open();
                SqlCommand cmd = new SqlCommand($"SELECT * FROM JOB_DETAILS WHERE CMPY_NAME LIKE '%{searchQuery}%' OR CMPY_LOCATION LIKE '%{searchQuery}%' OR JOB_ROLE LIKE '%{searchQuery}%'", _Connection);

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    JobSearchModel user = new();
                    user.Id = (int)dr[0];
                    user.Role = (string)dr[1];
                    user.Salary = ""+(dr[2]);
                    user.Cmpy_name = (string)(dr[3]);
                    user.Cmpy_Location = dr.GetString(4);
                    user.Req_exp = (int)(dr[5]);
                    user.Placed_emp = (int)dr[6];
                    search.Add(user);
                }
                dr.Close();
                _Connection.Close();
                return View(search);

            }
            return View(jobList);
        }

        [HttpGet]
      

            // GET: JobSearchController/Details/5
            public ActionResult Details(int id)
        {
            return View(GetJobId(id));
        }

        // GET: JobSearchController/Create
        public ActionResult Create()
        {
            return View();
        }

        void InsertJob(JobSearchModel job)
        {
            _Connection.Open();
            SqlCommand cmd = new SqlCommand("INSERT_JOB", _Connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ROLE", job.Role );
            cmd.Parameters.AddWithValue("@SALARY", job.Salary);
            cmd.Parameters.AddWithValue("@CMPY_NAME", job.Cmpy_name);
            cmd.Parameters.AddWithValue("@CMPY_LOCATION", job.Cmpy_Location);
            cmd.Parameters.AddWithValue("@REQ_EXP", job.Req_exp);
            cmd.Parameters.AddWithValue("@PLACED_EMP", job.Placed_emp);

            cmd.ExecuteNonQuery();
            _Connection.Close();
        }


        // POST: JobSearchController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(JobSearchModel job)
        {
            try
            {
                InsertJob(job);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return View();
            }
        }


        JobSearchModel GetJobId(int id)
        {
            _Connection.Open();
            SqlCommand cmd = new SqlCommand("GET_JOB", _Connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", id);
            SqlDataReader dr = cmd.ExecuteReader();
            JobSearchModel job = new();
            while (dr.Read())
            {

                job.Id = (int)dr[0];
                job.Role = (string)dr[1];
                job.Salary = "" + (dr[2]);
                job.Cmpy_name = dr.GetString(3);
                job.Cmpy_Location = dr.GetString(4);
                job.Req_exp = dr.GetInt32(5);
            }
            dr.Close();
            _Connection.Close();
            return job;
        }


        // GET: JobSearchController/Edit/5
        public ActionResult Edit(int id)
        {
            return View(GetJobId(id));
        }

        void EditJob(int id, JobSearchModel job)
        {
            _Connection.Open();
            SqlCommand cmd = new SqlCommand("EDIT_JOB", _Connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", job.Id);
            cmd.Parameters.AddWithValue("@ROLE", job.Role);
            cmd.Parameters.AddWithValue("@SALARY", job.Salary);
            cmd.Parameters.AddWithValue("@CMPY_NAME", job.Cmpy_name);
            cmd.Parameters.AddWithValue("@CMPY_LOCATION", job.Cmpy_Location);
            cmd.Parameters.AddWithValue("@REQ_EXP", job.Req_exp);
            cmd.Parameters.AddWithValue("@PLACED_EMP", job.Placed_emp);

            cmd.ExecuteNonQuery();
            _Connection.Close();

        }

        // POST: JobSearchController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, JobSearchModel job)
        {
            try
            {
                EditJob(id, job);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }




        // GET: JobSearchController/Delete/5
        public ActionResult Delete(int id)
        {
            return View(GetJobId(id));
        }

        // POST: JobSearchController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                Console.WriteLine(id);
                DeleteJob(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        void DeleteJob(int id)
        {
            _Connection.Open();
            SqlCommand cmd = new SqlCommand("DELETE_JOB", _Connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            _Connection.Close();
        }

       
    }
}
