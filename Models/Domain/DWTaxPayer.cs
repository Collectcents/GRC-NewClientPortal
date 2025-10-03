using System.Collections;
using GRC_NewClientPortal.Models.Domain;
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

        public void ProcessFacsLiability(DataTable _dtFacsLiability)
        {
            // Business Logic to Filter the Liability information and 
            // split it out into Registered and unregistered data tabels.

            //FACS dataset Schema :
            //"TPID"
            //"Revcode"
            //"TaxPerDte"
            //"TaxType"
            //"TaxTypeDscrptn"
            //"TaxYear"
            //"TPID_PRI"
            //"TPID_SEC"
            //"PRI_SEC"
            //"RegistrationFlag"
            //"Prot_Claim_Ind"
            //"Prot_Claim_Amt"
            //"ReturnFilingStatus"

            try
            {

                if (_dtFacsLiability == null || _dtFacsLiability.Rows.Count <= 0)
                {
                    taxpayer_found_on_facs = "N";
                    tax_liability_found_on_facs = "N";
                    return;
                }
                else
                {
                    taxpayer_found_on_facs = "Y";
                }

                DataTable _dtFacsTemp = new DataTable();
                DataRow[] _drSet;

                _dtFacsTemp = _dtFacsLiability.Clone();

                //Select all Primary TPID rows
                _drSet = _dtFacsLiability.Select("PRI_SEC = 'PRI'");
                AddRowsToDatatable(_dtFacsTemp, _drSet);

                _drSet = null;

                //Select all Secondary TPID rows where filing status is 2 or 4.
                _drSet = _dtFacsLiability.Select("TPID_SEC = 'SEC' AND ( ReturnFilingStatus = '2' OR  ReturnFilingStatus = '4')");
                AddRowsToDatatable(_dtFacsTemp, _drSet);


                //Now Split Registered and nonregistered.
                DataTable _dtFacsReg = new DataTable();
                DataTable _dtFacsUnreg = new DataTable();

                //Copy the structure
                _dtFacsReg = _dtFacsLiability.Clone();
                _dtFacsUnreg = _dtFacsLiability.Clone();

                _drSet = null;
                _drSet = _dtFacsTemp.Select("RegistrationFlag = 'Y'");
                AddRowsToDatatable(_dtFacsReg, _drSet);

                _drSet = null;
                _drSet = _dtFacsTemp.Select("RegistrationFlag <> 'Y'");
                AddRowsToDatatable(_dtFacsUnreg, _drSet);

                //Fill dt_liability_reg datatable
                dt_liability_reg.Rows.Clear();
                CopyFacsDataTableToDlwDataTable(_dtFacsReg, dt_liability_reg);

                //Fill dt_liability_unreg1 datatable.
                dt_liability_unreg1.Rows.Clear();
                CopyFacsDataTableToDlwDataTable(_dtFacsUnreg, dt_liability_unreg1);

                //Filter rows with invalid taxtypes.
                dt_liability_reg = FilterInvalidLiabilities(dt_liability_reg);
                dt_liability_unreg1 = FilterInvalidLiabilities(dt_liability_unreg1);

                SortLiabRegDataTable();
                SortLiabUnreg1DataTable();

                //Add dummy rows and setup the Registered Table.
                dt_liability_reg = SetupLiabUnreg1Table(dt_liability_reg);

                //Add dummy rows and setup the Unregistered1 table
                dt_liability_unreg1 = SetupLiabUnreg1Table(dt_liability_unreg1);


            }
            catch (Exception _ex)
            {
                throw (new Exception("Exception in - DWTaxPayer: ProcessFacsLiability", _ex));
            }
        }

        /// <summary>
        /// FilterInvalidLiabilities
        /// </summary>
        /// <param name="_dt"></param>
        /// <returns></returns>
        public static DataTable FilterInvalidLiabilities(DataTable _dt)
        {
            try
            {

                DataTable _dtTemp = new DataTable();
                _dtTemp = _dt.Clone();
                foreach (DataRow _dr in _dt.Rows)
                {
                    //if taxtype is found in the defined list, use it, else ignore it.
                    string _taxTypeKey = _dr[DLWDataTableCols.TAX_TYPE].ToString();
                    if (TaxTypes.IndexOfKey(_taxTypeKey) >= 0)
                    {
                        _dtTemp.ImportRow(_dr);
                    }
                    else
                    {
                        //ignore row
                    }
                }
                return _dtTemp;
            }
            catch (Exception _ex)
            {
                throw (new Exception("Exception in - DWTaxPayer: FilterInvalidLiabilities", _ex));
            }

        }

        public void SortLiabUnreg1DataTable()
        {
            return;
            dt_liability_unreg1.DefaultView.Sort =
                DLWDataTableCols.TAX_TYPE + " , " +
                DLWDataTableCols.TAX_YEAR + " , " +
                DLWDataTableCols.REV_CODE
                ;
        }

        public void SortLiabRegDataTable()
        {
            return;
            dt_liability_reg.DefaultView.Sort =
                DLWDataTableCols.TAX_TYPE + " , " +
                DLWDataTableCols.TAX_YEAR + " , " +
                DLWDataTableCols.REV_CODE
                ;
        }

        private DataTable SetupLiabUnreg1Table(DataTable _dtLiabUnreg1)
        {
            DataTable _dtModed, _dtRaw;


            DataRow[] _drFilteredList;
            DataRow _dr, _drDummy;
            string _tax_year;
            string _tax_type;
            string _filterQuery = "";

            _dtRaw = new DataTable();
            _dtModed = new DataTable();

            //Get the schema of the Liab datatable
            _dtModed = _dtLiabUnreg1.Clone();
            _dtRaw = _dtLiabUnreg1.Copy();

            int _iCur, _iMax;
            _iCur = 0;
            _iMax = _dtRaw.Rows.Count;

            // While there are rows left in the shrinking _dtraw table, 
            // create dummy tax type records and copy them to the new table -_dtmoded
            while (_iMax > 0)
            {
                _dr = _dtRaw.Rows[0];
                _tax_type = _dr[DLWDataTableCols.TAX_TYPE].ToString();
                _tax_year = _dr[DLWDataTableCols.TAX_YEAR].ToString();
                _filterQuery =
                    DLWDataTableCols.TAX_TYPE + " = '" + _tax_type + "'"
                    + " AND " +
                    DLWDataTableCols.TAX_YEAR + " = '" + _tax_year + "'";

                //Get the rows that match the tax year and taxtype
                _drFilteredList = _dtRaw.Select(_filterQuery);

                //Create a dummy row
                _drDummy = _dtModed.NewRow();
                _drDummy[DLWDataTableCols.REV_CODE] = "";
                _drDummy[DLWDataTableCols.TAX_TYPE] = _dr[DLWDataTableCols.TAX_TYPE];
                _drDummy[DLWDataTableCols.TAX_YEAR] = _dr[DLWDataTableCols.TAX_YEAR];
                if (_dr[DLWDataTableCols.REGISTRATION_FLAG].ToString() == "")
                {
                    _drDummy[DLWDataTableCols.REGISTRATION_FLAG] = "N";
                }
                else
                {
                    _drDummy[DLWDataTableCols.REGISTRATION_FLAG] = _dr[DLWDataTableCols.REGISTRATION_FLAG].ToString();
                }

                if (_dr[DLWDataTableCols.PROT_CLAIM_IND] == "")
                {
                    _drDummy[DLWDataTableCols.PROT_CLAIM_IND] = "N";
                }
                else
                {
                    _drDummy[DLWDataTableCols.PROT_CLAIM_IND] = _dr[DLWDataTableCols.REGISTRATION_FLAG].ToString();
                }

                _dtModed.Rows.Add(_drDummy);

                //  Add the Reve code rows to the moded table.
                AddRowsToDatatable(_dtModed, _drFilteredList);

                //Now remvoe the added rows from the raw table.
                foreach (DataRow _drFil in _drFilteredList)
                {
                    _dtRaw.Rows.Remove(_drFil);
                }

                //Update the max counter
                _iMax = _dtRaw.Rows.Count;

            }

            return _dtModed;
        }


        public static DataTable UpdateLiabUnreg1Table(DataTable _dtLiabUnreg1)
        {
            DataTable _dtTmpLiabUnreg1;
            DataRow[] _drFilteredList;
            DataRow[] _drRevCodeRowList;
            string _tax_year;
            string _tax_type;
            string _regnFlag;
            string _protClaim;
            string _filterQuery = "";

            //Copy the schema of Liab tables
            _dtTmpLiabUnreg1 = _dtLiabUnreg1.Clone();

            //Filter the to retrieve all dummy tax_type rows.
            _filterQuery = DLWDataTableCols.REV_CODE + " = ''";
            _drFilteredList = _dtLiabUnreg1.Select(_filterQuery);
            _filterQuery = "";

            //NOw browse through the filtered taxtype rows and for each, 
            // get the matching revcode rows and update the reg'n flag and other user
            // provided info.,
            foreach (DataRow _dr in _drFilteredList)
            {
                _regnFlag = _dr[DLWDataTableCols.REGISTRATION_FLAG].ToString();
                _protClaim = _dr[DLWDataTableCols.PROT_CLAIM_IND].ToString();

                _tax_type = _dr[DLWDataTableCols.TAX_TYPE].ToString();
                _tax_year = _dr[DLWDataTableCols.TAX_YEAR].ToString();
                _filterQuery =
                    DLWDataTableCols.TAX_TYPE + " = '" + _tax_type + "'" + " AND " +
                    DLWDataTableCols.TAX_YEAR + " = '" + _tax_year + "'";

                _drRevCodeRowList = _dtLiabUnreg1.Select(_filterQuery);

                //for each revcode row, update the user provided regn flag and other info
                foreach (DataRow _drRevRow in _drRevCodeRowList)
                {
                    _drRevRow[DLWDataTableCols.REGISTRATION_FLAG] = _regnFlag;
                    _drRevRow[DLWDataTableCols.PROT_CLAIM_IND] = _protClaim;
                }
                AddRowsToDatatable(_dtTmpLiabUnreg1, _drRevCodeRowList);
            }
            return _dtTmpLiabUnreg1;
        }

        private void CopyFacsDataTableToDlwDataTable(DataTable _FacsDt, DataTable _DlwDt)
        {
            try
            {
                DataRow _drDlw;

                foreach (DataRow _drFacs in _FacsDt.Rows)
                {
                    _drDlw = _DlwDt.NewRow();
                    _drDlw[DLWDataTableCols.TPID] = _drFacs["TPID"];
                    _drDlw[DLWDataTableCols.REV_CODE] = _drFacs["Revcode"];
                    _drDlw[DLWDataTableCols.TAX_PER_DTE] = _drFacs["TaxPerDte"];
                    _drDlw[DLWDataTableCols.TAX_TYPE] = _drFacs["TaxType"];
                    _drDlw[DLWDataTableCols.TAX_LIABILITY_FOUND_ON_FACS] = 'Y';
                    //_drDlw[""] = _drFacs["TaxTypeDscrptn"];
                    _drDlw[DLWDataTableCols.TAX_YEAR] = _drFacs["TaxYear"];
                    _drDlw[DLWDataTableCols.TPID_PRI] = _drFacs["TPID_PRI"];
                    _drDlw[DLWDataTableCols.TPID_SEC] = _drFacs["TPID_SEC"];
                    _drDlw[DLWDataTableCols.PRI_SEC] = _drFacs["PRI_SEC"];
                    _drDlw[DLWDataTableCols.REGISTRATION_FLAG] = _drFacs["RegistrationFlag"];
                    _drDlw[DLWDataTableCols.PROT_CLAIM_IND] = _drFacs["Prot_Claim_Ind"];
                    _drDlw[DLWDataTableCols.PROT_CLAIM_AMT] = _drFacs["Prot_Claim_Amt"];
                    _drDlw[DLWDataTableCols.RETURN_FILING_STATUS] = _drFacs["ReturnFilingStatus"];

                    _DlwDt.Rows.Add(_drDlw);
                }
            }
            catch (Exception _ex)
            {
                throw (new Exception("Exception in - DWTaxPayer: CopyFacsDataTableToDlwDataTable ", _ex));
            }

        }


        private static void AddRowsToDatatable(DataTable _dt, DataRow[] _rows)
        {
            try
            {
                foreach (DataRow _dr in _rows)
                {
                    _dt.ImportRow(_dr);
                }
            }
            catch (Exception _ex)
            {
                throw (new Exception("DWTaxPayer : AddRowsToDatatable", _ex));
            }
        }

        /// <summary>
        /// SaveRegistration : Saves Registration data tables in database.
        /// </summary>
        /// <returns></returns>
        public bool SaveRegistration()
        {
            //perform any pre-save validation/ processing.

            try
            {
                bool _result = true;

                //2009-09-02 - Double check to make sure all requiried fields are available.
                if (CheckRequiredFields() == false)
                {
                    throw new Exception("All Required Fields not available in DWTAxPayer. Cannot proceed with Save.");
                }

                CleanupLiabUnreg1();
                _result = _result & SaveLiabilityDataTable(dt_liability_unreg1);
                _result = _result & SaveLiabilityDataTable(dt_liability_unreg2);

                return _result;
            }
            catch (Exception _ex)
            {
                throw (new Exception("DWTaxPayer:SaveRegistration Failed", _ex));
            }

        }

        private bool IsValidLiabRow(DataRow _dr)
        {
            bool _isValid = true;

            if (_dr[DLWDataTableCols.REGISTRATION_FLAG].ToString() == "N" ||
                _dr[DLWDataTableCols.REGISTRATION_FLAG].ToString() == "")
            {
                _isValid = false;
            }
            else
            {
                _isValid &= true;
            }
            if (_dr[DLWDataTableCols.TAX_TYPE] == "0" || _dr[DLWDataTableCols.TAX_TYPE] == "")
            {
                _isValid = false;
            }
            else
            {
                _isValid &= true;
            }

            return _isValid;
        }


        private bool SaveLiabilityDataTable(DataTable _dtLiab)
        {
            try
            {
                DLWTaxPayerRegn _DLWTaxPayerRegn;

                bool _result = true;

                foreach (DataRow _dr in _dtLiab.Rows)
                {
                    _DLWTaxPayerRegn = new DLWTaxPayerRegn();

                    //If current row is not valid, then ignore it.
                    if (!IsValidLiabRow(_dr))
                    {
                        continue;
                    }

                    _DLWTaxPayerRegn.tpid = tpid;
                    _DLWTaxPayerRegn.rev_code = _dr[DLWDataTableCols.REV_CODE].ToString();
                    _DLWTaxPayerRegn.tax_per_dte = _dr[DLWDataTableCols.TAX_PER_DTE].ToString();

                    _DLWTaxPayerRegn.tax_type = _dr[DLWDataTableCols.TAX_TYPE].ToString();
                    _DLWTaxPayerRegn.tax_year = _dr[DLWDataTableCols.TAX_YEAR].ToString();
                    _DLWTaxPayerRegn.date_created = date_created;
                    _DLWTaxPayerRegn.date_registered = date_registered;
                    _DLWTaxPayerRegn.tpid_pri = _dr[DLWDataTableCols.TPID_PRI].ToString();
                    _DLWTaxPayerRegn.tpid_sec = _dr[DLWDataTableCols.TPID_SEC].ToString();
                    _DLWTaxPayerRegn.pri_sec = _dr[DLWDataTableCols.PRI_SEC].ToString();
                    _DLWTaxPayerRegn.tap_filing_status = tap_filing_status;
                    _DLWTaxPayerRegn.last_name = last_name;
                    _DLWTaxPayerRegn.first_name = first_name;
                    _DLWTaxPayerRegn.bus_name = bus_name;
                    _DLWTaxPayerRegn.addr_line_1 = addr_line_1;
                    _DLWTaxPayerRegn.addr_line_2 = addr_line_2;
                    _DLWTaxPayerRegn.city = city;
                    _DLWTaxPayerRegn.state = state;
                    _DLWTaxPayerRegn.zipcode = zipcode;
                    _DLWTaxPayerRegn.home_area_code = home_area_code;
                    _DLWTaxPayerRegn.home_phone_num = home_phone_num;
                    _DLWTaxPayerRegn.alt_area_code = alt_area_code;
                    _DLWTaxPayerRegn.alt_phone_num = alt_phone_num;
                    _DLWTaxPayerRegn.alt_phone_extn = alt_phone_extn;
                    _DLWTaxPayerRegn.taxpayer_found_on_facs = taxpayer_found_on_facs;
                    _DLWTaxPayerRegn.tax_liability_found_on_facs = _dr[DLWDataTableCols.TAX_LIABILITY_FOUND_ON_FACS].ToString();
                    _DLWTaxPayerRegn.registration_flag = _dr[DLWDataTableCols.REGISTRATION_FLAG].ToString();
                    _DLWTaxPayerRegn.prot_claim_ind = _dr[DLWDataTableCols.PROT_CLAIM_IND].ToString();
                    _DLWTaxPayerRegn.prot_claim_amt = _dr[DLWDataTableCols.PROT_CLAIM_AMT].ToString();
                    _DLWTaxPayerRegn.return_filing_status = _dr[DLWDataTableCols.RETURN_FILING_STATUS].ToString();
                    _DLWTaxPayerRegn.pay_plan_status = pay_plan_status;
                    _DLWTaxPayerRegn.emp_id = emp_id;
                    _DLWTaxPayerRegn.email = email;

                    try
                    {
                        _result =
                            _result &
                            _DLWTaxPayerRegn.SaveRegistration();
                    }
                    catch (Exception _ex)
                    {
                        throw (new Exception("Exception in - DWTaxPayer: _DLWTaxPayerRegn.SaveRegistration().", _ex));
                    }
                    finally
                    {
                        _DLWTaxPayerRegn = null;
                    }
                }
                return _result;
            }
            catch (Exception _ex)
            {
                throw (new Exception("Exception in - DWTaxPayer: SaveLiabilityDataTable", _ex.InnerException));
            }

        }


        /// <summary>
        /// DeriveTpid
        /// </summary>
        /// <returns></returns>
        public string DeriveTpid()
        {
            try
            {
                //If the taxpayer enters the SSN for an individual, 
                //then the SSN is prefixed with ‘2’ to create the 10 digit TPID.
                //If the taxpayer enters the EIN for a business, 
                //then the EIN is prefixed with a ‘1’ to create the 10 digit TPID.

                string _derivedTpid;
                if (ssn.ToString().Length > 0)
                {
                    _derivedTpid = "2" + ssn.ToString();
                }
                else if (ein.ToString().Length > 0)
                {
                    _derivedTpid = "1" + ein.ToString();
                }
                else
                {
                    _derivedTpid = String.Empty;
                }
                return _derivedTpid;
            }
            catch (Exception _ex)
            {
                throw (new Exception("Exception in - DWTaxPayer: DeriveTpid", _ex.InnerException));
            }


        }

        public string DeriveTaxPerDte(string _tax_year)
        {
            try
            {
                string _derivedTaxPerDte;
                if (_tax_year.Length > 0)
                {
                    //Convert tax year to CCYYMMDD, 12/31/CCYY using the tax year for CCYY
                    _derivedTaxPerDte = _tax_year.ToString() + "12" + "31";
                }
                else
                {
                    _derivedTaxPerDte = String.Empty;
                }
                return _derivedTaxPerDte;
            }
            catch (Exception _ex)
            {
                throw (new Exception("Exception in - DWTaxPayer: DeriveTaxPerDte", _ex.InnerException));
            }

        }

        public static bool IsValidTPID(string _tpid)
        {
            try
            {
                if ((_tpid == null) ||
                    (_tpid.Length != 10) ||
                    ((_tpid[0].CompareTo('2') < 0) ||
                        (_tpid[0].CompareTo('1') < 0))
                    )
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception _ex)
            {
                throw (new Exception("Exception in - DWTaxPayer: IsValidTPID", _ex.InnerException));
            }

        }

        public void AddNewUnregLiabDataRow(DataTable _dtNewUnRegLiab)
        {
            try
            {
                DataRow _dr;

                _dr = _dtNewUnRegLiab.NewRow();
                _dr[DLWDataTableCols.TPID] = tpid;
                _dr[DLWDataTableCols.REV_CODE] = String.Empty;
                _dr[DLWDataTableCols.TAX_PER_DTE] = String.Empty;
                _dr[DLWDataTableCols.TAX_TYPE] = String.Empty;
                _dr[DLWDataTableCols.TAX_YEAR] = String.Empty;
                _dr[DLWDataTableCols.TPID_PRI] = tpid;
                _dr[DLWDataTableCols.TPID_SEC] = default_tpid_sec;
                _dr[DLWDataTableCols.PRI_SEC] = default_pri_sec;
                _dr[DLWDataTableCols.REGISTRATION_FLAG] = 'Y';
                _dr[DLWDataTableCols.PROT_CLAIM_IND] = 'N';
                _dr[DLWDataTableCols.PROT_CLAIM_AMT] = String.Empty;
                _dr[DLWDataTableCols.RETURN_FILING_STATUS] = String.Empty;
                _dr[DLWDataTableCols.TAX_LIABILITY_FOUND_ON_FACS] = 'N';

                _dtNewUnRegLiab.Rows.Add(_dr);

            }
            catch (Exception _ex)
            {
                throw (new Exception("Exception in - DWTaxPayer: AddNewUnregLiabDataRow", _ex.InnerException));
            }

        }

        public static void CleanupLiabilityDataTable(DataTable _dt)
        {
            try
            {
                DataTable _dtTemp;
                _dtTemp = _dt.Copy();

                DataRow[] _drList;
                string _filterQuery = DLWDataTableCols.TAX_TYPE + " <> '0' AND " +
                                        DLWDataTableCols.TAX_TYPE + " <> '' ";
                _drList = _dtTemp.Select(_filterQuery);

                _dt.Rows.Clear();

                AddRowsToDatatable(_dt, _drList);
            }
            catch (Exception _ex)
            {
                throw new Exception("Exception in DWTaxPayer : CleanupLiabilityDataTable ", _ex);
            }
        }

        public void CleanupLiabUnreg1()
        {
            try
            {
                DataTable _dt = dt_liability_unreg1.Copy();

                string _filter = DLWDataTableCols.REV_CODE + " <> ''";
                DataRow[] _rows = _dt.Select(_filter);
                dt_liability_unreg1.Rows.Clear();
                AddRowsToDatatable(dt_liability_unreg1, _rows);
            }
            catch (Exception _ex)
            {
                throw new Exception("Exception in DWTaxPayer : CleanupLiabilityDataTable ", _ex);
            }
        }

        public static string GetValueDesc(string _colName, string _valCode)
        {
            try
            {
                string _returnText = "";
                switch (_colName.ToUpper())
                {
                    case "TAX_TYPE":
                        if (TaxTypes.IndexOfKey(_valCode.ToUpper()) < 0)
                        {
                            //							return "Invalid Tax Type";
                            throw new Exception("Invalid Tax type [] found ");
                        }
                        _returnText = TaxTypes[_valCode.ToUpper()].ToString();
                        break;

                    case "REGISTRATION_FLAG":
                        _returnText = (_valCode.ToUpper() == "Y") ? "Yes" : "No";
                        break;

                    case "PROT_CLAIM_IND":
                        _returnText = (_valCode.ToUpper() == "Y") ? "Yes" : "No";
                        break;
                }
                return _returnText;
            }
            catch (Exception _ex)
            {
                throw (new Exception("Exception in - DWTaxPayer: GetValueDesc", _ex));
            }


        }


        /// <summary>
        /// CheckRequiredFields
        /// </summary>
        /// <returns></returns>
        public bool CheckRequiredFields()
        {
            try
            {
                if (tpid == null || tpid == string.Empty) return false;
                if (last_name == null || last_name == string.Empty) return false;
                if (first_name == null || first_name == string.Empty) return false;
                if (addr_line_1 == null || addr_line_1 == string.Empty) return false;
                if (city == null || city == string.Empty) return false;
                if (state == null || state == string.Empty) return false;
                if (zipcode == null || zipcode == string.Empty) return false;
                if (home_area_code == null || home_area_code == string.Empty) return false;
                if (home_phone_num == null || home_phone_num == string.Empty) return false;
                if (taxpayer_found_on_facs == null || taxpayer_found_on_facs == string.Empty) return false;
                if (tap_filing_status == null || tap_filing_status == string.Empty) return false;
                if (pay_plan_status == null || pay_plan_status == string.Empty) return false;

                return true;
            }
            catch (Exception _ex)
            {
                throw new Exception("Exception in  : CheckRequiredFields ", _ex);
            }
        }
    }
}
