using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Text;

namespace GRC_NewClientPortal.Models.Domain
{
    public class Account
    {
        /// <summary>
		/// Class variable to hold the client name
		/// </summary>
		private string _client_name;
        /// <summary>
        /// Class variable to hold the loan type
        /// </summary>
        private string _loan_type;
        /// <summary>
        /// Class variable to hold amount placed
        /// </summary>
        private decimal _amount_placed;
        /// <summary>
        /// Class variable to hold principal balance
        /// </summary>
        private decimal _principal_balance;
        /// <summary>
        /// Class variable to hold principal past due
        /// </summary>
        private decimal _principal_past_due;
        /// <summary>
        /// Class variable to hold interest past due
        /// </summary>
        private decimal _interest_past_due;
        /// <summary>
        /// Class variable to hold interest rate
        /// </summary>
        private decimal _interest_rate;
        /// <summary>
        /// Class variable to hold total past due
        /// </summary>
        private decimal _total_past_due;
        /// <summary>
        /// Class variable to hold amount to collect
        /// </summary>
        private decimal _amount_to_collect;

        /// <summary>
        /// Returns the client name from the Account object
        /// </summary>
        public string client_name
        {
            get { return _client_name; }
        }

        /// <summary>
        /// Returns the loan type from the Account object
        /// </summary>
        public string loan_type
        {
            get { return _loan_type; }
        }

        /// <summary>
        /// Returns the amount placed from the Account object
        /// </summary>
        public decimal amount_placed
        {
            get { return _amount_placed; }
        }

        /// <summary>
        /// Returns the principal balance from the Account object
        /// </summary>
        public decimal principal_balance
        {
            get { return _principal_balance; }
        }

        /// <summary>
        /// Returns the principal past due from the Account object
        /// </summary>
        public decimal principal_past_due
        {
            get { return _principal_past_due; }
        }

        /// <summary>
        /// Returns the interest past due from the Account object
        /// </summary>
        public decimal interest_past_due
        {
            get { return _interest_past_due; }
        }

        /// <summary>
        /// Returns the interest rate from the Account object
        /// </summary>
        public decimal interest_rate
        {
            get { return _interest_rate; }
        }

        /// <summary>
        /// Returns the total past due from the Account object
        /// </summary>
        public decimal total_past_due
        {
            get { return _total_past_due; }
        }

        /// <summary>
        /// Returns the amount to collect from the Account object
        /// </summary>
        public decimal amount_to_collect
        {
            get { return _amount_to_collect; }
        }
        /// <summary>
		/// Constructor for the Account object
		/// </summary>
		/// <param name="ledp">Loan edp number</param>
        private readonly string _connectionString;
        public Account(string ledp, IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultDataSource");
            _client_name = "";
            _loan_type = "";
            _amount_placed = 0;
            _principal_balance = 0;
            _principal_past_due = 0;
            _interest_past_due = 0;
            _interest_rate = 0;
            _total_past_due = 0;
            _amount_to_collect = 0;
            SetLoanDetails(ledp);
        }

        /// <summary>
        /// Initializes the data for the account object
        /// </summary>
        /// <param name="ledp">Loan edp number</param>
        private void SetLoanDetails(string ledp)
        {
            using (SqlConnection strConn = new SqlConnection(_connectionString))
            using (SqlDataAdapter daAcct = new SqlDataAdapter("exec dbo.p_bwr_loan_details @Ledp", strConn))
            {
                daAcct.SelectCommand.Parameters.AddWithValue("@Ledp", ledp);

                DataSet dsAcct = new DataSet();
                daAcct.Fill(dsAcct);

                if (dsAcct.Tables.Count > 0 && dsAcct.Tables[0].Rows.Count > 0)
                {
                    DataRow drAcct = dsAcct.Tables[0].Rows[0];

                    _client_name = Convert.ToString(drAcct["ln_client_name"]);
                    _loan_type = Convert.ToString(drAcct["ln_dtyp"]);
                    _amount_placed = Convert.ToDecimal(drAcct["ln_camt"]);
                    _principal_balance = Convert.ToDecimal(drAcct["ln_cpbl"]);
                    _principal_past_due = Convert.ToDecimal(drAcct["ln_pbal"]);
                    _interest_past_due = Convert.ToDecimal(drAcct["ln_idue"]);
                    _interest_rate = Convert.ToDecimal(drAcct["ln_irat"]);
                    _total_past_due = Convert.ToDecimal(drAcct["ln_tdue"]);
                    _amount_to_collect = Convert.ToDecimal(drAcct["ln_cola"]);
                }
            }
        }

        /// <summary>
        /// Returns the account object details in html format
        /// </summary>
        /// <returns>String in html format to be used for writing out the account object data</returns>
        public string htmlDetails()
        {
            StringBuilder sb = new StringBuilder();
            // fill with html and formatting for each type
            // Note: all decimal types should be formatted as currency except
            //       for the interest rate which is formatted in percent.

            //@@@20050324ssm As per the decision show only the Amount due
            //			sb.Append("<table cellspacing=0 cellpadding=0 width=100% border=0>");
            //			sb.Append("<tr><td colspan=2 width=75%><b>"+_client_name+"</b></td></tr>");
            //			sb.Append("<tr><td colspan=2 bgcolor=#e2e2e2></td></tr>");
            //			sb.Append("<tr><td colspan=2 width=75% >"+_loan_type+"</td></tr>");
            //			sb.Append("<tr><td colspan=2 bgcolor=#e2e2e2></td></tr>");
            ////@@@20050225ssm Amount placed to be removed as per the decision made during the meeting on 2/24/05
            ////			sb.Append("<tr><td width=75% >Amount Placed:</td><td width=25% align=right>"+_amount_placed.ToString("C")+"</td></tr>");
            ////			sb.Append("<tr><td colspan=2 bgcolor=#e2e2e2></td></tr>");
            ////@@@20050225ssm end
            //			sb.Append("<tr><td width=75% >Principal Balance:</td><td width=25% align=right>"+_principal_balance.ToString("C")+"</td></tr>");
            //			sb.Append("<tr><td colspan=2 bgcolor=#e2e2e2></td></tr>");
            //			sb.Append("<tr><td width=75% >Principal Past Due:</td><td width=25% align=right>"+_principal_past_due.ToString("C")+"</td></tr>");
            //			sb.Append("<tr><td colspan=2 bgcolor=#e2e2e2></td></tr>");
            //			sb.Append("<tr><td width=75% >Interest Past Due:</td><td width=25% align=right>"+_interest_past_due.ToString("C")+"</td></tr>");
            //			sb.Append("<tr><td colspan=2 bgcolor=#e2e2e2></td></tr>");
            //			//@@@20050216ssm Interest rate must be divided by 100 before formatting as percentage
            //			_interest_rate/=100;
            //			sb.Append("<tr><td width=75% >Interest Rate:</td><td width=25% align=right>"+_interest_rate.ToString("P")+"</td></tr>");
            //			sb.Append("<tr><td colspan=2 bgcolor=#e2e2e2></td></tr>");
            //			sb.Append("<tr><td width=75%><span class=lightBlueHeading>Total Past Due:</span></td><td width=25% align=right><span class=lightBlueHeading>"+_total_past_due.ToString("C")+"</span></td></tr>");
            //			sb.Append("<tr><td colspan=2 bgcolor=#e2e2e2></td>");
            //			sb.Append("</tr><td width=75% >Amount to Collect:</td><td width=25% align=right>"+_amount_to_collect.ToString("C")+"</td></tr>");
            //			sb.Append("<tr><td colspan=2 bgcolor=#e2e2e2></td></tr></table>");
            sb.Append("<table cellspacing=0 cellpadding=0 width=100% border=0>");
            sb.Append("</tr><td width=75% ><span class=lightBlueHeading>*Amount:</span></td><td width=25% align=right><span class=lightBlueHeading>" + _amount_to_collect.ToString("C") + "</span></td></tr>");
            sb.Append("<tr><td colspan=2 bgcolor=#e2e2e2></td>");
            sb.Append("</tr><td>* This amount is an estimate.  Please contact GRC for your actual balance.</td></tr>");
            sb.Append("<tr><td colspan=2 bgcolor=#e2e2e2></td></tr></table>");
            //@@@20050324ssm end																	
            return sb.ToString();
        }
    }
}
