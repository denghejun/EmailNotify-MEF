using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Newegg.WMS.JobConsole.EmailNotification.EmailConfiguration
{
    public class SubjectElement : ConfigurationElement
    {
        [ConfigurationProperty("data", IsRequired = false)]
        public string Text { get; set; }

        protected override void DeserializeElement(System.Xml.XmlReader reader, bool serializeCollectionKey)
        {
            Text = reader.ReadElementContentAs(typeof(string), null) as string;
        }

        protected override bool SerializeElement(System.Xml.XmlWriter writer, bool serializeCollectionKey)
        {
            if (writer != null)
                writer.WriteCData(Text);
            return true;
        }
    }
}
