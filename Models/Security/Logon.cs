using System;
using System.Data;
using Microsoft.Data.SqlClient;
using GRC_NewClientPortal.Models.Domain;
using System.Configuration;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
//using System.Web.UI;

namespace GRC_NewClientPortal.Models.Security
{
	/// <summary>
	/// Logon class
	/// </summary>
	public class Logon
	{
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContextAccessor;
        /// <summary>
        /// Class variable to hold the Confirmed flag indicating acceptance of Mini Miranda
        /// </summary>
        private bool _confirmed;
		/// <summary>
		/// Class variable to hold the Logged in flag
		/// </summary>
		private bool _logged_in;
		/// <summary>
		/// Class variable to hold the user id
		/// </summary>
		private string _id;  // keep private.  Used for logon change.
		/// <summary>
		/// Class variable to hold the Password
		/// </summary>
		private string _pwd; // keep private.  Used for logon change.
		/// <summary>
		/// Class variable to hold the Message Code from the last logon attempt
		/// </summary>
		private int _msgCode;
		/// <summary>
		/// Class variable to hold the Message Text from the last logon attempt
		/// </summary>
		private string _msgText;
		/// <summary>
		/// Class variable to hold the Formatted social security number
		/// </summary>
		private string _ssn;
		/// <summary>
		/// Class variable to hold the dataset of BEDP's and LEDP's
		/// </summary>
		private DataSet _dsBLedps;
		/// <summary>
		/// Class variable to hold the Demographics object for the logged in borrower
		/// </summary>
		private Demographics _demo;
		//@@@20050517ssm Added following variables to overcome the issue of 000000000 ssn
		/// <summary>
		/// Class variable to hold the Formatted social security number
		/// </summary>
		private string _bedp;
		/// <summary>
		/// Class variable to hold the BEDP number associated with the login
		/// </summary>
		private string _fname;
		/// <summary>
		/// Class variable to hold the Last name of the borrower
		/// </summary>
		private string _lname;
		//@@@20050517ssm end change
		/// <summary>
		/// Constructor for the logon object.
		/// </summary>
		public Logon(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
			_confirmed=false;
			_logged_in=false;
			_id="";
			_pwd="";
			_msgCode=0;
			_msgText="";
			_dsBLedps=new DataSet();
			_ssn="";
			//@@@20050517ssm initialize the new variables 
			_bedp="";
			_fname="";
			_lname="";
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _connectionString = configuration.GetConnectionString("DefaultDataSource");
            //@@@20050517ssm end change
        }

		/// <summary>
		/// Reference to the Demographics object
		/// </summary>
		public Demographics demo
		{
			get { return _demo; }
		}
		
		/// <summary>
		/// Returns boolean value indicating if the borrower has logged in
		/// </summary>
		public bool logged_in
		{
			get{return _logged_in;}
		}

		/// <summary>
		/// Returns the error message code.
		/// </summary>
		public int MsgCode
		{
			get {return _msgCode;}
		}

		/// <summary>
		/// Returns the error message text
		/// </summary>
		public string MsgText
		{
			get { return _msgText; }
		}

		/// <summary>
		/// Returns the Social Security Number
		/// </summary>
		public string ssn
		{
			get { return _ssn; }
		}

		/// <summary>
		/// Returns a boolean value indicating acceptance of Miranda by the web user.
		/// </summary>
		/// <remarks>
		/// for accepting the miranda agreement.
		/// Once confirmed, it stays confirmed for the length of the session
		/// regardless of being logged in or not.
		/// </remarks>
		public bool confirmed
		{
			get{return _confirmed;}
			set{_confirmed=value;}
		}

		/// <summary>
		/// This function is called when the web user is requesting web access for his/her account with GRC.
		/// </summary>
		/// <param name="sID">User name</param>
		/// <param name="sEDP">EDP number provided by the user</param>
		/// <param name="sSSN4">Last 4 digits of the SSN</param>
		/// <param name="sZip5">5 digit zip code</param>
		public async Task LogonRequestAsync(string sID, string sEDP, string sSSN4, string sZip5)
		{
			string user_id,edp,ssn4,zip5;
			user_id=sID.Trim();
			edp=sEDP.Trim();
			ssn4=sSSN4.Trim();
			zip5=sZip5.Trim();
			if (!(Common.TestLength(user_id,"ID")&&Common.TestChars(user_id,"ID")&&
				Common.TestLength(edp,"EDP")&&Common.TestChars(edp,"EDP")&&
				Common.TestLength(ssn4,"SSN4")&&Common.TestChars(ssn4,"SSN4")&&
				Common.TestLength(zip5,"ZIP5")&&Common.TestChars(zip5,"ZIP5")))
			{
				_msgText = "Invalid values, please review.";
				_msgCode = 88;
				return;
			}
            string sNewPwd = Common.gen_pass(6);

            // Use parameterized query to prevent SQL injection
            string query = "exec dbo.p_bwr_logon_request @SSN4, @ZIP5, @EDP, @UserID, @NewPwd, @NewPwdHash";

            try
            {
                using (var conn = new SqlConnection(_connectionString))   // <- Microsoft.Data.SqlClient.SqlConnection
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@SSN4", ssn4);
                    cmd.Parameters.AddWithValue("@ZIP5", zip5);
                    cmd.Parameters.AddWithValue("@EDP", edp);
                    cmd.Parameters.AddWithValue("@UserID", user_id);
                    cmd.Parameters.AddWithValue("@NewPwd", sNewPwd);
                    cmd.Parameters.AddWithValue("@NewPwdHash", Common.GetHash(sNewPwd));

                    await conn.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            _msgCode = Convert.ToInt32(reader["msg_code"]);
                            _msgText = Convert.ToString(reader["msg_text"]);
                        }
                        else
                        {
                            _msgCode = 0;
                            _msgText = "";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _msgCode = -1;
                _msgText = $"Error while processing request: {ex.Message}";
            }
        }

        /// <summary>
        /// Change password request
        /// </summary>
        /// <param name="sOldPwd">Old password</param>
        /// <param name="sNewPwd1">New password</param>
        /// <param name="sNewPwd2">New password</param>
        public void LogonChange(string sOldPwd, string sNewPwd1, string sNewPwd2)
		{
			string sop=sOldPwd.Trim();
			string snp1=sNewPwd1.Trim();
			string snp2=sNewPwd2.Trim();
			if (!(Common.TestLength(sop,"Pwd")&&Common.TestChars(sop,"Pwd")&&
				Common.TestLength(snp1,"Pwd")&&Common.TestChars(snp1,"Pwd")&&
				Common.TestLength(snp2,"Pwd")&&Common.TestChars(snp2,"Pwd")))
			{
				_msgText = "Invalid values, please review.";
				_msgCode = 88;
			}
			else if(snp1!=snp2)
			{
				_msgText="New passwords must match.";
				_msgCode=87;
			}
			else 
			{
                string connectionString = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build()
                                                                     .GetConnectionString("DefaultDataSource");

                using var connection = new SqlConnection(connectionString);
                using var command = new SqlCommand("dbo.p_bwr_logon_change", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Use parameters instead of concatenation
                command.Parameters.AddWithValue("@id", _id);
                command.Parameters.AddWithValue("@oldPwd", Common.GetHash(sop));
                command.Parameters.AddWithValue("@newPwd", Common.GetHash(snp1));

                try
                {
                    connection.Open();

                    using var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        _msgCode = reader["msg_code"] != DBNull.Value ? Convert.ToInt32(reader["msg_code"]) : 0;
                        _msgText = reader["msg_text"]?.ToString() ?? "No message returned.";
                    }
                    else
                    {
                        _msgCode = 0;
                        _msgText = "No rows returned from stored procedure.";
                    }
                }
                catch (Exception ex)
                {
                    _msgText = $"Error changing password: {ex.Message}";
                    _msgCode = -1;
                }
            }
        }
		

		/// <summary>
		/// Validates the username and password combination.
		/// </summary>
		/// <param name="sUserID">User name</param>
		/// <param name="sPassword">Password</param>
		

public void Validate(string sUserID, string sPassword)
    {
        string sID = sUserID?.Trim() ?? string.Empty;
        string sPwd = sPassword?.Trim() ?? string.Empty;

        if (!(Common.TestLength(sID, "ID") && Common.TestChars(sID, "ID") &&
              Common.TestLength(sPwd, "Pwd") && Common.TestChars(sPwd, "Pwd")))
        {
            _msgText = "Invalid Logon ID or Password.";
            _msgCode = 88;
            return;
        }

        try
        {
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultDataSource")))
            {
                conn.Open();

                //Execute stored procedure for login validation
                using (SqlCommand cmd = new SqlCommand("dbo.p_bwr_logon", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserID", sID);
                    cmd.Parameters.AddWithValue("@PasswordHash", Common.GetHash(sPwd));

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataSet dsLogon = new DataSet();
                        adapter.Fill(dsLogon);

                        if (dsLogon.Tables.Count > 0 && dsLogon.Tables[0].Rows.Count > 0)
                        {
                            DataRow drLogon = dsLogon.Tables[0].Rows[0];
                            _msgCode = Convert.ToInt32(drLogon["msg_code"]);
                            _msgText = Convert.ToString(drLogon["msg_text"]);
                            _ssn = Convert.ToString(drLogon["ssn"]);

                            
                            if (_msgCode >= 100)
                            {
                                _logged_in = true;
                                _id = sUserID;
                                _pwd = sPassword;

                                
                                using (SqlCommand cmdAdditional = new SqlCommand("dbo.p_bwr_AdditionalBwrInfo", conn))
                                {
                                    cmdAdditional.CommandType = CommandType.StoredProcedure;
                                    cmdAdditional.Parameters.AddWithValue("@UserID", sID);

                                    using (SqlDataAdapter daAdditionalInfo = new SqlDataAdapter(cmdAdditional))
                                    {
                                        DataSet dsAdditionalInfo = new DataSet();
                                        daAdditionalInfo.Fill(dsAdditionalInfo);

                                        if (dsAdditionalInfo.Tables.Count > 0 && dsAdditionalInfo.Tables[0].Rows.Count > 0)
                                        {
                                            DataRow drInfo = dsAdditionalInfo.Tables[0].Rows[0];
                                            _bedp = drInfo[0]?.ToString() ?? string.Empty;
                                            _lname = drInfo[1]?.ToString() ?? string.Empty;
                                            _fname = drInfo[2]?.ToString() ?? string.Empty;
                                        }
                                    }
                                }
                                _demo = new Demographics(_configuration);
                                _demo.GetUserDemographicsAsync(_ssn, _bedp);
                                SetBedpList();
                            }
                        }
                        else
                        {
                            _msgText = "No data returned from login procedure.";
                            _msgCode = 0;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _msgText = $"Error during validation: {ex.Message}";
            _msgCode = -1;
        }
    }


    /// <summary>
    /// Logs out the user by resetting the memory variables.
    /// </summary>
    public void LogOut()
		{
			_dsBLedps=new DataSet();    // clears list
			_demo = new Demographics(_configuration); // clears demographics
            _id ="";  // clear upon logout
			_pwd=""; // clear upon logout
			_logged_in=false;
		}

        /// <summary>
        /// Initializes session variables
        /// </summary>
        public void InitializeSessionVariables()
        {
            var context = _httpContextAccessor.HttpContext;
            var session = context?.Session;

            if (session == null)
                throw new InvalidOperationException("Session is not available.");

            // Clear existing session values
            ClearSessionVariables();

            // Initialize session variables to those in user demographics
            session.SetString("last_name", _demo.last_name ?? string.Empty);
            session.SetString("first_name", _demo.first_name ?? string.Empty);
            session.SetString("addr_1", _demo.addr1 ?? string.Empty);
            session.SetString("addr_2", _demo.addr2 ?? string.Empty);
            session.SetString("city", _demo.city ?? string.Empty);
            session.SetString("state", _demo.state ?? string.Empty);
            session.SetString("zip", _demo.zip ?? string.Empty);
            session.SetString("home_phone", _demo.home_phone ?? string.Empty);
            session.SetString("social_security_number", _demo.ssn_fmt ?? string.Empty);
            session.SetString("employment_name", _demo.emp_name ?? string.Empty);
            session.SetString("employment_addr_1", _demo.emp_addr ?? string.Empty);
            session.SetString("employment_addr_2", string.Empty); // Not used but reserved
            session.SetString("employment_city", _demo.emp_city ?? string.Empty);
            session.SetString("employment_state", _demo.emp_state ?? string.Empty);
            session.SetString("employment_zip", _demo.emp_zip ?? string.Empty);
            session.SetString("work_phone", _demo.emp_phone ?? string.Empty);
        }

        /// <summary>
        /// Clears the session variables
        /// </summary>
        
        public void ClearSessionVariables()
        {
            var context = _httpContextAccessor.HttpContext;
            var session = context?.Session;

            if (session == null)
                throw new InvalidOperationException("Session is not available.");

            // You can either clear all or reset manually:
            session.Clear(); // Clears all session keys at once 

            // Optional – reinitialize specific keys with blank values if needed:
            session.SetString("last_name", string.Empty);
            session.SetString("first_name", string.Empty);
            session.SetString("addr_1", string.Empty);
            session.SetString("addr_2", string.Empty);
            session.SetString("city", string.Empty);
            session.SetString("state", string.Empty);
            session.SetString("zip", string.Empty);
            session.SetString("home_phone", string.Empty);
            session.SetString("social_security_number", string.Empty);
            session.SetString("employment_name", string.Empty);
            session.SetString("employment_addr_1", string.Empty);
            session.SetString("employment_addr_2", string.Empty);
            session.SetString("employment_city", string.Empty);
            session.SetString("employment_state", string.Empty);
            session.SetString("employment_zip", string.Empty);
            session.SetString("work_phone", string.Empty);
            session.SetString("email", string.Empty);
            session.SetString("account_number", string.Empty);
            session.SetString("amount_paid", string.Empty);
            session.SetString("payment_method", string.Empty);
            session.SetString("bank_account_name", string.Empty);
            session.SetString("bank_account_number", string.Empty);
            session.SetString("bank_acct_type", string.Empty);
            session.SetString("aba", string.Empty);
            session.SetString("credit_card", string.Empty);
            session.SetString("card_number", string.Empty);
            session.SetString("name_on_card", string.Empty);
            session.SetString("expires_month", string.Empty);
            session.SetString("expires_year", string.Empty);
            session.SetString("expires", string.Empty);
            session.SetString("billing_addr_1", string.Empty);
            session.SetString("billing_addr_2", string.Empty);
            session.SetString("billing_city", string.Empty);
            session.SetString("billing_state", string.Empty);
            session.SetString("billing_zip", string.Empty);
            session.SetString("checking_account_name", string.Empty);
            session.SetString("drivers_license_number", string.Empty);
            session.SetString("check_number", string.Empty);
            session.SetString("micr", string.Empty);
        }


        /// <summary>
        /// Loads dataset with bedp numbers for the SSN
        /// </summary>
        private void SetBedpList()
        {
            // Ensure connection string is retrieved from configuration
            var connectionString = _configuration.GetConnectionString("DefaultDataSource");

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Use parameterized query for safety
                using (var cmd = new SqlCommand("dbo.p_bwr_bedp_list", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SSN", _ssn ?? string.Empty);
                    cmd.Parameters.AddWithValue("@FirstName", _fname ?? string.Empty);
                    cmd.Parameters.AddWithValue("@LastName", _lname ?? string.Empty);

                    using (var daEdps = new SqlDataAdapter(cmd))
                    {
                        _dsBLedps.Clear(); // Clear previous data
                        daEdps.Fill(_dsBLedps);
                    }
                }
            }
        }


        /// <summary>
        /// Returns BEDP number corresponding to the position supplied
        /// </summary>
        /// <param name="i">Position at which the BEDP number is required</param>
        /// <returns>BEDP number</returns>
        public string GetBedpFromIndex(int i)
		{
			if(i<0 || i>_dsBLedps.Tables[0].Rows.Count-1)
				return "";
			else
				return _dsBLedps.Tables[0].Rows[i]["ln_bedp"].ToString();
		}

		/// <summary>
		/// Returns LEDP number corresponding to the position supplied
		/// </summary>
		/// <param name="i">Position at which the LEDP number is required</param>
		/// <returns>LEDP number</returns>
		public string GetLedpFromIndex(int i)
		{
			if(i<0 || i>_dsBLedps.Tables[0].Rows.Count-1)
				return "";
			else
				return _dsBLedps.Tables[0].Rows[i]["ln_ledp"].ToString();
		}

		/// <summary>
		/// Returns account details in html format
		/// </summary>
		/// <param name="ledp">LEDP number</param>
		/// <returns>String in html format containing account details</returns>
		public string htmlAccountDetails(string ledp)
		{
			Account ad = new Account(ledp, _configuration);
			return ad.htmlDetails();
		}
		
		/// <summary>
		/// Payment details in html format
		/// </summary>
		/// <param name="bedp">BEDP number for which the Payment details is required</param>
		/// <returns>String in html format containing payment details of the account</returns>
		public string htmlPayDetails(string bedp)
		{
			PayDetails pd = new PayDetails(bedp);
			return pd.htmlPayDetails();
		}

		/// <summary>
		/// Account navigation string in html format.  This includes the BEDP numbers with corresponding LEDP numbers corresponding to the logged in borrower.
		/// </summary>
		/// <returns>String in html format including the account navigation links</returns>
		public string htmlAcctNavigation()
		{
			StringBuilder sb = new StringBuilder();
			string lastBedp="";//,lastLedp="";
			DataRow dr;
			for(int i=0;i<_dsBLedps.Tables[0].Rows.Count;i++) // loop using index i from 0 to row count
			{
				dr = _dsBLedps.Tables[0].Rows[i];
//@@@20050324ssm It was decided in the meeting that ledp's must not be shown.  Only the pay online link must be 
//               shown under the bedp.
//				if(dr["ln_bedp"].ToString()!=lastBedp)
//				{
//					lastBedp=dr["ln_bedp"].ToString();
//					sb.Append("<tr><td width=10><img src=images/global/bulletYellowPlus.gif border=0 hspace=0 vspace=0></td>");
//					sb.Append("<td><font class=subnav2Off>" + lastBedp.ToString());
//					sb.Append("</font></td></tr>");
//					sb.Append("<tr><td width=10></td>");
//					sb.Append("<td><A class=subnav2Off href=Borrowers_PayDetails.aspx?b=" + i +">&nbsp;>&nbsp;Payment Details");
//					sb.Append("</A></td></tr>");
//					sb.Append("<tr><td width=10></td>");
//					sb.Append("<td><A class=subnav2Off href=Borrowers_Pay.aspx?b=" + i +">&nbsp;>&nbsp;Pay Online");
//					sb.Append("</A></td></tr>");
//				}
//				if(dr["ln_ledp"].ToString()!=lastLedp)
//				{
//					lastLedp=dr["ln_ledp"].ToString();
//					sb.Append("<tr><td width=10></td>");
//					sb.Append("<td><a href=Borrowers_Details.aspx?l="+ i + " class=subnav2Off>&nbsp;>&nbsp;" +lastLedp);
//					sb.Append("</a></td></tr>");
//				}
				if(dr["ln_bedp"].ToString()!=lastBedp)
				{
					lastBedp=dr["ln_bedp"].ToString();
					sb.Append("<tr><td width=10><img src=images/global/bulletYellowPlus.gif border=0 hspace=0 vspace=0></td>");
					sb.Append("<td><a href=Borrowers_Details.aspx?l="+ i + " class=subnav2Off>" + lastBedp.ToString());
					sb.Append("</A></td></tr>");
//					sb.Append("<tr><td width=10></td>");
//					sb.Append("<td><A class=subnav2Off href=Borrowers_PayDetails.aspx?b=" + i +">&nbsp;>&nbsp;Payment Details");
//					sb.Append("</A></td></tr>");
					sb.Append("<tr><td width=10></td>");
					sb.Append("<td><A class=subnav2Off href=Borrowers_Pay.aspx?b=" + i +">&nbsp;>&nbsp;Pay Online");
					sb.Append("</A></td></tr>");
				}
//				if(dr["ln_ledp"].ToString()!=lastLedp)
//				{
//					lastLedp=dr["ln_ledp"].ToString();
//					sb.Append("<tr><td width=10></td>");
//					sb.Append("<td><a href=Borrowers_Details.aspx?l="+ i + " class=subnav2Off>&nbsp;>&nbsp;" +lastLedp);
//					sb.Append("</a></td></tr>");
//				}
//@@@20050324ssm end change.
			}	
							
			return sb.ToString();
		}
		
		/// <summary>
		/// Payment history in html format
		/// </summary>
		/// <param name="bedp">BEDP number</param>
		/// <param name="top">Number of last n payments</param>
		/// <returns>String in html format including the last n payments</returns>
		//public string htmlPayHistory(string bedp,int top)
		//{
		//	SqlConnection strConn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultDataSource"].ToString());
		//	SqlDataAdapter daPayHistory = new SqlDataAdapter("exec dbo.p_bwr_payments '"+bedp+"',"+top.ToString(),strConn);
		//	DataSet dsPayHistory = new DataSet();
		//	daPayHistory.Fill(dsPayHistory);
		//	daPayHistory.Dispose();
		//	string pay_date,pay_amt,status;
		//	if(dsPayHistory.Tables[0].Rows.Count==0)
		//	{
		//		return "<tr><td><font class=orangeheading>No Payment History Available.</font></tr></td>";
		//	}
		//	else
		//	{
		//		StringBuilder sb = new StringBuilder();
		//		foreach(DataRow dr in dsPayHistory.Tables[0].Rows)
		//		{
		//			pay_date=((DateTime)dr["pmts_pay_date"]).ToString("d");
		//			pay_amt=((Decimal)dr["pmts_pay_amt"]).ToString("C");
		//			status=dr["pmts_status"].ToString();
		//			sb.Append("<tr><td>&nbsp"+pay_date+"</td><td>&nbsp"+pay_amt+"</td><td>&nbsp"+status+"</td></tr>");

		//		}
		//		dsPayHistory.Dispose();
		//		return sb.ToString();
		//	}	
		//}


	}
}
