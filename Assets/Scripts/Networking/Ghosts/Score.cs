using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public struct Scores
{
    public Score[] scores;
}

[System.Serializable]
public struct Score
{
    public string app;
    public int insertTimestamp;
    public string inserterID;

    public ScoreData scoreData;
}
