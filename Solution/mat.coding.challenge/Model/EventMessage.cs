using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace mat.coding.challenge.Model
{
    public class EventMessage
    {
        [JsonProperty("lat")] public int Latitude { get; set; }
        [JsonProperty("long")] public double Longitude { get; set; }
    }
}
