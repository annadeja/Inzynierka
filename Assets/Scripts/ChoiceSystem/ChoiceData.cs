using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ChoiceData
{
    public string portName;
    public string choiceTitle;
    public bool wasMade;
    public bool wasFailed = false;
    public NarrativePath narrativePath;
    public int requiredCharisma = 0;
    public int requiredDeception = 0;
    public int requiredThoughtfulness = 0;

    public ChoiceData(string portName, string choiceTitle = "Sample choice", bool wasMade = true)
    {
        this.portName = portName;
        this.choiceTitle = choiceTitle;
        this.wasMade = wasMade;
        this.narrativePath = NarrativePath.None;
    }

    public void skillCheck(CharacterStats playerStats)
    {
        if (!(playerStats.charisma >= requiredCharisma && playerStats.deception >= requiredDeception && playerStats.thoughtfulness >= requiredThoughtfulness))
            wasFailed = true;
    }
}
