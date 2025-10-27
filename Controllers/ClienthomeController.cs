using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using GRC_NewClientPortal.Models.GRCEmail;

namespace GRC_NewClientPortal.Controllers
{
    public class ClienthomeController : Controller
    {

        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<SendMail> _sendMailLogger;


        public ClienthomeController(IConfiguration config, IHttpContextAccessor httpContextAccessor, ILogger<SendMail> sendMailLogger)
        {
            _config = config;
            _httpContextAccessor = httpContextAccessor;
            _sendMailLogger = sendMailLogger;
        }

       
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var session = _httpContextAccessor.HttpContext.Session;

            // === Enforce HTTPS ===
            if (!_httpContextAccessor.HttpContext.Request.IsHttps)
            {
                var httpsUrl = "https://" + _httpContextAccessor.HttpContext.Request.Host +
                               _httpContextAccessor.HttpContext.Request.Path +
                               _httpContextAccessor.HttpContext.Request.QueryString;
                return Redirect(httpsUrl);
            }

            // === gogreen parameter check ===
            if (Request.Query.ContainsKey("gogreen"))
            {
                session.SetString("gogreenLogin", "No");
            }

            // === Initialize session defaults ===
            if (string.IsNullOrEmpty(session.GetString("signon")))
            {
                string defaultEmail = _config["AppSettings:DefaultClientContact"] ?? "GRCInfo@generalrevenue.com";
                session.SetString("CSR EMail", defaultEmail);
                session.SetInt32("placementCount", 0);
                session.SetInt32("placementFileCount", 0);
            }

            // === Display error message if previously set ===
            var errMsg = session.GetString("ErrMsg");
            if (!string.IsNullOrEmpty(errMsg))
            {
                ViewBag.Message = errMsg.Trim();
                session.SetString("ErrMsg", string.Empty);
            }

            // === Redirect if already logged in ===
            var signon = session.GetString("signon");
            if (!string.IsNullOrEmpty(signon))
            {
                await GetClientInfo();

                var facsUser = session.GetString("FACSUser") ?? "No";
                if (facsUser == "No")
                    return RedirectToAction("Menu", "Client");
                else
                    return RedirectToAction("Menu", "FW");
            }

            // === Session Timeout (handled globally in ASP.NET Core, left here for info) ===
            int timeoutMinutes = Convert.ToInt32(_config["AppSettings:SessionTimeoutForClients"]);
            // NOTE: session timeout is configured in Program.cs; cannot be set per-session.

            return View("clienthome");
        }

        /// <summary>
        /// Gets client info from database (converted from GetClientInfo method).
        /// </summary>
        private async Task GetClientInfo()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            string? recId = session.GetString("RecID");

            if (string.IsNullOrEmpty(recId))
                return;

            string? surveyLink = _config["Client_Survey_Link"];
            string? surveyExp = _config["Client_Survey_Exp_Date"];

            if (surveyLink == null || surveyExp == null)
                return;

            if (DateTime.Now >= Convert.ToDateTime(surveyExp))
                return;

            string connectionString = _config.GetConnectionString("DefaultDataSource")!;
            using var conn = new SqlConnection(connectionString);

            string sql = $"exec dbo.ssp_APG_GRCWeb_client_GetClientInfo {recId}";
            using var adapter = new SqlDataAdapter(sql, conn);
            var dsClientDetails = new DataSet();

            await Task.Run(() => adapter.Fill(dsClientDetails)); // mimic async data fill

            if (dsClientDetails.Tables.Count == 0 || dsClientDetails.Tables[0].Rows.Count == 0)
                return;

            var row = dsClientDetails.Tables[0].Rows[0];

            string clientBaseNumber = row["ClientBaseNumber"].ToString() ?? string.Empty;
            string clientName = row["ClientName"].ToString() ?? string.Empty;
            string personName = row["PersonName"].ToString() ?? string.Empty;
            string email = row["Email"].ToString() ?? string.Empty;
            string clientcsr = row["CSR"].ToString() ?? string.Empty;
            string clientsalesID = row["SalesID"].ToString() ?? string.Empty;
            string facsDirectory = row["FACSDirectory"].ToString() ?? string.Empty;

            // Only create survey link if FACS directory = POH
            if (!string.IsNullOrEmpty(facsDirectory) && facsDirectory.Equals("POH", StringComparison.OrdinalIgnoreCase))
            {
                string formattedSurveyLink = string.Format(
                    surveyLink,
                    clientBaseNumber, clientName, email, personName, clientcsr, clientsalesID
                );
                session.SetString("CLIENT_SURVEY_LINK", formattedSurveyLink);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login(string signon, string password, string lname)
        {
            var session = _httpContextAccessor.HttpContext.Session;

            session.SetString("ForcePasswordChange", "No");
            session.SetString("FACSUser", "No");

            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(lname))
            {
                session.SetString("ErrMsg", "Last name and password are required.");
                return View("ClientHome");
            }

            string connStr = _config.GetConnectionString("DefaultDataSource");
            string hashedPassword = password;
            bool boolLogit = false;
            bool boolMemos = false;

            using (var sqlConn = new SqlConnection(connStr))
            {
                // Check if the stored password is hashed
                var cmdCheck = new SqlCommand(@"
                    SELECT [Password] 
                    FROM tbl_FACS_client_contact_data a 
                    INNER JOIN t_client_login_data b ON a.RecID = b.RecID 
                    WHERE UPPER(C10BC#) = @Signon AND UPPER(C10LNM) = @LName", sqlConn);

                cmdCheck.Parameters.AddWithValue("@Signon", signon.ToUpper());
                cmdCheck.Parameters.AddWithValue("@LName", lname.ToUpper());

                var da = new SqlDataAdapter(cmdCheck);
                var dsCheck = new DataSet();
                da.Fill(dsCheck);

                if (dsCheck.Tables[0].Rows.Count == 0 ||
                    dsCheck.Tables[0].Rows[0]["Password"].ToString().Trim() != password.Trim())
                {
                    hashedPassword = GRC_NewClientPortal.Models.Domain.Common.GetHash(password);
                }

                // Authenticate via stored procedure
                var cmdLogin = new SqlDataAdapter($"exec p_cli_logon_request '{signon}', '{hashedPassword}', '{lname}'", sqlConn);
                var dsLogin = new DataSet();
                cmdLogin.Fill(dsLogin);

                if (dsLogin.Tables[0].Rows.Count == 0)
                {
                    session.SetString("ErrMsg", "Invalid Login, Last Name, and/or Password.");
                    return View("ClientHome");
                }

                var row = dsLogin.Tables[0].Rows[0];
                session.SetString("CSR EMail", row["CSR EMail"].ToString());
                string fname = row["C10FNM"].ToString().Trim();
                lname = row["C10LNM"].ToString().Trim();
                session.SetString("contactName", $"{fname} {lname}");
                boolMemos = row["C10MEM"].ToString() == "Y";
                session.SetString("boolMemos", boolMemos.ToString());
                session.SetInt32("RecID", Convert.ToInt32(row["RecID"]));
                session.SetString("FACSDirectory", row["FACSDirectory"].ToString());
                session.SetString("DPEntrystatus", row["DPEntry"].ToString());
                session.SetString("contactEmail", row["Email"].ToString().Trim());
                session.SetString("signon", row["C10BC#"].ToString());

                // Password expiration logic
                DateTime dtPassChangeDate = Convert.ToDateTime(row["PasswordChangeDate"]);
                int loginAttempts = Convert.ToInt32(row["LoginAttempts"]);
                int accountLockout = Convert.ToInt32(row["AccountLockout"]);
                int tempPassword = Convert.ToInt32(row["TempPassword"]);

                if (accountLockout == 1)
                    session.SetString("ErrMsg", "Your account has been locked. Please contact your CSR.");
                else if (loginAttempts > 0)
                    session.SetString("ErrMsg", "Invalid login attempt. Please try again.");
                else if (tempPassword == 1)
                {
                    session.SetString("ForcePasswordChange", "Yes");
                    session.SetString("ErrMsg", "You have logged in with a temporary password. Please update your password now.");
                    boolLogit = true;
                }
                else if (dtPassChangeDate > DateTime.MinValue)
                {
                    int expDays = (int)(dtPassChangeDate.AddDays(Convert.ToInt32(_config["PasswordExpPeriod"])) - DateTime.Now).TotalDays;
                    if (expDays <= 0)
                    {
                        session.SetString("ForcePasswordChange", "Yes");
                        session.SetString("ErrMsg", "Your password has expired. Please update your password now.");
                    }
                    else
                    {
                        session.SetInt32("PasswordExpDaysLeft", expDays);
                        boolLogit = true;
                    }
                }

                // Set school name
                session.SetString("schoolname", row["School Name"].ToString().Trim());
                session.SetString("SessionVariablesSet", "No");
                session.SetString("SessionDPVariablesSet", "No");
                session.SetString("SessionACHVariablesSet", "No");

                // Role Permissions
                if (dsLogin.Tables.Count > 1)
                {
                    var roles = dsLogin.Tables[1].AsEnumerable()
                        .Select(r => r.Field<string>("Role")).ToList();
                    session.SetString("rolePermissions", string.Join(",", roles));
                }

                // Check ACH Campaign
                string sqlACH = $"exec p_chk_ACHCampaign_exits '{session.GetInt32("RecID")}', '{_config["ACH_RemindMeLaterExpire_Days"]}'";
                int iach = 0;
                using (var cmdACH = new SqlCommand(sqlACH, sqlConn))
                {
                    await sqlConn.OpenAsync();
                    iach = Convert.ToInt32(await cmdACH.ExecuteScalarAsync());
                    await sqlConn.CloseAsync();
                }
                session.SetString("ACHPopUp", iach >= 1 ? "Yes" : "No");

                // Generate MFA Code
                string verificationCode = await GenerateVerificationCodeAsync(session.GetString("contactEmail"));
                session.SetString("VerificationCode", verificationCode);

                if (boolLogit)
                {
                    await LogItAsync(verificationCode);
                    await GetClientInfo();
                }

                // MFA screen redirect
                return RedirectToAction("VerifyMFA");
            }
        }

        private async Task LogItAsync(string verificationCode)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            string loginIP = httpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            string recId = httpContext.Session.GetInt32("RecID")?.ToString() ?? "0";

            string connStr = _config.GetConnectionString("DefaultDataSource");
            string sql = @"INSERT INTO tbl_client_logins (login, loginIP, loginDate, verificationCode)
                           VALUES (@login, @loginIP, @loginDate, @verificationCode)";

            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@login", recId);
                cmd.Parameters.AddWithValue("@loginIP", loginIP);
                cmd.Parameters.AddWithValue("@loginDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@verificationCode", verificationCode);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                await conn.CloseAsync();
            }
        }

        // ---------------------------------------------------------------------
        // Generate verification code + send email (replacement for GenerateVerificationCode)
        // ---------------------------------------------------------------------
        private async Task<string> GenerateVerificationCodeAsync(string emailTo)
        {
            var random = new Random();
            string code = random.Next(0, 1000000).ToString("D6");
            //GRC_NewClientPortal.Models.GRCEmail.SendMail
            // Send email (you can replace with your own mail service)
            var mailer = new GRC_NewClientPortal.Models.GRCEmail.SendMail(_config, _sendMailLogger);
            await mailer.SendEmailAsync(
                emailTo,
                _config["EmailClientSrvs"],
                "",
                "",
                "sachin.mukherjee@generalrevenue.com",
                "Your General Revenue verification code:",
                "<img src='https://www.generalrevenue.com/ContentSite/styles/i/logo_home.gif'>" +
                "<p>Here's your one-time login code.</p>" +
                "<p>Do not share this code with anyone. This code will expire in 15 minutes.</p><b>" +
                code ,
                "</b><p>If you or an authorized user did not initiate this action, please contact us.</p>" +
                "<p>Call (800) 234-1472</p>"
            );

            // Mask email for confirmation message
            string pattern = @"(?<=[\w]{1})[\w\-._\+%]*(?=[\w]{1}@)";
            string maskedEmail = Regex.Replace(emailTo, pattern, m => new string('*', m.Length));

            // Save to session
            HttpContext.Session.SetString("verificationCode", code);
            HttpContext.Session.SetString("maskedEmail", maskedEmail);

            return code;
        }

        // ---------------------------------------------------------------------
        // Verify MFA code (replacement for btnVerify_Click)
        // ---------------------------------------------------------------------
        [HttpPost]
        public IActionResult VerifyCode(string inputCode)
        {
            string storedCode = HttpContext.Session.GetString("verificationCode");

            if (storedCode == inputCode)
            {
                return RedirectToAction("CompleteVerify");
            }

            ViewBag.Message = "Verification code does not match. Please re-enter or request a new code.";
            ViewBag.MaskedEmail = HttpContext.Session.GetString("maskedEmail");
            return View("VerifyMFA");
        }

        // ---------------------------------------------------------------------
        // Request new verification code (replacement for btnNewVerify_Click)
        // ---------------------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> NewVerificationCode()
        {
            string email = HttpContext.Session.GetString("contactEmail") ?? "";
            string newCode = await GenerateVerificationCodeAsync(email);
            await LogItAsync(newCode);

            TempData["Message"] = "A new verification code has been sent.";
            ViewBag.MaskedEmail = HttpContext.Session.GetString("maskedEmail");
            return View("VerifyMFA");
        }

        // ---------------------------------------------------------------------
        // Complete verification and redirect (replacement for CompleteVerify)
        // ---------------------------------------------------------------------
        public IActionResult CompleteVerify()
        {
            string facsUser = HttpContext.Session.GetString("FACSUser") ?? "No";
            string forcePwdChange = HttpContext.Session.GetString("ForcePasswordChange") ?? "No";
            string gogreenLogin = HttpContext.Session.GetString("gogreenLogin") ?? "Yes";
            string recId = HttpContext.Session.GetInt32("RecID")?.ToString() ?? "0";
            string connectionString = _config.GetConnectionString("DefaultDataSource");

            if (gogreenLogin == "No")
            {
                return Redirect("~/FW/GoGreen_request");
            }

            if (forcePwdChange == "Yes")
            {
                if (facsUser == "Yes")
                    return Redirect("~/FW/ChangePassword");
                else
                    return Redirect("~/ChangePassword");
            }

            // Default redirect logic
            string pageRedirect = "";
            if (facsUser == "Yes")
            {
                try
                {
                    using (var conn = new SqlConnection(connectionString))
                    using (var da = new SqlDataAdapter("exec dbo.p_cli_Get_GoGreen_Form_Info " + recId, conn))
                    {
                        var ds = new DataSet();
                        da.Fill(ds);
                        pageRedirect = ds.Tables[0].Rows.Count > 0
                            ? "~/FW/menu?PopUp=NO"
                            : "~/FW/menu?PopUp=YES";
                    }
                }
                catch
                {
                    pageRedirect = "~/FW/menu?PopUp=NO";
                }
            }
            else
            {
                pageRedirect = "~/menu";
            }

            return Redirect(pageRedirect);
        }

        // ---------------------------------------------------------------------
        // MFA view (for verification step)
        // ---------------------------------------------------------------------
        [HttpGet]
        public IActionResult VerifyMFA()
        {
            ViewBag.MaskedEmail = HttpContext.Session.GetString("maskedEmail");
            ViewBag.Message = TempData["Message"];
            return View();
        }
    }
}

        
    

    

    

