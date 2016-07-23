using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Newegg.WMS.JobConsole.EmailNotificationContract
{
    public class EmailType
    {
        public string Name { get; set; }
        public string Class { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Cc { get; set; }
        public string ScriptKey { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
