using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Newegg.WMS.JobConsole.EmailNotificationService.Models
{
    public class TVTrackingInfo
    {
        public string SONumber { get; set; }
        public string WarehouseNumber { get; set; }
        public string TrackingNumber { get; set; }
        public string ShippingCarrierType { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ItemNumber { get; set; }
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
