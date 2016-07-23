using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Newegg.WMS.JobConsole.EmailNotification.EmailConfiguration
{
    [ConfigurationCollection(typeof(EmailTypeSection), AddItemName = "EmailType")]
    public class EmailTypeCollection : ConfigurationElementCollection
    {
        public EmailTypeSection this[int index]
        {
            get
            {
                return (EmailTypeSection)base.BaseGet(index);
            }
        }

        public EmailTypeSection this[string name]
        {
            get
            {
                return (EmailTypeSection)base.BaseGet(name);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new EmailTypeSection();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as EmailTypeSection).Name;
        }
    }
}
