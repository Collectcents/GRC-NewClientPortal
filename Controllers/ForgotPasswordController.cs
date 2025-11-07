using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System;
using GRC_NewClientPortal.Models;
using GRC_NewClientPortal.Models.GRCEmail;

namespace GRC_NewClientPortal.Controllers
{
    public class ForgotPasswordController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<SendMail> _sendMailLogger;
        private readonly IWebHostEnvironment _env;
        public ForgotPasswordController(IConfiguration configuration, ILogger<SendMail> sendMailLogger, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _sendMailLogger = sendMailLogger;
            _env = env;
        }

        [HttpGet("/forgotpassword")] // <— this defines the route explicitly
        public IActionResult ForgotPassword()
        {
            return View();
        }

        //[HttpGet]
        //public IActionResult Index()
        //{
        //    return View(new ForgotPasswordModel());
        //}


        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody] ForgotPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                Console.WriteLine(errors);
                return Json(new { success = false, message = "Please enter valid data" });

            }
            //if (!ModelState.IsValid)
            //    return Json(new { success = false, message = "Invalid input data." });


            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultDataSource");
                string fname = string.Empty;
                string userName = string.Empty;
                int max_num = 8;

                using (SqlConnection sqlConn = new SqlConnection(connectionString))
                {
                    await sqlConn.OpenAsync();

                    using (SqlCommand cmdCheck = new SqlCommand(@"
                    SELECT C10FNM FROM tbl_FACS_client_contact_data 
                    WHERE UPPER(C10BC#) = @Signon 
                    AND UPPER(C10LNM) = @LName 
                    AND UPPER(MaidenName) = @MaidenName 
                    AND UPPER(Email) = @Email", sqlConn))
                    {
                        cmdCheck.Parameters.AddWithValue("@Signon", model.Signon.ToUpper());
                        cmdCheck.Parameters.AddWithValue("@LName", model.LastName.ToUpper());
                        cmdCheck.Parameters.AddWithValue("@MaidenName", model.MaidenName.ToUpper());
                        cmdCheck.Parameters.AddWithValue("@Email", model.Email.ToUpper());

                        var result = await cmdCheck.ExecuteScalarAsync();

                        if (result == null)
                        {
                            return Json(new { success = false, message = "Supplied information does not match the records. Please contact your Client Services Representative for Assistance." });
                            
                        }

                        fname = result.ToString().Trim();
                        userName = $"{fname} {model.LastName}";
                    }
                    string path = Path.Combine(_env.WebRootPath, "content", "500worst-contains.list");
                    string strTempPassword = GRC_NewClientPortal.Models.Domain.Common.generateStrongPassword(max_num, userName, path);

                    using (SqlCommand cmd = new SqlCommand("p_cli_forgot_password", sqlConn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@BaseClientNumber", SqlDbType.NVarChar, 8) { Value = model.Signon });
                        cmd.Parameters.Add(new SqlParameter("@LastName", SqlDbType.NVarChar, 25) { Value = model.LastName });
                        cmd.Parameters.Add(new SqlParameter("@MaidenName", SqlDbType.NVarChar, 25) { Value = model.MaidenName });
                        cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 40) { Value = model.Email });
                        cmd.Parameters.Add(new SqlParameter("@TmpPassword", SqlDbType.NVarChar, 8) { Value = strTempPassword });

                        int iReturn = Convert.ToInt32(await cmd.ExecuteScalarAsync());

                        if (iReturn == 1)
                        {
                            var mailer = new GRC_NewClientPortal.Models.GRCEmail.SendMail(_configuration, _sendMailLogger);
                            await mailer.SendEmailAsync(
                                model.Email,
                                _configuration["AppSettings:EmailFrom_Password_Reset"],
                                "",
                                _configuration["AppSettings:EmailCC_Password_Reset"],
                                _configuration["AppSettings:EmailBCC_Password_Reset"],
                                _configuration["AppSettings:EmailSubject_Password_Reset"],
                                _configuration["AppSettings:EmailBody_Password_Reset"]
                                    .Replace("~tmppass~", strTempPassword)
                                    .Replace("~n~", "\n"),
                                ""
                            );

                            return Json(new { success = true, message = "Temporary password has been emailed to you. Please login using that password." });
                        }
                        else
                        {
                            return Json(new { success = false, message = "Supplied information does not match our records." });
                        }
                    }
                }
            }
            catch (Exception ex)
            { 
                return Json(new { success = false, message = "An error occurred: " + ex.Message });
            }
        }
    }

   

}


