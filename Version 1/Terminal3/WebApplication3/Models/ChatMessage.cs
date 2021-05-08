using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Terminal3WebAPI.Models
{
    public class ChatMessage
    {
        public string ClientId { get; set; }
        public string Message { get; set; }
        public string EventName { get; set; }
        public string Date { get; set; }
    }
   
}
