using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace GRC_NewClientPortal.Models.Domain
{
    public class SystemAvailability
    {
        private static IConfiguration _config;
        #region Splash down
        public const string SYSTEM_UP = "SYSTEM_UP";
        public const string SYSTEM_DOWN = "SYSTEM_DOWN";
        #endregion

        #region Private Declarations
        // Track database status, splash status & overall system status
        private string _databaseStatus = SYSTEM_UP;
        private string _systemStatus = SYSTEM_UP;
        private string _splashStatus = SYSTEM_UP;
        private string _splashDBConnName = _config["SplashDatabase"].ToString();

        private int _timeInterval = 3;// Default time period is to check every three minutes
        private DateTime _nextCheckTime = new DateTime(1900, 1, 1); // Next time to perform check
        private bool _checkingStatus = false;
        private static SystemAvailability sysAvail;
        private string __sysDownEndTime = string.Empty;

        #endregion

        public static void Initialize(IConfiguration config)
        {
            _config = config;
        }

        #region Constructors
        /// <summary>
        /// Constructor - Get the time interval for the check from the config file
        /// </summary>
        private SystemAvailability()
        {
            try
            {
                if (_config["CheckAvailabilityInterval"] != null)
                    _timeInterval = Int32.Parse(_config["CheckAvailabilityInterval"]);
            }
            catch
            {
                // Set interval to default if there are any exceptions
                _timeInterval = 3;
            }
        }
        #endregion

        #region private static SystemAvailability Singleton()
        /// <summary>
        /// private static SystemAvailability Singleton()
        /// Method provides a single instance to be used.
        /// </summary>
        /// <returns>SystemAvailablility Singleton</returns>
        private static SystemAvailability Singleton()
        {
            if (sysAvail == null)
                sysAvail = new SystemAvailability();
            return sysAvail;
        }
        #endregion

        #region CheckSystem
        /// <summary>
        /// Check if Life Skills Application is Healthy
        /// </summary>
        /// <returns></returns>
        private void CheckSystem()
        {
            //Since every page calls this method, we need to make sure
            //to reduce contention, but at the same time try and guarantee
            //only one thread does the work.
            //So we do a "lastCheckDone" in one line to see if we are
            //over the two minute time limit
            if (DateTime.Now > _nextCheckTime)
            {
                // Now if 
                if (!_checkingStatus)
                {
                    lock (this) // Nobody checking status so lock
                    {
                        _checkingStatus = true;  // this thread checking status
                        _nextCheckTime = DateTime.Now.AddMinutes(_timeInterval);  // set next time to check

                        if (CheckSplash(_config["SplashKey"]))
                            _systemStatus = SYSTEM_DOWN;
                        else
                        {
                            _systemStatus = SYSTEM_UP;
                        }
                        _checkingStatus = false;
                    }
                }
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// public static string GetStatus()
        /// </summary>
        /// <returns>Singleton systemStatus</returns>
        public static string GetStatus()
        {
            return Singleton()._systemStatus;
        }

        /// <summary>
        /// public static string Status
        /// Overall status of the LC Application 
        /// </summary>
        public static string Status
        {
            get
            {
                Singleton().CheckSystem();
                return Singleton()._systemStatus;
            }
        }

        /// <summary>
        /// public static string DatabaseStatus
        /// Status of database
        /// </summary>
        public static string DatabaseStatus
        {
            get
            {
                return Singleton()._databaseStatus;
            }
        }

        /// <summary>
        /// public static string SplashStatus
        /// Status according to the splash table (used for scheduled downtimes)
        /// </summary>
        public static string SplashStatus
        {
            get
            {
                return Singleton()._splashStatus;
            }
        }

        /// <summary>
        /// public static string SysDownEndTime
        /// End Time from the splash table
        /// </summary>
        public static string SysDownEndTime
        {
            get
            {
                return Singleton().__sysDownEndTime;
            }
        }
        #endregion


        #region CheckSplash
        /// <summary>
        /// 
        /// </summary>
        /// <param name="appName"></param>
        /// <returns></returns>
        private bool CheckSplash(string appName)
        {
            IDataReader idr = null;
            string cmd = string.Empty;
            SqlConnection sqlConn;
            if (_config["CheckSplash"] != null && _config["CheckSplash"] == "Y")
            {
                sqlConn = new SqlConnection(_splashDBConnName);
                cmd = "SELECT Description, AppID, EndTime " +
                    "FROM SCHEDULE " +
                    "WHERE AppID = '" + appName + "' " +
                    " AND Enabled = 1 " +
                    "AND SUBSTRING(days,DATEPART(weekday,GETDATE()),1) = 'Y' " +
                    "AND CAST(CONVERT(VARCHAR,GETDATE()) AS DATETIME) >= CASE WHEN StartDate IS NULL THEN CAST(CONVERT(VARCHAR,GETDATE()) AS DATETIME) ELSE StartDate END  " +
                    "AND CAST(CONVERT(VARCHAR,GETDATE()) AS DATETIME) <= CASE WHEN EndDate IS NULL THEN CAST(CONVERT(VARCHAR,GETDATE()) AS DATETIME) ELSE EndDate END  "; //+
                                                                                                                                                                          //"AND CAST(CONVERT(VARCHAR,DATEADD(minute, 2,GETDATE()),14) AS DATETIME) >= CAST(CONVERT(VARCHAR, CASE WHEN StartTime IS NULL THEN GETDATE() ELSE StartTime END,14) AS DATETIME) " +
                                                                                                                                                                          //"AND CAST(CONVERT(VARCHAR,GETDATE(),14) AS DATETIME) <= CAST(CONVERT(VARCHAR,CASE WHEN EndTime IS NULL THEN GETDATE() ELSE DATEADD(second, -1,EndTime) END,14) AS DATETIME) ";
                try
                {
                    sqlConn.Open();
                    SqlCommand sqlCmd = new SqlCommand(cmd, sqlConn);
                    idr = sqlCmd.ExecuteReader();

                    if (idr.Read())
                    {
                        __sysDownEndTime = idr["EndTime"].ToString();
                        return true;
                    }
                    else
                    { return false; }
                }
                catch (Exception ex)
                {
                    // Ignore any problems 
                    // Log Message
                    //Utilities.AppEventLogger.LogError("Error Occurred while accessing Splash database: ", ex, "Exception");

                    // Check the reader and close if necessary.
                    if (idr != null && !idr.IsClosed) { idr.Close(); }

                    // Make this return false instead of true (we won't consider
                    // splash db errors as a fatal error).
                    return false;
                }
                finally
                {
                    if (idr != null && !idr.IsClosed) { idr.Close(); }
                    if (sqlConn != null)
                        sqlConn.Close();
                }
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}
