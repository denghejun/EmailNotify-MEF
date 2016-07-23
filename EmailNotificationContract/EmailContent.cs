using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Newegg.WMS.JobConsole.EmailNotificationContract
{
    public class EmailContent
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Cc { get; set; }
        public object State { get; set; }
        public EmailType EmailType { get; set; }
    }
}
