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
    public abstract class Update
    {
        public Update()
        { }

        public string ToJson()
        {
            var serializer = new DataContractJsonSerializer(this.GetType(), new DataContractJsonSerializerSettings()
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
