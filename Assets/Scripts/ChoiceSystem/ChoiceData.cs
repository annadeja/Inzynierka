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
    public NarrativePath narrativePath;

    public ChoiceData(string portName, string choiceTitle)
    {
        this.portName = portName;
        this.choiceTitle = choiceTitle;
        this.wasMade = true;
        this.narrativePath = NarrativePath.None;
    }
}
