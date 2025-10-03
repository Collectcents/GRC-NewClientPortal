using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Text;
using Microsoft.AspNetCore.Http.Extensions;

namespace GRC_NewClientPortal.Models.Domain
{
    public class Common
    {
        private readonly IConfiguration _configuration;
        public Common(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        /// <summary>
		/// Performs initial validation for number of characters in the input string from the web user.
		/// </summary>
		/// <param name="sInput">The input string</param>
		/// <param name="sType">Type of the input string</param>
		/// <returns>Boolean value indicating success or failure of the validation</returns>
		public static bool TestLength(string sInput, string sType)
        {
            int iMin, iMax;
            switch (sType)
            {
                case "Code":
                    iMin = 4;
                    iMax = 4;
                    break;
                case "EDP":
                    iMin = 11;
                    iMax = 11;
                    break;
                case "ID":
                    iMin = 6;
                    iMax = 40;
                    break;
                case "Pwd":
                    iMin = 6;
                    iMax = 12;
                    break;
                case "SSN4":
                    iMin = 4;
                    iMax = 4;
                    break;
                case "ZIP5":
                    iMin = 5;
                    iMax = 5;
                    break;
                case "ABA":
                    iMin = 9;
                    iMax = 9;
                    break;
                case "CC": // Mastercard 16 digits
                    iMin = 13; // Visa 13 or 16 digits
                    iMax = 16; // Amex 15 digits
                    break;   // Discover 16 digits
                default:
                    iMin = 0;
                    iMax = 0;
                    break;
            }
            return (sInput.Length >= iMin && sInput.Length <= iMax);
        }


        /// <summary>
        /// Performs initial validation for acceptable characters in the input string from the web user.
        /// </summary>
        /// <param name="sInput">The input string</param>
        /// <param name="sType">Type of the input string</param>
        /// <returns>Boolean value indicating success or failure of the validation</returns>
        public static bool TestChars(string sInput, string sType)
        {
            string sValid;
            int i;
            string c;
            switch (sType)
            {
                case "Code":
                    sValid = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                    break;
                case "EDP":
                    sValid = "0123456789";
                    break;
                case "ID":
                    sValid = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ._@";
                    break;
                case "Pwd":
                    sValid = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
                    break;
                case "SSN4":
                    sValid = "0123456789";
                    break;
                case "ZIP5":
                    sValid = "0123456789";
                    break;
                case "ABA":
                    sValid = "0123456789";
                    break;
                case "CC":
                    sValid = "0123456789";
                    break;
                default:
                    sValid = "";
                    break;
            }
            for (i = 0; i < sInput.Length; i++)
            {
                c = sInput.Substring(i, 1);
                if (sValid.IndexOf(c) == -1)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Returns the encrypted value for the input string
        /// </summary>
        /// <param name="sInput">String to encrypt</param>
        /// <returns>Encrypted string</returns>
        public static string GetHash(string sInput)
        {
            return Convert.ToBase64String((new SHA1CryptoServiceProvider()).ComputeHash(Encoding.ASCII.GetBytes(sInput)));
        }


        /// <summary>
        /// Returns decimal data type for the input string
        /// </summary>
        /// <param name="value">String to convert</param>
        /// <returns>Decimal value</returns>
        public static Decimal ToDecimal(string value)
        {
            //@@@naz 02262010 code change
            //try
            //{
            return Convert.ToDecimal(value.Trim());
            //}
            //catch
            //{
            //	return 0M;
            //}
            //
            //@@@naz 02262010 end code change
        }


        /// <summary>
        /// Converts input value to currency data type
        /// </summary>
        /// <param name="value">String to convert</param>
        /// <returns>Currency equivalent</returns>
        public static string ToCurrency(string value)
        {
            try
            {
                return ToDecimal(value.Trim()).ToString("F2");
            }
            catch
            {
                return "0.00";
            }
        }

        /// <summary>
        /// Generates string of random characters and numbers
        /// </summary>
        /// <param name="max_num">Maximum characters and numbers of the random string</param>
        /// <returns>String of random characters and numbers</returns>
        public static string gen_pass(int max_num)
        {
            string output = "";
            string num;
            string[] gen_array = new string[27];
            const int MAX_Rand = 27;
            Random r = new Random();
            gen_array[0] = "3";
            gen_array[1] = "4";
            gen_array[2] = "6";
            gen_array[3] = "7";
            gen_array[4] = "8";
            gen_array[5] = "9";
            gen_array[6] = "A";
            gen_array[7] = "B";
            gen_array[8] = "C";
            gen_array[9] = "D";
            gen_array[10] = "E";
            gen_array[11] = "F";
            gen_array[12] = "G";
            gen_array[13] = "H";
            gen_array[14] = "J";
            gen_array[15] = "K";
            gen_array[16] = "M";
            gen_array[17] = "N";
            gen_array[18] = "P";
            gen_array[19] = "Q";
            gen_array[20] = "R";
            gen_array[21] = "T";
            gen_array[22] = "U";
            gen_array[23] = "V";
            gen_array[24] = "W";
            gen_array[25] = "X";
            gen_array[26] = "Y";

            while (output.ToString().Length < max_num)
            {
                num = gen_array[r.Next(MAX_Rand)];
                output = output + num;
            }
            return output;
        }

        public static string GenerateSecurityCode()
        {
            var buffer = new byte[sizeof(UInt64)];
            var cryptoRng = new RNGCryptoServiceProvider();
            cryptoRng.GetBytes(buffer);
            var num = BitConverter.ToUInt64(buffer, 0);
            var code = num % 1000000;
            return code.ToString("D6");
        }

        public static string generateStrongPassword(int max_num, string userName)
        {
            try
            {
                char[] chars = new char[70];
                chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%&".ToCharArray();
                byte[] data = new byte[1];
                RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
                crypto.GetNonZeroBytes(data);
                data = new byte[max_num];
                crypto.GetNonZeroBytes(data);
                StringBuilder result = new StringBuilder(max_num);
                foreach (byte b in data)
                {
                    result.Append(chars[b % (chars.Length)]);
                }
                string password = result.ToString();
                if (!ValidatePassword(password, userName))
                {
                    password = generateStrongPassword(max_num, userName);
                }

                return password;
            }
            catch (Exception _ex)
            {
                throw new Exception("Exception in gen_pass : ", _ex);
            }

        }

        static bool ValidatePassword(string password, string userName)
        {
            if (Regex.IsMatch(password, @"(.)\1{2,}", RegexOptions.IgnoreCase))
            {
                return false;
            }
            if (password.Length < 8)
            {
                return false;
            }

            int cnt = 0;
            if (Regex.IsMatch(password, @"^(?=.*[a-z]).+$"))
            {
                cnt = cnt + 1;
            }
            if (Regex.IsMatch(password, @"^(?=.*[A-Z]).+$"))
            {
                cnt = cnt + 1;
            }
            if (Regex.IsMatch(password, @"^(?=.*\d).+$"))
            {
                cnt = cnt + 1;
            }
            if (Regex.IsMatch(password, @"^(?=.*[!@#$%&]).+$"))
            {
                cnt = cnt + 1;
            }
            if (cnt < 3)
            {
                return false;
            }

            if (userName != null || userName != String.Empty)
            {
                string[] names = userName.Split(null);
                foreach (var item in names)
                {
                    if (item != "")
                    {
                        if (password.Contains(item))
                        {
                            return false;
                        }
                    }

                }
            }


            //password should not contain word from restricted word list
            string path = System.AppDomain.CurrentDomain.BaseDirectory;
            using (StreamReader reader = new StreamReader(path + @"/Content/500worst-contains.list"))
            {
                string[] lines = File.ReadAllLines(path + @"/Content/500worst-contains.list");
                foreach (var line in lines)
                {
                    if (line.Length > 0)
                    {
                        string passwordLower = password.ToLower();
                        if (passwordLower.Contains(line.Trim().ToLower()))
                        {
                            return false;
                        }

                    }

                }

            }

            return true;
        }

        /// <summary>
        /// Changes the protocol based on the scheme http/https
        /// </summary>
        /// <remarks>The protocol is changed if EnforceProtocol in web.config has a value of "yes"</remarks>
        /// <param name="p">Page object for which the protocol needs to be changed</param>
        /// <param name="scheme">http/https</param>
        public void FixProtocol(HttpContext context, string scheme)
        {
            // Check if EnforceProtocol setting is enabled
            if (_configuration["EnforceProtocol"]?.ToLower() == "yes")
            {
                // If the current protocol is different from the requested scheme
                UriBuilder ub = new UriBuilder(context.Request.GetDisplayUrl());

                if (ub.Scheme != scheme)
                {
                    // Redirect to the requested protocol
                    var redirectUrl = $"{scheme}://{ub.Host}{ub.Path}{ub.Query}";
                    context.Response.Redirect(redirectUrl, permanent: true);
                }
            }
        }

        public bool EnableTitaniumDebtorSearch()
        {
            // Fetch the value from the appsettings (or other sources)
            string s = _configuration["EnableTitaniumDebtorSearch"];

            if (!string.IsNullOrEmpty(s))
            {
                if (s.ToUpper() == "YES")
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Formats SNN to (xxx-xx-xxxx) format
        /// </summary>
        /// <param name="SSN">SSN string before formatting</param>
        /// <returns>formatted SSN String</returns>
        public static string FormatSSN(string SSN)
        {
            SSN = SSN.Trim();
            string FormattedSSN = SSN;
            if (SSN == "")
                SSN = "000000000";
            SSN = SSN.Replace("-", "");
            if (SSN != "" || SSN.Length != 9)
            {
                string p1 = SSN.Substring(0, 3);
                string p2 = SSN.Substring(3, 2);
                string p3 = SSN.Substring(5, 4);
                FormattedSSN = p1 + "-" + p2 + "-" + p3;
            }
            if (FormattedSSN == "")
                return SSN;
            else
                return FormattedSSN;
        }

        /// <summary>
        /// Formats Names to First Letter UpperCase the rest Lowercase
        /// </summary>
        /// <param name="Name">Name string before formatting</param>
        /// <returns>Formatted name string</returns>
        public static string FormatNames(string Name)
        {
            Name = Name.Trim();
            string FormattedName = "";
            int LastSpaceAt = 0;
            if (Name.Length != 0)
            {
                while (LastSpaceAt != -1)
                {
                    LastSpaceAt = Name.IndexOf(" ", 0);
                    string FirstLetter = Name.Substring(0, 1).ToUpper();
                    string Rest = "";
                    if (LastSpaceAt == -1)
                        Rest = Name.Substring(1, Name.Length - 1).ToLower();
                    else
                        Rest = Name.Substring(1, LastSpaceAt).ToLower();
                    Name = Name.Substring(LastSpaceAt + 1, Name.Length - LastSpaceAt - 1);
                    FormattedName = FormattedName + FirstLetter + Rest;
                }
            }
            if (FormattedName == "")
                return Name;
            else
                return FormattedName;
        }

        /// <summary>
        /// Formats Zip codes to #####-#### for zips length of 9
        /// </summary>
        /// <param name="ZIP">unformatted zip code</param>
        /// <returns>formatted zip code</returns>
        public static string FormatZip(string ZIP)
        {
            ZIP = ZIP.Trim();
            string FormattedZip = "";
            if (ZIP.Length != 0)
            {
                if (ZIP.Length == 9)
                {
                    string zip_pt1 = ZIP.Substring(0, 5);
                    string zip_pt2 = ZIP.Substring(5, 4);
                    FormattedZip = zip_pt1 + "-" + zip_pt2;
                }
            }
            if (FormattedZip == "")
                return ZIP;
            else
                return FormattedZip;
        }
    }
}
