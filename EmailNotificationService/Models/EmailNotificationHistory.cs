using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Newegg.WMS.JobConsole.EmailNotificationService.Models
{
    public class EmailNotificationHistory
    {
        public int TransactionNumber { get; set; }
        public string EmailTypeName { get; set; }
        public string BizKey { get; set; }
        public string InUser { get; set; }
        public DateTime InDate { get; set; }
        public string LastEditUser { get; set; }
        public DateTime LastEditDate { get; set; }

        #region Condition
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        #endregion
    }
}
