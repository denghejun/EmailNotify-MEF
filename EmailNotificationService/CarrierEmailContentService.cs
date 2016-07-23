using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newegg.WMS.JobConsole.EmailNotificationContract;
using System.ComponentModel.Composition;
using Newegg.Oversea.DataAccess;
using Newegg.WMS.JobConsole.EmailNotificationService.Models;
using Newegg.WMS.JobConsole.EmailNotificationService.Biz;

namespace Newegg.WMS.JobConsole.EmailNotificationService
{
    [Export("Newegg.WMS.JobConsole.EmailNotificationService.CarrierEmailContentService,Newegg.WMS.JobConsole.EmailNotificationService", typeof(IEmailContentProviderContract))]
    public class CarrierEmailContentService : IEmailContentProviderContract
    {
        public List<EmailContent> CreateEmailContent(EmailType config)
        {
            return CarrierEmailContentBiz.CreateEmailContent(config);
        }

        public void EmailSendCompleted(EmailContent content, bool isSuccessful)
        {
            if (!isSuccessful || content == null || content.State as TVTrackingInfo == null || content.EmailType == null)
            {
                return;
            }

            string trackingNumber = (content.State as TVTrackingInfo).TrackingNumber;
            string eamilTypeName = content.EmailType.Name;
            EmailContentSentHistoryBiz.RecordEmailSentHistory(trackingNumber, eamilTypeName);
        }
    }
}
