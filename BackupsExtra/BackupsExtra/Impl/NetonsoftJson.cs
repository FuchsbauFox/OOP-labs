using Newtonsoft.Json;

namespace BackupsExtra.BackupsExtra.Impl
{
    public class NetonsoftJson
    {
        public string Serialization(Backup backup)
        {
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
            };
            return JsonConvert.SerializeObject(backup, Formatting.Indented, settings);
        }

        public Backup Deserialization(string serialized)
        {
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
            };
            return JsonConvert.DeserializeObject<Backup>(serialized, settings);
        }
    }
}