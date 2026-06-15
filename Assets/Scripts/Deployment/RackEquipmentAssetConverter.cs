using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace VzDev.DCIM.Deployment
{
    /// <summary>
    /// 供解析JsonConvert.DeserializeObject使用
    /// </summary>
    public class RackEquipment_AssetBaseConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
            => objectType == typeof(RackEquipment_AssetBase);

        public override object ReadJson(JsonReader reader, Type objectType,
            object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);
            string assetType = jo["assetType"]?.Value<string>();

            RackEquipment_AssetBase target = assetType switch
            {
                "DCN" => new DCN_Asset(),
                "DCS" => new DCS_Asset(),
                _ => throw new JsonSerializationException($"Unknown assetType: {assetType}")
            };

            serializer.Populate(jo.CreateReader(), target);
            return target;
        }

        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            => throw new NotImplementedException();
    }
}