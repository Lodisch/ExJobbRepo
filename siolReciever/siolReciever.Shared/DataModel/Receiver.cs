using System;
using System.Collections.Generic;
using System.Text;

namespace siolReciever.DataModel
{
    public class Receiver
    {
        public string Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string UserId { get; set; }

        public ReceiverGroup ReceiverGroup { get; set; }
    }
}
