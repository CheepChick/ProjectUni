using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Text;

public class JsonFileManager 
{
    public void SaveJsonFile(string createPath,string fileName,string jsonData)
    {
        FileStream fs = new FileStream($"{createPath}/{fileName}.json", FileMode.Create);

        byte[] data = Encoding.UTF8.GetBytes(jsonData);
        fs.Write(data, 0, data.Length);
        fs.Close();
    }
    public T LoadJsonFile<T>(string createPath,string fileName)
    {
        if (File.Exists($"{createPath}/{fileName}.json"))
        {
            FileStream fs = new FileStream($"{createPath}/{fileName}.json", FileMode.Open);
            byte[] data = new byte[fs.Length];
            fs.Read(data, 0, data.Length);
            fs.Close();
            string jsonData = Encoding.UTF8.GetString(data);
            return  JsonConvert.DeserializeObject<T>(jsonData);
        }
        else
        {
            return default(T);
        }
    }
}
