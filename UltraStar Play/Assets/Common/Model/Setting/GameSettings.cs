using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameSettings
{
    public SystemLanguage language = SystemLanguage.English;
    public List<string> songDirs = new();
    public EScoreMode ScoreMode { get; set; } = EScoreMode.Individual;
    public string CommonScoreNameSeparator { get; set; } = " & ";
}
