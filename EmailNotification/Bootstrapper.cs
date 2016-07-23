using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newegg.WMS.JobConsole.EmailNotification.EmailConfiguration;
using Newegg.WMS.JobConsole.EmailNotificationContract;
using System.ComponentModel.Composition;
using System.IO;
using System.ComponentModel.Composition.Hosting;
using System.Threading.Tasks;
using System.Configuration;

namespace Newegg.WMS.JobConsole.EmailNotification
{
    public class Bootstrapper
    {
        // Events
        public delegate void EventHandler<T>(T message);
        public static event EventHandler<string> Error;
        public static event EventHandler<string> Completed;

        // Fields
        private static readonly string PATH_COMPONENTS_CATALOG = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Components");
        private static readonly string SERVICE_DEFAULT_CLASS = "Newegg.WMS.JobConsole.EmailNotificationService.DefaultEmailContentService,Newegg.WMS.JobConsole.EmailNotificationService";
        private static readonly string DEFAULT_EMAIL_FROM = ConfigurationManager.AppSettings["DefaultMailFrom"];
        private CompositionContainer _compositionContainer; // MEF container to search service.
        private static object _syncLocker = new object(); // to keep the instance single

        // Properties
        private static Bootstrapper m_Instance;
        public static Bootstrapper Instance
        {
            get
            {
                if (Bootstrapper.m_Instance == null)
                {
                    lock (Bootstrapper._syncLocker)
                    {
                        if (Bootstrapper.m_Instance == null)
                        {
                            Bootstrapper.m_Instance = new Bootstrapper();
                        }
                    }
                }

                return Bootstrapper.m_Instance;
            }
        }

        // Ctor
        private Bootstrapper()
        {
            if (!Directory.Exists(Bootstrapper.PATH_COMPONENTS_CATALOG))
            {
                Directory.CreateDirectory(Bootstrapper.PATH_COMPONENTS_CATALOG);
            }

            var catalog = new DirectoryCatalog(Bootstrapper.PATH_COMPONENTS_CATALOG);
            this._compositionContainer = new CompositionContainer(catalog);
            this._compositionContainer.ComposeParts(this);
        }

        // Methods
        private void Send(EmailType emailType)
        {
            // 1. Find the provided service by eamil type's class (Type Full Name).
            emailType.Class = !string.IsNullOrWhiteSpace(emailType.Class) ? emailType.Class : Bootstrapper.SERVICE_DEFAULT_CLASS;
            emailType.From = !string.IsNullOrWhiteSpace(emailType.From) ? emailType.From : Bootstrapper.DEFAULT_EMAIL_FROM;
            var providedService = Bootstrapper.Instance._compositionContainer.GetExport<IEmailContentProviderContract>(emailType.Class);
            if (providedService.Value == null)
            {
                return; // can push some error to client here(.Config Error).
            }

            // 2. Create the eamil contents if service be found.
            var emailContents = providedService.Value.CreateEmailContent(emailType);

            // 3. Then, send email here.
            if (emailContents == null || emailContents.Count == 0)
            {
                return;
            }

            Parallel.ForEach<EmailContent>(emailContents, content =>
            {
                var isSuccessful = false;
                try
                {
                    isSuccessful = MailSender.Send(content.From, content.To, content.Cc, content.Subject, content.Body);
                }
                finally
                {
                    content.EmailType = emailType;
                    providedService.Value.EmailSendCompleted(content, isSuccessful);
                }
            });
        }

        public static void Start()
        {
            try
            {
                // Deal with the config loop sync.
                while (EmailConfigurationManager.EmailTypeQueue.Count > 0)
                {
                    // Try dequeue each eamil type in Q.
                    var emailType = EmailConfigurationManager.EmailTypeQueue.Dequeue();
                    Bootstrapper.Instance.Send(emailType);
                }
            }
            catch (Exception e)
            {
                if (Bootstrapper.Error != null)
                {
                    Bootstrapper.Error("System Error : " + e.Message);
                }
            }
            finally
            {
                if (Bootstrapper.Completed != null)
                {
                    Bootstrapper.Completed("System End : Completed.");
                }
            }
        }
    }
}
