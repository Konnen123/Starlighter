
using UnityEngine;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using JetBrains.Annotations;
using Unity.VisualScripting;

public static class SaveSystem 
{
    public static void SaveLevel(int level)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/level.dat";

        FileStream stream = new FileStream(path, FileMode.Create);

        LevelData levelData = new LevelData(level);
        
        formatter.Serialize(stream,levelData);
        stream.Close();
    }

    public static LevelData LoadLevel()
    {
        string path = Application.persistentDataPath + "/level.dat";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

           LevelData levelData = formatter.Deserialize(stream) as LevelData;
           stream.Close();

           return levelData;
        }
     
        Debug.Log("save file not found " +path);
        return null;
        
    }
    
}
