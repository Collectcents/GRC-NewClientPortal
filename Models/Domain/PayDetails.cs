using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace GRC_NewClientPortal.Models.Domain
{
    public class PayDetails
    {
        private readonly string _connectionString;

        public PayDetails(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultDataSource");
        }
        /// <summary>
		/// Class variable to hold the Scheduled amount
		/// </summary>
		private decimal _sched_amt;
        /// <summary>
        /// Class variable to hold the Current due date
        /// </summary>
        private DateTime _cur_due_date;
        /// <summary>
        /// Class variable to hold the Current amount due
        /// </summary>
        private decimal _cur_amt_due;
        /// <summary>
        /// Class variable to hold the Amount last paid
        /// </summary>
        private decimal _amt_last_paid;
        /// <summary>
        /// Class variable to hold the Date last paid
        /// </summary>
        private DateTime _date_last_paid;
        /// <summary>
        /// Class variable to hold the total paid
        /// </summary>
        private decimal _total_paid;

        /// <summary>
        /// Returns decimal value corresponding to the scheduled amount
        /// </summary>
        public decimal scheduled_amt
        {
            get { return _sched_amt; }
        }

        /// <summary>
        /// Returns DateTime object corresponding to the current due date
        /// </summary>
        public DateTime current_due_date
        {
            get { return _cur_due_date; }
        }

        /// <summary>
        /// Returns decimal value corresponding to the current amount due
        /// </summary>
        public decimal current_amt_due
        {
            get { return _cur_amt_due; }
        }

        /// <summary>
        /// Returns decimal value corresponding to the amount last paid
        /// </summary>
        public decimal amt_last_paid
        {
            get { return _amt_last_paid; }
        }

        /// <summary>
        /// Returns DateTime object corresponding to the date last paid
        /// </summary>
        public DateTime date_last_paid
        {
            get { return _date_last_paid; }
        }

        /// <summary>
        /// Returns decimal value corresponding to the total amount paid
        /// </summary>
        public decimal total_paid
        {
            get { return _total_paid; }
            //@@@20050228ssm Added the set property to support the BR in payment_history.asp page on the client side code.
            set { _total_paid = value; }
        }
        /// <summary>
		/// Calls SetPayDetails to load the Payment details corresponding to the bedp number supplied
		/// </summary>
		/// <param name="bedp">BEDP number</param>
		public PayDetails(string bedp)
        {
            SetPayDetails(bedp);
        }

        /// <summary>
        /// Loads the Payment details corresponding to the bedp number supplied
        /// </summary>
        /// <param name="bedp">BEDP number</param>
        private void SetPayDetails(string bedp)
        {
            using (SqlConnection strConn = new SqlConnection(_connectionString))
            {
                SqlDataAdapter daPayDetails = new SqlDataAdapter(
                    "exec dbo.p_bwr_pay_arrangements @bedp", strConn);

                daPayDetails.SelectCommand.Parameters.AddWithValue("@bedp", bedp);

                DataSet dsPayDetails = new DataSet();
                daPayDetails.Fill(dsPayDetails);

                if (dsPayDetails.Tables.Count > 0 && dsPayDetails.Tables[0].Rows.Count > 0)
                {
                    DataRow drPayDetails = dsPayDetails.Tables[0].Rows[0];
                    _sched_amt = Convert.ToDecimal(drPayDetails["ln_sched_amt"]);

                    if (drPayDetails["ln_cur_due_date"] != DBNull.Value)
                        _cur_due_date = Convert.ToDateTime(drPayDetails["ln_cur_due_date"]);

                    _cur_amt_due = Convert.ToDecimal(drPayDetails["ln_cur_amt_due"]);
                    _amt_last_paid = Convert.ToDecimal(drPayDetails["ln_amt_last_paid"]);

                    if (drPayDetails["ln_date_last_paid"] != DBNull.Value)
                        _date_last_paid = Convert.ToDateTime(drPayDetails["ln_date_last_paid"]);

                    _total_paid = Convert.ToDecimal(drPayDetails["ln_total_paid"]);
                }
            }
        }

        /// <summary>
        /// Returns payment details in html format
        /// </summary>
        /// <returns>String in html format containing the payment details for the borrower</returns>
        public string htmlPayDetails()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<table cellspacing=0 cellpadding=0 width=500 border=0>\n\t");
            sb.Append("<tr>\n\t\t<td colspan=2 bgcolor=#e2e2e2></td>\n\t</tr>\n\t");
            sb.Append("<tr>\n\t\t<td width=50% >Scheduled Amount:</td>\n\t\t<td width=50% align=right>" + _sched_amt.ToString("C") + "</td>\n\t</tr>\n\t");
            sb.Append("<tr>\n\t\t<td colspan=2 bgcolor=#e2e2e2></td>\n\t</tr>\n\t");
            sb.Append("<tr>\n\t\t<td width=50% >Current Due Date:</td>\n\t\t<td width=50% align=right>");
            //@@@20050216ssm If dates are not set then don't display the default value.
            if (_cur_due_date == Convert.ToDateTime("1/1/1900"))
                sb.Append("</td>\n\t</tr>\n\t");
            else
                sb.Append(_cur_due_date.ToShortDateString() + "</td>\n\t</tr>\n\t");
            sb.Append("<tr>\n\t\t<td colspan=2 bgcolor=#e2e2e2></td>\n\t</tr>\n\t");
            sb.Append("<tr>\n\t\t<td width=50% >Current Amount Due:</td>\n\t\t<td width=50% align=right>" + _cur_amt_due.ToString("C") + "</td>\n\t\t</tr>\n\t");
            sb.Append("<tr>\n\t\t<td colspan=2 bgcolor=#e2e2e2></td>\n\t</tr>\n\t");
            sb.Append("<tr>\n\t\t<td width=50% >Amount Last Paid:</td>\n\t\t<td width=50% align=right>" + _amt_last_paid.ToString("C") + "</td>\n\t\t</tr>\n\t");
            sb.Append("<tr>\n\t\t<td colspan=2 bgcolor=#e2e2e2></td>\n\t</tr>\n\t");
            sb.Append("<tr>\n\t\t<td width=50% >Date Last Paid:</td>\n\t\t<td width=50% align=right>");
            //@@@20050216ssm If dates are not set then dont display the default value.
            if (_date_last_paid == Convert.ToDateTime("1/1/1900"))
                sb.Append("</td>\n\t</tr>\n\t");
            else
                sb.Append(_date_last_paid.ToShortDateString() + "</td>\n\t</tr>\n\t");

            sb.Append("<tr>\n\t\t<td colspan=2 bgcolor=#e2e2e2></td>\n\t</tr>\n\t");
            sb.Append("<tr>\n\t\t<td width=50%><span class=lightBlueHeading>Total Paid</span></td>\n\t\t<td width=50% align=right><span class=lightBlueHeading>" + _total_paid.ToString("C") + "</span></td>\n\t</tr>\n\t");
            sb.Append("<tr>\n\t\t<td colspan=2 bgcolor=#e2e2e2></td>\n\t</tr>\n</table>");

            return sb.ToString();
        }
    }
}
