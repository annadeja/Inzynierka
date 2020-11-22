using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public CharacterStats playerStats;
    public List<ChoiceData> pastChoices;
    public string lastLocation;
    public int[] playerPosition;
    public int revolutionChoices;
    public int reformChoices;
    public int conquestChoices;

    public SaveData()
    {
        playerStats = new CharacterStats();
        pastChoices = new List<ChoiceData>();
        lastLocation = "SampleScene";
        playerPosition = new int[3] {0, 10, 0};
        revolutionChoices = 0;
        reformChoices = 0;
        conquestChoices = 0;
    }
}
