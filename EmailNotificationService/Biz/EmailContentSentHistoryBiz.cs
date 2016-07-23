using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newegg.WMS.JobConsole.EmailNotificationService.Models;
using Newegg.Oversea.DataAccess;

namespace Newegg.WMS.JobConsole.EmailNotificationService.Biz
{
    public static class EmailContentSentHistoryBiz
    {
        private static readonly string USER_SYSTEM = "EmailSystem";

        public static void RecordEmailSentHistory(string bizKey, string emailTypeName)
        {
            if (string.IsNullOrWhiteSpace(bizKey) || string.IsNullOrWhiteSpace(emailTypeName))
            {
                return;
            }

            EmailNotificationHistory record = new EmailNotificationHistory()
            {
                BizKey = bizKey,
                EmailTypeName = emailTypeName,
                InUser = EmailContentSentHistoryBiz.USER_SYSTEM,
                LastEditUser = EmailContentSentHistoryBiz.USER_SYSTEM
            };

            var command = DataCommandManager.CreateCustomDataCommandFromConfig("EmailContentSentHistory.InsertEamilSentHistory");
            command.SetParameterValue("@EmailTypeName", record.EmailTypeName);
            command.SetParameterValue("@BizKey", record.BizKey);
            command.SetParameterValue("@InUser", record.InUser);
            command.SetParameterValue("@LastEditUser", record.LastEditUser);
            command.ExecuteNonQuery();
        }

        public static List<EmailNotificationHistory> GetEmailSentHistories(EmailNotificationHistory condtion)
        {
            var command = DataCommandManager.CreateCustomDataCommandFromConfig("EmailContentSentHistory.GetEamilSentHistories");
            command.SetParameterValue("@FromDate", condtion.FromDate);
            command.SetParameterValue("@ToDate", condtion.ToDate);
            command.SetParameterValue("@EmailTypeName", condtion.EmailTypeName);
            return command.ExecuteEntityList<EmailNotificationHistory>();
        }
    }
}
