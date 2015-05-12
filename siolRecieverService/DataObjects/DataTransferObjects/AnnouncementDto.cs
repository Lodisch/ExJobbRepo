using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace siolRecieverService.DataObjects.DataTransferObjects
{
    public class AnnouncementDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
    }
}