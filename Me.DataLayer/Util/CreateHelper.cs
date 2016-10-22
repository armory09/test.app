using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Me.DataLayer.Util
{
    public class CreateHelper<T>
    {
        private static string _path = @"c:\temp\";

        /// <summary>
        /// Create a directory name temp
        /// see global variable _path
        /// </summary>
        public void CreateFolder()
        {
            DirectoryInfo di = Directory.CreateDirectory(_path);
            di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
        }

        public async Task DeleteFile(string fileName)
        {
            await Task.Run(() =>
            {
                if (File.Exists(JsonPath(_path, fileName)))
                {
                    File.Delete(JsonPath(_path,fileName));
                }
            });
        }

        /// <summary>
        /// Delete all files in _path variable
        /// </summary>
        public void CleanDirectory()
        {
            DirectoryInfo di = new DirectoryInfo(_path);

            foreach (FileInfo file in di.GetFiles()) file.Delete();
            foreach (DirectoryInfo subDirectory in di.GetDirectories()) subDirectory.Delete(true);
        }



        public void WriteJson(IEnumerable<T> model, string fileName)
        {
            using (FileStream fs = File.Open(JsonPath(_path, fileName), FileMode.Create))
            using (StreamWriter sw = new StreamWriter(fs))
            using (JsonWriter jw = new JsonTextWriter(sw))
            {
                jw.Formatting = Formatting.Indented;

                JsonSerializer serializer = new JsonSerializer();

                serializer.Serialize(jw, model);
            }
        }

        public void WriteJson(T model, string fileName)
        {
            using (FileStream fs = File.Open(JsonPath(_path, fileName), FileMode.Create))
            using (StreamWriter sw = new StreamWriter(fs))
            using (JsonWriter jw = new JsonTextWriter(sw))
            {
                jw.Formatting = Formatting.Indented;

                JsonSerializer serializer = new JsonSerializer();

                serializer.Serialize(jw, model);
            }

        }

        public IEnumerable<T> ReadListJson(string fileName)
        {
            if (File.Exists(JsonPath(_path, fileName)))
            {
                string jsonString = File.ReadAllText(JsonPath(_path, fileName));
                return JsonConvert.DeserializeObject<IEnumerable<T>>(jsonString);
            }
            return null;
        }

        public T ReadJson(string fileName)
        {
            if (File.Exists(JsonPath(_path, fileName)))
            {
                string jsonString = File.ReadAllText(JsonPath(_path, fileName));
                return JsonConvert.DeserializeObject<T>(jsonString);
            }
            return default(T);   
        }

        private string JsonPath(string path, string fileName)
        {
            return string.Format("{0}{1}.json", path, fileName);
        }
    }
}
