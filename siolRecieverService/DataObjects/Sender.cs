using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Mobile.Service;

namespace siolRecieverService.DataObjects
{
    public class Sender: EntityData
    {
        [ForeignKey("Id")]
        public virtual Receivers Reciever { get; set; }

        [ForeignKey("Id")]
        public virtual Announcement Announcement { get; set; }
    }
}