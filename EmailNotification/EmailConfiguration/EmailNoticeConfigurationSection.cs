using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Newegg.WMS.JobConsole.EmailNotification.EmailConfiguration
{
    public class EmailNoticeConfigurationSection : ConfigurationSection
    {
        private static readonly ConfigurationProperty s_property = new ConfigurationProperty(string.Empty, typeof(EmailTypeCollection), null,
                                  ConfigurationPropertyOptions.IsDefaultCollection);

        [ConfigurationProperty("", Options = ConfigurationPropertyOptions.IsDefaultCollection)]
        public EmailTypeCollection EmailTypes
        {
            get
            {
                return (EmailTypeCollection)base[s_property];
            }
        }
    }
}
