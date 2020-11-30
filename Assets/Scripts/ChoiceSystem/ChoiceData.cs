using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ChoiceData
{
    public string PortName;
    public string ChoiceTitle;
    public bool WasMade;
    public bool WasFailed;
    public NarrativePath NarrativePath;
    public int RequiredCharisma;
    public int RequiredDeception;
    public int RequiredThoughtfulness;

    public ChoiceData(string portName, string choiceTitle = "Sample choice", bool wasMade = true)
    {
        this.PortName = portName;
        this.ChoiceTitle = choiceTitle;
        this.WasMade = wasMade;
        this.NarrativePath = NarrativePath.None;
        WasFailed = false;
        RequiredCharisma = 0;
        RequiredDeception = 0;
        RequiredThoughtfulness = 0;
    }

    public void skillCheck(CharacterStats playerStats)
    {
        if (!(playerStats.charisma >= RequiredCharisma && playerStats.deception >= RequiredDeception && playerStats.thoughtfulness >= RequiredThoughtfulness))
            WasFailed = true;
    }
}
