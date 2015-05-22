using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace siolReciever.DataModel
{
    public class Sender
    {
        public string Id { get; set; }
        public Announcement Announcement { get; set; }
        public Receiver Receiver { get; set; }
    }
}
