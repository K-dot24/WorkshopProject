using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3
{    
    public class Config
    {
        [JsonProperty("email")]
        public String email { get; set;}

        [JsonProperty("password")]
        public String password { get; set;}

        [JsonProperty("signalRServer_url")]
        public String signalRServer_url { get; set; }

        [JsonProperty("mongoDB_url")]
        public String mongoDB_url { get; set; }

        [JsonProperty("externalSystem_url")]
        public String externalSystem_url { get; set; }
    }
}
