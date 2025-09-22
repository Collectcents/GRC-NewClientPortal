using Microsoft.AspNetCore.Http;
using System.IO;

namespace GRC_NewClientPortal.Models.GRCEmail
{
    public static class FileValidator
    {
        public static bool ValidMIMEType(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return false;

            string content;
            string fileExt = Path.GetExtension(file.FileName).Replace(".", "").ToUpper();

            // Use OpenReadStream() in ASP.NET Core
            using (StreamReader reader = new StreamReader(file.OpenReadStream()))
            {
                // Read only the first 1KB (avoid loading huge files)
                char[] buffer = new char[1024];
                int read = reader.Read(buffer, 0, buffer.Length);
                content = new string(buffer, 0, read).ToUpper();
            }

            if (content.Contains("PDF") ||
                content.Contains("XL/WORKBOOK.XMLPK") ||
                content.Contains("WORD/SETTINGS.XMLPK") ||
                content.Contains("MICROSOFT EXCEL 2003 WORKSHEET") ||
                content.Contains("WORKSHEETS") ||
                content.Contains("MICROSOFT WORD 97-2003 DOCUMENT") ||
                fileExt == "TXT" ||
                fileExt == "CSV" ||
                fileExt == "ZIP")
            {
                return true;
            }

            return false;
        }
    }
}
