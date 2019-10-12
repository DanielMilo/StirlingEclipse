using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileManager
{
    public SaveData data;

    string path;

    public FileManager()
    {
        path = Application.dataPath + "/Saves/saveFile.file";
        LoadData();
    }

    public void LoadData()
    {
        if(System.IO.File.Exists(path))
        {
            string dataString = System.IO.File.ReadAllText(path);
            data = JsonUtility.FromJson<SaveData>(dataString);
        }
        else
        {
            data = new SaveData();
        }
    }

    public void SaveData()
    {
        string dataString = JsonUtility.ToJson(data);
        System.IO.File.WriteAllText(path, dataString);
    }
}
