using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public struct Ghosts
{
    public Ghost[] ghosts;
}

[System.Serializable]
public struct Ghost
{
    public string app;
    public int insertTimestamp;
    public string inserterID;

    public GhostData ghostData;
}