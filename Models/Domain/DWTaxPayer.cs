using System.Collections;
using System.Data;

namespace GRC_NewClientPortal.Models.Domain
{
    public class DWTaxPayer
    {
        public class DLWDataTableCols
        {
            public static string TPID = "tpid";
            public static string REV_CODE = "rev_code";
            public static string TAX_PER_DTE = "tax_per_dte";
            public static string TAX_TYPE = "tax_type";
            public static string TAX_YEAR = "tax_year";
            public static string TPID_PRI = "tpid_pri";
            public static string TPID_SEC = "tpid_sec";
            public static string PRI_SEC = "pri_sec";
            public static string REGISTRATION_FLAG = "registration_flag";
            public static string PROT_CLAIM_IND = "prot_claim_ind";
            public static string PROT_CLAIM_AMT = "prot_claim_amt";
            public static string RETURN_FILING_STATUS = "return_filing_status";
            public static string TAX_LIABILITY_FOUND_ON_FACS = "tax_liability_found_on_facs";
        }

        #region variables

        public static SortedList TaxYears;
        public static SortedList TaxTypes;
        public static SortedList RevenueCodeTypes;
        public static SortedList DefRevenueCodes;
        public static DataTable TemplateLiabDataTable;

        DataTable _dt_liability_reg;
        DataTable _dt_liability_unreg1;
        DataTable _dt_liability_unreg2;
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


        public DataTable dt_liability_reg
        {
            get
            {
                return _dt_liability_reg;
            }
            set
            {
                _dt_liability_reg = value;
            }
        }
        public DataTable dt_liability_unreg1
        {
            get
            {
                return _dt_liability_unreg1;
            }
            set
            {
                _dt_liability_unreg1 = value;
            }
        }
        public DataTable dt_liability_unreg2
        {
            get
            {
                return _dt_liability_unreg2;
            }
            set
            {
                _dt_liability_unreg2 = value;
            }
        }


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


        public string default_tpid_sec
        {
            get
            {
                return "0000000000";
            }
        }

        public string default_pri_sec
        {
            get
            {
                return "PRI";
            }
        }



        #endregion


        /// <summary>
        /// 
        /// </summary>
        public DWTaxPayer()
        {

            //Create empty tables.
            dt_liability_reg = new DataTable();
            dt_liability_unreg1 = new DataTable();
            dt_liability_unreg2 = new DataTable();

            //Copy the structure of the table.
            dt_liability_reg = TemplateLiabDataTable.Clone();
            dt_liability_unreg1 = TemplateLiabDataTable.Clone();
            dt_liability_unreg2 = TemplateLiabDataTable.Clone();

            tpid = string.Empty;
            ssn = string.Empty;
            ein = string.Empty;
            rev_code = string.Empty;
            tax_per_dte = string.Empty;
            tax_type = string.Empty;
            tax_year = string.Empty;
            date_created = DateTime.MinValue;
            date_registered = DateTime.MinValue;
            tpid_pri = string.Empty;
            tpid_sec = string.Empty;
            pri_sec = string.Empty;
            tap_filing_status = string.Empty;
            last_name = string.Empty;
            first_name = string.Empty;
            bus_name = string.Empty;
            addr_line_1 = string.Empty;
            addr_line_2 = string.Empty;
            city = string.Empty;
            state = string.Empty;
            zipcode = string.Empty;
            home_area_code = string.Empty;
            home_phone_num = string.Empty;
            alt_area_code = string.Empty;
            alt_phone_num = string.Empty;
            alt_phone_extn = string.Empty;
            taxpayer_found_on_facs = string.Empty;
            tax_liability_found_on_facs = string.Empty;
            registration_flag = string.Empty;
            prot_claim_ind = string.Empty;
            prot_claim_amt = string.Empty;
            return_filing_status = string.Empty;
            pay_plan_status = string.Empty;
            emp_id = string.Empty;
            email = string.Empty;
        }

        static DWTaxPayer()
        {
            TaxTypes = new SortedList();
            DefRevenueCodes = new SortedList();
            TaxYears = new SortedList();
            RevenueCodeTypes = new SortedList();

            TaxTypes.Add("C", "CORPORATE INCOME TAX");
            TaxTypes.Add("F", "FIDUCIARY TAX");
            TaxTypes.Add("I", "INHERITANCE TAX");
            TaxTypes.Add("L", "LIC./GR.RECEIPTS TAX");
            //2009-09-08 To be removed based on delaware request
            //TaxTypes.Add("M", "MFG HOME TRUST FUND");
            TaxTypes.Add("P", "PERSONAL INCOME TAX");
            TaxTypes.Add("Q", "PARTNERSHIP TAX");
            TaxTypes.Add("R", "REALTY TRANSFER TAX");
            TaxTypes.Add("W", "WITHHOLDING TAX");
            TaxTypes.Add("0", "Select One");


            DefRevenueCodes.Add("C", "0042");
            DefRevenueCodes.Add("F", "0007");
            DefRevenueCodes.Add("I", "0003");
            DefRevenueCodes.Add("L", "0028");
            //2009-09-08 To be removed based on delaware request
            //DefRevenueCodes.Add("M", "0029");
            DefRevenueCodes.Add("P", "0001");
            DefRevenueCodes.Add("Q", "0006");
            DefRevenueCodes.Add("R", "0050");
            DefRevenueCodes.Add("W", "0089");
            DefRevenueCodes.Add("0", "0");


            TaxYears.Add("1994", "1994");
            TaxYears.Add("1995", "1995");
            TaxYears.Add("1996", "1996");
            TaxYears.Add("1997", "1997");
            TaxYears.Add("1998", "1998");
            TaxYears.Add("1999", "1999");
            TaxYears.Add("2000", "2000");
            TaxYears.Add("2001", "2001");
            TaxYears.Add("2002", "2002");
            TaxYears.Add("2003", "2003");
            TaxYears.Add("2004", "2004");
            TaxYears.Add("2005", "2005");
            TaxYears.Add("2006", "2006");
            TaxYears.Add("2007", "2007");
            TaxYears.Add("2008", "2008");
            /*
			 * 2009-09-03 : 2009 to be removed based on Delaware request.
			TaxYears.Add("2009", "2009");
			*/

            RevenueCodeTypes.Add("0001", "PERSONAL INCOME TAX");
            RevenueCodeTypes.Add("0002", "INHERITANCE TAX");
            RevenueCodeTypes.Add("0003", "ESTATE TAX");
            RevenueCodeTypes.Add("0004", "FIDUCIARY TENTATIVE TAX");
            RevenueCodeTypes.Add("0005", "PARTNERSHIP ESTIMATED TAX");
            RevenueCodeTypes.Add("0006", "PARTNERSHIP TAX");
            RevenueCodeTypes.Add("0007", "FIDUCIARY TAX");
            RevenueCodeTypes.Add("0008", "RAILROAD COMUTATION");
            RevenueCodeTypes.Add("0009", "TELEPHONE & TELEGRAPH");
            RevenueCodeTypes.Add("0010", "STEAM GAS & ELECTRIC");
            RevenueCodeTypes.Add("0020", "LIC THOROUGHBRED BETTING");
            RevenueCodeTypes.Add("0028", "LICENSE GROSS RECEIPTS");
            RevenueCodeTypes.Add("0029", "MANUFACTURED HOME TRUST");
            RevenueCodeTypes.Add("0035", "CIGARETTE STAMPS");
            RevenueCodeTypes.Add("0036", "OTHER TOBACCO PRODUCT");
            RevenueCodeTypes.Add("0038", "CORPORATE TENTATIVE");
            RevenueCodeTypes.Add("0042", "CORPORATE INCOME TAX");
            RevenueCodeTypes.Add("0050", "REALTY TRANSFER TAX");
            RevenueCodeTypes.Add("0053", "PUBLIC ACCOMMODATIONS TAX");
            RevenueCodeTypes.Add("0054", "PUBLIC UTILITIES TAX");
            RevenueCodeTypes.Add("0089", "WITHHOLDING MONTHLY TAX");
            RevenueCodeTypes.Add("0090", "WITHHOLDING QUARTERLY TAX");
            RevenueCodeTypes.Add("0091", "PERSONAL ESTIMATED TAX");
            RevenueCodeTypes.Add("0092", "PERSONAL INCOME PRIOR YR");
            RevenueCodeTypes.Add("0093", "SUBCHAPTER RETURN");
            RevenueCodeTypes.Add("0094", "SUBCHAPTER S ESTIMATED");
            RevenueCodeTypes.Add("0101", "LICENSE TAX");
            RevenueCodeTypes.Add("", "");


            TemplateLiabDataTable = new DataTable();

            TemplateLiabDataTable.Columns.Add(new DataColumn(DLWDataTableCols.TPID));
            TemplateLiabDataTable.Columns.Add(new DataColumn(DLWDataTableCols.REV_CODE));
            TemplateLiabDataTable.Columns.Add(new DataColumn(DLWDataTableCols.TAX_PER_DTE));
            TemplateLiabDataTable.Columns.Add(new DataColumn(DLWDataTableCols.TAX_TYPE));
            TemplateLiabDataTable.Columns.Add(new DataColumn(DLWDataTableCols.TAX_YEAR));
            TemplateLiabDataTable.Columns.Add(new DataColumn(DLWDataTableCols.TPID_PRI));
            TemplateLiabDataTable.Columns.Add(new DataColumn(DLWDataTableCols.TPID_SEC));
            TemplateLiabDataTable.Columns.Add(new DataColumn(DLWDataTableCols.PRI_SEC));
            TemplateLiabDataTable.Columns.Add(new DataColumn(DLWDataTableCols.REGISTRATION_FLAG));
            TemplateLiabDataTable.Columns.Add(new DataColumn(DLWDataTableCols.PROT_CLAIM_IND));
            TemplateLiabDataTable.Columns.Add(new DataColumn(DLWDataTableCols.PROT_CLAIM_AMT));
            TemplateLiabDataTable.Columns.Add(new DataColumn(DLWDataTableCols.RETURN_FILING_STATUS));
            TemplateLiabDataTable.Columns.Add(new DataColumn(DLWDataTableCols.TAX_LIABILITY_FOUND_ON_FACS));
        }
    }
}
