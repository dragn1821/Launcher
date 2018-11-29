using Newtonsoft.Json;
using System;
using System.IO;

namespace GameLibrary.Utilities
{
    public class JsonManager<T>
    {
        public T Load(string path)
        {
            return Load(path, typeof(T));
        }

        public T Load(string path, Type type)
        {
            T instance;

            using (TextReader reader = new StreamReader(path))
            {
                instance = JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
            }

            return instance;
        }

        public void Save(string path, object obj)
        {
            Save(path, obj, typeof(T));
        }

        public void Save(string path, object obj, Type type)
        {
            using (TextWriter writer = new StreamWriter(path))
            {
                writer.Write(JsonConvert.SerializeObject(obj));
            }
        }
    }
}