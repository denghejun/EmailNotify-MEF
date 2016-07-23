using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newegg.WMS.JobConsole.EmailNotificationContract;
using Newegg.WMS.JobConsole.EmailNotification.Core;

namespace Newegg.WMS.JobConsole.EmailNotification.EmailConfiguration
{
    public static class EmailConfigurationManager
    {
        // Fields
        private static readonly string PATH_EMAILTYPE_CONFIG = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configuration\\EmailType\\EmailNotification.config");
        private static CommonConfigManager m_ConfigManager;

        // Ctor
        static EmailConfigurationManager()
        {
            EmailConfigurationManager.m_ConfigManager = new CommonConfigManager(EmailConfigurationManager.PATH_EMAILTYPE_CONFIG);
            EmailConfigurationManager.m_ConfigManager.SetEnableConfigWatcher(true);
            EmailConfigurationManager.m_ConfigManager.Changed += new FileSystemEventHandler(configManager_Changed);
            EmailConfigurationManager.ReloadConfigData();
        }

        // Event Handler
        private static void configManager_Changed(object sender, FileSystemEventArgs e)
        {
            EmailConfigurationManager.ReloadConfigData();
        }

        // Properties
        public static List<EmailType> EmailTypes { get; private set; }
        public static Queue<EmailType> EmailTypeQueue { get; private set; } // you can use List<EmailType> Or Queue<EmailType>.

        // Methods
        private static List<EmailType> Convert(EmailTypeCollection collection)
        {
            if (collection == null || collection.Count == 0)
            {
                return new List<EmailType>();
            }

            var enumerableCollection = collection.Cast<EmailTypeSection>();
            var emailTypeList = from emailType in enumerableCollection
                                select new EmailType()
                                {
                                    Body = emailType.Body.Text,
                                    To = emailType.ToAddress,
                                    Subject = emailType.Subject.Text,
                                    ScriptKey = emailType.SqlQuery,
                                    Name = emailType.Name,
                                    From = emailType.FromAddress,
                                    Cc = emailType.CcAddress,
                                    Class = emailType.Class.Replace(" ", string.Empty)
                                };

            return emailTypeList.ToList();
        }
        private static void ReloadConfigData()
        {
            if (EmailConfigurationManager.m_ConfigManager == null)
            {
                return;
            }

            EmailNoticeConfigurationSection rootSection = EmailConfigurationManager.m_ConfigManager.GetConfigurationSection<EmailNoticeConfigurationSection>("EmailNoticeConfiguration");
            if (rootSection != null)
            {
                // We must exclude the already sent email types when the *.config changed.
                List<EmailType> alreadySentEamilTypes = null;
                if (EmailConfigurationManager.EmailTypes != null && EmailConfigurationManager.EmailTypes.Count > 0)
                {
                    alreadySentEamilTypes = EmailConfigurationManager.EmailTypes.FindAll(o => !EmailConfigurationManager.EmailTypeQueue.Any(p => p.Name.Equals(o.Name)));
                }

                EmailConfigurationManager.EmailTypes = EmailConfigurationManager.Convert(rootSection.EmailTypes);
                var tempEmailTypes = EmailConfigurationManager.EmailTypes.ToList();
                if (alreadySentEamilTypes != null && alreadySentEamilTypes.Count > 0)
                {
                    // exclude already sent email
                    tempEmailTypes = tempEmailTypes.FindAll(o => !alreadySentEamilTypes.Any(p => p.Name.Equals(o.Name)));
                }

                EmailConfigurationManager.EmailTypeQueue = new Queue<EmailType>(tempEmailTypes);
            }
        }
        public static EmailType GetEmailType(string name)
        {
            if (EmailConfigurationManager.EmailTypes == null || EmailConfigurationManager.EmailTypes.Count == 0)
            {
                return null;
            }

            return EmailConfigurationManager.EmailTypes.Find(o => o.Name.Equals(name));
        }
    }
}
