using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Mobile.Service;

namespace siolRecieverService.DataObjects
{
    public class ReceiverGroup:EntityData
    {
        public string Groupname { get; set; }
    }
}