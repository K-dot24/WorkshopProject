using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Terminal3WebAPI
{
    public class StoryConfig
    {
        [JsonProperty("story")]
        public Story[] story { get; set; }
    }

    [JsonObject("story")]
    public class Story
    {
        [JsonProperty("function")]
        public string function { get; set; }
        [JsonProperty("args")]
        public string[] args { get; set; }
    }
}
