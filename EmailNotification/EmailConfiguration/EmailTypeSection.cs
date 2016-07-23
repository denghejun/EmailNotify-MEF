using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Newegg.WMS.JobConsole.EmailNotification.EmailConfiguration
{
    public class EmailTypeSection : ConfigurationSection
    {
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get
            {
                return this["name"].ToString();
            }
            set
            {
                this["name"] = value;
            }
        }

        [ConfigurationProperty("class", IsRequired = false)]
        public string Class
        {
            get
            {
                return this["class"].ToString();
            }
            set
            {
                this["class"] = value;
            }
        }

        [ConfigurationProperty("fromAddress", IsRequired = false)]
        public string FromAddress
        {
            get
            {
                return this["fromAddress"].ToString();
            }
            set
            {
                this["fromAddress"] = value;
            }
        }

        [ConfigurationProperty("toAddress", IsRequired = false)]
        public string ToAddress
        {
            get
            {
                return this["toAddress"].ToString();
            }
            set
            {
                this["toAddress"] = value;
            }
        }

        [ConfigurationProperty("ccAddress", IsRequired = false)]
        public string CcAddress
        {
            get
            {
                return this["ccAddress"].ToString();
            }
            set
            {
                this["ccAddress"] = value;
            }
        }

        [ConfigurationProperty("sqlQuery", IsRequired = false)]
        public string SqlQuery
        {
            get
            {
                return this["sqlQuery"].ToString();
            }
            set
            {
                this["sqlQuery"] = value;
            }
        }

        [ConfigurationProperty("Subject", IsRequired = false)]
        public SubjectElement Subject
        {
            get
            {
                return this["Subject"] as SubjectElement;
            }
        }

        [ConfigurationProperty("Body", IsRequired = false)]
        public BodyElement Body
        {
            get
            {
                return this["Body"] as BodyElement;
            }
        }
    }
}
