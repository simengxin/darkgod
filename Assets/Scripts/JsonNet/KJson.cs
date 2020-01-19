using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

public class KJson
{

    /**
     * 解析JSON为对象
     */
    public static T Parse<T>(string json)
    {
        if (string.IsNullOrEmpty(json))
        {
            return default;
        }

        return JsonConvert.DeserializeObject<T>(json);
    }
    
    /**
     * 解析JSON为对象列表；
     */
    public static List<T> ParseList<T>(string json)
    {
        if (string.IsNullOrEmpty(json))
        {
            return default;
        }

        return JsonConvert.DeserializeObject<List<T>>(json);
    }
    
    /**
     * 解析JSON为对象数组；
     */
    public static T[] ParseArray<T>(string json)
    {
        if (string.IsNullOrEmpty(json))
        {
            return default;
        }
        return JsonConvert.DeserializeObject<T[]>(json);
    }

    /**
     * 对象生成JSON；
     */
    public static string ToJson<T>(T bean)
    {
        if (bean == null)
        {
            return null;
        }

        return JsonConvert.SerializeObject(bean);
    }

    /**
     * 对象数组或列表生成JSON；
     */
    public static string ArrayToJson<T>(IEnumerable<T> beans)
    {
        if (beans == null || !beans.Any())
        {
            return null;
        }

        return JsonConvert.SerializeObject(beans);
    }

    /**
     * 生成JSON文件；
     */
    public static void WriteJson(string path, string json, bool overwrite = false)
    {
        if (string.IsNullOrEmpty(json))
        {
            return;
        }

        if (overwrite && File.Exists(path))
        {
            File.Delete(path);
        }
        
        File.WriteAllText(path, json);
    }

}