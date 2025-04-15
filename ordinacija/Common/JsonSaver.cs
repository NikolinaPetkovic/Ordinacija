using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Common
{
    public class JsonSaver
    {
        public static void Cuvanje<T>(string putanja, List<T> lista)
        {
            using (StreamWriter sw = new StreamWriter(putanja))
            {
                string json = JsonConvert.SerializeObject(lista, Formatting.Indented);
                sw.Write(json);
            }
        }
    }
}
