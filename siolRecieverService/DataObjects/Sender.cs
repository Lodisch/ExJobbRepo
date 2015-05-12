using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace siolRecieverService.DataObjects
{
    public class Sender
    {
        public Announcement Announcement { get; set; }
        public Receiver Receiver { get; set; }
    }
}