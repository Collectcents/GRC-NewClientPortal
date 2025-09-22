using System.Data;

namespace GRC_NewClientPortal.Models.Domain
{
    public class ReportDetailsGrid
    {
        #region variables

        DataTable _dtReportDetails;
        /// <summary>
        /// Variable for url for navigation to showreport.aspx
        /// </summary>
        private string _pdfPath;
        /// <summary>
        /// Variable for url navigation to showreport.aspx
        /// </summary>
        private string _xlsPath;
        /// <summary>
        /// Variable for name of the Report 
        /// </summary>
        private string _reportType;
        /// <summary>
        /// Variable for Client Number
        /// </summary>
        private string _clientNumber;
        /// <summary>
        ///  Variable for Debt Type
        /// </summary>
        private string _debtType;
        /// <summary>
        ///  Variable for Report Date
        /// </summary>
        private DateTime _reportDate;
        /// <summary>
        ///  Variable for Run Date 
        /// </summary>
        private DateTime _runDate;
        /// <summary>
        /// Variable for Number of Pages
        /// </summary>
        private int _numberPages;
        /// <summary>
        /// Variable for Name of the file
        /// </summary>
        private string _fileName;
        /// <summary>
        /// Variable for the physical filepath on file system
        /// </summary>
        private string _pdfFilepath;
        /// <summary>
        /// Variable for the physical filepath on file system
        /// </summary>
        private string _xlsFilepath;
        /// <summary>
        ///Variable for the format of file
        /// </summary>
        private bool _formatSpecifier;
        /// <summary>
        /// Variable for if file is selected
        /// </summary>
        private bool _isFileSelected;
        /// <summary>
        /// Variable for file size
        /// </summary>
        private string _fileSize;
        /// <summary>
        /// Variable for number of the file
        /// </summary>
        private int _fileNumber;

        #endregion


        #region properties

        /// <summary>
        /// Sets or gets the dtReportDetails data table
        /// </summary>
        public DataTable dtReportDetails
        {
            get
            {
                return _dtReportDetails;
            }
            set
            {
                _dtReportDetails = value;
            }
        }


        /// <summary>
        /// Sets or gets the url for navigation for pdf file
        /// </summary>
        public string pdfPath
        {
            get
            {
                return _pdfPath;
            }
            set
            {
                _pdfPath = value;
            }
        }
        /// <summary>
        /// Sets or gets the url for navigation for xls file
        /// </summary>
        public string xlsPath
        {
            get
            {
                return _xlsPath;
            }
            set
            {
                _xlsPath = value;
            }
        }
        /// <summary>
        /// Sets or gets the Report Type
        /// </summary>
        public string reportType
        {
            get
            {
                return _reportType;
            }
            set
            {
                _reportType = value;
            }
        }
        /// <summary>
        /// Sets or gets the Client number
        /// </summary>
        public string clientNumber
        {
            get
            {
                return _clientNumber;
            }
            set
            {
                _clientNumber = value;
            }
        }
        /// <summary>
        /// Sets or gets the Debt Type
        /// </summary>
        public string debtType
        {
            get
            {
                return _debtType;
            }
            set
            {
                _debtType = value;
            }
        }
        /// <summary>
        /// Sets or gets the Report Date
        /// </summary>
        public DateTime reportDate
        {
            get
            {
                return _reportDate;
            }
            set
            {
                _reportDate = value;
            }
        }
        /// <summary>
        /// Sets or gets the Run Date
        /// </summary>
        public DateTime runDate
        {
            get
            {
                return _runDate;
            }
            set
            {
                _runDate = value;
            }
        }
        /// <summary>
        /// Sets or gets the Number of Pages
        /// </summary>
        public int numberPages
        {
            get
            {
                return _numberPages;
            }
            set
            {
                _numberPages = value;
            }
        }
        /// <summary>
        /// Sets or gets the File Name
        /// </summary>
        public string fileName
        {
            get
            {
                return _fileName;
            }
            set
            {
                _fileName = value;
            }
        }
        /// <summary>
        /// Sets or gets the physical filepath on file system
        /// </summary>
        public string pdfFilepath
        {
            get
            {
                return _pdfFilepath;
            }
            set
            {
                _pdfFilepath = value;
            }
        }
        /// <summary>
        /// Sets or gets the physical filepath on file system
        /// </summary>
        public string xlsFilepath
        {
            get
            {
                return _xlsFilepath;
            }
            set
            {
                _xlsFilepath = value;
            }
        }
        /// <summary>
        /// Sets or gets the format specifier
        /// </summary>
        public bool formatSpecifier
        {
            get
            {
                return _formatSpecifier;
            }
            set
            {
                _formatSpecifier = value;
            }
        }
        /// <summary>
        /// Sets or gets the file selected
        /// </summary>
        public bool isFileSelected
        {
            get
            {
                return _isFileSelected;
            }
            set
            {
                _isFileSelected = value;
            }
        }
        /// <summary>
        /// Sets or gets the file size
        /// </summary>
        public string fileSize
        {
            get
            {
                return _fileSize;
            }
            set
            {
                _fileSize = value;
            }
        }
        /// <summary>
        /// Sets or gets the file number
        /// </summary>
        public int fileNumber
        {
            get
            {
                return _fileNumber;
            }
            set
            {
                _fileNumber = value;
            }
        }


        #endregion
    }
}
