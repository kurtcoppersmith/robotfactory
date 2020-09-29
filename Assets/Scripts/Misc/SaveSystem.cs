using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveSystem
{    
    /// <summary>
    /// The generic passed in MUST be an instance of a class.
    /// </summary>
    /// <typeparam name="D"></typeparam>
    /// <param name="dataToBeSaved"></param>
    /// <param name="fileName"></param>
    public static void SaveData(GameManager dataToBeSaved, string fileName)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + fileName;

        Directory.CreateDirectory(Path.GetDirectoryName(path));

        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData data = new SaveData(dataToBeSaved);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    /// <summary>
    /// Will return only an instance of a class.
    /// </summary>
    /// <typeparam name="D"></typeparam>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static SaveData LoadData(string fileName)
    {
        string path = Application.persistentDataPath + "/" + fileName;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogWarning("Save not found in: " + path);
            return null;
        }
    }

    public static void ClearData(string filename)
    {
        string path = Application.persistentDataPath + "/" + filename;
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}
