using System.Data;
using System.Data.SqlClient;

namespace GRC_NewClientPortal.Models.Domain
{
    public class ReportDetailsGrid
    {
        private readonly IConfiguration _config;

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

        /// <summary>
		/// Constructor
		/// </summary>
		public ReportDetailsGrid(IConfiguration config)
        {
            _config = config;
            dtReportDetails = new DataTable();
            dtReportDetails.Columns.Add(new DataColumn("pdfPath", System.Type.GetType("System.String")));
            dtReportDetails.Columns.Add(new DataColumn("xlsPath", System.Type.GetType("System.String")));
            dtReportDetails.Columns.Add(new DataColumn("reportType", System.Type.GetType("System.String")));
            dtReportDetails.Columns.Add(new DataColumn("clientNumber", System.Type.GetType("System.String")));
            dtReportDetails.Columns.Add(new DataColumn("debtType", System.Type.GetType("System.String")));
            dtReportDetails.Columns.Add(new DataColumn("reportDate", System.Type.GetType("System.DateTime")));
            dtReportDetails.Columns.Add(new DataColumn("runDate", System.Type.GetType("System.DateTime")));
            dtReportDetails.Columns.Add(new DataColumn("numberPages", System.Type.GetType("System.Int32")));
            dtReportDetails.Columns.Add(new DataColumn("fileName", System.Type.GetType("System.String")));
            dtReportDetails.Columns.Add(new DataColumn("pdfFilepath", System.Type.GetType("System.String")));
            dtReportDetails.Columns.Add(new DataColumn("xlsFilepath", System.Type.GetType("System.String")));
            dtReportDetails.Columns.Add(new DataColumn("formatSpecifier", System.Type.GetType("System.Boolean")));
            dtReportDetails.Columns.Add(new DataColumn("isFileSelected", System.Type.GetType("System.Boolean")));
            dtReportDetails.Columns.Add(new DataColumn("fileSize", System.Type.GetType("System.String")));
            dtReportDetails.Columns.Add(new DataColumn("fileNumber", System.Type.GetType("System.Int32")));

        }

        /// <summary>
        /// Get reports from file system to populate the datagrid
        /// </summary>
        public DataTable GetReportDetails(string basePath, string reportName)
        {
            try
            {
                string[] firstLevel, secondLevel;
                System.Collections.IEnumerator enLevel1, enLevel2;
                string sqlGetFormat = "";
                string connectionString = _config.GetConnectionString("DefaultDataSource");
                SqlConnection sqlConn = new SqlConnection(connectionString);
                sqlConn.Open();
                string[] rowValues = new string[15];
                string fileName = string.Empty;
                string baseClient;
                int iRecord = 0;

                if (System.IO.Directory.Exists(basePath))
                {
                    // Get directories under D://Reports//BaseClient
                    firstLevel = System.IO.Directory.GetDirectories(basePath);
                    enLevel1 = firstLevel.GetEnumerator();

                    while (enLevel1.MoveNext())
                    {
                        // Get directories under D://Reports//BaseClient//Client
                        secondLevel = System.IO.Directory.GetDirectories(Convert.ToString(enLevel1.Current));
                        enLevel2 = secondLevel.GetEnumerator();
                        while (enLevel2.MoveNext())
                        {
                            //Report Directories under D://Reports//BaseClient//Client
                            System.IO.DirectoryInfo directoryName = new System.IO.DirectoryInfo(Convert.ToString(enLevel2.Current));
                            //Report Directories under D://Reports//BaseClient//
                            System.IO.DirectoryInfo directoryParent = new System.IO.DirectoryInfo(Convert.ToString(enLevel1.Current));
                            baseClient = directoryParent.Parent.ToString();
                            //compare Current directory and Report Name
                            if (directoryName.Name.ToString() == reportName)
                            {
                                FileInfo[] pdfFiles = directoryName.GetFiles("*.pdf");

                                FileInfo[] filesLst = directoryName.GetFiles();

                                string[] fileNamelst = new string[filesLst.Length];

                                bool isExist = false;
                                int iCount = 0;

                                foreach (FileInfo rptInfo in filesLst)
                                {
                                    //fileName = rptInfo.Name;

                                    if (!(rptInfo.Attributes.ToString().Contains("Hidden")) || (rptInfo.Extension.ToUpper() == ".PDF" || rptInfo.Extension.ToUpper() == ".XLSX"))
                                        fileName = rptInfo.Name.Remove(63, rptInfo.Extension.Length);

                                    isExist = false;
                                    int index = 0;
                                    //For each of the pdf files
                                    foreach (FileInfo filInfo in filesLst)
                                    {
                                        if (fileNamelst[index] == fileName)
                                        {
                                            isExist = true;
                                        }
                                        index++;
                                    }

                                    fileNamelst[iCount] = fileName;
                                    iCount++;

                                    if (isExist == false && (rptInfo.Extension.ToUpper() == ".PDF" || rptInfo.Extension.ToUpper() == ".XLSX" || rptInfo.Extension.ToUpper() == ".CSV"))
                                    {

                                        rowValues[0] = "";
                                        rowValues[1] = "";
                                        if (rptInfo.Extension.ToUpper() == ".PDF")
                                        {
                                            rowValues[0] = "type=" + reportName + "&cln=" + directoryParent.Name.ToString() + "&file=" + iRecord.ToString() + "&format=pdf";
                                            //FileInfo[] xlsFiles = directoryName.GetFiles("*.xls");
                                            var xlsFiles = Directory.EnumerateFiles(directoryName.FullName)
                                                .Where(file => file.ToLower().EndsWith("xls") || file.ToLower().EndsWith("xlsx"))
                                                .ToList();
                                            foreach (string xlsInfo in xlsFiles)
                                            {
                                                if (fileName == xlsInfo.Substring(xlsInfo.LastIndexOf('\\') + 1, xlsInfo.LastIndexOf('.') - xlsInfo.LastIndexOf('\\') - 1))
                                                {
                                                    rowValues[1] = "type=" + reportName + "&cln=" + directoryParent.Name.ToString() + "&file=" + iRecord.ToString() + "&format=" + xlsInfo.Substring(xlsInfo.LastIndexOf('.') + 1, xlsInfo.Length - xlsInfo.LastIndexOf('.') - 1);
                                                    break;
                                                }

                                            }
                                        }
                                        else

                                        {
                                            rowValues[1] = "type=" + reportName + "&cln=" + directoryParent.Name.ToString() + "&file=" + iRecord.ToString() + "&format=xls";
                                            FileInfo[] xlsFiles = directoryName.GetFiles("*.pdf");
                                            foreach (FileInfo xlsInfo in xlsFiles)
                                            {
                                                if (fileName == xlsInfo.Name.Remove(63, rptInfo.Extension.Length))
                                                {
                                                    rowValues[0] = "type=" + reportName + "&cln=" + directoryParent.Name.ToString() + "&file=" + iRecord.ToString() + "&format=pdf";
                                                    break;
                                                }

                                            }
                                        }


                                        //Client Number
                                        rowValues[3] = directoryParent.Name.ToString();
                                        rowValues[4] = fileName.Substring(23, 10).Replace("_", "");
                                        if (reportName == "FACS CLIENT STATEMENTS")
                                        {
                                            rowValues[2] = (fileName.Substring(16, 7).ToString());
                                            if (rowValues[4].EndsWith("S"))
                                                rowValues[2] += " (Totals Page)";
                                            rowValues[4] = "";
                                            SqlCommand cmd = new SqlCommand("SELECT Debt_Type FROM tbl_client_xref where CLIENT=@Client", sqlConn);
                                            cmd.Parameters.Add("@Client", SqlDbType.VarChar);
                                            cmd.Parameters["@Client"].Value = rowValues[3];
                                            rowValues[4] = Convert.ToString(cmd.ExecuteScalar()); //Added role base menu access Request#4661391 (Existing Testing Issue)

                                        }
                                        else
                                        {
                                            rowValues[2] = reportName;
                                        }
                                        //Report Date
                                        rowValues[5] = fileName.Substring(0, 2) + "/" + fileName.Substring(2, 2) + "/" + fileName.Substring(4, 4);
                                        //Run Date
                                        rowValues[6] = fileName.Substring(8, 2) + "/" + fileName.Substring(10, 2) + "/" + fileName.Substring(12, 4);
                                        //Number of pages
                                        rowValues[7] = fileName.Substring(58, 5);
                                        //Report Path on file system
                                        rowValues[8] = directoryName.ToString() + "\\" + fileName;
                                        if (rowValues[0] != "")
                                            rowValues[9] = "View";
                                        else
                                            rowValues[9] = "";
                                        if (rowValues[1] != "")
                                            rowValues[10] = "View";
                                        else
                                            rowValues[10] = "";
                                        //fileSize
                                        rowValues[13] = GetFileSize(rptInfo.Length);

                                        //Format specifier is obtained from tbl_clnt_reports
                                        //sqlGetFormat = "exec p_Get_ReportDelivered '" + rowValues[8]+"','"+reportName+"'";
                                        sqlGetFormat = "exec p_Get_ReportDelivered '" + rowValues[8] + "','" + reportName + "','" + rowValues[3] + "','" + baseClient + "'";


                                        try
                                        {
                                            //sqlConn.Open();
                                            SqlDataAdapter daReportFormat = new SqlDataAdapter(sqlGetFormat, sqlConn);
                                            DataSet dsReportFormat = new DataSet();
                                            daReportFormat.Fill(dsReportFormat);
                                            daReportFormat.Dispose();
                                            //sqlConn.Close();
                                            if (dsReportFormat.Tables[0].Rows.Count > 0)
                                            {
                                                //If there is no entry in SQL table apply bold formatting
                                                if (dsReportFormat.Tables[0].Rows[0][0].ToString() == null)
                                                    rowValues[11] = "False";
                                                if (dsReportFormat.Tables[0].Rows[0][0].ToString() != "" || dsReportFormat.Tables[0].Rows[0][0].ToString() != null)
                                                {
                                                    rowValues[11] = dsReportFormat.Tables[0].Rows[0][0].ToString();
                                                }
                                            }
                                            else
                                            {
                                                rowValues[11] = "False";
                                            }
                                        }
                                        catch (Exception)
                                        {
                                            ApplicationException ex = new ApplicationException("Could not get file format.");
                                            throw ex;
                                        }
                                        //Is selected false for first time
                                        rowValues[12] = "False";
                                        //Number of the file
                                        rowValues[14] = iRecord.ToString();
                                        dtReportDetails.Rows.Add(rowValues);
                                        iRecord++;
                                    }
                                }
                            }
                        }
                    }
                }
                sqlConn.Close();
            }
            catch (Exception e)
            {
                ApplicationException ex = new ApplicationException("Could not get ReportGrid details." + e);
                throw ex;
            }
            return dtReportDetails;
        }
        /// <summary>
        /// To display the file size
        /// </summary>
        /// <param name="Bytes"></param>
        /// <returns></returns>
        private string GetFileSize(long Bytes)
        {
            if (Bytes >= 1073741824)
            {
                Decimal size = Decimal.Divide(Bytes, 1073741824);
                return String.Format("{0:##.##} GB", size);
            }
            else if (Bytes >= 1048576)
            {
                Decimal size = Decimal.Divide(Bytes, 1048576);
                return String.Format("{0:##.##} MB", size);
            }
            else if (Bytes >= 1024)
            {
                Decimal size = Decimal.Divide(Bytes, 1024);
                return String.Format("{0:##.##} KB", size);
            }
            else if (Bytes > 0 & Bytes < 1024)
            {
                Decimal size = Bytes;
                return String.Format("{0:##.##} Bytes", size);
            }
            else
            {
                return "0 Bytes";
            }
        }
    }
}
