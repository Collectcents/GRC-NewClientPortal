using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;
using System.Text;
using static Org.BouncyCastle.Math.EC.ECCurve;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;

namespace GRC_NewClientPortal.Models.Domain
{
    public class PayOnline
    {
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _config;


        public PayOnline(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _connectionString = configuration.GetConnectionString("DefaultDataSource");
            _config = configuration;
        }
        // pay flow pro parameters
        /// <summary>
        /// Class variable to hold the Host address for pay flow pro
        /// </summary>
        private string strHostAddress;
        /// <summary>
        /// Class variable to hold the Host port for pay flow pro
        /// </summary>
        private int intHostPort;
        /// <summary>
        /// Class variable to hold the Vendor name for pay flow pro
        /// </summary>
        private string strVendor;
        /// <summary>
        /// Class variable to hold the Partner name for pay flow pro
        /// </summary>
        private string strPartner;
        /// <summary>
        /// Class variable to hold the User name for pay flow pro
        /// </summary>
        private string strUserName;
        /// <summary>
        /// Class variable to hold the password for pay flow pro
        /// </summary>
        private string strPassword;

        /// <summary>
        /// Class variable to hold the Amount to charge
        /// </summary>
        private string strAmount;
        /// <summary>
        /// Class variable to hold the Credit card type
        /// </summary>
        private string strCCType;
        /// <summary>
        /// Class variable to hold the Expiration date of the credit card
        /// </summary>
        private string strExpires = "";
        /// <summary>
        /// Class variable to hold the Credit card account number
        /// </summary>
        private string strCCAccountNo;
        /// <summary>
        /// Class variable to hold the masked credit card account number
        /// </summary>
        private string strCCAccountNoShow;
        /// <summary>
        /// Class variable to hold the Street address on the credit card account
        /// </summary>
        private string strStreet;
        /// <summary>
        /// Class variable to hold the City on the credit card account
        /// </summary>
        private string strCity;
        /// <summary>
        /// Class variable to hold the Zip for the credit card account
        /// </summary>
        private string strZip;
        /// <summary>
        /// Class variable to hold the email address for payment confirmation
        /// </summary>
        private string strEmail;
        /// <summary>
        /// Class variable to hold the Bank account number
        /// </summary>
        private string strBankAccount;
        /// <summary>
        /// Class variable to hold the Bank account type
        /// </summary>
        private string strBankAccountType;
        /// <summary>
        /// Class variable to hold the ABA number
        /// </summary>
        private string strABA;
        /// <summary>
        /// Class variable to hold the Bank account name
        /// </summary>
        private string strBankAccountName;
        /// <summary>
        /// Class variable to hold the First name
        /// </summary>
        private string strFirstName;
        /// <summary>
        /// Class variable to hold the Last name
        /// </summary>
        private string strLastName;
        /// <summary>
        /// Class variable to hold the Comment 1
        /// </summary>
        private string strComment1;
        /// <summary>
        /// Class variable to hold the Comment 2
        /// </summary>
        private string strComment2;
        /// <summary>
        /// Class variable to hold the Trans code
        /// </summary>
        private string strTransCode;
        /// <summary>
        /// Class variable to hold the Transaction type
        /// </summary>
        private string strTransactionType;
        /// <summary>
        /// Class variable to hold the Parameter list
        /// </summary>
        private string strParamList;
        /// <summary>
        /// Class variable to hold the Pay flow pro return string
        /// </summary>
        private string strPFPReturnString = "";
        /// <summary>
        /// Class variable to hold the reference to the pay flow pro object
        /// </summary>
        //		private  PFProCOMLib.PNComClass pfPayFlowPro;
        //		/// <summary>
        //		/// Class variable to hold the Pay flow pro context
        //		/// </summary>
        private int iCtx1;
        /// <summary>
        /// Class variable to hold the Authorization code
        /// </summary>
        private string strAuthCode;
        /// <summary>
        /// Class variable to hold the PNREF from pay flow pro
        /// </summary>
        private string strPNREF;
        /// <summary>
        /// Class variable to hold the Response message from pay flow pro
        /// </summary>
        private string strResponseMsg = "";
        /// <summary>
        /// Class variable to hold the Result code from pay flow pro
        /// </summary>
        private string strResultCode;
        /// <summary>
        /// Class variable to hold the Result code from pay flow pro
        /// </summary>
        private int intResultCode;
        /// <summary>
        /// Class variable to hold the Error message
        /// </summary>
        private string strErrorMessage = "";
        /// <summary>
        /// Class variable to hold the Result message
        /// </summary>
        private string strResultMessage = "";
        /// <summary>
        /// Class variable to hold the Subject for the email
        /// </summary>
        private string strSubject = "";
        /// <summary>
		/// Constructor for the class
		/// </summary>
		public PayOnline()
        {
        }

        /// <summary>
        /// Completes an online payment using payflowpro and saves details to the database then emails the same
        /// </summary>
        /// <param name="p">Page object corresponding to the page calling this function.  This is required to access session variable values</param>
        public void MakePayment(HttpContext context)
        {
            InitializeVariables(context);

            if (strTransCode == "C")
            {
                context.Session.SetString("result_message",
                    "<font size=\"medium\" color=\"red\"><strong>This option is temporarily unavailable. To make a payment please call your GRC Collections Representative or hit the back button to make a payment via an \"Electronic Fund or ACH Transfer\". Our apologies for the inconvenience.</strong></font>");
                return;
            }
            else
            {
                strResultMessage = "Thank you!<BR><bR>Following is a description of your transaction. "
                                 + "Please keep a copy of it for your records.<br><br>"
                                 + "Amount Paid: $" + strAmount + "<br>";
                strSubject = "ACH or Check";
                strPNREF = "None";
            }

            SaveTransactionInfo(context, strPNREF, strResponseMsg, intResultCode, strAuthCode);
            EmailTransactionAsync();
            ClearPaymentSessionVariables();
            context.Session.SetString("result_message", strResultMessage);
        }


        /// <summary>
        /// Saves DemographicsInfo
        /// ///</summary>
        /// <param name="p">Page object corresponding to the page calling this function.  This is required to access session variable values</param>
        public async Task SaveDemographicsInfo(HttpContext httpContext)
        {
            using var cn = new SqlConnection(_connectionString);
            await cn.OpenAsync();

            var sql = @"
            INSERT INTO t_online_payments
            (first_name, last_name, email, account_number, ssn,
             home_phone, work_phone, home_addr1, home_addr2, home_city, home_state, home_zip,
             emp_addr1, emp_addr2, emp_city, emp_state, emp_zip, Reference_ID, log_msg, emp_name)
            VALUES
            (@FirstName, @LastName, @Email, @AccountNumber, @SSN,
             @HomePhone, @WorkPhone, @HomeAddr1, @HomeAddr2, @HomeCity, @HomeState, @HomeZip,
             @EmpAddr1, @EmpAddr2, @EmpCity, @EmpState, @EmpZip, @ReferenceId, @LogMsg, @EmpName)";

            using var cmd = new SqlCommand(sql, cn);

            // Safely pull from session
            cmd.Parameters.AddWithValue("@FirstName", httpContext.Session.GetString("first_name") ?? "");
            cmd.Parameters.AddWithValue("@LastName", httpContext.Session.GetString("last_name") ?? "");
            cmd.Parameters.AddWithValue("@Email", httpContext.Session.GetString("email") ?? "");
            cmd.Parameters.AddWithValue("@AccountNumber", httpContext.Session.GetString("account_number") ?? "");
            cmd.Parameters.AddWithValue("@SSN", httpContext.Session.GetString("social_security_number") ?? "");
            cmd.Parameters.AddWithValue("@HomePhone", httpContext.Session.GetString("home_phone") ?? "");
            cmd.Parameters.AddWithValue("@WorkPhone", httpContext.Session.GetString("work_phone") ?? "");
            cmd.Parameters.AddWithValue("@HomeAddr1", httpContext.Session.GetString("addr_1") ?? "");
            cmd.Parameters.AddWithValue("@HomeAddr2", httpContext.Session.GetString("addr_2") ?? "");
            cmd.Parameters.AddWithValue("@HomeCity", httpContext.Session.GetString("city") ?? "");
            cmd.Parameters.AddWithValue("@HomeState", httpContext.Session.GetString("state") ?? "");
            cmd.Parameters.AddWithValue("@HomeZip", httpContext.Session.GetString("zip") ?? "");
            cmd.Parameters.AddWithValue("@EmpAddr1", httpContext.Session.GetString("employment_addr_1") ?? "");
            cmd.Parameters.AddWithValue("@EmpAddr2", httpContext.Session.GetString("employment_addr_2") ?? "");
            cmd.Parameters.AddWithValue("@EmpCity", httpContext.Session.GetString("employment_city") ?? "");
            cmd.Parameters.AddWithValue("@EmpState", httpContext.Session.GetString("employment_state") ?? "");
            cmd.Parameters.AddWithValue("@EmpZip", httpContext.Session.GetString("employment_zip") ?? "");
            cmd.Parameters.AddWithValue("@ReferenceId", httpContext.Session.GetString("ReferenceID") ?? "");
            cmd.Parameters.AddWithValue("@LogMsg", httpContext.Session.GetString("log_msg") ?? "");
            cmd.Parameters.AddWithValue("@EmpName", httpContext.Session.GetString("employment_name") ?? "");

            await cmd.ExecuteNonQueryAsync();
        }
        /// <summary>
        /// Saves ACH Payment Info
        /// ///</summary>
        /// <param name="p">Page object corresponding to the page calling this function.  This is required to access session variable values</param>
        public async Task SavePaymentInfoAsync()
        {
            var session = _httpContextAccessor.HttpContext.Session;

            string accountName = session.GetString("bank_account_name");
            string accountNumber = session.GetString("bank_account_number");
            string accountType = session.GetString("bank_acct_type");
            string abaNumber = session.GetString("ABA");
            string amountPaid = session.GetString("amount_paid");
            string referenceId = session.GetString("ReferenceID");

            string connectionString = _config.GetConnectionString("DefaultDataSource");

            string query = @"
            UPDATE t_online_payments 
            SET 
                ach_acctholdername = @AcctName,
                ach_acctNumber = @AcctNumber,
                ach_acctType = @AcctType,
                ach_abaNumber = @AbaNumber,
                amount_paid = @AmountPaid,
                transaction_date = @TransactionDate
            WHERE Reference_ID = @ReferenceID";

            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@AcctName", accountName ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@AcctNumber", accountNumber ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@AcctType", accountType ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@AbaNumber", abaNumber ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@AmountPaid", amountPaid ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@TransactionDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@ReferenceID", referenceId ?? (object)DBNull.Value);

                    await cn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }

                await EmailTransactionAsync(); // you’ll rewrite this also for .NET Core
            }
            catch (Exception ex)
            {
                // log exception with ILogger
                throw new ApplicationException("Error saving payment info", ex);
            }
        }
        /// <summary>
        /// Initializes memory variables for the payment object
        /// </summary>
        /// <param name="p">Page object corresponding to the page calling this function.  This is required to access session variable values</param>
        public void InitializeVariables(HttpContext context)
        {
            // Host and port
            strHostAddress = _config["VerisignURL"]; // from appsettings.json
            intHostPort = 443;

            // Vendor details
            strVendor = "GenRev";
            strPartner = "VeriSign";
            strUserName = "GenRev";
            strPassword = "GenRev1981";

            // Payment details
            strAmount = Common.ToCurrency(context.Session.GetString("amount_paid"));

            strTransCode = context.Session.GetString("payment_method");
            strCCType = context.Session.GetString("credit_card");

            if (strTransCode == "C")
            {
                string expMonth = context.Session.GetString("expires_month");
                string expYear = context.Session.GetString("expires_year");
                if (!string.IsNullOrEmpty(expMonth) && !string.IsNullOrEmpty(expYear))
                {
                    strExpires = expMonth + expYear.Substring(2, 2);
                }
            }

            strCCAccountNo = context.Session.GetString("card_number");
            if (!string.IsNullOrEmpty(strCCAccountNo) && strCCAccountNo.Length > 3)
            {
                strCCAccountNoShow = "xxxxxxxx" + strCCAccountNo[^4..]; // last 4 digits
            }

            strStreet = (context.Session.GetString("addr_1") ?? "") + " " + (context.Session.GetString("addr_2") ?? "");
            strCity = context.Session.GetString("city");
            strZip = context.Session.GetString("zip");
            strEmail = context.Session.GetString("email");
            if (!string.IsNullOrEmpty(strEmail) && strEmail.Length > 64)
                strEmail = strEmail.Substring(0, 64);

            strBankAccount = (context.Session.GetString("bank_account_number") ?? "").Replace(" ", "");
            strBankAccountType = context.Session.GetString("bank_acct_type");
            strABA = (context.Session.GetString("aba") ?? "").Replace(" ", "");
            strBankAccountName = context.Session.GetString("bank_account_name");

            strFirstName = context.Session.GetString("first_name");
            if (!string.IsNullOrEmpty(strFirstName) && strFirstName.Length > 15)
                strFirstName = strFirstName.Substring(0, 15);

            strLastName = context.Session.GetString("last_name");
            if (!string.IsNullOrEmpty(strLastName) && strLastName.Length > 15)
                strLastName = strLastName.Substring(0, 15);

            strComment1 = "Account Number: " + context.Session.GetString("account_number");
            if (strComment1.Length > 128)
                strComment1 = strComment1.Substring(0, 128);

            strComment2 = "SSN: " + context.Session.GetString("social_security_number");
            if (strComment2.Length > 128)
                strComment2 = strComment2.Substring(0, 128);

            // Format parameters list
            if (strTransCode == "C")
            {
                strParamList = "TRXTYPE=S&TENDER=C" +
                               "&PARTNER=" + strPartner +
                               "&VENDOR=" + strVendor +
                               "&USER=" + strUserName +
                               "&PWD=" + strPassword +
                               "&ACCT=" + strCCAccountNo +
                               "&EXPDATE=" + strExpires +
                               "&AMT=" + strAmount +
                               "&FIRSTNAME[" + strFirstName.Length + "]=" + strFirstName +
                               "&LASTNAME[" + strLastName.Length + "]=" + strLastName +
                               "&COMMENT1[" + strComment1.Length + "]=" + strComment1 +
                               "&COMMENT2[" + strComment2.Length + "]=" + strComment2;
            }
        }

        //		/// <summary>
        //		/// Completes an online payment
        //		/// </summary>
        //		private void MakePayFlowPayment()
        //		{
        //			//))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))
        //			//instantiate and ionitialize the PayFlow Pro Object
        //			//Set objPayFlowPro = Server.CreateObject("PFProCOMControl.PFProCOMControl.1")
        //			//<timeOut>:          "30" (optional)
        //			//<proxyAddress>:     [address of your proxy server] (optional)
        //			//<proxyPort>:        [port number of your proxy server] (optional)
        //			//<proxyLogon>:       [login name of your proxy server] (optional)
        //			//<proxyPassword>:    [password for the login name] (optional)
        //			//<debugMode>:        "1" to turn on debug mode (optional, COM client)
        //			try
        //			{
        //				pfPayFlowPro = new PFProCOMLib.PNComClass();
        //				iCtx1 = pfPayFlowPro.CreateContext(strHostAddress, intHostPort, 30, "", 0, "", "");
        //			}
        //			catch(Exception e)
        //			{
        //				strErrorMessage = strErrorMessage + "Error: " + e.Message + "<BR>";
        //			}
        //			try
        //			{
        //				strPFPReturnString = Convert.ToString(pfPayFlowPro.SubmitTransaction(iCtx1, strParamList, strParamList.Length));
        //			}
        //			catch(Exception e)
        //			{
        //				strErrorMessage = strErrorMessage + "Error: " + e.Message + "<BR>";
        //			}
        //			pfPayFlowPro.DestroyContext(iCtx1);
        //			// pay flow processing complete
        //
        //			strAuthCode = getPPFValue("AUTHCODE");
        //			strPNREF = getPPFValue("PNREF");
        //			strResponseMsg = getPPFValue("RESPMSG");
        //			strResultCode = getPPFValue("result");
        //
        //			try
        //			{
        //				intResultCode = Convert.ToInt32(strResultCode);
        //			}
        //			catch
        //			{
        //				intResultCode = 69;				//bad return data
        //	 			strSubject = "Not Approved";
        //			}
        //
        //			//case -1				'failed to connect to host
        //			//case -2				'failed to resolve hostname
        //			//case -5				'failed to initialize SSL context
        //			//case -6				'parameter list format error: & in name
        //			//case -7				'parameter list format error: invalid [] name length clause
        //			//case -8				'ssl failed to connect to host
        //			//case -9				'SSL read failed
        //			//case -10				'SSL write failed
        //			//case -11				'Proxy authorization failed
        //			//case -12				'Timeout waiting for response
        //			//case -13				'Select failure
        //			//case -14				'Too many connection
        //			//case -15				'Failed to set socket options
        //			//case -20				'Proxy read failed
        //			//case -21				'Proxy write failed
        //			//case -22				'Failed to initialize SSL Cerificate
        //			//case -23				'Host address not specified
        //			//case -24				'Invalid transaction type
        //			//case -25				'Failed to create a socket
        //			//case -26				'Failed to initialize socket layer
        //			//case -27				'parameter list format error: invalid [] name length clause
        //			//case -28				'parameter list format error: name
        //			//case -29				'Failed to initialize ssl connection
        //			//case -30				'Invalid timeout value
        //			//case -31				'the certificate chain did not validate, no local certificate found
        //			//case -32				'the certificate chain did not validate, common name did not match URL
        //			//case -99				'out of memory
        //			if (intResultCode == 0)
        //			{
        //				//successful transaction
        //				strResultMessage = "Following is a description of your transaction. " + " Please keep a copy of it for your records.<br><br>" + "Transaction ID: " + strPNREF + "<br>" + "Amount Paid: $" + strAmount + "<br>" + "<br><br><Br>VeriSign has routed, processed and secured your payment information.<br>" + "<A href='http://www.verisign.com/products/payment.html' class='link' target='_blank'>More information about VeriSign.</A><br>";
        //				strSubject = "Approved";
        //			}
        //			else if (intResultCode == 1)
        //			{
        //				//user authentication failed
        //				strResultMessage = "Your transaction could not be completed due to a technical error: User authentication failed.<br>  Please try again or contact us.<br>";
        //			}
        //			else if (intResultCode == 2)
        //			{
        //				//invalid tender.  merchant bank does not support the credit card type that was submitted.
        //				strResultMessage = "Your transaction could not be completed: Invalid tender.<br>  Please notify the webmaster if this problem persists.<br>";
        //			}
        //			else if (intResultCode == 3)
        //			{
        //				//invalid transaction type
        //				strResultMessage = "Your transaction could not be completed due to a technical error: Invalid transaction type.<br>  Please notify the webmaster if this problem persists.<br>";
        //			}
        //			else if (intResultCode == 4)
        //			{
        //				//invalid amount
        //				strResultMessage = "Your transaction could not be completed due to a technical error: Invalide amount.<br>  Please notify the webmaster if this problem persists.<br>";
        //			}
        //			else if (intResultCode == 5)
        //			{
        //				//invalid merchant information
        //				strResultMessage = "Your transaction could not be completed due to a technical error: Invalid merchant information.<br>  Please notify the webmaster if this problem persists.<br>";
        //			}
        //			else if (intResultCode == 7)
        //			{
        //				//field format error
        //				strResultMessage = "Your transaction could not be completed due to a technical error: Field format error.<br>  Please notify the webmaster if this problem persists.<br>";
        //			}
        //			else if (intResultCode == 8)
        //			{
        //				//not a transaction server
        //				strResultMessage = "Your transaction could not be completed due to a technical error: Not a transaction server.<br>  Please notify the webmaster if this problem persists.<br>";
        //			}
        //			else if (intResultCode == 9)
        //			{
        //				//too many parameters or invalid stream
        //				strResultMessage = "Your transaction could not be completed due to a technical error: Too many parameters or invalid stream.<br>  Please notify the webmaster if this problem persists.<br>";
        //			}
        //			else if (intResultCode == 10)
        //			{
        //				//too many line items
        //				strResultMessage = "Your transaction could not be completed due to a technical error: Too many line items.<br>  Please notify the webmaster if this problem persists.<br>";
        //			}
        //			else if (intResultCode == 11)
        //			{
        //				//client timeout
        //				strResultMessage = "Your transaction could not be completed due to a technical error: Timeout waiting for response.<br> Please try again or contact us.<br>";
        //			}
        //			else if (intResultCode == 12)
        //			{
        //				//decline
        //				strResultMessage = "Your transaction could not be completed: Declined.<br>  Please check your credit card number and personal information and try again or contact us.<br>";
        //			}
        //			else if (intResultCode == 13)
        //			{
        //				//referral
        //				strResultMessage = "Your transaction could not be completed: Declined.<br>  Please check your credit card number and personal information and try again or contact us.<br>";
        //				//case 19				'original transaction id not found
        //				//strResultMessage = "Your transaction could not be completed due to a technical error.<br>  Please notify the webmaster if this problem persists.<br>"
        //				//case 20				'cannot find the customer reference number
        //			}
        //			else if (intResultCode == 22)
        //			{
        //				//invalid ABA number
        //				strResultMessage = "Your transaction could not be completed: Invalid ABA number.<br> Please try again or contact us.<br>";
        //			}
        //			else if (intResultCode == 23)
        //			{
        //				//invalid account number
        //				strResultMessage = "Your transaction could not be completed: Invalid account number.<br>  Check your credit card number and try again or contact us.<br>";
        //			}
        //			else if (intResultCode == 24)
        //			{
        //				//invalid expiration date
        //				strResultMessage = "Your transaction could not be completed: Invalid expiration date.<br>  Please check the expiration date and try again or contact us.<br>";
        //			}
        //			else if (intResultCode == 25)
        //			{
        //				//transaction type not mapped to this host
        //				strResultMessage = "Your transaction could not be completed: Transaction type not mapped to this host.<br>  Please notify the webmaster if this problem persists.<br>";
        //				//case 26				'invalid vendor account
        //				//case 27				'insufficient partner permissions
        //				//case 28				'insufficient user permissions
        //			}
        //			else if (intResultCode == 50)
        //			{
        //				//insufficient funds available
        //				strResultMessage = "Your transaction could not be completed: Insufficient funds available.<br>  Please try again or contact us.<br>";
        //			}
        //			else if (intResultCode == 69)
        //			{
        //				//bad return data
        //				strResultMessage = "Your transaction could not be completed due to a technical error: Bad return data.<br>  Please notify the webmaster if this problem persists.<br>";
        //			}
        //			else if (intResultCode == 99)
        //			{
        //				//general error
        //				strResultMessage = "Your transaction could not be completed due to a technical error: General error.<br>  Please notify the webmaster if this problem persists.<br>";
        //			}
        //			else if (intResultCode == 100)
        //			{
        //				//invalid transaction returned from host
        //				strResultMessage = "Your transaction could not be completed due to a technical error: Invalid transaction returned from host.<br>  Please notify the webmaster if this problem persists.<br>";
        //			}
        //			else if (intResultCode == 101)
        //			{
        //				//timeout value too small
        //				strResultMessage = "Your transaction could not be completed due to a technical error: Timeout value too small.<br>  Please notify the webmaster if this problem persists.<br>";
        //			}
        //			else if (intResultCode == 102)
        //			{
        //				//processor not available
        //				strResultMessage = "Your transaction could not be completed due to a technical error: Payment processor not available.<br>  Please notify the webmaster if this problem persists.<br>";
        //			}
        //			else if (intResultCode == 103)
        //			{
        //				//error reading response from host
        //				strResultMessage = "Your transaction could not be completed due to a technical error: Error reading response from host.<br>  Please notify the webmaster if this problem persists.<br>";
        //			}
        //			else if (intResultCode == 104)
        //			{
        //				//timeout waiting for host reponse
        //				strResultMessage = "Your transaction could not be completed due to a technical error: Timeout waiting for host response.<br>  Please try again or contact us.<br>";
        //			}
        //			else if (intResultCode == 105)
        //			{
        //				//credit error.  Make sure that you have not already credited this transaction.
        //				strResultMessage = "Your transaction could not be completed: Credit error.<br>  Please notify the webmaster if this problem persists.<br>";
        //			}
        //			else if (intResultCode == 106)
        //			{
        //				//host not available
        //				strResultMessage = "Your transaction could not be completed due to a technical error: Payment processor not available.<br>  Please notify the webmaster if this problem persists.<br>";
        //			}
        //			else if (intResultCode == 107)
        //			{
        //				//duplicate suppression time out
        //				strResultMessage = "Your transaction could not be completed due to a technical error: Duplicate supression time out.<br>  Please notify the webmaster if this problem persists.<br>";
        //			}
        //			else if (intResultCode == 108)
        //			{
        //				//void error.  Make sure the transaction entered has not already been voided
        //				strResultMessage = "Your transaction could not be completed: Void error.<br>  Please try again or contact us.<br>";
        //			}
        //			else if (intResultCode == 109)
        //			{
        //				//timeout waiting for host response
        //				strResultMessage = "Your transaction could not be completed due to a technical error: Timeout waiting for host response.<br>  Please try again or contact us.<br>";
        //			}
        //			else if (intResultCode == 111)
        //			{
        //				//capture error.  Only authorized transactions can be captured.
        //				strResultMessage = "Your transaction could not be completed: Capture error.<br>  Please try again or contact us.<br>";
        //			}
        //			else if (intResultCode == 112)
        //			{
        //				//failed AVS check
        //				strResultMessage = "Your transaction could not be completed: Failed AVS check - address and zip code do not match credit card billing address.<br>  Please try again or contact us.<br>";
        //			}
        //			else if (intResultCode == 113)
        //			{
        //				//cannot exceed sales cap - for ACH transactions
        //				strResultMessage = "Your transaction could not be completed: Cannot exceed sales cap.<br>  Please try again or contact us.<br>";
        //			}
        //			else if (intResultCode == 114)
        //			{
        //				//CVV2 Mismatch
        //				strResultMessage = "Your transaction could not be completed: CVV2 mismatch.<br>Please try again or contact us.<br>";
        //			}
        //			else if (intResultCode == 1000)
        //			{
        //				//generic host error
        //				strResultMessage = "Your transaction could not be completed due to a technical error: Generic host error.<br>  Please notify the webmaster if this problem persists.<br>";
        //			}
        //			else
        //			{
        //				//unknown error
        //				strResultMessage = "Your transaction could not be completed: Unspecified error.<br>  Please try again or contact us.<br>";
        //			}; 
        //
        //		}

        /// <summary>
        /// Returns the payflowpro key value
        /// </summary>
        /// <param name="key">Key to search</param>
        /// <returns>Value corresponding to the key</returns>
        private string getPPFValue(string key)
        {
            int key_pos, key_len, sep_pos, val_pos;
            string res = strPFPReturnString + "&";
            key_len = key.Length;
            key_pos = res.IndexOf(key.ToUpper(), 0);
            if (key_pos == -1)
                return ""; // key not found
            sep_pos = res.IndexOf("&", key_pos);
            val_pos = key_pos + key_len + 1;
            return res.Substring(val_pos, sep_pos - val_pos);
        }

        /// <summary>
        /// SQL text for the value supplied
        /// </summary>
        /// <param name="value">value to be SQL encoded for storage in the database</param>
        /// <returns>sql encoded string</returns>
        private string SQLText(string value)
        {
            return "'" + ClientCommon.SQLEncode(value) + "'";
        }

        /// <summary>
        /// save all the info for manual processing
        /// </summary>
        /// <param name="p">Page object corresponding to the page calling this function.  This is required to access session variable values</param>

        public async Task SaveTransactionInfo(HttpContext httpContext, string strPNREF, string strResponseMsg, int intResultCode, string strAuthCode)
        {
            var sql = @"
            INSERT INTO t_online_payments
            (first_name, last_name, email, account_number, ssn,
             home_phone, work_phone, home_addr1, home_addr2, home_city, home_state, home_zip,
             bill_addr1, bill_addr2, bill_city, bill_state, bill_zip,
             emp_addr1, emp_addr2, emp_city, emp_state, emp_zip,
             ach_acctholdername, ach_acctNumber, ach_acctType, ach_abaNumber,
             check_acctholdername, check_dlnumber, check_checkNumber, check_micrNumber,
             pnref, response_msg, amount_paid, transaction_date, return_code, auth_code)
            VALUES
            (@FirstName, @LastName, @Email, @AccountNumber, @SSN,
             @HomePhone, @WorkPhone, @HomeAddr1, @HomeAddr2, @HomeCity, @HomeState, @HomeZip,
             @BillAddr1, @BillAddr2, @BillCity, @BillState, @BillZip,
             @EmpAddr1, @EmpAddr2, @EmpCity, @EmpState, @EmpZip,
             @AchAcctHolderName, @AchAcctNumber, @AchAcctType, @AchAbaNumber,
             @CheckAcctHolderName, @CheckDLNumber, @CheckNumber, @MicrNumber,
             @PNREF, @ResponseMsg, @AmountPaid, @TransactionDate, @ReturnCode, @AuthCode)";

            using var cn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, cn);

            // Handle Oklahoma flag (prefix * to first name)
            var firstName = httpContext.Session.GetString("first_name") ?? "";
            if (httpContext.Session.GetString("OK_link") == "True")
                firstName = "*" + firstName;

            // Parameters
            cmd.Parameters.AddWithValue("@FirstName", firstName);
            cmd.Parameters.AddWithValue("@LastName", httpContext.Session.GetString("last_name") ?? "");
            cmd.Parameters.AddWithValue("@Email", httpContext.Session.GetString("email") ?? "");
            cmd.Parameters.AddWithValue("@AccountNumber", httpContext.Session.GetString("account_number") ?? "");
            cmd.Parameters.AddWithValue("@SSN", httpContext.Session.GetString("social_security_number") ?? "");
            cmd.Parameters.AddWithValue("@HomePhone", httpContext.Session.GetString("home_phone") ?? "");
            cmd.Parameters.AddWithValue("@WorkPhone", httpContext.Session.GetString("work_phone") ?? "");
            cmd.Parameters.AddWithValue("@HomeAddr1", httpContext.Session.GetString("addr_1") ?? "");
            cmd.Parameters.AddWithValue("@HomeAddr2", httpContext.Session.GetString("addr_2") ?? "");
            cmd.Parameters.AddWithValue("@HomeCity", httpContext.Session.GetString("city") ?? "");
            cmd.Parameters.AddWithValue("@HomeState", httpContext.Session.GetString("state") ?? "");
            cmd.Parameters.AddWithValue("@HomeZip", httpContext.Session.GetString("zip") ?? "");
            cmd.Parameters.AddWithValue("@BillAddr1", httpContext.Session.GetString("billing_addr_1") ?? "");
            cmd.Parameters.AddWithValue("@BillAddr2", httpContext.Session.GetString("billing_addr_2") ?? "");
            cmd.Parameters.AddWithValue("@BillCity", httpContext.Session.GetString("billing_city") ?? "");
            cmd.Parameters.AddWithValue("@BillState", httpContext.Session.GetString("billing_state") ?? "");
            cmd.Parameters.AddWithValue("@BillZip", httpContext.Session.GetString("billing_zip") ?? "");
            cmd.Parameters.AddWithValue("@EmpAddr1", httpContext.Session.GetString("employment_addr_1") ?? "");
            cmd.Parameters.AddWithValue("@EmpAddr2", httpContext.Session.GetString("employment_addr_2") ?? "");
            cmd.Parameters.AddWithValue("@EmpCity", httpContext.Session.GetString("employment_city") ?? "");
            cmd.Parameters.AddWithValue("@EmpState", httpContext.Session.GetString("employment_state") ?? "");
            cmd.Parameters.AddWithValue("@EmpZip", httpContext.Session.GetString("employment_zip") ?? "");
            cmd.Parameters.AddWithValue("@AchAcctHolderName", httpContext.Session.GetString("bank_account_name") ?? "");
            cmd.Parameters.AddWithValue("@AchAcctNumber", httpContext.Session.GetString("bank_account_number") ?? "");
            cmd.Parameters.AddWithValue("@AchAcctType", httpContext.Session.GetString("bank_acct_type") ?? "");
            cmd.Parameters.AddWithValue("@AchAbaNumber", httpContext.Session.GetString("ABA") ?? "");
            cmd.Parameters.AddWithValue("@CheckAcctHolderName", httpContext.Session.GetString("checking_account_name") ?? "");
            cmd.Parameters.AddWithValue("@CheckDLNumber", httpContext.Session.GetString("drivers_license_number") ?? "");
            cmd.Parameters.AddWithValue("@CheckNumber", httpContext.Session.GetString("check_number") ?? "");
            cmd.Parameters.AddWithValue("@MicrNumber", httpContext.Session.GetString("micr") ?? "");
            cmd.Parameters.AddWithValue("@PNREF", strPNREF ?? "");
            cmd.Parameters.AddWithValue("@ResponseMsg", strResponseMsg ?? "");
            cmd.Parameters.AddWithValue("@AmountPaid", decimal.TryParse(httpContext.Session.GetString("amount_paid"), out var amount) ? amount : 0);
            cmd.Parameters.AddWithValue("@TransactionDate", DateTime.UtcNow);
            cmd.Parameters.AddWithValue("@ReturnCode", intResultCode);
            cmd.Parameters.AddWithValue("@AuthCode", strAuthCode ?? "");

            await cn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }


        /// <summary>
        /// Email the online payment details for entry into CMS about the online payment
        /// </summary>
        /// <param name="p">Page object corresponding to the page calling this function.  This is required to access session variable values</param>
        public async Task EmailTransactionAsync()
        {
            var session = _httpContextAccessor.HttpContext.Session;

            var sb = new StringBuilder();
            string emailSubject = "Online Payment Received - ACH or Check";

            sb.AppendLine("A payment attempt was just made online");
            sb.AppendLine($"Name: {session.GetString("first_name")} {session.GetString("last_name")}");
            sb.AppendLine($"Email: {session.GetString("email")}");
            sb.AppendLine($"Acct Number: {session.GetString("account_number")}");
            sb.AppendLine($"SSN: {session.GetString("social_security_number")}");
            sb.AppendLine($"Home Phone: {session.GetString("home_phone")}");
            sb.AppendLine($"Work Phone: {session.GetString("work_phone")}");
            sb.AppendLine($"Address: {session.GetString("addr_1")} {session.GetString("addr_2")}");
            sb.AppendLine($"City: {session.GetString("city")}, State: {session.GetString("state")}, Zip: {session.GetString("zip")}");

            sb.AppendLine("---------- Billing Address ---------");
            sb.AppendLine($"Address: {session.GetString("billing_addr_1")} {session.GetString("billing_addr_2")}");
            sb.AppendLine($"City: {session.GetString("billing_city")}, State: {session.GetString("billing_state")}, Zip: {session.GetString("billing_zip")}");

            sb.AppendLine("---------- Employment Address ---------");
            sb.AppendLine($"Name: {session.GetString("employment_name")}");
            sb.AppendLine($"Address: {session.GetString("employment_addr_1")} {session.GetString("employment_addr_2")}");
            sb.AppendLine($"City: {session.GetString("employment_city")}, State: {session.GetString("employment_state")}, Zip: {session.GetString("employment_zip")}");

            // Payment Type from class variable
            string paymentType = strTransCode switch
            {
                "C" => "Credit Card",
                "A" => "ACH",
                "K" => "Electronic Check",
                _ => "Unknown"
            };

            sb.AppendLine($"Payment Type = {paymentType}");

            sb.AppendLine("----------- CC Payment Info ----------");
            sb.AppendLine($"CC Type = {strCCType}");
            sb.AppendLine($"CC Number = {strCCAccountNoShow}");
            sb.AppendLine($"Expiration Date = {strExpires}");

            sb.AppendLine("----------- ACH Payment Info ----------");
            sb.AppendLine($"Acct Holder Name = {session.GetString("bank_account_name")}");
            sb.AppendLine($"Acct Number = {session.GetString("bank_account_number")}");
            sb.AppendLine($"Acct Type = {session.GetString("bank_acct_type")}");
            sb.AppendLine($"ABA Routing Number = {session.GetString("ABA")}");

            sb.AppendLine("----------- Check Payment Info ----------");
            sb.AppendLine($"Acct Holder Name = {session.GetString("checking_account_name")}");
            sb.AppendLine($"DL Number = {session.GetString("drivers_license_number")}");
            sb.AppendLine($"Check Number = {session.GetString("check_number")}");
            sb.AppendLine($"MICR = {session.GetString("micr")}");

            sb.AppendLine("----------- Transaction Info ------------");
            sb.AppendLine($"PNREF = {strPNREF}");
            sb.AppendLine($"Auth Code = {strAuthCode}");
            sb.AppendLine($"Response Msg = {strResponseMsg} {strErrorMessage}");
            sb.AppendLine($"Amount Paid = {session.GetString("amount_paid")}");

            string emailBody = sb.ToString();

            // Load mail settings from appsettings.json
            string strMailTo = _config["Email:To_Bwr_PayOnline"];
            string strMailCc = _config["Email:CC_Bwr_PayOnline"];
            string strMailBcc = _config["Email:Bcc_Bwr_PayOnline"];
            string strMailFrom = _config["Email:From_Bwr_PayOnline"];

            // Configure SMTP
            using (var smtp = new SmtpClient(_config["Smtp:Host"], int.Parse(_config["Smtp:Port"])))
            {
                smtp.Credentials = new NetworkCredential(
                    _config["Smtp:Username"],
                    _config["Smtp:Password"]);
                smtp.EnableSsl = true;

                var mail = new MailMessage
                {
                    From = new MailAddress(strMailFrom),
                    Subject = emailSubject,
                    Body = emailBody,
                    IsBodyHtml = false
                };

                mail.To.Add(strMailTo);
                if (!string.IsNullOrEmpty(strMailCc)) mail.CC.Add(strMailCc);
                if (!string.IsNullOrEmpty(strMailBcc)) mail.Bcc.Add(strMailBcc);

                await smtp.SendMailAsync(mail);
            }
        }

        /// <summary>
        /// Clear all the session variables
        /// </summary>
        /// <param name="p">Page object corresponding to the page calling this function.  This is required to access session variable values</param>
        public void ClearPaymentSessionVariables()
        {
            var session = _httpContextAccessor.HttpContext.Session;

            session.SetString("billing_addr_1", string.Empty);
            session.SetString("billing_addr_2", string.Empty);
            session.SetString("billing_city", string.Empty);
            session.SetString("billing_state", string.Empty);
            session.SetString("billing_ZIP", string.Empty);
            session.SetString("billing_country", string.Empty);
            session.SetString("payment_method", string.Empty);
            session.SetString("action", string.Empty);
            session.SetString("bank_account_name", string.Empty);
            session.SetString("bank_account_number", string.Empty);
            session.SetString("bank_acct_type", string.Empty);
            session.SetString("ABA", string.Empty);
            session.SetString("credit_card", string.Empty);
            session.SetString("card_number", string.Empty);
            session.SetString("name_on_card", string.Empty);
            session.SetString("expires_month", string.Empty);
            session.SetString("expires_year", string.Empty);
            session.SetString("checking_account_name", string.Empty);
            session.SetString("drivers_license_number", string.Empty);
            session.SetString("check_number", string.Empty);
            session.SetString("micr", string.Empty);
            session.SetString("amount_paid", string.Empty);
            session.SetString("submit", string.Empty);
        }


        #region DLW_Tax_AmnestyPaymentProcessing
        /* New Function added for Delaware Tax Amnesty Program Payment processing.
		 */

        String tpid;

        public async Task DLW_MakePaymentAsync(HttpContext context)
        {
            var session = _httpContextAccessor.HttpContext.Session;

            // Example: you’d load strTransCode/strAmount from session or method params
            strTransCode = session.GetString("trans_code");
            strAmount = session.GetString("amount_paid");

            InitializeVariables(context);

            if (strTransCode == "C") // credit card payment
            {
                // Credit card disabled
                session.SetString("result_message",
                    "<font size=\"medium\" color=\"red\"><strong>This option is temporarily unavailable. " +
                    "To make a payment please call your GRC Collections Representative or hit the back button " +
                    "to make a payment via an \"Electronic Fund or ACH Transfer\". Our apologies for the inconvenience.</strong></font>");
                return;
            }
            else // ACH payment
            {
                strResultMessage =
                    "Thank you!<br><br>Following is a description of your transaction. " +
                    "Please keep a copy of it for your records.<br><br>" +
                    $"Amount Paid: ${strAmount}<br>";
                strSubject = "ACH or Check";
                strPNREF = "None";
            }

            // Delaware Taxpayer ID
            tpid = session.GetString("tpid");

            // Save transaction & email receipt
            await DLW_SaveTransactionInfoAsync();
            await DLW_EmailTransactionAsync();

            // Clear sensitive data
            ClearPaymentSessionVariables();

            session.SetString("result_message", strResultMessage);
        }

        public async Task DLW_SaveTransactionInfoAsync()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            string connectionString = _config.GetConnectionString("DefaultDataSource");

            using var cnSaveInfo = new SqlConnection(connectionString);

            try
            {
                await cnSaveInfo.OpenAsync();

                string sql = @"
                INSERT INTO t_DLW_tax_amnesty_online_payments
                (tpid, first_name, last_name, email, account_number,
                 ssn, home_phone, work_phone, home_addr1, home_addr2, home_city, home_state, home_zip,
                 bill_addr1, bill_addr2, bill_city, bill_state, bill_zip,
                 emp_addr1, emp_addr2, emp_city, emp_state, emp_zip,
                 ach_acctholdername, ach_acctNumber, ach_acctType, ach_abaNumber,
                 check_acctholdername, check_dlnumber, check_checkNumber, check_micrNumber,
                 pnref, response_msg, amount_paid, return_code, auth_code)
                VALUES
                (@tpid, @first_name, @last_name, @email, @account_number,
                 @ssn, @home_phone, @work_phone, @home_addr1, @home_addr2, @home_city, @home_state, @home_zip,
                 @bill_addr1, @bill_addr2, @bill_city, @bill_state, @bill_zip,
                 @emp_addr1, @emp_addr2, @emp_city, @emp_state, @emp_zip,
                 @ach_acctholdername, @ach_acctNumber, @ach_acctType, @ach_abaNumber,
                 @check_acctholdername, @check_dlnumber, @check_checkNumber, @check_micrNumber,
                 @pnref, @response_msg, @amount_paid, @return_code, @auth_code)";

                using var cmd = new SqlCommand(sql, cnSaveInfo);

                // Add parameters safely
                cmd.Parameters.AddWithValue("@tpid", session.GetString("tpid") ?? "");
                cmd.Parameters.AddWithValue("@first_name", session.GetString("first_name") ?? "");
                cmd.Parameters.AddWithValue("@last_name", session.GetString("last_name") ?? "");
                cmd.Parameters.AddWithValue("@email", session.GetString("email") ?? "");
                cmd.Parameters.AddWithValue("@account_number", session.GetString("account_number") ?? "");
                cmd.Parameters.AddWithValue("@ssn", session.GetString("social_security_number") ?? "");
                cmd.Parameters.AddWithValue("@home_phone", session.GetString("home_phone") ?? "");
                cmd.Parameters.AddWithValue("@work_phone", session.GetString("work_phone") ?? "");
                cmd.Parameters.AddWithValue("@home_addr1", session.GetString("addr_1") ?? "");
                cmd.Parameters.AddWithValue("@home_addr2", session.GetString("addr_2") ?? "");
                cmd.Parameters.AddWithValue("@home_city", session.GetString("city") ?? "");
                cmd.Parameters.AddWithValue("@home_state", session.GetString("state") ?? "");
                cmd.Parameters.AddWithValue("@home_zip", session.GetString("zip") ?? "");
                cmd.Parameters.AddWithValue("@bill_addr1", session.GetString("billing_addr_1") ?? "");
                cmd.Parameters.AddWithValue("@bill_addr2", session.GetString("billing_addr_2") ?? "");
                cmd.Parameters.AddWithValue("@bill_city", session.GetString("billing_city") ?? "");
                cmd.Parameters.AddWithValue("@bill_state", session.GetString("billing_state") ?? "");
                cmd.Parameters.AddWithValue("@bill_zip", session.GetString("billing_zip") ?? "");
                cmd.Parameters.AddWithValue("@emp_addr1", session.GetString("employment_addr_1") ?? "");
                cmd.Parameters.AddWithValue("@emp_addr2", session.GetString("employment_addr_2") ?? "");
                cmd.Parameters.AddWithValue("@emp_city", session.GetString("employment_city") ?? "");
                cmd.Parameters.AddWithValue("@emp_state", session.GetString("employment_state") ?? "");
                cmd.Parameters.AddWithValue("@emp_zip", session.GetString("employment_zip") ?? "");
                cmd.Parameters.AddWithValue("@ach_acctholdername", session.GetString("bank_account_name") ?? "");
                cmd.Parameters.AddWithValue("@ach_acctNumber", session.GetString("bank_account_number") ?? "");
                cmd.Parameters.AddWithValue("@ach_acctType", session.GetString("bank_acct_type") ?? "");
                cmd.Parameters.AddWithValue("@ach_abaNumber", session.GetString("ABA") ?? "");
                cmd.Parameters.AddWithValue("@check_acctholdername", session.GetString("checking_account_name") ?? "");
                cmd.Parameters.AddWithValue("@check_dlnumber", session.GetString("drivers_license_number") ?? "");
                cmd.Parameters.AddWithValue("@check_checkNumber", session.GetString("check_number") ?? "");
                cmd.Parameters.AddWithValue("@check_micrNumber", session.GetString("micr") ?? "");
                cmd.Parameters.AddWithValue("@pnref", strPNREF ?? "");
                cmd.Parameters.AddWithValue("@response_msg", strResponseMsg ?? "");
                cmd.Parameters.AddWithValue("@amount_paid", decimal.TryParse(session.GetString("amount_paid"), out var amt) ? amt : 0);
                cmd.Parameters.AddWithValue("@return_code", intResultCode);
                cmd.Parameters.AddWithValue("@auth_code", strAuthCode ?? "");

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception e)
            {
                strErrorMessage += "Error: " + e.Message + "<br>";
            }
        }

        /// <summary>
        /// Email the online payment details for entry into CMS about the online payment
        /// </summary>
        /// <param name="p">Page object corresponding to the page calling this function.  This is required to access session variable values</param>
        public async Task DLW_EmailTransactionAsync()
        {
            var session = _httpContextAccessor.HttpContext.Session;

            var sb = new StringBuilder();
            string strSubject = "ACH or Check";
            string emailSubject = $"Delaware Online Payment Received - {strSubject}";

            sb.AppendLine("A payment attempt was just made online\n");
            sb.AppendLine($"Name: {session.GetString("first_name")} {session.GetString("last_name")}");
            sb.AppendLine($"Email: {session.GetString("email")}");
            sb.AppendLine($"Acct Number: {session.GetString("account_number")}");
            sb.AppendLine($"TPID: {session.GetString("tpid")}");
            sb.AppendLine($"SSN: {session.GetString("social_security_number")}");
            sb.AppendLine($"Home Phone: {session.GetString("home_phone")}");
            sb.AppendLine($"Work Phone: {session.GetString("work_phone")}");
            sb.AppendLine($"Address: {session.GetString("addr_1")} {session.GetString("addr_2")}");
            sb.AppendLine($"City: {session.GetString("city")}, State: {session.GetString("state")}, Zip: {session.GetString("zip")}");
            sb.AppendLine("---------- Billing Address ---------");
            sb.AppendLine($"{session.GetString("billing_addr_1")} {session.GetString("billing_addr_2")}");
            sb.AppendLine($"{session.GetString("billing_city")}, {session.GetString("billing_state")} {session.GetString("billing_zip")}");
            sb.AppendLine("---------- Employment Address ---------");
            sb.AppendLine($"Name: {session.GetString("employment_name")}");
            sb.AppendLine($"{session.GetString("employment_addr_1")} {session.GetString("employment_addr_2")}");
            sb.AppendLine($"{session.GetString("employment_city")}, {session.GetString("employment_state")} {session.GetString("employment_zip")}");

            // Payment type
            strTransactionType = strTransCode switch
            {
                "C" => "Credit Card",
                "A" => "ACH",
                "K" => "Electronic Check",
                _ => "Unknown"
            };

            sb.AppendLine($"Payment Type = {strTransactionType}");
            sb.AppendLine("----------- CC Payment Info ----------");
            sb.AppendLine($"CC Type = {strCCType}");
            sb.AppendLine($"CC Number = {strCCAccountNoShow}");
            sb.AppendLine($"Expiration Date = {strExpires}");
            sb.AppendLine("----------- ACH Payment Info ----------");
            sb.AppendLine($"Acct Holder Name = {session.GetString("bank_account_name")}");
            sb.AppendLine($"Acct Number = {session.GetString("bank_account_number")}");
            sb.AppendLine($"Acct Type = {session.GetString("bank_acct_type")}");
            sb.AppendLine($"ABA Routing Number = {session.GetString("ABA")}");
            sb.AppendLine("----------- Check Payment Info ----------");
            sb.AppendLine($"Acct Holder Name = {session.GetString("checking_account_name")}");
            sb.AppendLine($"DL Number = {session.GetString("drivers_license_number")}");
            sb.AppendLine($"Check Number = {session.GetString("check_number")}");
            sb.AppendLine($"MICR = {session.GetString("micr")}");
            sb.AppendLine("----------- Transaction Info ------------");
            sb.AppendLine($"PNREF = {strPNREF}");
            sb.AppendLine($"Auth Code = {strAuthCode}");
            sb.AppendLine($"Response Msg = {strResponseMsg} {strErrorMessage}");
            sb.AppendLine($"Amount Paid = {session.GetString("amount_paid")}");

            string emailBody = sb.ToString();

            // Read email config from appsettings.json
            string strMailTo = _config["Email:To_Bwr_PayOnline_DLW"];
            string strMailCc = _config["Email:CC_Bwr_PayOnline_DLW"];
            string strMailBcc = _config["Email:Bcc_Bwr_PayOnline_DLW"];
            string strMailFrom = _config["Email:From_Bwr_PayOnline_DLW"];

            // Configure SMTP
            using var smtp = new SmtpClient(_config["Smtp:Host"], int.Parse(_config["Smtp:Port"]))
            {
                Credentials = new NetworkCredential(
                    _config["Smtp:Username"],
                    _config["Smtp:Password"]),
                EnableSsl = true
            };

            var mail = new MailMessage
            {
                From = new MailAddress(strMailFrom),
                Subject = emailSubject,
                Body = emailBody,
                IsBodyHtml = false
            };

            mail.To.Add(strMailTo);
            if (!string.IsNullOrEmpty(strMailCc)) mail.CC.Add(strMailCc);
            if (!string.IsNullOrEmpty(strMailBcc)) mail.Bcc.Add(strMailBcc);

            await smtp.SendMailAsync(mail);
        }


        /// <summary>
        /// Saves DemographicsInfo
        /// ///</summary>
        /// <param name="p">Page object corresponding to the page calling this function.  This is required to access session variable values</param>
        public void DLW_SaveDemographicsInfo()
        {
            var session = _httpContextAccessor.HttpContext.Session;

            string connectionString = _config.GetConnectionString("DefaultDataSource");

            // ✅ Use parameterized query instead of concatenation (avoid SQL Injection)
            string strSQL = @"
            INSERT INTO t_DLW_tax_amnesty_online_payments
            (tpid, first_name, last_name, email, account_number,
             ssn, home_phone, work_phone, home_addr1, home_addr2, home_city, home_state, home_zip,
             emp_addr1, emp_addr2, emp_city, emp_state, emp_zip, reference_id, emp_name)
            VALUES (@tpid, @first_name, @last_name, @email, @account_number,
                    @ssn, @home_phone, @work_phone, @home_addr1, @home_addr2, @home_city, @home_state, @home_zip,
                    @emp_addr1, @emp_addr2, @emp_city, @emp_state, @emp_zip, @reference_id, @emp_name)";

            try
            {
                using (SqlConnection cnSaveInfo = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(strSQL, cnSaveInfo))
                {
                    cmd.Parameters.AddWithValue("@tpid", session.GetString("tpid") ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@first_name", session.GetString("first_name") ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@last_name", session.GetString("last_name") ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@email", session.GetString("email") ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@account_number", session.GetString("account_number") ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@ssn", session.GetString("social_security_number") ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@home_phone", session.GetString("home_phone") ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@work_phone", session.GetString("work_phone") ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@home_addr1", session.GetString("addr_1") ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@home_addr2", session.GetString("addr_2") ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@home_city", session.GetString("city") ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@home_state", session.GetString("state") ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@home_zip", session.GetString("zip") ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@emp_addr1", session.GetString("employment_addr_1") ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@emp_addr2", session.GetString("employment_addr_2") ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@emp_city", session.GetString("employment_city") ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@emp_state", session.GetString("employment_state") ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@emp_zip", session.GetString("employment_zip") ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@reference_id", session.GetString("ReferenceID") ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@emp_name", session.GetString("employment_name") ?? (object)DBNull.Value);

                    cnSaveInfo.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                // Log error instead of string concatenation
                Console.WriteLine("Error while saving demographics info: " + ex.Message);
            }
        }


        /// <summary>
        /// Saves ACH Payment Info
        /// ///</summary>
        /// <param name="p">Page object corresponding to the page calling this function.  This is required to access session variable values</param>
        public void DLW_SavePaymentInfo()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            string connectionString = _config.GetConnectionString("DefaultDataSource");

            string strSQL = @"
            UPDATE t_DLW_tax_amnesty_online_payments
            SET ach_acctholdername = @acctHolderName,
                ach_acctNumber = @acctNumber,
                ach_acctType = @acctType,
                ach_abaNumber = @abaNumber,
                amount_paid = @amountPaid,
                transaction_date = @transactionDate
            WHERE reference_id = @referenceId";

            try
            {
                using (SqlConnection cnSaveInfo = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(strSQL, cnSaveInfo))
                {
                    cmd.Parameters.AddWithValue("@acctHolderName", session.GetString("bank_account_name") ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@acctNumber", session.GetString("bank_account_number") ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@acctType", session.GetString("bank_acct_type") ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@abaNumber", session.GetString("ABA") ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@amountPaid", session.GetString("amount_paid") ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@transactionDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@referenceId", session.GetString("ReferenceID") ?? (object)DBNull.Value);

                    cnSaveInfo.Open();
                    cmd.ExecuteNonQuery();
                }

                // After saving → send confirmation email
                DLW_EmailTransactionAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in DLW_SavePaymentInfo: " + ex.Message);
            }
        }


        #endregion

    }
}
