using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using TopazFormTrack.Models;

namespace TopazFormTrack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        private IConfiguration _configuration;
        public ActivityController(IConfiguration configuration)
        {
                _configuration = configuration;
        }

        [HttpPost]
        [Route("LoadForm/{userID}")]
        public IActionResult AddLoadDetails(int userID)
        {
            
            try
            {
                
                var sqlConnection = GetSQLConnection();

                SqlCommand cmd = new SqlCommand("AddActivity", sqlConnection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userID);
                cmd.Parameters.AddWithValue("@Activity", "Form Loaded");
                cmd.Parameters.AddWithValue("@OccuredTime", DateTime.Now);

                sqlConnection.Open();
                cmd.ExecuteNonQuery();
                sqlConnection.Close();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        [Route("SubmitForm/{userID}")]
        public IActionResult AddSubmitDetails(int userID)
        {
            
            try
            {

                var sqlConnection = GetSQLConnection();

                SqlCommand cmd = new SqlCommand("AddActivity", sqlConnection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userID);
                cmd.Parameters.AddWithValue("@Activity", "Form Submitted");
                cmd.Parameters.AddWithValue("@OccuredTime", DateTime.Now);

                sqlConnection.Open();
                cmd.ExecuteNonQuery();
                sqlConnection.Close();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        [Route("GetAllActivity")]
        public IActionResult GetAllActivity()
        {
            try
            {
                List<Activity> activityList = new List<Activity>();

                var sqlConnection = GetSQLConnection();
                SqlCommand cmd = new SqlCommand("GetAllActivity", sqlConnection);
                cmd.CommandType = CommandType.StoredProcedure;

                sqlConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Activity activity = new Activity
                    {
                        UserId = Convert.ToInt32(reader["UserId"]),
                        UserActivity = Convert.ToString(reader["Activity"]),
                        OccuredTime = Convert.ToDateTime(reader["OccuredTime"])

                    };
                    activityList.Add(activity);
                }
                sqlConnection.Close();
                if (activityList.Count > 0)
                {
                    return Ok(activityList);
                }
                else
                {
                    return NotFound();
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }


        [HttpGet]
        [Route("GetAllActivitybyDate/{startDate}/{endDate}")]
        public IActionResult GetAllActivityDate(DateTime startDate, DateTime endDate)
        {
            try
            {
                List<Activity> activityList = new List<Activity>();

                var sqlConnection = GetSQLConnection();
                SqlCommand cmd = new SqlCommand("GetAllActivityBetweenDates", sqlConnection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", startDate);
                cmd.Parameters.AddWithValue("@EndDate", endDate);

                sqlConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Activity activity = new Activity
                    {
                        UserId = Convert.ToInt32(reader["UserId"]),
                        UserActivity = Convert.ToString(reader["Activity"]),
                        OccuredTime = Convert.ToDateTime(reader["OccuredTime"])

                    };
                    activityList.Add(activity);
                }
                sqlConnection.Close();
                if(activityList.Count > 0)
                {
                    return Ok(activityList);
                }
                else
                {
                    return NotFound();
                }
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        private SqlConnection GetSQLConnection()
        {
            var con = new SqlConnection(_configuration.GetConnectionString("DBConnectionString").ToString());
            if (con is not null)
                return con;
            else
                return null;
        }
    }
}
