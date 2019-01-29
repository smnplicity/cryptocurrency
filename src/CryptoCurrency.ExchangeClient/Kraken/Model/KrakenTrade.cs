using System;
using System.Collections.Generic;
using System.Reflection;

using Newtonsoft.Json;

namespace CryptoCurrency.ExchangeClient.Kraken.Model
{
    internal class KrakenTradeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => true;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var result = Activator.CreateInstance(objectType);

            var typeKey = objectType.GenericTypeArguments[0];
            var typeValue = objectType.GenericTypeArguments[1];

            var propLast = objectType.GetRuntimeProperty(nameof(KrakenTrade<int, int>.Last));
            var methodAdd = objectType.GetRuntimeMethod(nameof(KrakenTrade<int, int>.Add), new[] { typeKey, typeValue });

            reader.Read();

            while (reader.TokenType != JsonToken.EndObject)
            {
                var key = reader.Value.ToString();

                reader.Read();

                if (string.Equals(key, "last", StringComparison.CurrentCultureIgnoreCase))
                    propLast.SetValue(result, serializer.Deserialize<long>(reader));
                else
                    methodAdd.Invoke(result, new[] { key, serializer.Deserialize(reader, typeValue) });

                reader.Read();
            }

            return result;
        }

        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    [JsonConverter(typeof(KrakenTradeConverter))]
    public class KrakenTrade<TKey, TValue> : Dictionary<TKey, TValue>
    {
        [JsonProperty(PropertyName = "last")]
        public long Last { get; set; }
    }
}
