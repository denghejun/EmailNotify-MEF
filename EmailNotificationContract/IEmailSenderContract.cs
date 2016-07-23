using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Newegg.WMS.JobConsole.EmailNotificationContract
{
    public interface IEmailContentProviderContract
    {
        List<EmailContent> CreateEmailContent(EmailType config);
        void EmailSendCompleted(EmailContent content, bool isSuccessful);
    }
}
