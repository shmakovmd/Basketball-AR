using System;
using System.IO;
using UnityEngine;
using Directory = System.IO.Directory;
using File = System.IO.File;

public class SaveSystem : MonoBehaviour
{
    public GameData gameData = new();
    private string savesDir;

    private void Awake()
    {
        savesDir = Path.Combine(Application.persistentDataPath, "saves");
        Load();
    }

    public void Save()
    {
        if (!Directory.Exists(savesDir)) Directory.CreateDirectory(savesDir);
        File.WriteAllText(Path.Combine(savesDir, "preferences.json"), JsonUtility.ToJson(gameData));
    }

    private void Load()
    {
        if (File.Exists(Path.Combine(savesDir, "preferences.json")))
            gameData = JsonUtility.FromJson<GameData>(File.ReadAllText(Path.Combine(savesDir, "preferences.json")));
    }
}

[Serializable]
public class GameData
{
    public int record;
    public bool isMute;
}