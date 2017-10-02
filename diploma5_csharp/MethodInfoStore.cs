using diploma5_csharp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diploma5_csharp
{
    /// <summary>
    /// Stores methods info such as metrics, execution time
    /// </summary>
    public class MethodInfoStore
    {
        private const string StoreFilePath = "methodinfostore.json";
        private readonly List<EnhanceMethodInfoModel> Store;

        public MethodInfoStore()
        {
            Store = this.LoadFromFile();
        }

        public void AddOrUpdate(EnhanceMethodInfoModel data)
        {
            var info = Store.FirstOrDefault(x => x.ImageFileName == data.ImageFileName && x.EnhanceMethodName == data.EnhanceMethodName);
            if(info == null)
            {
                // add
                Store.Add(data);
            }
            else
            {
                // update
                info.ExecutionTimeMs = data.ExecutionTimeMs;
                info.Metrics = data.Metrics;
            }

            this.SaveToFile();
        }

        // saves store to file
        public void SaveToFile()
        {
            var text = JsonConvert.SerializeObject(this.Store, Formatting.Indented);
            File.WriteAllText(StoreFilePath, text, Encoding.UTF8);
        }

        public void SaveToCsv()
        {
            throw new NotImplementedException();
        }

        // loads stroe from file
        private List<EnhanceMethodInfoModel> LoadFromFile()
        {
            if (!File.Exists(StoreFilePath))
            {
                // create empty file
                var emptyStore = new List<EnhanceMethodInfoModel>();
                var text = JsonConvert.SerializeObject(emptyStore);
                File.WriteAllText(StoreFilePath, text);
                return emptyStore;
                //throw new Exception($"Method info store file not found: {StoreFilePath}");
            }
            else
            {
                var text = File.ReadAllText(StoreFilePath, Encoding.UTF8);
                var store = JsonConvert.DeserializeObject<List<EnhanceMethodInfoModel>>(text);
                return store;
            }
        }
    }

    public class EnhanceMethodInfoModel
    {
        public string ImageFileName { get; set; }
        public string EnhanceMethodName { get; set; }
        public MetricsResult Metrics { get; set; }
        public double ExecutionTimeMs { get; set; }
    }
}
