using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public CharacterStats PlayerStats;
    public List<ChoiceData> PastChoices;
    public string LastLocation;
    public float[] PlayerPosition;
    public int RevolutionChoices;
    public int ReformChoices;
    public int ConquestChoices;

    public SaveData()
    {
        PlayerStats = new CharacterStats();
        PastChoices = new List<ChoiceData>();
        LastLocation = "DialogDemonstration";
        PlayerPosition = new float[3] {0, 10, 0};
        RevolutionChoices = 0;
        ReformChoices = 0;
        ConquestChoices = 0;
    }

    public SaveData(string lastLocation)
    {
        PlayerStats = new CharacterStats();
        PastChoices = new List<ChoiceData>();
        LastLocation = lastLocation;
        PlayerPosition = new float[3] { 0, 10, 0 };
        RevolutionChoices = 0;
        ReformChoices = 0;
        ConquestChoices = 0;
    }
}
