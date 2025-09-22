namespace GRC_NewClientPortal.Models.Domain
{
    public class PayOnline
    {
        // pay flow pro parameters
        /// <summary>
        /// Class variable to hold the Host address for pay flow pro
        /// </summary>
        private string strHostAddress;
        /// <summary>
        /// Class variable to hold the Host port for pay flow pro
        /// </summary>
        private int intHostPort;
        /// <summary>
        /// Class variable to hold the Vendor name for pay flow pro
        /// </summary>
        private string strVendor;
        /// <summary>
        /// Class variable to hold the Partner name for pay flow pro
        /// </summary>
        private string strPartner;
        /// <summary>
        /// Class variable to hold the User name for pay flow pro
        /// </summary>
        private string strUserName;
        /// <summary>
        /// Class variable to hold the password for pay flow pro
        /// </summary>
        private string strPassword;

        /// <summary>
        /// Class variable to hold the Amount to charge
        /// </summary>
        private string strAmount;
        /// <summary>
        /// Class variable to hold the Credit card type
        /// </summary>
        private string strCCType;
        /// <summary>
        /// Class variable to hold the Expiration date of the credit card
        /// </summary>
        private string strExpires = "";
        /// <summary>
        /// Class variable to hold the Credit card account number
        /// </summary>
        private string strCCAccountNo;
        /// <summary>
        /// Class variable to hold the masked credit card account number
        /// </summary>
        private string strCCAccountNoShow;
        /// <summary>
        /// Class variable to hold the Street address on the credit card account
        /// </summary>
        private string strStreet;
        /// <summary>
        /// Class variable to hold the City on the credit card account
        /// </summary>
        private string strCity;
        /// <summary>
        /// Class variable to hold the Zip for the credit card account
        /// </summary>
        private string strZip;
        /// <summary>
        /// Class variable to hold the email address for payment confirmation
        /// </summary>
        private string strEmail;
        /// <summary>
        /// Class variable to hold the Bank account number
        /// </summary>
        private string strBankAccount;
        /// <summary>
        /// Class variable to hold the Bank account type
        /// </summary>
        private string strBankAccountType;
        /// <summary>
        /// Class variable to hold the ABA number
        /// </summary>
        private string strABA;
        /// <summary>
        /// Class variable to hold the Bank account name
        /// </summary>
        private string strBankAccountName;
        /// <summary>
        /// Class variable to hold the First name
        /// </summary>
        private string strFirstName;
        /// <summary>
        /// Class variable to hold the Last name
        /// </summary>
        private string strLastName;
        /// <summary>
        /// Class variable to hold the Comment 1
        /// </summary>
        private string strComment1;
        /// <summary>
        /// Class variable to hold the Comment 2
        /// </summary>
        private string strComment2;
        /// <summary>
        /// Class variable to hold the Trans code
        /// </summary>
        private string strTransCode;
        /// <summary>
        /// Class variable to hold the Transaction type
        /// </summary>
        private string strTransactionType;
        /// <summary>
        /// Class variable to hold the Parameter list
        /// </summary>
        private string strParamList;
        /// <summary>
        /// Class variable to hold the Pay flow pro return string
        /// </summary>
        private string strPFPReturnString = "";
        /// <summary>
        /// Class variable to hold the reference to the pay flow pro object
        /// </summary>
        //		private  PFProCOMLib.PNComClass pfPayFlowPro;
        //		/// <summary>
        //		/// Class variable to hold the Pay flow pro context
        //		/// </summary>
        private int iCtx1;
        /// <summary>
        /// Class variable to hold the Authorization code
        /// </summary>
        private string strAuthCode;
        /// <summary>
        /// Class variable to hold the PNREF from pay flow pro
        /// </summary>
        private string strPNREF;
        /// <summary>
        /// Class variable to hold the Response message from pay flow pro
        /// </summary>
        private string strResponseMsg = "";
        /// <summary>
        /// Class variable to hold the Result code from pay flow pro
        /// </summary>
        private string strResultCode;
        /// <summary>
        /// Class variable to hold the Result code from pay flow pro
        /// </summary>
        private int intResultCode;
        /// <summary>
        /// Class variable to hold the Error message
        /// </summary>
        private string strErrorMessage = "";
        /// <summary>
        /// Class variable to hold the Result message
        /// </summary>
        private string strResultMessage = "";
        /// <summary>
        /// Class variable to hold the Subject for the email
        /// </summary>
        private string strSubject = "";
    }
}
