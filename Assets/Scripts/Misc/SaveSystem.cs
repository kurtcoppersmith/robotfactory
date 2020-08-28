using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using UnityEngine;

public static class SaveSystem
{
    /// <summary>
    /// The generic passed in MUST be an instance of a class.
    /// </summary>
    /// <typeparam name="D"></typeparam>
    /// <param name="dataToBeSaved"></param>
    /// <param name="fileName"></param>
    public static void SaveData<D>(D dataToBeSaved, string fileName) where D : class
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + fileName;

        Directory.CreateDirectory(Path.GetDirectoryName(path));

        FileStream stream = new FileStream(path, FileMode.Create);

        D data = dataToBeSaved;

        formatter.Serialize(stream, data);
        stream.Close();
    }

    /// <summary>
    /// Will return only an instance of a class.
    /// </summary>
    /// <typeparam name="D"></typeparam>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static D LoadData<D>(string fileName) where D : class
    {
        string path = Application.persistentDataPath + "/" + fileName;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            D data = formatter.Deserialize(stream) as D;
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
