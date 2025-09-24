using Microsoft.VisualBasic;
using System;
using System.Text;
using System.Web;
using System.Configuration;
//using Microsoft.Data.Odbc;
//using System.Web.UI.WebControls;
//using System.Web.UI;
using System.Data;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Data.Common;
using static Org.BouncyCastle.Math.EC.ECCurve;
using System.Data.Odbc;

namespace GRC_NewClientPortal.Models.Domain
{
    public class ClientCommon
    {
        private readonly IConfiguration _configuration;
        private static IHttpContextAccessor _httpContextAccessor;
        public ClientCommon(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        
        /// <summary>
		/// Replaces singe quote with 2 single quotes so that SQL server does not throw errors
		/// </summary>
		/// <param name="strText">SQL Query string</param>
		/// <returns>Fixed SQL statement</returns>
		public static string SQLEncode(string strText)
        {
            string SQLEncode = "";
            if (strText != "" && Convert.IsDBNull(strText) == false)
            {
                strText = Strings.Replace(strText, "'", "''", 1, -1, CompareMethod.Binary);
            }
            SQLEncode = strText;
            return SQLEncode;
        }

        /// <summary>
        /// Read the text from config file to display in the balance footer section
        /// </summary>
        /// <returns>The Balance footer string from config file</returns>
        public string Bal_Footer()
        {
            return _configuration["AppSettings:Bal_Footer"];
        }

        /// <summary>
        /// Read the text from config file to display in the account footer section
        /// </summary>
        /// <returns>Account footer string from the config file</returns>
        public string Acct_Footer()
        {
            return _configuration["AppSettings:Acct_Footer"];
        }

        /// <summary>
        /// Create html string to be used for Information footer.
        /// </summary>
        /// <param name="p">Page object corresponding to the page calling this function.  This is required to access session variable values</param>
        /// <param name="boolShowJudgement">Boolean value indicating whether the amounts are Judgement amounts</param>
        /// <param name="boolShowLegal">Boolean value indicating if the account is with the legal department</param>
        /// <param name="C1FOL2">Email address of the secondary follow up collector</param>
        /// <param name="name">Name of the borrower</param>
        /// <param name="C1FOL">Email address of the primary follow up collector</param>
        /// <param name="bedp">Base edp number for the selected account</param>
        /// <returns>String in html format to be displayed in the info footer section</returns>
        public static string Info_Footer(ISession session, bool boolShowJudgement, bool boolShowLegal,
    string C1FOL2, string name, string C1FOL, string bedp)
        {
            StringBuilder sb = new StringBuilder();

            if (boolShowJudgement || boolShowLegal)
            {
                if (boolShowJudgement)
                {
                    sb.Append("<p><font size=\"-1\">These are judgement amounts, for current balance contact your legal representative by ");
                }
                else
                {
                    sb.Append("<p><font size=\"-1\">This account is in GRC's Legal Department, for questions please contact your legal representative by");
                }

                if (!string.IsNullOrWhiteSpace(C1FOL2))
                {
                    sb.Append("<a href=\"mailto:" + C1FOL.Trim() + "@generalrevenue.com?cc=" + C1FOL2.Trim() +
                              "@generalrevenue.com&subject=" + session.GetString("schoolname")?.Trim() +
                              " RE:" + name.Trim() + " #" + bedp.Trim() +
                              "\">clicking here</a> for email, or by ");
                }
                else if (!string.IsNullOrWhiteSpace(C1FOL))
                {
                    sb.Append("<a href=\"mailto:" + C1FOL.Trim() + "@generalrevenue.com?subject=" +
                              session.GetString("schoolname")?.Trim() +
                              " RE:" + name.Trim() + " #" + bedp.Trim() +
                              "\">clicking here</a> for email, or by ");
                }

                sb.Append("calling 1-800-234-1472.</font></p>");
            }

            sb.Append("<p><font size=\"-1\">If you have more current information or questions, please click on the link below to contact your Client Services Representative, or call 1-800-234-1472.</font></p>");

            if (!string.IsNullOrWhiteSpace(name))
            {
                sb.Append("<p align=\"center\"><a href=\"mailto:" +
                          session.GetString("CSR EMail")?.Trim() +
                          "?subject=" + session.GetString("schoolname")?.Trim() +
                          " RE:" + name.Trim() + " #" + bedp.Trim() +
                          "\"><font size=\"-2\"><b>Contact my CSR</b></font></a></p>");
            }
            else
            {
                sb.Append("<p align=\"center\"><a href=\"mailto:" +
                          session.GetString("CSR EMail")?.Trim() +
                          "\"><font size=\"-2\"><b>Contact my CSR</b></font></a></p>");
            }

            return sb.ToString();
        }


        /// <summary>
        /// Return the value stored in CSR EMail session variable.
        /// </summary>
        /// <param name="p">Page object corresponding to the page calling this function.  This is required to access session variable values</param>
        /// <returns>The CSR Email value from the session variable</returns>
        public static string CSREmail(HttpContext httpContext)
        {
            return httpContext.Session.GetString("CSR EMail")?.Trim();
        }


        /// <summary>
        /// Return the value stored in contactName session variable.  This function is called from the Menu.aspx page.
        /// </summary>
        /// <param name="p">Page object corresponding to the page calling this function.  This is required to access session variable values</param>
        /// <returns>Contact name value from the session variable prepended with "Hello, "</returns>
        public static string ContactName(HttpContext httpContext)
        {
            return "Hello, " + httpContext.Session.GetString("contactName")?.Trim() + ".  ";
        }

        /// <summary>
        /// This function is called from the Menu.aspx page.
        /// </summary>
        /// <param name="p">Page object corresponding to the page calling this function.  This is required to access session variable values</param>
        /// <returns>Returns Days before password expires</returns>
        public static string ExpDays(HttpContext httpContext)
        {
            return httpContext.Session.GetString("PasswordExpDaysLeft")?.Trim();
        }


        /// <summary>
        /// Function used for Printable view functionality
        /// </summary>
        /// <param name="control">Page object corresponding to the page calling this function.  This is required to access session variable values</param>
        /// <returns>None</returns>
        //public static void ClearControls(Control control)
        //{
        //    for (int i = control.Controls.Count - 1; i >= 0; i--)
        //    {
        //        ClearControls(control.Controls[i]);
        //    }

        //    if (!(control is TableCell))
        //    {
        //        if (control.GetType().GetProperty("SelectedItem") != null)
        //        {
        //            LiteralControl literal = new LiteralControl();
        //            control.Parent.Controls.Add(literal);
        //            try
        //            {
        //                literal.Text = (string)control.GetType().GetProperty("SelectedItem").GetValue(control, null);
        //            }
        //            catch

        //            {

        //            }

        //            control.Parent.Controls.Remove(control);
        //        }

        //        else

        //            if (control.GetType().GetProperty("Text") != null)
        //        {
        //            LiteralControl literal = new LiteralControl();
        //            control.Parent.Controls.Add(literal);
        //            literal.Text = (string)control.GetType().GetProperty("Text").GetValue(control, null);
        //            control.Parent.Controls.Remove(control);
        //        }
        //    }
        //    return;
        //}

        /// <summary>
        /// Checks session variables to determine if the client is properly signed on, 
        /// and redirects to the appropriate page if the session is invalid or requires action.
        /// </summary>
        /// <param name="httpContext">Current HTTP context used to access session values and perform redirects.</param>
        /// <returns>None</returns>
        public static void ClientSignonLogic(HttpContext httpContext)
        {
            if (string.IsNullOrWhiteSpace(httpContext.Session.GetString("signon")))
            {
                httpContext.Session.SetString("ErrMsg",
                    "Invalid Login, Last Name, and/or Password or your secure session has expired. <br>Please login again or contact your CSR for assistance.");
                httpContext.Response.Redirect("/clienthome");
            }

            if (httpContext.Session.GetString("FACSUser") == "Yes")
            {
                httpContext.Response.Redirect("/unauthorized");
            }

            if (httpContext.Session.GetString("ForcePasswordChange") == "Yes")
            {
                httpContext.Response.Redirect("/ChangePassword");
            }
        }


        /// <summary>
        /// Checks user signon session variable to determine if the user has access to the FACS webpages.
        /// </summary>
        /// <param name="p">Page object corresponding to the page calling this function.  This is required to access session variable values</param>
        /// <returns>None</returns>
        /// <remarks>The user will be validated and redirected to different pages based on the following:
        /// 1) unauthorized.htm - if user changes the url manually
        /// 2) hnd_logoff.aspx- if the session has expired
        /// 3) ChangePassword.aspx - if the user needs to change his password
        /// </remarks>
        public static void FACSSignOnLogic(HttpContext httpContext, IConfiguration configuration)
        {
            if (string.IsNullOrWhiteSpace(httpContext.Session.GetString("signon")))
            {
                httpContext.Session.SetString("ErrMsg",
                    "Invalid Login, Last Name, and/or Password or your secure session has expired.<br>Please login again or contact your CSR for assistance.");

                var redirectUrl = configuration["SessionExpiredRedirectUrl"];
                httpContext.Response.Redirect(redirectUrl ?? "/SessionExpired");
            }

            if (httpContext.Session.GetString("FACSUser") == "No")
            {
                httpContext.Response.Redirect("/unauthorized");
            }

            if (httpContext.Session.GetString("ForcePasswordChange") == "Yes")
            {
                httpContext.Response.Redirect("/ChangePassword");
            }
        }


        /// <summary>
        /// Stores account search results in a session variable to be accessed by the audit.aspx page. 
        /// Account search results are filtered by the accounts accesible for the client logged in.
        /// Search results will consist of either none, single or multiple accounts.
        /// </summary>
        /// <param name="p">Page object corresponding to the page calling this function.  This is required to access session variable values</param>
        /// <returns>None</returns>
        public static void QuickAudit(HttpContext httpContext, IConfiguration configuration, string connectionString)
        {
            var accountNumber = httpContext.Session.GetString("AccountNumber")?.Trim().Replace("'", "''");

            if (string.IsNullOrEmpty(accountNumber))
                return;

            string query = $@"
        SELECT 
            DBTR_CLNT_GENERL_INF.ACCTNUM_FROM_CLIENT,
            DBTR_CLNT_GENERL_INF.CLIENT,
            DBTR_CLNT_GENERL_INF.ACCOUNT_NUM,
            DBTR_CLNT_GENERL_INF.DATE_LISTED,
            DBTR_BALANCES.INITIAL_BALANCE
        FROM 
            DBTR_CLNT_GENERL_INF
        INNER JOIN DBTR_BALANCES 
            ON DBTR_BALANCES.ACCOUNT_NUM = DBTR_CLNT_GENERL_INF.ACCOUNT_NUM
        WHERE 
            DBTR_CLNT_GENERL_INF.ACCTNUM_FROM_CLIENT LIKE '%
            {accountNumber}%'";

            using (var connection = new OdbcConnection(connectionString))
            {
                var adapter = new OdbcDataAdapter(query, connection);
                var ds = new DataSet();
                adapter.Fill(ds, "LocateAccount");

                var table = ds.Tables["LocateAccount"];
                if (table.Rows.Count == 0)
                {
                    httpContext.Session.SetString("AccNotFound", "true");
                    httpContext.Response.Redirect("/audit");
                }
                else if (table.Rows.Count == 1)
                {
                    httpContext.Session.SetString("AccountNumber", table.Rows[0]["ACCOUNT_NUM"].ToString());
                    httpContext.Response.Redirect("/demo_info");
                }
                else
                {
                    var sb = new StringBuilder();
                    sb.Append("<table id=\"accountListings\" border=\"1\">")
                      .Append("<thead>")
                      .Append("<th>Client Account Number</th>")
                      .Append("<th>Client Name</th>")
                      .Append("<th>Date Listed</th>")
                      .Append("<th>Amount Listed</th>")
                      .Append("</thead>");

                    foreach (DataRow row in table.Rows)
                    {
                        sb.Append("<tr>")
                          .Append("<td><a href='/audit?ID=" + row["ACCOUNT_NUM"] + "'>" + row["ACCTNUM_FROM_CLIENT"] + "</a></td>")
                          .Append("<td>" + row["CLIENT"] + "</td>")
                          .Append("<td>" + Convert.ToDateTime(row["DATE_LISTED"]).ToShortDateString() + "</td>")
                          .Append("<td align='right'>" + Convert.ToDecimal(row["INITIAL_BALANCE"]).ToString("C") + "</td>")
                          .Append("</tr>");
                    }

                    sb.Append("</table>");

                    httpContext.Session.SetString("MultipleAccount", sb.ToString());
                    httpContext.Response.Redirect("/audit");
                }
            }
        }

        private static string GetConnectionString(IHttpContextAccessor httpContextAccessor, IConfiguration config)
        {
            var facsDir = httpContextAccessor.HttpContext.Session.GetString("FACSDirectory");

            // Read from appsettings.json
            var connString = config.GetConnectionString("FACSDSN");

            // If your old logic had ~FACSDirectory~ placeholder, replace it
            return connString.Replace("~FACSDirectory~", facsDir ?? "");
        }

        /// <summary>
        /// Fills a passed dataset with results from a passed sql string. 
        /// </summary>
        /// <param name="p">Page object corresponding to the page calling this function.  This is required to access session variable values</param>
        /// <param name="strsql">Sql string parameter describing the t-sql command to be used to fill the data adapter</param>
        /// <param name="ds">Dataset parameter to be used to fill data from the SQL results</param>
        /// <param name="strTableName">Table Name parameter to be used for setting a table name for the dataset</param>
        /// <returns>None</returns>
        public static void GetInfo(IHttpContextAccessor httpContextAccessor, string strSql, ref DataSet ds, string strTableName)
        {
            //using (var cn = new SqlConnection(GetConnectionString(httpContextAccessor)))
            //{
            //    using (var da = new SqlDataAdapter(strSql, cn))
            //    {
            //        da.Fill(ds, strTableName);
            //    }
            //}
        }

        /// <summary>
        /// Builds a list of subclients for the signed-on client and stores it in Session
        /// </summary>
        public static void BuildSubClientList(IHttpContextAccessor httpContextAccessor)
        {
            var ds = new DataSet();
            var signon = httpContextAccessor.HttpContext.Session.GetString("signon");

            string strSQL = $@"
            SELECT 
                CLNT_GENERAL.CLIENT_NUM, 
                CLNT_GENERAL.CLIENT_NAME
            FROM CU_CLIENT_AUDIT
            LEFT OUTER JOIN CLNT_GENERAL 
                ON CU_CLIENT_AUDIT.CLIENT_NUM1 = CLNT_GENERAL.CLIENT_NUM
            WHERE UPPER(CLIENT_NUMBER) = '{signon?.ToUpper()}'
              AND CLNT_GENERAL.ACTIVE_CLIENT = 'Y'
            ORDER BY CLNT_GENERAL.CLIENT_NAME, CLNT_GENERAL.CLIENT_NUM";

            GetInfo(httpContextAccessor, strSQL, ref ds, "Clients");

            // Save as JSON in session instead of DataTable
            var dt = ds.Tables["Clients"];
            if (dt != null)
            {
                httpContextAccessor.HttpContext.Session.SetString("subClients", Newtonsoft.Json.JsonConvert.SerializeObject(dt));
            }
        }

        /// <summary>
        /// Filters subclients for signed-on client and returns SQL WHERE clause
        /// </summary>
        public static string GetClientNumbersSQLFilter(IHttpContextAccessor httpContextAccessor)
        {
            var subClientsJson = httpContextAccessor.HttpContext.Session.GetString("subClients");
            if (string.IsNullOrEmpty(subClientsJson)) return "";

            var dt = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(subClientsJson);
            if (dt == null || dt.Rows.Count == 0) return "";

            var filters = new List<string>();
            foreach (DataRow row in dt.Rows)
            {
                filters.Add($"CLIENT = '{row["CLIENT_NUM"]}'");
            }

            return string.Join(" OR ", filters);
        }

        /// <summary>
        /// Retrieves client name from Client number
        /// </summary>
        /// <param name="p">Page object corresponding to the page calling this function.  This is required to access session variable values</param>
        /// <param name="strClientCode">Client code number string parameter to be used to dtermine the client name</param>
        /// <returns>string Client Name obtained from Client number</returns>
        public static string GetClientNameFromClientNumber(string strClientCode)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var subClientsJson = session.GetString("subClients");
            if (string.IsNullOrEmpty(subClientsJson))
                return string.Empty;

            // Deserialize DataTable from JSON (assuming you stored it serialized)
            var dt = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(subClientsJson);
            if (dt == null)
                return string.Empty;

            foreach (DataRow row in dt.Rows)
            {
                if (row["CLIENT_NUM"].ToString() == strClientCode)
                    return row["CLIENT_NAME"].ToString();
            }

            return string.Empty;
        }

        /// <summary>
        /// Formats long date strings with time to short date strings with no time
        /// </summary>
        /// <param name="strDate">Long date string to be converted</param>
        /// <returns>Short date time with no time</returns>
        public static string strDate(string strDate)
        {
            DateTime dt;
            try
            {
                dt = Convert.ToDateTime(strDate);
            }
            catch
            {
                return "";
            }
            return dt.ToShortDateString();
        }

        /// <summary>
        /// Formats a string amount to currency as a string
        /// </summary>
        /// <param name="strAmount">string amount</param>
        /// <returns>Converted string to currency string</returns>
        public static string strCurrency(string strAmount)
        {
            string strRet = "";
            try
            {
                strRet = Convert.ToDecimal(strAmount).ToString("C");
            }
            catch
            {
                return "";
            }
            return strRet;
        }

        /// <summary>
        /// Formats a 9 digit zip code to a 5 digit zip code
        /// </summary>
        /// <param name="strZip">9 digit zip code</param>
        /// <returns>5 digit zip code as a string</returns>
        public static string strZIP(string strZip)
        {
            string strRet = "";
            if (strZip.Length == 9)
                strRet = strZip.Substring(0, 5) + "-" + strZip.Substring(5, 4);
            else
                strRet = strZip;
            return strRet;
        }

        /// <summary>
        /// Formats 9 digit Social Security Number to include dashes
        /// </summary>
        /// <param name="strSSN">SSN without dashes</param>
        /// <returns>SSN with dashes</returns>
        public static string strSSN(string strSSN)
        {
            string strRet = "";

            if (strSSN.Length == 9)
                strRet = strSSN.Substring(0, 3) + "-" + strSSN.Substring(3, 2) + "-" + strSSN.Substring(5, 4);
            else
                strRet = strSSN;
            return strRet;
        }
        /// <summary>
        /// Gets full description from a frequency code
        /// </summary>
        /// <param name="strFreq">Frequency code</param>
        /// <returns>Frequency description</returns>
        public static string FreqDescription(string strFreq)
        {
            string strRet = "";
            switch (strFreq.ToUpper())
            {
                case "BIF":
                    strRet = "The Balance In Full will be sent.";
                    break;
                case "MON":
                    strRet = "Monthly payments will be made.";
                    break;
                case "WEK":
                    strRet = "Weekly payments will be made.";
                    break;
                case "BMO":
                    strRet = "Bimonthly payments will be made. (every 60 days)";
                    break;
                case "QTR":
                    strRet = "Quarterly payments will be made.";
                    break;
                case "SMO":
                    strRet = "Semimonthly payments will be made. (twice a month) on the 1st and 15th of each month.";
                    break;
                case "SMU":
                    strRet = "Semimonthly payments will be made on user specified dates.";
                    break;
                case "BWK":
                    strRet = "Biweekly payments will be made. (every 14 days)";
                    break;
                case "SET":
                    strRet = "A settlement payment will be sent.";
                    break;
                case "POI":
                    strRet = "A payment of intent. This will not advance wait.";
                    break;
                default:
                    strRet = "No payment arrangements found";
                    break;

            }
            return strRet;
        }


        /// <summary>
        /// Returns and http base URL string
        /// </summary>
        /// <param name="p">Page object corresponding to the page calling this function.  This is required to access session variable values</param>
        /// <returns>Base URL</returns>
        public static string HTTPBaseURL()
        {
            var request = _httpContextAccessor.HttpContext.Request;
            return $"{request.Scheme}://{request.Host}";
        }
    }
}
