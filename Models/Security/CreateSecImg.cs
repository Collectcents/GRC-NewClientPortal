
namespace GRC_NewClientPortal.Models.Security
{
	/// <summary>
	/// Class encapsulating the calls to ASPImage control used for generating the security image.
	/// </summary>
	public class CreateSecImg
	{
		/// <summary>
		/// Generates the security image by making a call to the ASPImage dll
		/// </summary>
		/// <param name="FileName">File name of the image</param>
		/// <returns>Status message from the image creation process</returns>
/*		public static string SecCode(string FileName)
		{
			string strMessage;
			ASPImage.Image Image = new ASPImage.Image();
			Image.PixelFormat = 3;
			Image.LoadImage(System.Configuration.ConfigurationSettings.AppSettings["ImageBackGround"]);
			Image.FontColor=000000;//"vbBlack"
			Image.Italic =false;
			Image.Bold=true;
			Image.FontName = "Arial";
			Image.FontSize = 25;
			Image.PadSize = 10;
				
			strMessage = Common.gen_pass(4);
			Image.MaxX = Image.TextWidth (strMessage);
			Image.MaxY = Image.TextHeight (strMessage);
			Image.Twist(100);  
			Image.TextAngle = 6;
			Image.TextOut(strMessage, Image.X, Image.Y, false);
			Image.FileName =FileName;
			if (Image.SaveImage())
			{
				return strMessage;	
			}
			else
			{
				return "Could not Save Image";
			}
					
		}
		*/

	}
}
