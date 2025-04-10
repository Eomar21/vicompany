using Newtonsoft.Json;
using System.Collections.Generic;
using Vri.Domain.Converters;

namespace Vri.Domain.Models
{
    internal class TicksResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("ticks")]
        [JsonConverter(typeof(TickConverter))]
        public List<Tick> Ticks { get; set; }
    }
}
