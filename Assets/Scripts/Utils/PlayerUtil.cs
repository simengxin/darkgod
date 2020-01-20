using System.Net;
using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEProtocol;
using Tools.Files;
using Newtonsoft.Json;
using System.IO;
public class PlayerUtil
{
    public static PlayerData LoadLocalPlayerData()
    {
        string text = "";
        string path = Application.persistentDataPath + "/player.json";
        if(File.Exists(path))
        {
            text = File.ReadAllText(path);
        }
        else
        {
            text = Resources.Load<TextAsset>("ResCfgs/player").text;
        }
        PlayerData pd = KJson.Parse<PlayerData>(text);
        return pd;
    }
    public static void SetLocalPlayerData(PlayerData data)
    {
        var file = KJson.ToJson(data);
        string path = Application.persistentDataPath + "/player.json";
        KJson.WriteJson(path,file,true);
    }
}
