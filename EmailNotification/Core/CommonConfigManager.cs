using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;

namespace Newegg.WMS.JobConsole.EmailNotification.Core
{
    public class CommonConfigManager
    {
        // Events
        public event FileSystemEventHandler Changed;

        // Fields
        private string _configPath;
        private Configuration m_Configuration;
        private FileSystemWatcher m_FileSystemWatcher;

        // Ctors
        public CommonConfigManager()
            : this(string.Empty)
        {
        }
        public CommonConfigManager(string configPath)
        {
            this._configPath = configPath;
            FileInfo fileInfo = new FileInfo(configPath);
            this.m_FileSystemWatcher = new FileSystemWatcher(fileInfo.Directory.FullName);
            this.m_FileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            this.m_FileSystemWatcher.Filter = "*.config";
            this.m_FileSystemWatcher.Changed += new FileSystemEventHandler(m_FileSystemWatcher_Changed);
        }

        // Event Handler
        private void m_FileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            this.m_Configuration = this.CreateConfigurationInstance();
            if (this.Changed != null)
            {
                this.Changed(sender, e);
            }
        }

        // Properties
        public Configuration Configuration
        {
            get
            {
                if (this.m_Configuration == null)
                {
                    this.m_Configuration = this.CreateConfigurationInstance();
                }

                return this.m_Configuration;
            }
        }

        // Methods
        private Configuration CreateConfigurationInstance()
        {
            var map = new ExeConfigurationFileMap() { ExeConfigFilename = this._configPath };
            var configuration = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
            return configuration;
        }
        public T GetConfigurationSection<T>(string sectionName) where T : ConfigurationSection
        {
            return this.Configuration.GetSection(sectionName) as T;
        }
        public void SetEnableConfigWatcher(bool isEnable)
        {
            if (this.m_FileSystemWatcher != null)
            {
                this.m_FileSystemWatcher.EnableRaisingEvents = isEnable;
            }
        }
    }
}
