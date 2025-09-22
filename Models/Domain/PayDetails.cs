using System.Data;
using System.Text;

namespace GRC_NewClientPortal.Models.Domain
{
    public class PayDetails
    {
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
    }
}
