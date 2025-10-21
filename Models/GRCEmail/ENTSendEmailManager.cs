using System;
using System.Net.Mail;
using System.Net;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GRC_NewClientPortal.Models.GRCEmail
{
    public class ENTSendEmailManager
    {
        // Enum types for Email Content Type
        public enum EmailContentType
        {
            PlainText,
            XMLText,
            HtmlText,
            EnrichedText,
            SGMLText,
            TabSeparatedValues
        }

        // Email properties
        public string EmailTo { get; set; }
        public string EmailFrom { get; set; }
        public string EmailCC { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
        public string EmailReplyTo { get; set; }
        public string ImportanceFlag { get; set; }
        public string EmailRecipientsSeparator { get; set; }
        public EmailContentType ContentType { get; set; }

        private readonly IConfiguration _configuration;
        private readonly ILogger<ENTSendEmailManager> _logger;
        private IConfiguration configuration;
        private ILogger<SendMail> logger;

        // Constructor with dependency injection
        public ENTSendEmailManager(IConfiguration configuration, ILogger<ENTSendEmailManager> logger)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            ContentType = EmailContentType.PlainText;
            EmailRecipientsSeparator = _configuration["EmailRecepientsSeparator"] ?? ";";
        }

        public ENTSendEmailManager(IConfiguration configuration, ILogger<SendMail> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
        }

        #region SettingsProperties

        public string AppName => _configuration["EmailManager_AppName"];
        public string AppComponent => _configuration["EmailManager_AppComponent"];
        public string AppVersion => _configuration["EmailManager_AppVersion"];
        public string CertSubjectName => _configuration["EmailManager_CertSubjectName"];

        #endregion

        /// <summary>
        /// Get the content type of the email
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        //protected object GetContentType(EmailContentType contentType)
        //{
        //    try
        //    {
        //        switch (contentType)
        //        {
        //            case EmailContentType.HtmlText:
        //                return ENTSendEmailRequestContentType.HtmlText;
        //            case EmailContentType.PlainText:
        //            default:
        //                return ENTSendEmailRequestContentType.PlainText;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error in ENTSendEmailManager:GetContentType");
        //        throw new Exception("Exception in ENTSendEmailManager:GetContentType", ex);
        //    }
        //}

        public async Task<bool> SendMailAsync(
            string fromList,
            string toList,
            string bccList,
            string ccList,
            string emailSubject,
            string emailBody,
            string emailRecipientsSeparator)
        {
            try
            {
                bool result = true;

                using var email = new MailMessage
                {
                    From = new MailAddress(fromList),
                    Subject = emailSubject,
                    Body = emailBody,
                    BodyEncoding = Encoding.UTF8,
                    IsBodyHtml = true
                };

                // Add To, CC, BCC recipients
                if (!string.IsNullOrWhiteSpace(toList))
                    email.To.Add(toList);

                if (!string.IsNullOrWhiteSpace(ccList))
                    email.CC.Add(ccList);

                if (!string.IsNullOrWhiteSpace(bccList))
                    email.Bcc.Add(bccList);

                // SMTP configuration from appsettings
                string smtpServer = _configuration["SmtpServer"];
                int smtpPort = Convert.ToInt32(_configuration["SmtpServerPort"]);
                string smtpUser = _configuration["SmtpUser"];
                string smtpPassword = _configuration["SmtpPass"];

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                using var client = new SmtpClient(smtpServer, smtpPort)
                {
                    Credentials = new NetworkCredential(smtpUser, smtpPassword),
                    EnableSsl = true
                };

                await client.SendMailAsync(email);
                _logger.LogInformation("Email sent successfully to {Recipients}", toList);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in ENTSendEmailManager: SendMailAsync");
                throw new Exception("Exception in ENTSendEmailManager: SendMailAsync", ex);
            }
        }

        public async Task<bool> SendMailWithAttachmentAsync(
            string fromList,
            string toList,
            string bccList,
            string ccList,
            string emailSubject,
            string emailBody,
            byte[] attachment,
            string fileName,
            string emailRecipientsSeparator)
        {
            try
            {
                bool result = true;
                EmailRecipientsSeparator = emailRecipientsSeparator;

                if (!string.IsNullOrWhiteSpace(toList))
                {
                    var recipients = toList.Contains(emailRecipientsSeparator)
                        ? toList.Split(emailRecipientsSeparator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                        : new[] { toList };

                    foreach (var recipient in recipients)
                    {
                        result = await ENTSendEmailCallAsync(
                            fromList,
                            recipient.Trim(),
                            bccList,
                            ccList,
                            emailSubject,
                            emailBody,
                            attachment,
                            fileName);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in ENTSendEmailManager: SendMailWithAttachmentAsync");
                throw new Exception("Exception in ENTSendEmailManager: SendMailWithAttachmentAsync", ex);
            }
        }

        private async Task<bool> ENTSendEmailCallAsync(
            string fromList,
            string toList,
            string bccList,
            string ccList,
            string emailSubject,
            string emailBody,
            byte[] attachment,
            string fileName)
        {
            // If WSMEnabled is true, skip sending email
            if (bool.TryParse(_configuration["WSMEnabled"], out bool wsmEnabled) && wsmEnabled)
                return false;

            bool result = true;

            using var email = new MailMessage
            {
                From = new MailAddress(fromList),
                Subject = emailSubject,
                Body = emailBody,
                BodyEncoding = Encoding.UTF8,
                IsBodyHtml = true
            };

            if (!string.IsNullOrWhiteSpace(toList))
                email.To.Add(toList);
            if (!string.IsNullOrWhiteSpace(ccList))
                email.CC.Add(ccList);
            if (!string.IsNullOrWhiteSpace(bccList))
                email.Bcc.Add(bccList);

            if (attachment != null && attachment.Length > 0)
            {
                using var memoryStream = new MemoryStream(attachment);
                email.Attachments.Add(new Attachment(memoryStream, fileName));
            }

            try
            {
                string smtpServer = _configuration["SmtpServer"];
                int smtpPort = Convert.ToInt32(_configuration["SmtpServerPort"]);
                string smtpUser = _configuration["SmtpUser"];
                string smtpPassword = _configuration["SmtpPass"];

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                using var client = new SmtpClient(smtpServer, smtpPort)
                {
                    Credentials = new NetworkCredential(smtpUser, smtpPassword),
                    EnableSsl = true
                };

                await client.SendMailAsync(email);
                _logger.LogInformation("Email sent to {Recipient}", toList);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in ENTSendEmailManager: ENTSendEmailCallAsync");
                throw new Exception("Exception in ENTSendEmailManager: ENTSendEmailCallAsync", ex);
            }
        }

        public async Task<bool> SendEmailWithMultipleAttachmentsAsync(
    string toList,
    string fromList,
    string[] attachmentList,
    string ccList,
    string bccList,
    string emailSubject,
    string emailBody,
    string emailRecipientsSeparator)
        {
            // Check if WSM is enabled
            if (bool.TryParse(_configuration["WSMEnabled"], out bool wsmEnabled) && wsmEnabled)
            {
                EmailRecipientsSeparator = emailRecipientsSeparator;
                return false;
            }

            bool result = true;

            using (var email = new MailMessage())
            {
                email.From = new MailAddress(fromList);
                email.To.Add(toList);

                if (!string.IsNullOrWhiteSpace(ccList))
                    email.CC.Add(ccList);

                if (!string.IsNullOrWhiteSpace(bccList))
                    email.Bcc.Add(bccList);

                email.Subject = emailSubject;
                email.Body = emailBody;
                email.BodyEncoding = Encoding.UTF8;
                email.IsBodyHtml = true;

                // Add attachments
                if (attachmentList?.Length > 0)
                {
                    foreach (var filePath in attachmentList)
                    {
                        if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
                        {
                            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                            email.Attachments.Add(new Attachment(fileStream, Path.GetFileName(filePath)));
                        }
                    }
                }

                // Configure SMTP
                var smtpServer = _configuration["SmtpServer"];
                var smtpPort = int.Parse(_configuration["SmtpServerport"]);
                var smtpUser = _configuration["SmtpUser"];
                var smtpPass = _configuration["SmtpPass"];

                using (var client = new SmtpClient(smtpServer, smtpPort))
                {
                    client.Credentials = new NetworkCredential(smtpUser, smtpPass);
                    client.EnableSsl = true;

                    try
                    {
                        await client.SendMailAsync(email);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Exception in ENTSendEmailManager : SendEmailWithMultipleAttachmentsAsync", ex);
                    }
                }
            }

            return result;
        }

    }

}
