using System.Collections.Generic;
using System.Linq;

namespace Utils
{
    public class JsonRequest
    {
        private readonly List<JsonRequestData> data;

        public JsonRequest(List<JsonRequestData> data = null)
        {
            if (data == null)
            {
                data = new List<JsonRequestData>();
            }
            this.data = data;
        }

        public JsonRequest(JSONNode data)
        {
            if (this.data == null)
            {
                this.data = new List<JsonRequestData>();
            }
            
            foreach (KeyValuePair<string, JSONNode> requestData in data.AsObject)
            {
                AddData(requestData.Key, requestData.Value);
            }
        }

        public void AddData(string key, string value)
        {
            data.Add(new JsonRequestData(key, value));
        }

        public bool TryGetData(string key, out string value)
        {
            foreach (var requestData in data.Where(requestData => requestData.key.Equals(key)))
            {
                value = requestData.value;
                return true;
            }

            value = null;
            return false;
        }
        
        public override string ToString()
        {
            string json = data.Aggregate("{", (current, requestData) => current + ("'" + requestData.key + "': " + "'" + requestData.value + "',"));
            json += "}";

            return json;
        }
    }

    public readonly struct JsonRequestData
    {
        public readonly string key;
        public readonly string value;

        public JsonRequestData(string key, string value)
        {
            this.key = key;
            this.value = value;
        }
    }
}