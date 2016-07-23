using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newegg.WMS.JobConsole.EmailNotificationContract;
using Newegg.Oversea.DataAccess;
using Newegg.WMS.JobConsole.EmailNotificationService.Models;

namespace Newegg.WMS.JobConsole.EmailNotificationService.Biz
{
    public static class CarrierEmailContentBiz
    {
        public static List<EmailContent> CreateEmailContent(EmailType config)
        {
            // Get not sent tv tracking info.
            var models = CarrierEmailContentBiz.GetNotSendTVTrackingInfo(config);
            if (models == null || models.Count == 0)
            {
                return null;
            }

            // convert to email contents.
            var contents = from item in models
                           select new EmailContent()
                           {
                               Body = string.Format(config.Body, item.Description, item.UnitPrice, item.TrackingNumber),
                               Cc = config.Cc,
                               To = config.To,
                               From = config.From,
                               Subject = string.Format(config.Subject, item.SONumber),
                               State = item // To keep current data state, will use for record send history function.
                           };

            return contents.ToList();
        }

        private static List<TVTrackingInfo> GetNotSendTVTrackingInfo(EmailType config)
        {
            // Get all tv tracking info.
            var cmdTvTrackingInfo = DataCommandManager.CreateCustomDataCommandFromConfig(config.ScriptKey);
            var models = cmdTvTrackingInfo.ExecuteEntityList<TVTrackingInfo>();
            if (models == null || models.Count == 0)
            {
                return null;
            }

            // Get all sent histories by current email type.
            var sentTvTrackingHistories = EmailContentSentHistoryBiz.GetEmailSentHistories(new EmailNotificationHistory()
                 {
                     EmailTypeName = config.Name,
                 });

            if (sentTvTrackingHistories == null || sentTvTrackingHistories.Count == 0)
            {
                return models;
            }

            // Exclude sent tracking info.
            models = models.FindAll(o => !sentTvTrackingHistories.Any(p => p.BizKey.Equals(o.TrackingNumber, StringComparison.InvariantCultureIgnoreCase)));
            return models;
        }
    }
}
