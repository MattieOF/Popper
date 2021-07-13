using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class VideoSettings
{
    public int  width, height, refreshRate;
    public bool fullscreen;
}

[Serializable]
public class GameInfo
{
    public int highscore = 0;
    public double bestTime = 0;
    public VideoSettings videoSettings;

    public GameInfo()
    {
        VideoSettings videoSettings = new VideoSettings
        {
            width = Screen.currentResolution.width,
            height = Screen.currentResolution.height,
            refreshRate = Screen.currentResolution.refreshRate,
            fullscreen = Screen.fullScreen
        };
        this.videoSettings = videoSettings;
    }
}

public class GameInfoSerialiser
{
    public static void SaveGameInfo(GameInfo gameInfo, string filename = "gameinfo.json")
    {
        JsonSerializer serializer = JsonSerializer.CreateDefault();
        serializer.Formatting = Formatting.Indented;
        TextWriter writer = new StreamWriter($"{Application.persistentDataPath}/{filename}");
        serializer.Serialize(writer, gameInfo);
        writer.Close();
    }

    public static GameInfo LoadGameInfo(string filename = "gameinfo.json")
    {
        if (!File.Exists($"{Application.persistentDataPath}/{filename}"))
        {
            GameInfo ginfo = new GameInfo();
            SaveGameInfo(ginfo, filename);
            return ginfo;
        }

        JsonSerializer serializer = JsonSerializer.CreateDefault();
        TextReader reader = new StreamReader($"{Application.persistentDataPath}/{filename}");
        GameInfo info = (GameInfo) serializer.Deserialize(reader, typeof(GameInfo));
        reader.Close();
        return info;
    }
}
