namespace GRC_NewClientPortal.Models.Domain
{
    public class Demographics
    {
        /// <summary>
		/// Class variable to hold the Formatted social security number
		/// </summary>
		private string _ssn_fmt;
        /// <summary>
        /// Class variable to hold the Last Name of the borrower
        /// </summary>
        private string _last_name;
        /// <summary>
        /// Class variable to hold the First name of the borrower
        /// </summary>
        private string _first_name;
        /// <summary>
        /// Class variable to hold the Full Name of the borrower
        /// </summary>
        private string _full_name;
        /// <summary>
        /// Class variable to hold the Address line 1
        /// </summary>
        private string _addr1;
        /// <summary>
        /// Class variable to hold the Address line 2
        /// </summary>
        private string _addr2;
        /// <summary>
        /// Class variable to hold the City
        /// </summary>
        private string _city;
        /// <summary>
        /// Class variable to hold the State
        /// </summary>
        private string _state;
        /// <summary>
        /// Class variable to hold the Zip code
        /// </summary>
        private string _zip;
        /// <summary>
        /// Class variable to hold the Home Phone
        /// </summary>
        private string _home_phone;
        /// <summary>
        /// Class variable to hold the Employer Name
        /// </summary>
        private string _emp_name;
        /// <summary>
        /// Class variable to hold the Employer Address
        /// </summary>
        private string _emp_addr;
        /// <summary>
        /// Class variable to hold the Employer City
        /// </summary>
        private string _emp_city;
        /// <summary>
        /// Class variable to hold the Employer State
        /// </summary>
        private string _emp_state;
        /// <summary>
        /// Class variable to hold the Employer Zip
        /// </summary>
        private string _emp_zip;
        /// <summary>
        /// Class variable to hold the Employer Phone
        /// </summary>
        private string _emp_phone;

        /// <summary>
        /// Constructor for the demographics object.
        /// </summary>
        public Demographics()
        {
            _ssn_fmt = "";
            _last_name = "";
            _first_name = "";
            _full_name = "";
            _addr1 = "";
            _addr2 = "";
            _city = "";
            _state = "";
            _zip = "";
            _home_phone = "";
            _emp_name = "";
            _emp_addr = "";
            _emp_city = "";
            _emp_state = "";
            _emp_zip = "";
            _emp_phone = "";
        }

        /// <summary>
        /// Returns the formatted string representing the SSN of the borrower
        /// </summary>
        public string ssn_fmt
        {
            get { return _ssn_fmt; }
        }

        /// <summary>
        /// Returns the last name of the borrower
        /// </summary>
        public string last_name
        {
            get { return _last_name; }
        }

        /// <summary>
        /// Returns the first name of the borrower
        /// </summary>
        public string first_name
        {
            get { return _first_name; }
        }

        /// <summary>
        /// Returns the full name of the borrower
        /// </summary>
        public string full_name
        {
            get { return _full_name; }
        }

        /// <summary>
        /// Returns the Address line 1 of the borrower
        /// </summary>
        public string addr1
        {
            get { return _addr1; }
        }

        /// <summary>
        /// Returns the Address line 2 of the borrower
        /// </summary>
        public string addr2
        {
            get { return _addr2; }
        }

        /// <summary>
        /// Returns the city of the borrower
        /// </summary>
        public string city
        {
            get { return _city; }
        }

        /// <summary>
        /// Returns the state of the borrower
        /// </summary>
        public string state
        {
            get { return _state; }
        }

        /// <summary>
        /// Returns the zip code of the borrower
        /// </summary>
        public string zip
        {
            get { return _zip; }
        }

        /// <summary>
        /// Returns the home phone of the borrower
        /// </summary>
        public string home_phone
        {
            get { return _home_phone; }
        }

        /// <summary>
        /// Returns the employer name of the borrower
        /// </summary>
        public string emp_name
        {
            get { return _emp_name; }
        }

        /// <summary>
        /// Returns the employer address of the borrower
        /// </summary>
        public string emp_addr
        {
            get { return _emp_addr; }
        }

        /// <summary>
        /// Returns the employer city of the borrower
        /// </summary>
        public string emp_city
        {
            get { return _emp_city; }
        }

        /// <summary>
        /// Returns the employer state of the borrower
        /// </summary>
        public string emp_state
        {
            get { return _emp_state; }
        }

        /// <summary>
        /// Returns the employer zip of the borrower
        /// </summary>
        public string emp_zip
        {
            get { return _emp_zip; }
        }

        /// <summary>
        /// Returns the employer phone of the borrower
        /// </summary>
        public string emp_phone
        {
            get { return _emp_phone; }
        }
    }
}
