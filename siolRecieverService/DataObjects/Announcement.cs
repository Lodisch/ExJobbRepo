using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Mobile.Service;

namespace siolRecieverService.DataObjects
{
    public class Announcement: EntityData
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
    }
}