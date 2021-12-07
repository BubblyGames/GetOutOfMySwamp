using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class PersistenceManager
{
    public static void SaveData(PlayerData playerData)
    {
        GameManager gameManager = GameManager.instance;
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath+"/player.txt";
        FileStream stream = new FileStream(path, FileMode.Create);
        PlayerData data = new PlayerData(gameManager);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadData()
    {
        string path = Application.persistentDataPath+"/player.txt";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.Log("Save file not found");
            return null;
        }
      
    }
}
