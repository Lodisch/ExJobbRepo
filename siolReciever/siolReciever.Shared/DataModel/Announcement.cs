using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using siolReciever;

namespace siolReciever.DataModel
{
    public class Announcement
    {
        public string Id { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "isread")]
        public bool IsRead { get; set; }
    }
}
