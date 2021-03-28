using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UmaTools.Models
{
    [JsonObject]
    public class Setting
    {
        [JsonProperty("ProcessName")]
        public string ProcessName { get; set; }

        [JsonProperty("FolderPath")]
        public string FolderPath { get; set; }
    }

    public class DataAccessor
    {
        private static string FilePath = @"\setting.json";
        public static (bool res, Setting data) load()
        {
            string json = null;
            try
            {
                json = File.ReadAllText(FilePath);
            }
            catch
            {
                return (false, null);
            }

            var data = JsonConvert.DeserializeObject<Setting>(json);
            return (true, data);
        }

        public static void save(Setting data)
        {
            var json = JsonConvert.SerializeObject(data);

            var path = System.AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + FilePath;
            File.WriteAllText(path, json);
        }
    }
}
