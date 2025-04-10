using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Vri.Domain.Models;

namespace Vri.Domain.Converters
{
    internal class TickConverter : JsonConverter<List<Tick>>
    {
        public override List<Tick> ReadJson(JsonReader reader, Type objectType, List<Tick> existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var ticksArray = JArray.Load(reader);
            var ticks = new List<Tick>();
            foreach (JToken token in ticksArray)
            {
                if (token is JArray inner && inner.Count >= 2)
                {
                    long timestamp = inner[0].ToObject<long>();
                    decimal rate = inner[1].ToObject<decimal>();
                    ticks.Add(new Tick { Timestamp = timestamp, Rate = rate });
                }
            }
            return ticks;
        }

        public override void WriteJson(JsonWriter writer, List<Tick> value, JsonSerializer serializer)
        {
            var array = new JArray();
            foreach (var tick in value)
            {
                array.Add(new JArray(tick.Timestamp, tick.Rate));
            }
            array.WriteTo(writer);
        }
    }
}
