using System;
using System.IO;
using System.Xml.Serialization;

namespace GameLibrary.Utilities
{
  public class XmlManager<T>
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
        XmlSerializer xml = new XmlSerializer(type);
        instance = (T)xml.Deserialize(reader);
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
        XmlSerializer xml = new XmlSerializer(type);
        xml.Serialize(writer, obj);
      }
    }
  }
}