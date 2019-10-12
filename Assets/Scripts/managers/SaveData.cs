using System;
using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public SaveData()
    {
        name = "";
        masterVolume = 0;
        musicVolume = 0;
        effectsVolume = 0;
    }

    public string name;
    public float masterVolume;
    public float musicVolume;
    public float effectsVolume;
}
