using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Mobile.Service;

namespace siolRecieverService.DataObjects
{
    public class Receiver:EntityData
    {
        public string UserId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }

        public ReceiverGroup ReceiverGroup { get; set; }
    }
}