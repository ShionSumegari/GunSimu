using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    public static void SavePlayer(UserData player)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/player.terajet";

        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, player);

        stream.Close();
    }

    public static UserData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.terajet";
        if (File.Exists(path))
        {
            Debug.Log("Save file found in " + path);

            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = new FileStream(path, FileMode.Open);

            UserData data = formatter.Deserialize(stream) as UserData;

            stream.Close();

            return data;
        }
        else
        {
            Debug.LogWarning("Save file not found in " + path + "Generate Zero Data");
            UserData data = new UserData();
            return data;
        }
    }
}
