using System;
using Newtonsoft.Json;

namespace ciev.Models
{
    [JsonObject]
    public class LogClass
    {
        [JsonProperty("deviceId")]
        public string DeviceId { get; set; }
        [JsonProperty("lastConn")]
        public string LastConn { get; set; }
        [JsonProperty("lastDate")]
        public string LastDate { get; set; }
    }
}