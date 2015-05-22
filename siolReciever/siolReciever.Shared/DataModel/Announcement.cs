using System;
using System.Collections.Generic;
using System.Text;

namespace siolReciever.DataModel
{
    public class Announcement
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }       
    }
}
