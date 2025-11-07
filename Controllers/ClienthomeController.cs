using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using GRC_NewClientPortal.Models.GRCEmail;
using GRC_NewClientPortal.Models;

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
        public async Task<IActionResult> clienthome()
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

            var model = new GRC_NewClientPortal.Models.ClienthomeModel
            {
                ShowMFA = false,
                Message = string.IsNullOrEmpty(errMsg) ? "" : errMsg.Trim()
            };

            session.SetString("ErrMsg", string.Empty);

            // === Redirect if already logged in ===
            var signon = session.GetString("signon");
            if (!string.IsNullOrEmpty(signon))
            {
                await GetClientInfo();

                var facsUser = session.GetString("FACSUser");
                if (facsUser == "No")
                    return RedirectToAction("Menu", "Client");
                else
                    return RedirectToAction("Menu", "FW");
            }

            // === Session Timeout (handled globally in ASP.NET Core, left here for info) ===
            int timeoutMinutes = Convert.ToInt32(_config["AppSettings:SessionTimeoutForClients"]);
            // NOTE: session timeout is configured in Program.cs; cannot be set per-session.
            return View("clienthome", model);
        }

        /// <summary>
        /// Gets client info from database.
        /// </summary>
        private async Task GetClientInfo()
        {
            var session = _httpContextAccessor.HttpContext.Session;

            if (session.GetInt32("RecID") != null)
            {
                if (_config["AppSettings:Client_Survey_Link"] != null && _config["AppSettings:Client_Survey_Exp_Date"] != null)
                {
                    if (DateTime.Now < Convert.ToDateTime(_config["AppSettings:Client_Survey_Exp_Date"]))
                    {
                        string surveyLink = _config["AppSettings:Client_Survey_Link"]!;
                        string connectionString = _config.GetConnectionString("DefaultDataSource")!;
                        int? recId = session.GetInt32("RecID");

                        using SqlConnection conn = new SqlConnection(connectionString);
                        SqlDataAdapter daClientDetails = new SqlDataAdapter(
                            $"exec dbo.ssp_APG_GRCWeb_client_GetClientInfo {recId}", conn);

                        DataSet dsClientDetails = new DataSet();
                        await Task.Run(() => daClientDetails.Fill(dsClientDetails)); 

                        string clientBaseNumber = string.Empty,
                               clientName = string.Empty,
                               personName = string.Empty,
                               email = string.Empty,
                               clientcsr = string.Empty,
                               clientsalesID = string.Empty,
                               facsDirectory = string.Empty;

                        if (dsClientDetails.Tables[0].Rows.Count >= 1)
                        {
                            var row = dsClientDetails.Tables[0].Rows[0];
                            clientBaseNumber = row["ClientBaseNumber"].ToString() ?? string.Empty;
                            clientName = row["ClientName"].ToString() ?? string.Empty;
                            personName = row["PersonName"].ToString() ?? string.Empty;
                            email = row["Email"].ToString() ?? string.Empty;
                            clientcsr = row["CSR"].ToString() ?? string.Empty;
                            clientsalesID = row["SalesID"].ToString() ?? string.Empty;
                            facsDirectory = row["FACSDirectory"].ToString() ?? string.Empty;
                        }

                        if (!string.IsNullOrEmpty(facsDirectory) && facsDirectory.ToUpper() == "POH")
                        {
                            string formattedSurveyLink = string.Format(
                                surveyLink,
                                clientBaseNumber, clientName, email, personName, clientcsr, clientsalesID
                            );

                            session.SetString("CLIENT_SURVEY_LINK", formattedSurveyLink);
                        }
                    }
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> clienthome([FromBody] ClienthomeModel model)

        {
            var session = _httpContextAccessor.HttpContext.Session;

            session.SetString("ForcePasswordChange", "No");
            session.SetString("FACSUser", "No");

            if (string.IsNullOrWhiteSpace(model.Password) || string.IsNullOrWhiteSpace(model.LName) || string.IsNullOrWhiteSpace(model.Signon))
            {
                model.ErrorMessage = "Client Login, Last name and password are required.";
                model.ShowMFA = false;
                return Json(model);
            }

            string connStr = _config.GetConnectionString("DefaultDataSource");
            string hashedPassword = model.Password;
            bool boolLogit = false;
            string boolMemos; 

            string storedPassword = null;
            using (var sqlConn = new SqlConnection(connStr))
            {
                await sqlConn.OpenAsync();
                // Check if the stored password is hashed
                var dacheckPassword = new SqlCommand(@"
                    SELECT [Password] 
                    FROM tbl_FACS_client_contact_data a 
                    INNER JOIN t_client_login_data b ON a.RecID = b.RecID 
                    WHERE UPPER([C10BC#]) = @Signon AND UPPER(C10LNM) = @LName", sqlConn);

                dacheckPassword.Parameters.AddWithValue("@Signon", model.Signon.ToUpper());
                dacheckPassword.Parameters.AddWithValue("@LName", model.LName.ToUpper());

                // Execute the query and to show in console the output form it.
                var sqlresult = await dacheckPassword.ExecuteScalarAsync(); // returns first column of first row
                if (sqlresult != null)
                {
                    storedPassword = sqlresult.ToString();
                    Console.WriteLine($"Password from DB: {storedPassword}"); 
                }
                else
                {
                    Console.WriteLine("No matching record found.");
                }

                var da = new SqlDataAdapter(dacheckPassword);
                var dscheckPassword = new DataSet();
                da.Fill(dscheckPassword);

                if (dscheckPassword.Tables[0].Rows.Count > 0)
                {

                    if (dscheckPassword.Tables[0].Rows[0]["Password"].ToString().Trim() != model.Password.Trim())
                    {
                        hashedPassword = GRC_NewClientPortal.Models.Domain.Common.GetHash(model.Password);
                    }
                }
                else
                {
                    model.ErrorMessage = "Invalid Login or Last Name.";
                    Console.WriteLine("DB doesnot have the Login. Please enter the details again.");
                    return Json(model);
                    

                    //hashedPassword = GRC_NewClientPortal.Models.Domain.Common.GetHash(model.Password);
                }

                if (dscheckPassword.Tables[0].Rows[0]["Password"].ToString().Trim() == hashedPassword.Trim())

                {
                    var cmdLogin = new SqlDataAdapter($"exec p_cli_logon_request '{model.Signon.ToUpper()}', '{hashedPassword}', '{model.LName.ToUpper()}'", sqlConn);
                    var dsLogin = new DataSet();
                    cmdLogin.Fill(dsLogin);

                    var row = dsLogin.Tables[0].Rows[0];
                    session.SetString("CSR EMail", row["CSR EMail"].ToString());
                    string fname = row["C10FNM"].ToString().Trim();
                    string lname = row["C10LNM"].ToString().Trim();
                    session.SetString("contactName", $"{fname} {lname}");
                    boolMemos = row["C10MEM"].ToString();
                    if (boolMemos == "Y")
                    {
                        session.SetString("boolMemos","true");
                    }
                    else
                    {
                        session.SetString("boolMemos", "false");
                    }
                    
                    session.SetInt32("RecID", Convert.ToInt32(row["RecID"]));
                    session.SetString("FACSDirectory", row["FACSDirectory"].ToString());
                    session.SetString("DPEntrystatus", row["DPEntry"].ToString());

                    // Password expiration logic
                    DateTime dtPassChangeDate = Convert.ToDateTime(row["PasswordChangeDate"]);
                    int loginAttempts = Convert.ToInt32(row["LoginAttempts"]);
                    int accountLockout = Convert.ToInt32(row["AccountLockout"]);
                    int tempPassword = Convert.ToInt32(row["TempPassword"]);

                    if (accountLockout == 1)
                    {
                        //session.SetString("ErrMsg", "Your account has been locked. Please contact your CSR.");

                        model.ErrorMessage = "Your account has been locked or you are using wrong login. <br>Please contact your CSR for assistance.";
                        model.ShowMFA = false;
                        // stop execution and show error on screen
                        return Json(model);
                    }
                    else if (loginAttempts > 0)
                    {
                        model.ErrorMessage = "Invalid Login, Last Name, and/or Password. <br>Please login again or contact your CSR for assistance.";
                        model.ShowMFA = false;
                        // stop execution and show error on screen
                        return Json(model);
                      
                    }
                    else if (tempPassword == 1)
                    {
                        session.SetString("ForcePasswordChange", "Yes");
                        model.ErrorMessage = "You have logged in with a temporary password. Please update your password now.";
                        session.SetString("PasswordChangeMessage", model.ErrorMessage);
                        boolLogit = true;
                    }
                    else if (dtPassChangeDate > DateTime.MinValue)
                    {
                        int passwordexpperiod = Convert.ToInt32(_config["AppSettings:PasswordExpPeriod"]);
                        int expDays = (int)(dtPassChangeDate.AddDays(Convert.ToInt32(_config["AppSettings:PasswordExpPeriod"])) - DateTime.Now).TotalDays;
                        if (expDays <= 0)
                        {
                            session.SetString("ForcePasswordChange", "Yes");
                            model.ErrorMessage = "Your password has expired. Please update your password now.";
                            session.SetString("PasswordChangeMessage", model.ErrorMessage);
                            boolLogit = true;
                        }
                        else
                        {
                            session.SetInt32("PasswordExpDaysLeft", expDays);
                            boolLogit = true;
                        }
                    }
                    else

                        boolLogit = true;
                    // Set school name
                    session.SetString("schoolname", row["School Name"].ToString().Trim());
                    session.SetString("SessionVariablesSet", "No");
                    session.SetString("SessionDPVariablesSet", "No");
                    session.SetString("SessionACHVariablesSet", "No");
                    session.SetString("signon", row["C10BC#"].ToString());
                    session.SetString("contactEmail", row["Email"].ToString().Trim());
                    session.SetString("pword", model.Password.Trim());

                    // Role Permissions
                    if (dsLogin.Tables.Count > 1)
                    {
                        var roles = dsLogin.Tables[1].AsEnumerable()
                            .Select(r => r.Field<string>("Role")).ToList();
                        session.SetString("rolePermissions", string.Join(",", roles));
                    }


                    if (session.GetInt32("RecID") >= 10000)
                    {
                        session.SetString("FACSUser", "Yes");

                        // Build WebServerURL and FACSsignon
                        string facsDirectory = session.GetString("FACSDirectory");
                        string strResourceConfig = _config[$"AppSettings:{facsDirectory}"]; 
                        int i = strResourceConfig.IndexOf(",");
                        string strResource = strResourceConfig.Substring(0, i);
                        string strNamespaceNumber = strResourceConfig.Substring(i + 1); // everything after comma

                        // Set WebServerURL (original code removed www issue)
                        session.SetString("WebServerURL", strResource);

                        // Set FACSsignon
                        session.SetString("FACSsignon", $"C-{strNamespaceNumber}-{session.GetString("signon")}");

                        // Set log message (replace hdnBrowserVersionClt.Value with proper value from form/model)
                        string browserVersion = model.BrowserVersion ?? "UnknownBrowser"; // you need to pass this from your form/model
                        session.SetString("log_msgClt", $"{browserVersion}; {Environment.MachineName}; {DateTime.Now}");
                    }



                    // Check ACH Campaign
                    string sqlACH = $"exec p_chk_ACHCampaign_exits '{session.GetInt32("RecID")}', '{_config["AppSettings:ACH_RemindMeLaterExpire_Days"]}'";
                    int iach = 0;
                    using (var cmdACH = new SqlCommand(sqlACH, sqlConn))
                    {
                   
                        iach = Convert.ToInt32(await cmdACH.ExecuteScalarAsync());
                        await sqlConn.CloseAsync();
                    }
                    session.SetString("ACHPopUp", iach >= 1 ? "Yes" : "No");
                }

                else
                {
                    model.ErrorMessage = "Invalid Login, Last Name, and/or Password. <br>Please login again or contact your CSR for assistance.";
                    model.ShowMFA = false;
                    return Json(model);
                   
                }
            }
                Console.WriteLine($"ErrorMessage: {model.ErrorMessage}");

                // Generate MFA only if there is no error or if it's a temporary password
                if (string.IsNullOrEmpty(model.ErrorMessage) || model.ErrorMessage == "You have logged in with a temporary password. Please update your password now.")
                {
                    string verificationCode = await GenerateVerificationCodeAsync(session.GetString("contactEmail"));
                var contactEmail = session.GetString("contactEmail");
                    string maskedEmail = HttpContext.Session.GetString("maskedEmail");
                    session.SetString("VerificationCode", verificationCode);
                    model.ShowMFA = true;
                    model.Message = $"To verify it's you, please enter the code you received through an email message at {maskedEmail}.";
                 }
                
                if (boolLogit)
                {
                    await LogItAsync(session.GetString("VerificationCode"));
                    await GetClientInfo();
                    return Json(model);
               
                 }

                else
                {
                    session.SetString("signon", "");
                    session.SetString("CSR EMail", _config["AppSettings:DefaultClientContact"]);
                    session.SetString("pword", "");
                    session.SetString("schoolname", "");
                    session.SetString("boolMemos", "false");

                    
                    model.ErrorMessage = "Invalid login. <br>Please login again or contact your CSR for assistance.";
                    model.ShowMFA = false;
                return Json(model);
                }
            }           
        
        private async Task LogItAsync(string verificationCode)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            string loginIP = httpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            int? recId = httpContext.Session.GetInt32("RecID");

            string connStr = _config.GetConnectionString("DefaultDataSource");
            string sql = @"INSERT INTO tbl_client_logins (login, loginIP, loginDate, verificationCode)
                           VALUES (@login, @loginIP, @loginDate, @verificationCode)";

            try
            {
                using (var conn = new SqlConnection(connStr))
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@login", recId);
                    cmd.Parameters.AddWithValue("@loginIP", loginIP);
                    cmd.Parameters.AddWithValue("@loginDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@verificationCode", verificationCode);

                    await conn.OpenAsync();
                    int rows = await cmd.ExecuteNonQueryAsync();
                    await conn.CloseAsync();

                    if (rows > 0)
                        Console.WriteLine($"[LogItAsync] Inserted {rows} row(s) into tbl_client_logins for RecID={recId}, IP={loginIP}, Code={verificationCode}");
                    else
                        Console.WriteLine("[LogItAsync] No rows inserted (ExecuteNonQuery returned 0).");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[LogItAsync] Exception: {ex.Message}");
            }
        }

        private async Task<string> GenerateVerificationCodeAsync(string emailTo)
        {
            var random = new Random();
            string code = random.Next(0, 1000000).ToString("D6");
            //GRC_NewClientPortal.Models.GRCEmail.SendMail
            // Send email (you can replace with your own mail service)
            var mailer = new GRC_NewClientPortal.Models.GRCEmail.SendMail(_config, _sendMailLogger);
            await mailer.SendEmailAsync(
                emailTo,
                _config["AppSettings:EmailClientSrvs"],
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
        public async Task<IActionResult> VerifyMFA([FromBody] ClienthomeModel model)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            string correctCode = session.GetString("VerificationCode");

            if (string.IsNullOrEmpty(correctCode))
            {
                return Json(new
                {
                    success = false,
                    errorMessage = "Your session has expired. Please request a new verification code."
                });
            }

            if (model.MFACode == correctCode)
            {
                // If code matches, proceed to CompleteVerify (old CompleteVerify logic)
                return await CompleteVerify();
            }
            else
            {
                return Json(new
                {
                    success = false,
                    errorMessage = "Verification code does not match. Please enter the correct code or request a new one."
            });                

            }
        }

        // ---------------------------------------------------------------------
        // Request new verification code (replacement for btnNewVerify_Click)
        // ---------------------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> NewVerificationCode()
        {
            string email = HttpContext.Session.GetString("contactEmail");
            string newCode = await GenerateVerificationCodeAsync(email);
            await LogItAsync(newCode);

            TempData["Message"] = "A new verification code has been sent.";
            ViewBag.MaskedEmail = HttpContext.Session.GetString("maskedEmail");
            return View("VerifyMFA");
        }



        public async Task<IActionResult> CompleteVerify()
        {
            var session = _httpContextAccessor.HttpContext.Session;

            string facsUser = session.GetString("FACSUser");
            string forcePwdChange = session.GetString("ForcePasswordChange");
            string gogreenLogin = session.GetString("gogreenLogin");
            string recId = session.GetInt32("RecID")?.ToString();
            string connectionString = _config.GetConnectionString("DefaultDataSource");

            if (gogreenLogin == "No")
                return Redirect("~/FW/GoGreen_request");

            if (forcePwdChange == "Yes")
            {
                var msg = session.GetString("PasswordChangeMessage");
                if (!string.IsNullOrEmpty(msg))
                {
                    TempData["PasswordChangeMessage"] = msg;
                }
                return Redirect(facsUser == "Yes" ? "/changepassword" : "~/menu");
            }

            string pageRedirect;
            if (facsUser == "Yes")
            {
                try
                {
                    using (var conn = new SqlConnection(connectionString))
                    using (var da = new SqlDataAdapter($"exec dbo.p_cli_Get_GoGreen_Form_Info {recId}", conn))
                    {
                        var ds = new DataSet();
                        da.Fill(ds);
                        pageRedirect = ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0
                            ? "~/FW/menu?PopUp=NO"
                            : "~/FW/menu?PopUp=YES";
                    }
                }
                catch (Exception ex)
                {
                   // _logger.LogError(ex, "Error checking GoGreen info.");
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

        
    

    

    

