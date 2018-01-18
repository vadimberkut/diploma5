using CsvHelper;
using diploma5_csharp.Helpers;
using diploma5_csharp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
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
        private const string DATA_STORE_FOLDER = "ApplicationData";
        private const string METRICS_STORE_FOLDER = "Metrics";
        private const string STORE_FILE_NAME = "methodinfostore.json";
        private const string BACKUP_FILE_NAME = "methodinfostore.backup.json";
        private const string CSV_DELIMITER = ";"; // exel automatically formats CSV with semicolon (alternatively add "sep=;" to start of the CSV file)
        private readonly List<EnhanceMethodInfoModel> Store;

        private object _lock = new object();

        public Dictionary<string, string> MethodNameMap = new Dictionary<string, string>()
        {
            // FOG
            { nameof(Fog.RemoveFogUsingDarkChannelPrior), "DCP" },
            { nameof(Fog.RemoveFogUsingMedianChannelPrior), "MCP" },
            { nameof(Fog.RemoveFogUsingIdcpWithClahe), "DCP&CLAHE" },
            { nameof(Fog.RemoveFogUsingDCPAndDFT), "DCP&DFT" },
            { nameof(Fog.RemoveFogUsingMultiCoreDSPMethod), "DSP" },
            { nameof(Fog.EnhaceVisibilityUsingRobbyTanMethodForRoads), "RTFR" },
            { nameof(Fog.RemoveFogUsingCustomMethod), "CUS" },
            { nameof(Fog.RemoveFogUsingCustomMethodWithDepthEstimation), "CUSD" },

            // DUST
            { nameof(Dust.VisibilityEnhancementUsingTunedTriThresholdFuzzyIntensificationOperatorsMethod), "TTFIO" },
            { nameof(Dust.RecoveringOfWeatherDegradedImagesBasedOnRGBResponseRatioConstancyMethod), "RGBRRC" },
        };

        public MethodInfoStore()
        {
            Store = this.LoadFromFile();
            this.Backup();
        }

        public void AddOrUpdate(EnhanceMethodInfoModel data)
        {
            lock (_lock)
            {
                var info = Store.FirstOrDefault(x => x.ImageFileName == data.ImageFileName && x.EnhanceMethodName == data.EnhanceMethodName);
                if (info == null)
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
        }

        // saves store to file
        public void SaveToFile()
        {
            var text = JsonConvert.SerializeObject(this.Store, Formatting.Indented);
            File.WriteAllText(this.GetSavePath(STORE_FILE_NAME), text, Encoding.UTF8);
        }

        // backup
        public void Backup()
        {
            var text = JsonConvert.SerializeObject(this.Store, Formatting.Indented);
            File.WriteAllText(this.GetSavePath(BACKUP_FILE_NAME), text, Encoding.UTF8);
        }

        public string SaveToCsv(string folderPath = "")
        {
            // use current directory if no path specified
            if (String.IsNullOrEmpty(folderPath))
            {
                folderPath = Directory.GetCurrentDirectory();
            }

            // delete all csv in store folder
            var files = Directory.GetFiles(this.GetSavePath(""));
            foreach (var file in files)
            {
                if (Path.GetExtension(file) == ".csv")
                {
                    File.Delete(file);
                }
            }

            // Save only methods with maps
            var whiteList = this.MethodNameMap.Keys;
            var store = this.Store.Where(x => whiteList.Contains(x.EnhanceMethodName));

            // create metrics folder
            if (!Directory.Exists(this.GetMetricsSavePath()))
            {
                Directory.CreateDirectory(this.GetMetricsSavePath());
            }

            // get exec time statistics for each method
            var execStat = store.GroupBy(x => x.EnhanceMethodName, (key, g) =>
            {
                return new
                {
                    EnhanceMethodName = MethodNameMap[key],
                    Min = g.Min(x => x.ExecutionTimeMs),
                    Max = g.Max(x => x.ExecutionTimeMs),
                    Average = g.Average(x => x.ExecutionTimeMs),
                    Moda = StatisticsHelper.Moda(g.Select(x => x.ExecutionTimeMs))
                };
            }).ToList();
            string execStatFilePath = this.GetMetricsSavePath("methodsExecutionStatistic.csv");
            using (var textWriter = File.CreateText(execStatFilePath))
            {
                using (var csv = new CsvWriter(textWriter, new CsvHelper.Configuration.Configuration { Delimiter = CSV_DELIMITER }))
                {
                    csv.WriteRecords(execStat);
                }
            }

            // save metrcis
            string savedPath = this.SaveMetricsToCsv(store);
            return savedPath;
        }

        private string SaveMetricsToCsv(IEnumerable<EnhanceMethodInfoModel> store)
        {
            // for each metric get list of results
            string[] metricNames = typeof(MetricsResult).GetProperties().Select(x => x.Name).ToArray();
            string[] methodNames = store.Select(x => x.EnhanceMethodName).Distinct().OrderBy(x => x).ToArray();
            var metricProps = typeof(MetricsResult).GetProperties();

            var metricsValues = store.Where(x => x.Metrics != null).Select(x =>
            {
                return metricProps.Select(y => y).Select(y => new
                {
                    ImageFileName = x.ImageFileName,
                    EnhanceMethodName = x.EnhanceMethodName,
                    MetricName = y.Name,
                    MetricValue = y.GetValue(x.Metrics).ToString()
                }).ToList();
            }).SelectMany(x => x)
            .ToList();

            var metricTables = metricsValues.GroupBy(x => x.MetricName, (key, g) =>
            {
                var metricRows = g.GroupBy(y => y.ImageFileName, (key2, g2) =>
                {
                    Dictionary<string, string> metricRow = new Dictionary<string, string>();
                    metricRow.Add("MetricName", key);
                    metricRow.Add("ImageFileName", key2);

                    foreach (var methodName in methodNames)
                    {
                        var metricValueForMethod = g2.Where(z => z.EnhanceMethodName == methodName).Select(z => z.MetricValue).FirstOrDefault();
                        metricRow.Add(MethodNameMap[methodName], metricValueForMethod);

                    }

                    return metricRow;
                }).ToList();
                return new { MetricName = key, Rows = metricRows };
            }).ToList();

           

            foreach (var metricTable in metricTables)
            {
                string suffix = ""; 
                string metricStatFilePath = this.GetMetricsSavePath($"imageMetricStatistic_{metricTable.MetricName}{suffix}.csv");
                if (metricTable.Rows.Count > 0)
                {
                    using (var fs = new FileStream(metricStatFilePath, FileMode.Create, FileAccess.Write))
                    {
                        using (var sw = new StreamWriter(fs))
                        {
                            // write header
                            var columnNames = metricTable.Rows[0].Select(x => x.Key);
                            var header = String.Join(CSV_DELIMITER, columnNames);

                            sw.WriteLine(header);
                            //sw.WriteLine(Environment.NewLine);
                            foreach (var row in metricTable.Rows)
                            {
                                var rowValues = columnNames.Select(col => row[col]);
                                var rowStr = String.Join(CSV_DELIMITER, rowValues);
                                sw.WriteLine(rowStr);
                                //sw.WriteLine(Environment.NewLine);
                            }
                        }
                    }
                }
            }

            return this.GetMetricsSavePath();
        }

        public void Reset()
        {
            // delete store json file
            if (File.Exists(this.GetSavePath(STORE_FILE_NAME)))
            {
                File.Delete(this.GetSavePath(STORE_FILE_NAME));
            }

            // delete all csv
            var allFiles = Directory.GetFiles(DATA_STORE_FOLDER);
            foreach (var file in allFiles)
            {
                if (Path.GetExtension(file) == ".csv")
                {
                    if (File.Exists(file)) File.Delete(file);
                }
            }

        }

        // loads store from file
        private List<EnhanceMethodInfoModel> LoadFromFile()
        {
            if (!Directory.Exists(DATA_STORE_FOLDER))
            {
                Directory.CreateDirectory(DATA_STORE_FOLDER);
            }
            if (!File.Exists(this.GetSavePath(STORE_FILE_NAME)))
            {
                // create empty file
                var emptyStore = new List<EnhanceMethodInfoModel>();
                var text = JsonConvert.SerializeObject(emptyStore);
                File.WriteAllText(this.GetSavePath(STORE_FILE_NAME), text);
                return emptyStore;
            }
            else
            {
                var text = File.ReadAllText(this.GetSavePath(STORE_FILE_NAME), Encoding.UTF8);
                var store = JsonConvert.DeserializeObject<List<EnhanceMethodInfoModel>>(text);
                return store;
            }
        }

        private string GetSavePath(string filename)
        {
            return Path.Combine(DATA_STORE_FOLDER, filename);
        }
        private string GetMetricsSavePath(string filename = "")
        {
            return Path.Combine(DATA_STORE_FOLDER, METRICS_STORE_FOLDER, filename);
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
