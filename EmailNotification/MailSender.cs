using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newegg.FrameworkAPI.SDK.Mail;

namespace Newegg.WMS.JobConsole.EmailNotification
{
    public static class MailSender
    {
        public static bool Send(string from, string to, string cc, string subject, string body, bool isNeedLog = false)
        {
            bool isSuccessful = false;
            string errorMsg = string.Empty;

            try
            {
                MailRequest request = new MailRequest();
                request.Subject = subject;
                request.From = from;
                request.To = to;
                request.CC = cc;
                request.Body = body;
                request.ContentType = MailContentType.Text;
                //request.IsNeedLog = isNeedLog;
                request.Priority = MailPriority.Low;
                request.MailType = MailType.Smtp;
                request.SmtpSetting = new SmtpSetting
                {
                    BodyEncoding = MailEncoding.UTF8,
                    SubjectEncoding = MailEncoding.UTF8,
                };

                isSuccessful = Newegg.FrameworkAPI.SDK.Mail.MailSender.Send(request).IsSendSuccess;
                return isSuccessful;
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return false;
            }
            finally
            {
                if (isNeedLog || !isSuccessful)
                {
                    MailSender.WriteEmailLog(from, to, cc, subject, body, isSuccessful, errorMsg);
                }
            }
        }

        private static void WriteEmailLog(string from, string to, string cc, string subject, string body, bool isSuccessful, string errorMsg)
        {
            Newegg.Framework.Tools.Log.Logger.WriteLog(new Framework.Tools.Log.LogEntry()
            {
                ID = DateTime.Now.ToString(),
                ExtendedProperties = new List<Framework.Tools.Log.ExtendProperty>() { 
                            new Framework.Tools.Log.ExtendProperty() { Key = "From", Value = from } ,
                            new Framework.Tools.Log.ExtendProperty() { Key = "To", Value = to } ,
                            new Framework.Tools.Log.ExtendProperty() { Key = "Cc", Value =cc } ,
                            new Framework.Tools.Log.ExtendProperty() { Key = "Subject", Value = subject } ,
                            new Framework.Tools.Log.ExtendProperty() { Key = "Body", Value = body } ,
                            new Framework.Tools.Log.ExtendProperty() { Key = "IsSuccessful", Value =isSuccessful.ToString() } ,
                            new Framework.Tools.Log.ExtendProperty() { Key = "ErrorMsg", Value = errorMsg} 
                        }
            });
        }
    }
}
