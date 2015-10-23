using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.IO;

namespace civstats
{
    [Serializable]
    public class StatsUpdate
    {
        public Dictionary<string, float> demographics;
        public Dictionary<string, int> policies;
        public string religion;

        public StatsUpdate()
        {
            demographics = new Dictionary<string, float>();
            policies = new Dictionary<string, int>();
            religion = null;
        }

        public string ToJson()
        {
            var serializer = new DataContractJsonSerializer(typeof(StatsUpdate), new DataContractJsonSerializerSettings()
            {
                UseSimpleDictionaryFormat = true
            });

            using (var tempStream = new MemoryStream())
            {
                serializer.WriteObject(tempStream, this);
                return Encoding.Default.GetString(tempStream.ToArray());
            }
        }
    }
}
