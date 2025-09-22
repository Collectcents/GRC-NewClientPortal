using System.Text;

namespace GRC_NewClientPortal.Models.Domain
{
    public class GRCBorrowerDetails
    {
        /// <summary>
		/// This function is not being used.  
		/// </summary>
		/// <returns></returns>
		public static string LeftSideDetails()
        {

            StringBuilder sb = new StringBuilder();
            sb.Append("<tr><td width=10><img src=images/global/bulletYellowPlus.gif border=0 hspace=0 vspace=0></td>");
            sb.Append("<td><font class=\"subnav2Off\">01041312849</font></td>");
            sb.Append("</tr><tr><td width=10></td>");
            sb.Append("<td><A class=\"subnav2Off\" href=\"Borrowers_PayDetails.aspx?b=1\">Payment Details</A></td>");
            sb.Append("</tr><tr><td width=\"10\"></td>");
            sb.Append("<td><A class=\"subnav2Off\" href=\"Borrowers_Pay.aspx?b=1\">Pay Online</A></td>");
            sb.Append("</tr><tr><td width='10'></td>");
            sb.Append("<td><a href=\"Borrowers_Details.aspx?l=1\" class=\"subnav2Off\">01041312849</a></td></tr>");

            return sb.ToString();
        }
    }
}
