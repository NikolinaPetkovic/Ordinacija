using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Common
{
    public class JsonLoader
    {
        public static void Ucitavanje<T>(string putanja, ref List<T> lista)
        {
            if (File.Exists(putanja))
            {
                using (StreamReader sr = new StreamReader(putanja))
                {
                    lista = JsonConvert.DeserializeObject<List<T>>(sr.ReadToEnd());
                }
            }
        }
    }
}
