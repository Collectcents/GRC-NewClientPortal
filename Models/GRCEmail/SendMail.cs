using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GRC_NewClientPortal.Models.GRCEmail
{
    /// <summary>
    /// Modernized email sending service
    /// </summary>
    public class SendMail
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<SendMail> _logger;
        private readonly string _connectionString;
        private readonly string _emailRecipientSeparator;

        public SendMail(IConfiguration configuration, ILogger<SendMail> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _connectionString = _configuration.GetConnectionString("DefaultDataSource") ?? throw new Exception("Connection string not found");
            _emailRecipientSeparator = _configuration["EmailRecepientsSperator"] ?? ";";
        }

        /// <summary>
        /// Send an email asynchronously
        /// </summary>
        public async Task SendEmailAsync(
            string to,
            string from,
            string attachments,
            string cc,
            string bcc,
            string subject,
            string body,
            string area)
        {
            bool emailSentStatus = true;

            try
            {
                // pass logger as ILogger<ENTSendEmailManager>
                using var emmLoggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
                var emmLogger = emmLoggerFactory.CreateLogger<ENTSendEmailManager>();

                var emm = new ENTSendEmailManager(_configuration, emmLogger);

                if (!string.IsNullOrWhiteSpace(attachments) && File.Exists(attachments))
                {
                    byte[] fileData = await File.ReadAllBytesAsync(attachments);
                    emailSentStatus = await emm.SendMailWithAttachmentAsync(
                        from, to, bcc, cc, subject, body, fileData, Path.GetFileName(attachments), _emailRecipientSeparator);
                }
                else
                {
                    emailSentStatus = await emm.SendMailAsync(from, to, bcc, cc, subject, body, _emailRecipientSeparator);
                }
            }
            catch (Exception ex)
            {
                emailSentStatus = false;
                _logger.LogError(ex, "Error sending email");
            }

            // Store email information in database
            await StoreEmailInfoAsync(to, from, attachments, cc, bcc, subject, body, area, emailSentStatus);
        }

        /// <summary>
        /// Store email information in SQL Server asynchronously
        /// </summary>
        private async Task StoreEmailInfoAsync(
            string to,
            string from,
            string attachments,
            string cc,
            string bcc,
            string subject,
            string body,
            string area,
            bool emailSentStatus)
        {
            if (string.IsNullOrWhiteSpace(area)) return;

            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand("p_cli_brw_Store_Email_Info", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@EmailTo", to);
                cmd.Parameters.AddWithValue("@EmailFrom", from);
                cmd.Parameters.AddWithValue("@AttachmentFileName", string.IsNullOrWhiteSpace(attachments) ? "" : Path.GetFileName(attachments));
                cmd.Parameters.AddWithValue("@EmailCC", cc);
                cmd.Parameters.AddWithValue("@EmailBcc", bcc);
                cmd.Parameters.AddWithValue("@EmailSubject", subject);
                cmd.Parameters.AddWithValue("@EmailBody", body);
                cmd.Parameters.AddWithValue("@Area", area);
                cmd.Parameters.AddWithValue("@DateEmailSent", DateTime.Now);
                cmd.Parameters.AddWithValue("@EmailSentStatus", emailSentStatus);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to store email info for area {Area}", area);
            }
        }
        public class BorrowerPaymentInfo
        {
            // Borrower info
            public string LastName { get; set; }
            public string FirstName { get; set; }
            public string Address1 { get; set; }
            public string Address2 { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string Zip { get; set; }
            public string Country { get; set; }
            public string Phone { get; set; }
            public string SSN { get; set; }
            public string GRCFileNumber { get; set; }
            public string Email { get; set; }

            // Employer info
            public string EmployerName { get; set; }
            public string EmployerAddress1 { get; set; }
            public string EmployerAddress2 { get; set; }
            public string EmployerCity { get; set; }
            public string EmployerState { get; set; }
            public string EmployerZip { get; set; }
            public string EmployerCountry { get; set; }
            public string EmployerPhone { get; set; }

            // Payment info
            public string PaymentMethod { get; set; }

            // ACH / EFT
            public string BankAccountHolder { get; set; }
            public string BankAccountNumber { get; set; }
            public string AccountType { get; set; }
            public string ABARoutingNumber { get; set; }
            public string AmountPaid { get; set; }

            // Credit card
            public string CreditCardType { get; set; }
            public string CardNumber { get; set; }
            public string NameOnCard { get; set; }
            public string ExpiryMonth { get; set; }
            public string ExpiryYear { get; set; }
            public string BillingAddress1 { get; set; }
            public string BillingAddress2 { get; set; }
            public string BillingCity { get; set; }
            public string BillingState { get; set; }
            public string BillingZip { get; set; }
            public string BillingCountry { get; set; }
            public string BillingAmount { get; set; }
        }

        public class SendMailService
        {
            private readonly IConfiguration _config;
            private readonly ILogger<SendMailService> _logger;
            private readonly string _emailRecipientSeparator;

            public SendMailService(IConfiguration config, ILogger<SendMailService> logger)
            {
                _config = config;
                _logger = logger;
                _emailRecipientSeparator = _config["EmailRecepientsSperator"] ?? ";";
            }

            public async Task<bool> SendBorrowerPaymentEmailAsync(BorrowerPaymentInfo info)
            {
                string emailTo = _config["EmailTo_Bwr_PayOnline"];
                string emailFrom = _config["EmailFrom_Bwr_PayOnline"];
                string subject = "Borrower Online Payment";
                string cc = _config["EmailCC_Bwr_PayOnline"];
                string bcc = _config["EmailBcc_Bwr_PayOnline"];
                string attachments = ""; // no file attachment here

                var body = BuildBorrowerPaymentBody(info);

                return await SendEmailAsync(emailFrom, emailTo, cc, bcc, subject, body, attachments);
            }

            private string BuildBorrowerPaymentBody(BorrowerPaymentInfo info)
            {
                var sb = new StringBuilder();

                sb.AppendLine($"Last Name  : {info.LastName}");
                sb.AppendLine($"First Name : {info.FirstName}");
                sb.AppendLine($"Address 1  : {info.Address1}");
                sb.AppendLine($"Address 2  : {info.Address2}");
                sb.AppendLine($"City       : {info.City}");
                sb.AppendLine($"State      : {info.State}");
                sb.AppendLine($"Zip        : {info.Zip}");
                sb.AppendLine($"Country    : {info.Country}");
                sb.AppendLine($"Phone      : {info.Phone}");
                sb.AppendLine($"Social Security No.: {info.SSN}");
                sb.AppendLine($"GRC File Number    : {info.GRCFileNumber}");
                sb.AppendLine($"Email      : {info.Email}");
                sb.AppendLine(new string('-', 66));
                sb.AppendLine("Place of Employment");
                sb.AppendLine(new string('-', 66));
                sb.AppendLine($"Name       : {info.EmployerName}");
                sb.AppendLine($"Address 1  : {info.EmployerAddress1}");
                sb.AppendLine($"Address 2  : {info.EmployerAddress2}");
                sb.AppendLine($"City       : {info.EmployerCity}");
                sb.AppendLine($"State      : {info.EmployerState}");
                sb.AppendLine($"Zip        : {info.EmployerZip}");
                sb.AppendLine($"Country    : {info.EmployerCountry}");
                sb.AppendLine($"Work Phone : {info.EmployerPhone}");
                sb.AppendLine(new string('-', 66));
                sb.AppendLine($"Payment Type : {info.PaymentMethod}");
                sb.AppendLine(new string('-', 66));

                if (info.PaymentMethod == "Electronic Fund Transfer/ ACH")
                {
                    sb.AppendLine($"Account Holder: {info.BankAccountHolder}");
                    sb.AppendLine($"Account Number: {info.BankAccountNumber}");
                    sb.AppendLine($"Account Type: {info.AccountType}");
                    sb.AppendLine($"ABA routing number: {info.ABARoutingNumber}");
                    sb.AppendLine($"Amount Paid: {info.AmountPaid}");
                }
                else if (info.PaymentMethod == "Credit Card Transaction")
                {
                    sb.AppendLine($"Credit Card: {info.CreditCardType}");
                    sb.AppendLine($"Card Number : {info.CardNumber}");
                    sb.AppendLine($"Name on Card: {info.NameOnCard}");
                    sb.AppendLine($"Expires     : {info.ExpiryMonth}/{info.ExpiryYear}");
                    sb.AppendLine(new string('-', 66));
                    sb.AppendLine("Billing Address");
                    sb.AppendLine(new string('-', 66));
                    sb.AppendLine($"Address 1   : {info.BillingAddress1}");
                    sb.AppendLine($"Address 2   : {info.BillingAddress2}");
                    sb.AppendLine($"City        : {info.BillingCity}");
                    sb.AppendLine($"State       : {info.BillingState}");
                    sb.AppendLine($"Zip         : {info.BillingZip}");
                    sb.AppendLine($"Country     : {info.BillingCountry}");
                    sb.AppendLine($"Amount Paid : {info.BillingAmount}");
                }

                return sb.ToString();
            }

            public async Task<bool> SendEmailAsync(string from, string to, string cc, string bcc, string subject, string body, string attachmentPath)
            {
                try
                {
                    using var message = new MailMessage();
                    message.From = new MailAddress(from);
                    message.To.Add(to);
                    if (!string.IsNullOrWhiteSpace(cc)) message.CC.Add(cc);
                    if (!string.IsNullOrWhiteSpace(bcc)) message.Bcc.Add(bcc);
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = false;
                    message.BodyEncoding = Encoding.UTF8;

                    if (!string.IsNullOrWhiteSpace(attachmentPath) && File.Exists(attachmentPath))
                        message.Attachments.Add(new Attachment(attachmentPath));

                    using var client = new SmtpClient(_config["SmtpServer"])
                    {
                        Port = int.Parse(_config["SmtpServerport"]),
                        Credentials = new NetworkCredential(_config["SmtpUser"], _config["SmtpPass"]),
                        EnableSsl = true
                    };
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    await client.SendMailAsync(message);
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to send email");
                    return false;
                }
            }

            public async Task SendMailWithMultipleAttachmentsAsync(string to, string from, string[] attachments, string cc, string bcc, string subject, string body)
            {
                try
                {
                    using var message = new MailMessage();
                    message.From = new MailAddress(from);
                    message.To.Add(to);
                    if (!string.IsNullOrWhiteSpace(cc)) message.CC.Add(cc);
                    if (!string.IsNullOrWhiteSpace(bcc)) message.Bcc.Add(bcc);
                    message.Subject = subject;
                    message.Body = body;
                    message.BodyEncoding = Encoding.UTF8;
                    message.IsBodyHtml = false;

                    foreach (var file in attachments)
                    {
                        if (!string.IsNullOrWhiteSpace(file) && File.Exists(file))
                            message.Attachments.Add(new Attachment(file));
                    }

                    using var client = new SmtpClient(_config["SmtpServer"])
                    {
                        Port = int.Parse(_config["SmtpServerport"]),
                        Credentials = new NetworkCredential(_config["SmtpUser"], _config["SmtpPass"]),
                        EnableSsl = true
                    };
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    await client.SendMailAsync(message);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to send email with multiple attachments");
                    throw;
                }
            }
        }
    }

}




