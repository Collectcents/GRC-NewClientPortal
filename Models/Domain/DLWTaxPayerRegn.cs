namespace GRC_NewClientPortal.Models.Domain
{
    public class DLWTaxPayerRegn
    {
        #region properties
        /// <summary>
        /// The private member referenced by the
        /// <see cref="reg_id" /> property.
        /// </summary>
        private int _reg_id;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="tpid" /> property.
        /// </summary>
        private string _tpid;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="ssn" /> property.
        /// </summary>
        private string _ssn;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="ssn" /> property.
        /// </summary>
        private string _ein;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="rev_code" /> property.
        /// </summary>
        private string _rev_code;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="tax_per_dte" /> property.
        /// </summary>
        private string _tax_per_dte;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="tax_type" /> property.
        /// </summary>
        private string _tax_type;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="tax_year" /> property.
        /// </summary>
        private string _tax_year;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="date_created" /> property.
        /// </summary>
        private DateTime _date_created;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="date_registered" /> property.
        /// </summary>
        private DateTime _date_registered;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="date_registeredSpecified" /> property.
        /// </summary>
        private bool _date_registeredSpecified;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="tpid_pri" /> property.
        /// </summary>
        private string _tpid_pri;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="tpid_sec" /> property.
        /// </summary>
        private string _tpid_sec;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="pri_sec" /> property.
        /// </summary>
        private string _pri_sec;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="tap_filing_status" /> property.
        /// </summary>
        private string _tap_filing_status;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="last_name" /> property.
        /// </summary>
        private string _last_name;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="first_name" /> property.
        /// </summary>
        private string _first_name;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="bus_name" /> property.
        /// </summary>
        private string _bus_name;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="addr_line_1" /> property.
        /// </summary>
        private string _addr_line_1;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="addr_line_2" /> property.
        /// </summary>
        private string _addr_line_2;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="city" /> property.
        /// </summary>
        private string _city;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="state" /> property.
        /// </summary>
        private string _state;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="zipcode" /> property.
        /// </summary>
        private string _zipcode;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="home_area_code" /> property.
        /// </summary>
        private string _home_area_code;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="home_phone_num" /> property.
        /// </summary>
        private string _home_phone_num;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="alt_area_code" /> property.
        /// </summary>
        private string _alt_area_code;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="alt_phone_num" /> property.
        /// </summary>
        private string _alt_phone_num;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="alt_phone_extn" /> property.
        /// </summary>
        private string _alt_phone_extn;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="taxpayer_found_on_facs" /> property.
        /// </summary>
        private string _taxpayer_found_on_facs;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="tax_liability_found_on_facs" /> property.
        /// </summary>
        private string _tax_liability_found_on_facs;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="registration_flag" /> property.
        /// </summary>
        private string _registration_flag;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="prot_claim_ind" /> property.
        /// </summary>
        private string _prot_claim_ind;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="prot_claim_amt" /> property.
        /// </summary>
        private string _prot_claim_amt;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="prot_claim_amtSpecified" /> property.
        /// </summary>
        private bool _prot_claim_amtSpecified;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="return_filing_status" /> property.
        /// </summary>
        private string _return_filing_status;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="pay_plan_status" /> property.
        /// </summary>
        private string _pay_plan_status;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="emp_id" /> property.
        /// </summary>
        private string _emp_id;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="email" /> property.
        /// </summary>
        private string _email;

        #endregion


        #region properties


        /// <summary>
        /// Sets or gets the <see cref="_reg_id" />
        /// value of the <see cref="GRC_WebDataSetT_DLW_tax_amnesty_registration" />
        /// object class.
        /// </summary>
        /// 
        public int reg_id
        {
            get
            {
                return _reg_id;
            }
            set
            {
                _reg_id = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="_tpid" />
        /// value of the <see cref="GRC_WebDataSetT_DLW_tax_amnesty_registration" />
        /// object class.
        /// </summary>
        public string tpid
        {
            get
            {
                return _tpid;
            }
            set
            {
                _tpid = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="_ssn" />
        /// value of the <see cref="GRC_WebDataSetT_DLW_tax_amnesty_registration" />
        /// object class.
        /// </summary>
        public string ssn
        {
            get
            {
                return _ssn;
            }
            set
            {
                _ssn = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="_ein" />
        /// value of the <see cref="GRC_WebDataSetT_DLW_tax_amnesty_registration" />
        /// object class.
        /// </summary>
        public string ein
        {
            get
            {
                return _ein;
            }
            set
            {
                _ein = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="_rev_code" />
        /// value of the <see cref="GRC_WebDataSetT_DLW_tax_amnesty_registration" />
        /// object class.
        /// </summary>
        public string rev_code
        {
            get
            {
                return _rev_code;
            }
            set
            {
                _rev_code = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="_tax_per_dte" />
        /// value of the <see cref="GRC_WebDataSetT_DLW_tax_amnesty_registration" />
        /// object class.
        /// </summary>
        public string tax_per_dte
        {
            get
            {
                return _tax_per_dte;
            }
            set
            {
                _tax_per_dte = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="_tax_type" />
        /// value of the <see cref="GRC_WebDataSetT_DLW_tax_amnesty_registration" />
        /// object class.
        /// </summary>
        public string tax_type
        {
            get
            {
                return _tax_type;
            }
            set
            {
                _tax_type = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="_tax_year" />
        /// value of the <see cref="GRC_WebDataSetT_DLW_tax_amnesty_registration" />
        /// object class.
        /// </summary>
        public string tax_year
        {
            get
            {
                return _tax_year;
            }
            set
            {
                _tax_year = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="_date_created" />
        /// value of the <see cref="GRC_WebDataSetT_DLW_tax_amnesty_registration" />
        /// object class.
        /// </summary>
        public DateTime date_created
        {
            get
            {
                return _date_created;
            }
            set
            {
                _date_created = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="_date_registered" />
        /// value of the <see cref="GRC_WebDataSetT_DLW_tax_amnesty_registration" />
        /// object class.
        /// </summary>
        public DateTime date_registered
        {
            get
            {
                return _date_registered;
            }
            set
            {
                _date_registered = value;
                date_registeredSpecified = true;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="_date_registeredSpecified" />
        /// value of the <see cref="GRC_WebDataSetT_DLW_tax_amnesty_registration" />
        /// object class.
        /// </summary>
        [System.Xml.Serialization.XmlIgnore()]
        public bool date_registeredSpecified
        {
            get
            {
                return _date_registeredSpecified;
            }
            set
            {
                _date_registeredSpecified = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="_tpid_pri" />
        /// value of the <see cref="GRC_WebDataSetT_DLW_tax_amnesty_registration" />
        /// object class.
        /// </summary>
        public string tpid_pri
        {
            get
            {
                return _tpid_pri;
            }
            set
            {
                _tpid_pri = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="_tpid_sec" />
        /// value of the <see cref="GRC_WebDataSetT_DLW_tax_amnesty_registration" />
        /// object class.
        /// </summary>
        public string tpid_sec
        {
            get
            {
                return _tpid_sec;
            }
            set
            {
                _tpid_sec = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="_pri_sec" />
        /// value of the <see cref="GRC_WebDataSetT_DLW_tax_amnesty_registration" />
        /// object class.
        /// </summary>
        public string pri_sec
        {
            get
            {
                return _pri_sec;
            }
            set
            {
                _pri_sec = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="_tap_filing_status" />
        /// value of the <see cref="GRC_WebDataSetT_DLW_tax_amnesty_registration" />
        /// object class.
        /// </summary>
        public string tap_filing_status
        {
            get
            {
                return _tap_filing_status;
            }
            set
            {
                _tap_filing_status = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="_last_name" />
        /// value of the <see cref="GRC_WebDataSetT_DLW_tax_amnesty_registration" />
        /// object class.
        /// </summary>
        public string last_name
        {
            get
            {
                return _last_name;
            }
            set
            {
                _last_name = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="_first_name" />
        /// value of the <see cref="GRC_WebDataSetT_DLW_tax_amnesty_registration" />
        /// object class.
        /// </summary>
        public string first_name
        {
            get
            {
                return _first_name;
            }
            set
            {
                _first_name = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="_bus_name" />
        /// value of the <see cref="GRC_WebDataSetT_DLW_tax_amnesty_registration" />
        /// object class.
        /// </summary>
        public string bus_name
        {
            get
            {
                return _bus_name;
            }
            set
            {
                _bus_name = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="_addr_line_1" />
        /// value of the <see cref="GRC_WebDataSetT_DLW_tax_amnesty_registration" />
        /// object class.
        /// </summary>
        public string addr_line_1
        {
            get
            {
                return _addr_line_1;
            }
            set
            {
                _addr_line_1 = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="_addr_line_2" />
        /// value of the <see cref="GRC_WebDataSetT_DLW_tax_amnesty_registration" />
        /// object class.
        /// </summary>
        public string addr_line_2
        {
            get
            {
                return _addr_line_2;
            }
            set
            {
                _addr_line_2 = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="_city" />
        /// value of the <see cref="GRC_WebDataSetT_DLW_tax_amnesty_registration" />
        /// object class.
        /// </summary>
        public string city
        {
            get
            {
                return _city;
            }
            set
            {
                _city = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="_state" />
        /// value of the <see cref="GRC_WebDataSetT_DLW_tax_amnesty_registration" />
        /// object class.
        /// </summary>
        public string state
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="_zipcode" />
        /// value of the <see cref="GRC_WebDataSetT_DLW_tax_amnesty_registration" />
        /// object class.
        /// </summary>
        public string zipcode
        {
            get
            {
                return _zipcode;
            }
            set
            {
                _zipcode = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="_home_area_code" />
        /// value of the <see cref="GRC_WebDataSetT_DLW_tax_amnesty_registration" />
        /// object class.
        /// </summary>
        public string home_area_code
        {
            get
            {
                return _home_area_code;
            }
            set
            {
                _home_area_code = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="_home_phone_num" />
        /// value of the <see cref="GRC_WebDataSetT_DLW_tax_amnesty_registration" />
        /// object class.
        /// </summary>
        public string home_phone_num
        {
            get
            {
                return _home_phone_num;
            }
            set
            {
                _home_phone_num = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="_alt_area_code" />
        /// value of the <see cref="GRC_WebDataSetT_DLW_tax_amnesty_registration" />
        /// object class.
        /// </summary>
        public string alt_area_code
        {
            get
            {
                return _alt_area_code;
            }
            set
            {
                _alt_area_code = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="_alt_phone_num" />
        /// value of the <see cref="GRC_WebDataSetT_DLW_tax_amnesty_registration" />
        /// object class.
        /// </summary>
        public string alt_phone_num
        {
            get
            {
                return _alt_phone_num;
            }
            set
            {
                _alt_phone_num = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="_alt_phone_extn" />
        /// value of the <see cref="GRC_WebDataSetT_DLW_tax_amnesty_registration" />
        /// object class.
        /// </summary>
        public string alt_phone_extn
        {
            get
            {
                return _alt_phone_extn;
            }
            set
            {
                _alt_phone_extn = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="_taxpayer_found_on_facs" />
        /// value of the <see cref="GRC_WebDataSetT_DLW_tax_amnesty_registration" />
        /// object class.
        /// </summary>
        public string taxpayer_found_on_facs
        {
            get
            {
                return _taxpayer_found_on_facs;
            }
            set
            {
                _taxpayer_found_on_facs = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="_tax_liability_found_on_facs" />
        /// value of the <see cref="GRC_WebDataSetT_DLW_tax_amnesty_registration" />
        /// object class.
        /// </summary>
        public string tax_liability_found_on_facs
        {
            get
            {
                return _tax_liability_found_on_facs;
            }
            set
            {
                _tax_liability_found_on_facs = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="_registration_flag" />
        /// value of the <see cref="GRC_WebDataSetT_DLW_tax_amnesty_registration" />
        /// object class.
        /// </summary>
        public string registration_flag
        {
            get
            {
                return _registration_flag;
            }
            set
            {
                _registration_flag = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="_prot_claim_ind" />
        /// value of the <see cref="GRC_WebDataSetT_DLW_tax_amnesty_registration" />
        /// object class.
        /// </summary>
        public string prot_claim_ind
        {
            get
            {
                return _prot_claim_ind;
            }
            set
            {
                _prot_claim_ind = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="_prot_claim_amt" />
        /// value of the <see cref="GRC_WebDataSetT_DLW_tax_amnesty_registration" />
        /// object class.
        /// </summary>
        public string prot_claim_amt
        {
            get
            {
                return _prot_claim_amt;
            }
            set
            {
                _prot_claim_amt = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="_return_filing_status" />
        /// value of the <see cref="GRC_WebDataSetT_DLW_tax_amnesty_registration" />
        /// object class.
        /// </summary>
        public string return_filing_status
        {
            get
            {
                return _return_filing_status;
            }
            set
            {
                _return_filing_status = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="_pay_plan_status" />
        /// value of the <see cref="GRC_WebDataSetT_DLW_tax_amnesty_registration" />
        /// object class.
        /// </summary>
        public string pay_plan_status
        {
            get
            {
                return _pay_plan_status;
            }
            set
            {
                _pay_plan_status = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="_emp_id" />
        /// value of the <see cref="GRC_WebDataSetT_DLW_tax_amnesty_registration" />
        /// object class.
        /// </summary>
        public string emp_id
        {
            get
            {
                return _emp_id;
            }
            set
            {
                _emp_id = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="_email" />
        /// value of the <see cref="GRC_WebDataSetT_DLW_tax_amnesty_registration" />
        /// object class.
        /// </summary>
        public string email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
            }
        }

        #endregion

        public DLWTaxPayerRegn()
        {
            //
            // TODO: Add constructor logic here
            //
        }
    }
}
