using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newegg.WMS.JobConsole.EmailNotificationContract;
using System.ComponentModel.Composition;

namespace Newegg.WMS.JobConsole.EmailNotificationService
{
    [Export("Newegg.WMS.JobConsole.EmailNotificationService.DefaultEmailContentService,Newegg.WMS.JobConsole.EmailNotificationService", typeof(IEmailContentProviderContract))]
    public class DefaultEmailContentService : IEmailContentProviderContract
    {
        public List<EmailContent> CreateEmailContent(EmailType config)
        {
            var contents = new List<EmailContent>()
            {
                new EmailContent()
                {
                 Body=config.Body,
                 Subject=config.Subject,
                 Cc=config.Cc,
                 From=config.From,
                 To=config.To,
                }
            };

            return contents;
        }

        public void EmailSendCompleted(EmailContent content, bool isSuccessful)
        {
            // DO NOTHING
        }
    }
}
