using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace siolRecieverService.DataObjects.DataTransferObjects
{
    public class ReceiverDto
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string RecieverGroupId { get; set; }
        
    }
}