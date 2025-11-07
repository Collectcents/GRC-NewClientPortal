using GRC_NewClientPortal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;

namespace GRC_NewClientPortal.Controllers
{
    public class ChangePasswordController : Controller
    {

        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;

        public ChangePasswordController(IConfiguration config, IWebHostEnvironment env)
        {
            _config = config;
            _env = env;
        }

       

        [HttpGet("/changepassword")]
        public IActionResult ChangePassword()
        {
            Console.WriteLine($"ChangePassword() called at {DateTime.Now} | URL: {Request.Path}");


            // Check if user is logged in
            var signon = HttpContext.Session.GetString("signon");
            if (string.IsNullOrWhiteSpace(signon))
            {
                HttpContext.Session.SetString("ErrMsg", "Invalid login or your secure session has expired. Please login again or contact your CSR for assistance.");
                //return RedirectToAction("Index", "ClientHome");
            }
            var passwordChangeMessage = TempData["PasswordChangeMessage"] as string;
            if (!string.IsNullOrEmpty(passwordChangeMessage))
            {
                ViewBag.Message = passwordChangeMessage;
            }

            var rolePermissionsJson = HttpContext.Session.GetString("rolePermissions");

            List<string> rolePermissions = new List<string>();

            if (!string.IsNullOrEmpty(rolePermissionsJson))
            {
                // Handle comma-separated string
                rolePermissions = rolePermissionsJson
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(r => r.Trim())
                    .ToList();
            }

            // Pass access permissions to the view via ViewBag or ViewData
            ViewBag.CanAccessAccounts = rolePermissions.Contains("CanAccessAccounts");
            ViewBag.CanAccessPlacements = rolePermissions.Contains("CanAccessPlacements");
            ViewBag.CanAccessReports = rolePermissions.Contains("CanAccessReports");
            ViewBag.CanAccessACH = rolePermissions.Contains("CanAccessACH");
            ViewBag.CanAccessValidMedia = rolePermissions.Contains("CanAccessValidMedia");

            // Pass Role Access Denied message from config
            ViewBag.RoleAccessDeniedMsg = _config["AppSettings:RoleAccessDeniedMsg"] ?? "Access Denied";

            // Pass any error message from session
            ViewBag.ErrMessage = HttpContext.Session.GetString("ErrMsg");

            // Set password label text based on password length in session
            var pword = HttpContext.Session.GetString("pword");
            ViewBag.OldPasswordLabel = !string.IsNullOrEmpty(pword) && pword.Length > 6 ? "Old Password: " : "Temporary Password: ";

            // Clear the error message from session if you want to
            HttpContext.Session.Remove("ErrMsg");

            return View(new ChangePasswordModel());
            
        }

        [HttpPost]
        public IActionResult UpdatePassword([FromBody] ChangePasswordModel model)
        {

            var Password = HttpContext.Session.GetString("pword");
            var recId = HttpContext.Session.GetInt32("RecID");


            if (model.OldPassword == null || Password == null || recId == null)
            {
                return Json(new { success = false, message = "Session expired or invalid access." });
            }


            if (model.OldPassword != Password)
            {
                return Json(new { success = false, message = "Old password is incorrect." });
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();

                return Json(new { success = false, message = "Validation failed.", errors });
            }

            // Validate new password
            string newPassword = model.NewPassword?.Trim();

            var validationResult = ValidatePassword(newPassword);
            if (!validationResult.IsValid)
            {
                return Json(new { success = false, message = validationResult.ErrorMessage });
            }

            // Execute password update stored procedure with parameterized query
            var connectionString = _config.GetConnectionString("DefaultDataSource");
            try
            {
                int result;
                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand("p_cli_change_password", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RecID", recId);
                    cmd.Parameters.AddWithValue("@Pword", GRC_NewClientPortal.Models.Domain.Common.GetHash(newPassword));

                    conn.Open();
                    var scalarResult = cmd.ExecuteScalar();
                    result = Convert.ToInt32(scalarResult);
                }

                if (result == 1)
                {
                    // Update session values
                    HttpContext.Session.SetString("pword", GRC_NewClientPortal.Models.Domain.Common.GetHash(newPassword));
                    HttpContext.Session.SetString("ForcePasswordChange", "No");
                    HttpContext.Session.Remove("ErrMsg");

                    if (string.IsNullOrEmpty(HttpContext.Session.GetString("PasswordExpDaysLeft")))
                    {
                        HttpContext.Session.SetString("PasswordExpDaysLeft", _config["AppSettings:PasswordExpPeriod"]);
                    }

                    return Json(new { success = true, message = "Your password has been updated successfully." });
                }
                else
                {
                    return Json(new { success = false, message = "Your password could not be updated.Please try again later." });
                }
            }

            catch (SqlException ex)
            {
                return Json(new
                {
                    success = false,
                    message = "An error occurred while updating your password. Please log in again and try.",
                    //error = ex.Message // Optional: remove in production
                });
            }
        }


private (bool IsValid, string ErrorMessage) ValidatePassword(string password)
        {
            if (Regex.IsMatch(password, @"(.)\1{2,}", RegexOptions.IgnoreCase))
            {
                return (false, "New password should not contain repetitive characters.");
            }
            if (password.Length < 8)
            {
                return (false, "New password must be at least 8 characters.");
            }
            if (password.Contains("&"))
            {
                return (false, "Password is not acceptable.");
            }

            int cnt = 0;
            if (Regex.IsMatch(password, @"[a-z]")) cnt++;
            if (Regex.IsMatch(password, @"[A-Z]")) cnt++;
            if (Regex.IsMatch(password, @"\d")) cnt++;
            if (Regex.IsMatch(password, @"[!@#$%]")) cnt++;

            if (cnt < 3)
            {
                return (false, "New password must contain at least 3 of the following: lowercase, uppercase, number, symbol");
            }

            var contactName = HttpContext.Session.GetString("contactName");
            
            if (!string.IsNullOrEmpty(contactName))
            {
                var names = contactName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                foreach (var name in names)
                {
                    if (password.Contains(name, StringComparison.OrdinalIgnoreCase))
                    {
                        return (false, "New password should not contain first name or last name.");
                    }
                }
            }

            // Check against restricted word list
            
            var filePath = Path.Combine(_env.WebRootPath, "content", "500worst-contains.list");
            if (System.IO.File.Exists(filePath))
            {
                var restrictedWords = System.IO.File.ReadAllLines(filePath);
                var passwordLower = password.ToLowerInvariant();

                foreach (var word in restrictedWords)
                {
                    if (!string.IsNullOrWhiteSpace(word) && passwordLower.Contains(word.Trim().ToLowerInvariant()))
                    {
                        return (false, "Invalid password. Please provide a different password.");
                    }
                }
            }

            return (true, string.Empty);
        }
    }
}


